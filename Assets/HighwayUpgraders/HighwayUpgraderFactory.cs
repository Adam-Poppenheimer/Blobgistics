using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.BlobSites;
using Assets.Highways;
using Assets.Core;

namespace Assets.HighwayUpgraders {

    public class HighwayUpgraderFactory : HighwayUpgraderFactoryBase {

        #region instance fields and properties

        public UIControlBase UIControl {
            get { return _uiControl; }
            set { _uiControl = value; }
        }
        [SerializeField] private UIControlBase _uiControl;

        [SerializeField] private List<BlobHighwayProfile> ChainOfProfiles = new List<BlobHighwayProfile>();

        [SerializeField, HideInInspector] private List<HighwayUpgraderBase> InstantiatedUpgraders =
            new List<HighwayUpgraderBase>();

        [SerializeField] private GameObject HighwayUpgraderPrefab;

        #endregion

        #region instance methods

        #region from HighwayUpgraderFactoryBase

        public override HighwayUpgraderBase GetHighwayUpgraderOfID(int id) {
            return InstantiatedUpgraders.Find(upgrader => upgrader.ID == id);
        }

        public override bool HasUpgraderTargetingHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            return InstantiatedUpgraders.Exists(upgrader => upgrader.TargetedHighway == highway);
        }

        public override HighwayUpgraderBase GetUpgraderTargetingHighway(BlobHighwayBase highway) {
            if(highway == null) {
                throw new ArgumentNullException("highway");
            }
            var upgraderOn = InstantiatedUpgraders.Find(upgrader => upgrader.TargetedHighway == highway);
            if(upgraderOn != null) {
                return upgraderOn;
            }else {
                throw new HighwayUpgraderException("There exists no HighwayUpgrader with the specified TargetedHighway");
            }
        }

        public override HighwayUpgraderBase BuildHighwayUpgrader(BlobHighwayBase targetedHighway, BlobSiteBase underlyingSite,
            BlobHighwayProfile profileToInsert) {
            if(targetedHighway == null) {
                throw new ArgumentNullException("targetedHighway");
            }else if(underlyingSite == null) {
                throw new ArgumentNullException("underlyingSite");
            }else if(HasUpgraderTargetingHighway(targetedHighway)) {
                throw new HighwayUpgraderException("There already exists an upgrader targeting the specified highway");
            }

            GameObject hostingObject;
            HighwayUpgrader newUpgrader;
            if(HighwayUpgraderPrefab != null) {
                hostingObject = Instantiate(HighwayUpgraderPrefab);
                newUpgrader = hostingObject.GetComponent<HighwayUpgrader>();
                if(newUpgrader == null) {
                    throw new HighwayUpgraderException("The HighwayUpgrader prefab lacks a HighwayUpgrader component");
                }
            }else {
                hostingObject = new GameObject();
                newUpgrader = hostingObject.AddComponent<HighwayUpgrader>();
            }

            var privateData = hostingObject.AddComponent<HighwayUpgraderPrivateData>();
            privateData.SetSourceFactory(this);
            privateData.SetUIControl(UIControl);
            privateData.SetTargetedHighway(targetedHighway);
            privateData.SetUnderlyingSite(underlyingSite);
            privateData.SetProfileToInsert(profileToInsert);

            newUpgrader.PrivateData = privateData;
            newUpgrader.transform.SetParent(targetedHighway.transform, false);

            newUpgrader.name = "HighwayUpgrader on " + targetedHighway.name;

            InstantiatedUpgraders.Add(newUpgrader);

            return newUpgrader;
        }

        public override void DestroyHighwayUpgrader(HighwayUpgraderBase highwayUpgrader) {
            if(highwayUpgrader == null) {
                throw new ArgumentNullException("highwayUpgrader");
            }
            InstantiatedUpgraders.Remove(highwayUpgrader);
            DestroyImmediate(highwayUpgrader.gameObject);
        }

        public override BlobHighwayProfile GetNextProfileInUpgradeChain(BlobHighwayProfile currentProfile) {
            if(currentProfile == null) {
                throw new ArgumentNullException("currentProfile");
            }
            var indexOfCurrent = ChainOfProfiles.IndexOf(currentProfile);
            if(indexOfCurrent < 0) {
                return ChainOfProfiles.FirstOrDefault();
            }else if(indexOfCurrent == ChainOfProfiles.Count - 1) {
                return null;
            }else {
                return ChainOfProfiles[(indexOfCurrent + 1) % ChainOfProfiles.Count];
            }
        }

        #endregion

        #endregion

    }

}
