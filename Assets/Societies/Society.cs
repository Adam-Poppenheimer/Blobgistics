using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Blobs;
using UnityCustomUtilities.Extensions;
using UnityEngine;

namespace Assets.Societies {

    public class Society : SocietyBase {

        #region instance fields and properties

        #region from SocietyBase

        public override ComplexityLadderBase ActiveComplexityLadder {
            get {
                return PrivateData.ActiveComplexityLadder;
            }
        }
        [SerializeField] private ComplexityLadderBase _activeComplexityLadder;

        public override ComplexityDefinitionBase CurrentComplexity {
            get { return currentComplexity; }
        }
        [SerializeField] private ComplexityDefinitionBase currentComplexity;

        public override bool NeedsAreSatisfied {
            get { return needsAreSatisfied; }
        }
        [SerializeField] private bool needsAreSatisfied = true;

        public override float SecondsOfUnsatisfiedNeeds {
            get { return secondsOfUnsatisfiedNeeds; }
        }
        [SerializeField] private float secondsOfUnsatisfiedNeeds;

        public override float SecondsUntilComplexityDescent {
            get {
                if(!NeedsAreSatisfied) {
                    return CurrentComplexity.ComplexityDescentDuration - SecondsOfUnsatisfiedNeeds;
                }else {
                    return -1;
                }
            }
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
                    var updatedCapacity = BuildCapacityForComplexity(CurrentComplexity);
                    PrivateData.Location.BlobSite.SetPermissionsAndCapacity(updatedCapacity);
                }
            }
        }
        private SocietyPrivateDataBase _privateData;

        private float CurrentProductionTimer  = 0f;
        private float CurrentNeedConsumptionTimer = 0f;

        #endregion

        #region instance methods

        #region from ISociety

        private bool CanAscendComplexityLadder() {
            var complexityAbove = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
            return complexityAbove != null && complexityAbove.CostOfAscent.IsContainedWithinBlobSite(PrivateData.Location);
        }

        public override void AscendComplexityLadder() {
            if(CanAscendComplexityLadder()) {
                var complexityAbove = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
                CurrentComplexity = complexityAbove;
                PrivateData.Location.Clear();

                var updatedCapacity = BuildCapacityForComplexity(CurrentComplexity);

                PrivateData.Location.SetPermissionsAndCapacity(updatedCapacity);
            }else {
                throw new SocietyException("Society cannot ascend its complexity ladder");
            }
        }

        private bool CanDescendComplexityLadder() {
            var descentTransition = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
            return descentTransition != null && SecondsUntilComplexityDescent <= 0f;
        }

        public override void DescendComplexityLadder() {
            if(CanDescendComplexityLadder()) {
                var complexityBelow = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
                CurrentComplexity = complexityBelow;
                PrivateData.Location.Clear();

                var updatedCapacity = BuildCapacityForComplexity(CurrentComplexity);

                PrivateData.Location.SetPermissionsAndCapacity(updatedCapacity);
            }else {
                throw new SocietyException("Society cannot descend its ComplexityLadder");
            }
        }

        public override IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent() {
            throw new NotImplementedException();
        }

        public override void TickConsumption(float secondsPassed) {
            TickNeedsConsumption(secondsPassed);
            if(CanAscendComplexityLadder()) {
                AscendComplexityLadder();
            }else if(CanDescendComplexityLadder()) {
                DescendComplexityLadder();
            }
        }

        private void TickNeedsConsumption(float secondsPassed) {
            CurrentNeedConsumptionTimer += secondsPassed;
            while(CurrentNeedConsumptionTimer >= CurrentComplexity.SecondsToFullyConsumeNeeds) {
                var needsSatisfiedThisCycle = PerformNeedsConsumptionCycle();
                if(needsSatisfiedThisCycle) {
                    NeedsAreSatisfied = true;
                    SecondsOfUnsatisfiedNeeds = 0f;
                }else if(NeedsAreSatisfied){
                    NeedsAreSatisfied = false;
                    SecondsOfUnsatisfiedNeeds = 0f;
                }else {
                    SecondsOfUnsatisfiedNeeds += CurrentComplexity.SecondsToFullyConsumeNeeds;
                }
                CurrentNeedConsumptionTimer -= CurrentComplexity.SecondsToFullyConsumeNeeds;
            }

            if(!NeedsAreSatisfied) {
                SecondsOfUnsatisfiedNeeds += CurrentNeedConsumptionTimer;
            }
        }

        private bool PerformNeedsConsumptionCycle() {
            bool needsSatisfiedThisCycle = true;

            foreach(var resourceType in CurrentComplexity.Needs) {
                for(int i = 0; i < CurrentComplexity.Needs[resourceType]; ++i) {
                    if(PrivateData.Location.CanExtractBlobOfType(resourceType)) {
                        var removedBlob = PrivateData.Location.ExtractBlobOfType(resourceType);
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
            CurrentProductionTimer += secondsPassed;
            if(CurrentProductionTimer >= CurrentComplexity.SecondsToPerformFullProduction) {
                PerformProductionCycle();
                CurrentProductionTimer -= CurrentComplexity.SecondsToPerformFullProduction;
            }
        }

        private void PerformProductionCycle() {
            var hasSatisfiedSomeWants = PerformWantsConsumptionCycle();
            foreach(var resourceType in CurrentComplexity.Production) {
                int blobsOfTypeToProduce = CurrentComplexity.Production[resourceType];
                if(hasSatisfiedSomeWants) {
                    ++blobsOfTypeToProduce;
                }
                for(int i = 0; i < blobsOfTypeToProduce; ++i) {
                    if(PrivateData.Location.CanPlaceBlobOfTypeInto(resourceType)) {
                        PrivateData.Location.PlaceBlobInto(PrivateData.BlobFactory.BuildBlob(resourceType));
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
                }else if(wantSummary.IsContainedWithinBlobSite(PrivateData.Location)) {
                    foreach(var resourceType in wantSummary) {
                        for(int i = 0; i < wantSummary[resourceType]; ++i) {
                            PrivateData.Location.ExtractBlobOfType(resourceType);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion

        private ResourceSummary BuildCapacityForComplexity(ComplexityDefinitionBase complexity) {
            var capacityDict = new Dictionary<ResourceType, int>();

            //Production
            foreach(var resourceType in complexity.Production) {
                int valueInDict;
                capacityDict.TryGetValue(resourceType, out valueInDict);
                valueInDict += (int)(complexity.Production[resourceType] * complexity.ProductionCapacityCoefficient);
                capacityDict[resourceType] = valueInDict;
            }

            //Needs
            foreach(var resourceType in complexity.Needs) {
                int valueInDict;
                capacityDict.TryGetValue(resourceType, out valueInDict);
                valueInDict += (int)(complexity.Needs[resourceType] * complexity.NeedsCapacityCoefficient);
                capacityDict[resourceType] = valueInDict;
            }

            //Wants
            Dictionary<ResourceType, int> maximumWantsByResource = new Dictionary<ResourceType, int>();
            foreach(var wantSummary in complexity.Wants) {
                foreach(var resourceType in wantSummary) {
                    int currentBiggestForResourceType;
                    maximumWantsByResource.TryGetValue(resourceType, out currentBiggestForResourceType);
                    maximumWantsByResource[resourceType] = Math.Max(
                        currentBiggestForResourceType,
                        (int)(wantSummary[resourceType] * complexity.WantsCapacityCoefficient)
                    );
                }
            }

            foreach(var wantSummaryKVPair in maximumWantsByResource) {
                int valueInDict;
                capacityDict.TryGetValue(wantSummaryKVPair.Key, out valueInDict);
                valueInDict += wantSummaryKVPair.Value;
                capacityDict[wantSummaryKVPair.Key] = valueInDict;
            }

            //Ascent Cost
            var ascentComplexity = ActiveComplexityLadder.GetAscentTransition(complexity);
            if(ascentComplexity != null) {
                var ascentCost = ascentComplexity.CostOfAscent;
                foreach(var resourceType in ascentCost) {
                    int valueInDict;
                    capacityDict.TryGetValue(resourceType, out valueInDict);
                    valueInDict += ascentCost[resourceType];
                    capacityDict[resourceType] = valueInDict;
                }
            }

            return new ResourceSummary(capacityDict);
        }

        #endregion

    }

}
