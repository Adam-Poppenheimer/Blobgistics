using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;
using Assets.Blobs;
using Assets.Core;
using System.Collections.ObjectModel;

namespace Assets.Societies {

    [ExecuteInEditMode]
    public class SocietyFactory : SocietyFactoryBase {

        #region instance fields and properties

        #region from SocietyFactoryBase

        public override ComplexityLadderBase StandardComplexityLadder {
            get { return _standardComplexityLadder; }
        }
        public void SetStandardComplexityLadder(ComplexityLadderBase value) {
            _standardComplexityLadder = value;
        }
        [SerializeField] private ComplexityLadderBase _standardComplexityLadder;

        public override ComplexityDefinitionBase DefaultComplexityDefinition {
            get { return _defaultComplexityDefinition; }
        }
        public void SetDefaultComplexityDefinition(ComplexityDefinitionBase value) {
            _defaultComplexityDefinition = value;
        }
        [SerializeField] private ComplexityDefinitionBase _defaultComplexityDefinition;

        public override ReadOnlyCollection<SocietyBase> Societies {
            get { return societies.AsReadOnly(); }
        }
        [SerializeField, HideInInspector] private List<SocietyBase> societies = new List<SocietyBase>();

        #endregion

        public ResourceBlobFactoryBase BlobFactory {
            get { return _blobFactory; }
            set { _blobFactory = value; }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        public ReadOnlyCollection<ComplexityDefinitionBase> ComplexityDefinitions {
            get { return _complexityDefinitions.AsReadOnly(); }
        }
        public void SetComplexityDefinitions(List<ComplexityDefinitionBase> value) {
            _complexityDefinitions = value;
        }
        [SerializeField] private List<ComplexityDefinitionBase> _complexityDefinitions;

        public ReadOnlyCollection<ComplexityLadderBase> ComplexityLadders {
            get { return _complexityLadders.AsReadOnly(); }
        }
        public void SetComplexityLadders(List<ComplexityLadderBase> value) {
            _complexityLadders = value;
        }
        [SerializeField] private List<ComplexityLadderBase> _complexityLadders;

        [SerializeField] private GameObject SocietyPrefab;

        [SerializeField] private AudioSource SocietyDestoryAudio;

        #endregion

        #region instance methods

        #region from SocietyFactoryBase

        public override SocietyBase GetSocietyOfID(int id) {
            return societies.Find(society => society.ID == id);
        }

        public override SocietyBase GetSocietyAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            var retval = societies.Find(society => society.Location == location);
            if(retval != null) {
                return retval;
            }else {
                throw new SocietyException("There exists no society at the specified location");
            }
        }

        public override bool HasSocietyAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return societies.Exists(society => society.Location == location);
        }

        public override bool CanConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder,
            ComplexityDefinitionBase startingComplexity) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(ladder == null) {
                throw new ArgumentNullException("ladder");
            }else if(startingComplexity == null) {
                throw new ArgumentNullException("startingComplexity");
            }
            return !HasSocietyAtLocation(location) && startingComplexity.PermittedTerrains.Contains(location.Terrain);
        }

        public override SocietyBase ConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(ladder == null) {
                throw new ArgumentNullException("ladder");
            }else if(startingComplexity == null) {
                throw new ArgumentNullException("startingComplexity");
            }else if(!ladder.ContainsComplexity(startingComplexity)) {
                throw new SocietyException("The starting complexity of a society must be contained within its ActiveComplexityLadder");
            }else if(!CanConstructSocietyAt(location, ladder, startingComplexity)) {
                throw new SocietyException("Cannot construct a society at this location");
            }

            Society newSociety = null;
            if(SocietyPrefab != null) {
                var prefabClone = Instantiate<GameObject>(SocietyPrefab);
                newSociety = prefabClone.GetComponent<Society>();
                if(newSociety == null) {
                    throw new SocietyException("SocietyPrefab lacks a Society component");
                }
            }else {
                var hostingObject = new GameObject();
                newSociety = hostingObject.AddComponent<Society>();
            }

            var newPrivateData = newSociety.gameObject.AddComponent<SocietyPrivateData>();

            newPrivateData.SetActiveComplexityLadder(ladder);
            newPrivateData.SetBlobFactory(BlobFactory);
            newPrivateData.SetLocation(location);
            newPrivateData.SetUIControl(UIControl);
            newPrivateData.SetParentFactory(this);

            newSociety.PrivateData = newPrivateData;
            newSociety.DestroyAudio = SocietyDestoryAudio;
            newSociety.SetCurrentComplexity(startingComplexity);
            newSociety.transform.SetParent(location.transform, false);
            newSociety.name = "Society at " + location.name;
            newSociety.AscensionIsPermitted = false;

            SubscribeSociety(newSociety);
            return newSociety;
        }

        public override void DestroySociety(SocietyBase society) {
            UnsubscribeSociety(society);
            if(Application.isPlaying) {
                Destroy(society.gameObject);
            }else {
                DestroyImmediate(society.gameObject);
            }
        }

        public override void SubscribeSociety(SocietyBase society) {
            societies.Add(society);
            RaiseSocietySubscribed(society);
        }

        public override void UnsubscribeSociety(SocietyBase society) {
            if(society == null) {
                throw new ArgumentNullException("societyBeingDestroyed");
            }
            societies.Remove(society);
            RaiseSocietyUnsubscribed(society);
        }

        public override void TickSocieties(float secondsPassed) {
            foreach(var society in societies) {
                society.TickProduction(secondsPassed);
                society.TickConsumption(secondsPassed);
            }
        }

        public override ComplexityDefinitionBase GetComplexityDefinitionOfName(string name) {
            return ComplexityDefinitions.Where(definition => definition.name.Equals(name)).FirstOrDefault();
        }

        public override ComplexityLadderBase GetComplexityLadderOfName(string name) {
            return ComplexityLadders.Where(ladder => ladder.name.Equals(name)).FirstOrDefault();
        }

        #endregion

        #endregion

    }

}
