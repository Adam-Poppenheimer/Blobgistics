using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

using Assets.Map;
using Assets.Blobs;
using Assets.Core;

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

        [SerializeField] private GameObject SocietyPrefab;

        [SerializeField, HideInInspector] private List<SocietyBase> InstantiatedSocieties = new List<SocietyBase>();

        #endregion

        #region instance methods

        #region from SocietyFactoryBase

        public override SocietyBase GetSocietyOfID(int id) {
            return InstantiatedSocieties.Find(society => society.ID == id);
        }

        public override SocietyBase GetSocietyAtLocation(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            var retval = InstantiatedSocieties.Find(society => society.Location == location);
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
            return InstantiatedSocieties.Exists(society => society.Location == location);
        }

        public override bool CanConstructSocietyAt(MapNodeBase location) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }
            return !HasSocietyAtLocation(location);
        }

        public override SocietyBase ConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder) {
            if(location == null) {
                throw new ArgumentNullException("location");
            }else if(ladder == null) {
                throw new ArgumentNullException("ladder");
            }else if(!CanConstructSocietyAt(location)) {
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
            newSociety.transform.SetParent(location.transform, false);

            InstantiatedSocieties.Add(newSociety);
            return newSociety;
        }

        public override void DestroySociety(SocietyBase society) {
            UnsubscribeSocietyBeingDestroyed(society);
            DestroyImmediate(society.gameObject);
        }

        public override void UnsubscribeSocietyBeingDestroyed(SocietyBase societyBeingDestroyed) {
            if(societyBeingDestroyed == null) {
                throw new ArgumentNullException("societyBeingDestroyed");
            }
            InstantiatedSocieties.Remove(societyBeingDestroyed);
        }

        public override void TickSocieties(float secondsPassed) {
            foreach(var society in InstantiatedSocieties) {
                society.TickProduction(secondsPassed);
                society.TickConsumption(secondsPassed);
            }
        }

        #endregion

        #endregion
        
    }

}
