using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;

using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    public class Society : SocietyBase {

        #region instance fields and properties

        #region from SocietyBase

        public override int ID {
            get { return GetInstanceID(); }
        }

        public override ComplexityLadderBase ActiveComplexityLadder {
            get {
                return PrivateData.ActiveComplexityLadder;
            }
        }
        [SerializeField, HideInInspector] private ComplexityLadderBase _activeComplexityLadder;

        public override ComplexityDefinitionBase CurrentComplexity {
            get { return currentComplexity; }
        }
        [SerializeField] private ComplexityDefinitionBase currentComplexity;

        public override bool NeedsAreSatisfied {
            get { return needsAreSatisfied; }
        }
        [SerializeField, HideInInspector] private bool needsAreSatisfied = true;

        public override float SecondsOfUnsatisfiedNeeds {
            get { return secondsOfUnsatisfiedNeeds; }
        }
        [SerializeField, HideInInspector] private float secondsOfUnsatisfiedNeeds;

        public override float SecondsUntilComplexityDescent {
            get {
                if(!NeedsAreSatisfied) {
                    return CurrentComplexity.ComplexityDescentDuration - SecondsOfUnsatisfiedNeeds;
                }else {
                    return -1;
                }
            }
        }

        public override MapNodeBase Location {
            get { return PrivateData.Location; }
        }

        #endregion

        public SocietyPrivateDataBase PrivateData {
            get {
                if(_privateData == null) {
                    throw new InvalidOperationException("PrivateData is uninitialized");
                } else {
                    return _privateData;
                }
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _privateData = value;
                    currentComplexity = _privateData.ActiveComplexityLadder.GetStartingComplexity();
                    RefreshBlobSitePermissionsAndCapacities();
                    DefaultProfile.InsertProfileIntoBlobSite(Location.BlobSite);
                }
            }
        }
        [SerializeField, HideInInspector] private SocietyPrivateDataBase _privateData;

        [SerializeField, HideInInspector] private float CurrentProductionTimer  = 0f;
        [SerializeField, HideInInspector] private float CurrentNeedConsumptionTimer = 0f;

        private BlobSitePermissionProfile ConsumptionProfile = new BlobSitePermissionProfile();
        private BlobSitePermissionProfile ProductionProfile = new BlobSitePermissionProfile();
        private BlobSitePermissionProfile DefaultProfile = new BlobSitePermissionProfile();

        #endregion

        #region instance methods

        #region from SocietyBase

        public override IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent() {
            throw new NotImplementedException();
        }

        public override void TickConsumption(float secondsPassed) {
            ConsumptionProfile.InsertProfileIntoBlobSite(Location.BlobSite);

            TickNeedsConsumption(secondsPassed);
            if(CanAscendComplexityLadder()) {
                AscendComplexityLadder();
            }else if(CanDescendComplexityLadder()) {
                DescendComplexityLadder();
            }

            DefaultProfile.InsertProfileIntoBlobSite(Location.BlobSite);
        }

        private void TickNeedsConsumption(float secondsPassed) {
            CurrentNeedConsumptionTimer += secondsPassed;
            while(CurrentNeedConsumptionTimer >= CurrentComplexity.SecondsToFullyConsumeNeeds) {
                var needsSatisfiedThisCycle = PerformNeedsConsumptionCycle();
                if(needsSatisfiedThisCycle) {
                    needsAreSatisfied = true;
                    secondsOfUnsatisfiedNeeds = 0f;
                }else if(NeedsAreSatisfied){
                    needsAreSatisfied = false;
                    secondsOfUnsatisfiedNeeds = 0f;
                }else {
                    secondsOfUnsatisfiedNeeds += CurrentComplexity.SecondsToFullyConsumeNeeds;
                }
                CurrentNeedConsumptionTimer -= CurrentComplexity.SecondsToFullyConsumeNeeds;
            }

            if(!NeedsAreSatisfied) {
                secondsOfUnsatisfiedNeeds += CurrentNeedConsumptionTimer;
            }
        }

        private bool PerformNeedsConsumptionCycle() {
            bool needsSatisfiedThisCycle = true;

            foreach(var resourceType in CurrentComplexity.Needs) {
                for(int i = 0; i < CurrentComplexity.Needs[resourceType]; ++i) {
                    if(PrivateData.Location.BlobSite.CanExtractBlobOfType(resourceType)) {
                        var removedBlob = PrivateData.Location.BlobSite.ExtractBlobOfType(resourceType);
                        PrivateData.BlobFactory.DestroyBlob(removedBlob);
                    }else {
                        needsSatisfiedThisCycle = false;
                        break;
                    }
                }
            }

            return needsSatisfiedThisCycle;
        }

        public override void TickProduction(float secondsPassed) {
            ProductionProfile.InsertProfileIntoBlobSite(Location.BlobSite);

            CurrentProductionTimer += secondsPassed;
            while(CurrentProductionTimer >= CurrentComplexity.SecondsToPerformFullProduction) {
                PerformProductionCycle();
                CurrentProductionTimer -= CurrentComplexity.SecondsToPerformFullProduction;
            }

            DefaultProfile.InsertProfileIntoBlobSite(Location.BlobSite);
        }

        private void PerformProductionCycle() {
            var hasSatisfiedSomeWants = PerformWantsConsumptionCycle();
            foreach(var resourceType in CurrentComplexity.Production) {
                int blobsOfTypeToProduce = CurrentComplexity.Production[resourceType];
                if(hasSatisfiedSomeWants) {
                    ++blobsOfTypeToProduce;
                }
                for(int i = 0; i < blobsOfTypeToProduce; ++i) {
                    if(PrivateData.Location.BlobSite.CanPlaceBlobOfTypeInto(resourceType)) {
                        PrivateData.Location.BlobSite.PlaceBlobInto(PrivateData.BlobFactory.BuildBlob(resourceType));
                    }else {
                        break;
                    }
                }
            }
        }

        private bool PerformWantsConsumptionCycle() {
            foreach(var wantSummary in CurrentComplexity.Wants) {
                if(wantSummary.GetTotalResourceCount() == 0) {
                    continue;
                }else if(wantSummary.IsContainedWithinBlobSite(PrivateData.Location.BlobSite)) {
                    foreach(var resourceType in wantSummary) {
                        for(int i = 0; i < wantSummary[resourceType]; ++i) {
                            PrivateData.Location.BlobSite.ExtractBlobOfType(resourceType);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion

        private bool CanAscendComplexityLadder() {
            var complexityAbove = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
            return complexityAbove != null && complexityAbove.CostOfAscent.IsContainedWithinBlobSite(PrivateData.Location.BlobSite);
        }

        private void AscendComplexityLadder() {
            if(CanAscendComplexityLadder()) {
                var complexityAbove = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
                currentComplexity = complexityAbove;
                PrivateData.Location.BlobSite.ClearContents();

                RefreshBlobSitePermissionsAndCapacities();
            }else {
                throw new SocietyException("Society cannot ascend its complexity ladder");
            }
        }

        private bool CanDescendComplexityLadder() {
            var descentTransition = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
            return descentTransition != null && SecondsUntilComplexityDescent <= 0f;
        }

        private void DescendComplexityLadder() {
            if(CanDescendComplexityLadder()) {
                var complexityBelow = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
                currentComplexity = complexityBelow;
                PrivateData.Location.BlobSite.ClearContents();

                RefreshBlobSitePermissionsAndCapacities();
            }else {
                throw new SocietyException("Society cannot descend its ComplexityLadder");
            }
        }

        private void RefreshBlobSitePermissionsAndCapacities() {
            ConsumptionProfile.Clear();
            ProductionProfile.Clear();
            DefaultProfile.Clear();

            var capacityDict = new Dictionary<ResourceType, int>();

            //Production
            foreach(var resourceType in CurrentComplexity.Production) {
                int valueInDict;
                capacityDict.TryGetValue(resourceType, out valueInDict);
                valueInDict += (int)(CurrentComplexity.Production[resourceType] * CurrentComplexity.ProductionCapacityCoefficient);
                capacityDict[resourceType] = valueInDict;

                ProductionProfile.SetPlacementPermission(resourceType, true);
                DefaultProfile.SetPlacementPermission(resourceType, true);
                DefaultProfile.SetExtractionPermission(resourceType, true);
            }

            //Ascent Cost
            var ascentComplexity = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
            if(ascentComplexity != null) {
                var ascentCost = ascentComplexity.CostOfAscent ?? ResourceSummary.Empty;
                foreach(var resourceType in ascentCost) {
                    int valueInDict;
                    capacityDict.TryGetValue(resourceType, out valueInDict);
                    valueInDict += ascentCost[resourceType];
                    capacityDict[resourceType] = valueInDict;

                    DefaultProfile.SetPlacementPermission(resourceType, true);
                    DefaultProfile.SetExtractionPermission(resourceType, true);
                }
            }

            //Needs
            foreach(var resourceType in CurrentComplexity.Needs) {
                int valueInDict;
                capacityDict.TryGetValue(resourceType, out valueInDict);
                valueInDict += (int)(CurrentComplexity.Needs[resourceType] * CurrentComplexity.NeedsCapacityCoefficient);
                capacityDict[resourceType] = valueInDict;

                ConsumptionProfile.SetPlacementPermission(resourceType, true);
                ConsumptionProfile.SetExtractionPermission(resourceType, true);
                DefaultProfile.SetPlacementPermission(resourceType, true);
                DefaultProfile.SetExtractionPermission(resourceType, false);
            }

            //Wants
            Dictionary<ResourceType, int> maximumWantsByResource = new Dictionary<ResourceType, int>();
            foreach(var wantSummary in CurrentComplexity.Wants) {
                foreach(var resourceType in wantSummary) {
                    int currentBiggestForResourceType;
                    maximumWantsByResource.TryGetValue(resourceType, out currentBiggestForResourceType);
                    maximumWantsByResource[resourceType] = Math.Max(
                        currentBiggestForResourceType,
                        (int)(wantSummary[resourceType] * CurrentComplexity.WantsCapacityCoefficient)
                    );
                    
                    DefaultProfile.SetPlacementPermission(resourceType, true);
                    DefaultProfile.SetExtractionPermission(resourceType, false);

                    ProductionProfile.SetPlacementPermission(resourceType, true);
                    ProductionProfile.SetExtractionPermission(resourceType, true);
                }
            }

            foreach(var wantSummaryKVPair in maximumWantsByResource) {
                int valueInDict;
                capacityDict.TryGetValue(wantSummaryKVPair.Key, out valueInDict);
                valueInDict += wantSummaryKVPair.Value;
                capacityDict[wantSummaryKVPair.Key] = valueInDict;
            }

            int totalCapacity = 0;
            foreach(var capacityKVPair in capacityDict) {
                totalCapacity += capacityKVPair.Value;
                ProductionProfile.SetCapacity (capacityKVPair.Key, capacityKVPair.Value);
                ConsumptionProfile.SetCapacity(capacityKVPair.Key, capacityKVPair.Value);
                DefaultProfile.SetCapacity    (capacityKVPair.Key, capacityKVPair.Value);
            }

            ProductionProfile.SetTotalCapacity(totalCapacity);
            ConsumptionProfile.SetTotalCapacity(totalCapacity);
            DefaultProfile.SetTotalCapacity(totalCapacity);
        }

        #endregion

    }

}
