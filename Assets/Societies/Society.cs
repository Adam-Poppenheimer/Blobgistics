using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.BlobEngine;
using UnityCustomUtilities.Extensions;
using UnityEngine;

namespace Assets.Societies {

    public class Society : BlobSiteBase, ISociety {

        #region instance fields and properties

        #region from BlobSiteBase

        public override bool AcceptsExtraction {
            get { return true; }
        }

        public override bool AcceptsPlacement {
            get { return true; }
        }

        public override Vector3 EastTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 NorthTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 SouthTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        public override Vector3 WestTubeConnectionPoint {
            get {
                throw new NotImplementedException();
            }
        }

        protected override BlobPileBase BlobsWithin {
            get {
                if(blobsWithin != null) {
                    return blobsWithin;
                }else {
                    throw new InvalidOperationException("BlobsWithin is not initialized");
                }
            }
        }
        private TypeConstrainedBlobPile blobsWithin = new TypeConstrainedBlobPile(ResourceSummary.Empty);

        protected override BlobPileBase BlobsWithReservedPositions {
            get {
                if(blobsWithReservedPositions != null) {
                    return blobsWithReservedPositions;
                }else {
                    throw new InvalidOperationException("BlobsWithReservedPositions is not initialized");
                }
            }
        }
        private TypeConstrainedBlobPile blobsWithReservedPositions = new TypeConstrainedBlobPile(ResourceSummary.Empty);

        #endregion

        #region from ISociety

        public IComplexityLadder ActiveComplexityLadder {
            get {
                if(_activeComplexityLadder == null) {
                    throw new InvalidOperationException("ActiveComplexityLadder is uninitialized");
                } else {
                    return _activeComplexityLadder;
                }
            }
            private set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _activeComplexityLadder = value;
                }
            }
        }
        private IComplexityLadder _activeComplexityLadder;

        public IComplexityDefinition CurrentComplexity {
            get {
                if(_currentComplexity == null) {
                    throw new InvalidOperationException("CurrentComplexity is uninitialized");
                } else {
                    return _currentComplexity;
                }
            }
            private set {
                if(value == null) {
                    throw new ArgumentNullException("value");
                } else {
                    _currentComplexity = value;
                }
            }
        }
        private IComplexityDefinition _currentComplexity;

        public bool NeedsAreSatisfied {
            get { return _needsAreSatisfied; }
            private set { _needsAreSatisfied = value; }
        }
        private bool _needsAreSatisfied = true;

        public float SecondsOfUnsatisfiedNeeds { get; private set; }

        public float SecondsUntilComplexityDescent {
            get {
                if(!NeedsAreSatisfied) {
                    return CurrentComplexity.ComplexityDescentDuration - SecondsOfUnsatisfiedNeeds;
                }else {
                    return -1;
                }
            }
        }

        #endregion

        public ISocietyPrivateData PrivateData {
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
                    ActiveComplexityLadder = _privateData.ActiveComplexityLadder;
                    CurrentComplexity = _privateData.StartingComplexity;
                    var updatedCapacity = BuildCapacityForComplexity(CurrentComplexity);
                    blobsWithin.Capacity = updatedCapacity;
                    blobsWithReservedPositions.Capacity = updatedCapacity;
                }
            }
        }
        private ISocietyPrivateData _privateData;

        private float CurrentProductionTimer  = 0f;
        private float CurrentNeedConsumptionTimer = 0f;

        #endregion

        #region instance methods

        #region from BlobSiteBase

        protected override bool BlobTypeMeetsExternalExtractionConditions(ResourceType type) {
            if(CurrentComplexity.Needs.GetCountOfResourceType(type) > 0) {
                return false;
            }
            foreach(var wantSummary in CurrentComplexity.Wants) {
                if(wantSummary.GetCountOfResourceType(type) > 0) {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region from ISociety

        private bool CanAscendComplexityLadder() {
            var ascentSummary = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
            return ascentSummary.ComplexityAbove != null && ascentSummary.Cost.IsContainedWithinBlobPile(BlobsWithin);
        }

        public void AscendComplexityLadder() {
            if(CanAscendComplexityLadder()) {
                var ascentSummary = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
                CurrentComplexity = ascentSummary.ComplexityAbove;
                ClearAllBlobs(true, true);

                var updatedCapacity = BuildCapacityForComplexity(CurrentComplexity);

                blobsWithin.Capacity = updatedCapacity;
                blobsWithReservedPositions.Capacity = updatedCapacity;
            }else {
                throw new SocietyException("Society cannot ascend its complexity ladder");
            }
        }

        private bool CanDescendComplexityLadder() {
            var descentTransition = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
            return descentTransition != null && SecondsUntilComplexityDescent <= 0f;
        }

        public void DescendComplexityLadder() {
            if(CanDescendComplexityLadder()) {
                var complexityBelow = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
                CurrentComplexity = complexityBelow;
                ClearAllBlobs(true, true);

                var updatedCapacity = BuildCapacityForComplexity(CurrentComplexity);

                blobsWithin.Capacity = updatedCapacity;
                blobsWithReservedPositions.Capacity = updatedCapacity;
            }else {
                throw new SocietyException("Society cannot descend its ComplexityLadder");
            }
        }

        public IReadOnlyDictionary<ResourceType, int> GetResourcesUntilSocietyAscent() {
            throw new NotImplementedException();
        }

        public void TickConsumption(float secondsPassed) {
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
                    if(CanExtractBlobOfType_Internal(resourceType)) {
                        var removedBlob = ExtractBlobOfType_Internal(resourceType);
                        PrivateData.BlobFactory.DestroyBlob(removedBlob);
                    }else {
                        needsSatisfiedThisCycle = false;
                        break;
                    }
                }
            }

            return needsSatisfiedThisCycle;
        }

        public void TickProduction(float secondsPassed) {
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
                    if(CanPlaceBlobOfTypeInto(resourceType)) {
                        PlaceBlobInto(PrivateData.BlobFactory.BuildBlob(resourceType));
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
                }else if(blobsWithin.ContainsResourceSummary(wantSummary)) {
                    foreach(var resourceType in wantSummary) {
                        for(int i = 0; i < wantSummary[resourceType]; ++i) {
                            ExtractBlobOfType_Internal(resourceType);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion

        private ResourceSummary BuildCapacityForComplexity(IComplexityDefinition complexity) {
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
                var ascentCost = ascentComplexity.Cost;
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
