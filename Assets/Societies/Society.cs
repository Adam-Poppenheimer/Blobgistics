using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Blobs;
using Assets.Map;

using Assets.BlobSites;

using UnityCustomUtilities.Extensions;

namespace Assets.Societies {

    [ExecuteInEditMode]
    public class Society : SocietyBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {

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
            get { return _currentComplexity; }
        }
        public void SetCurrentComplexity(ComplexityDefinitionBase value) {
            _currentComplexity = value;
            RefreshAppearance();
            RefreshBlobSitePermissionsAndCapacities();
            DefaultProfile.InsertProfileIntoBlobSite(Location.BlobSite);
        }
        [SerializeField] private ComplexityDefinitionBase _currentComplexity;

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
                    return Math.Max(0f, CurrentComplexity.ComplexityDescentDuration - SecondsOfUnsatisfiedNeeds);
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
            get { return _privateData; }
            set { _privateData = value; }
        }
        [SerializeField, HideInInspector] private SocietyPrivateDataBase _privateData;

        [SerializeField, HideInInspector] private float CurrentProductionTimer  = 0f;
        [SerializeField, HideInInspector] private float CurrentNeedConsumptionTimer = 0f;

        private BlobSitePermissionProfile ConsumptionProfile = new BlobSitePermissionProfile();
        private BlobSitePermissionProfile ProductionProfile = new BlobSitePermissionProfile();
        private BlobSitePermissionProfile DefaultProfile = new BlobSitePermissionProfile();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            if(PrivateData != null && PrivateData.ParentFactory != null) {
                var parentFactory = PrivateData.ParentFactory;
                if(parentFactory.HasSocietyAtLocation(Location) &&
                    parentFactory.GetSocietyAtLocation(Location) != this) {
                    Debug.LogErrorFormat("Duplicate Society detected on MapNode. Removing...");
                    parentFactory.DestroySociety(this);
                }
            }
        }

        private void Start() {
            if(CurrentComplexity != null && Location != null) {
                RefreshBlobSitePermissionsAndCapacities();
                DefaultProfile.InsertProfileIntoBlobSite(Location.BlobSite);
            }
        }

        private void OnValidate() {
            if(CurrentComplexity != null && Location != null) {
                RefreshAppearance();
                RefreshBlobSitePermissionsAndCapacities();
                DefaultProfile.InsertProfileIntoBlobSite(Location.BlobSite);
            }
        }

        private void OnDestroy() {
            if(PrivateData != null && PrivateData.ParentFactory != null) {
                PrivateData.ParentFactory.UnsubscribeSocietyBeingDestroyed(this);
            }
        }

        #endregion

        #region EventSystem interface implementations

        public void OnBeginDrag(PointerEventData eventData) {
            PrivateData.UIControl.PushBeginDragEvent(new SocietyUISummary(this), eventData);
        }

        public void OnDrag(PointerEventData eventData) {
            PrivateData.UIControl.PushDragEvent(new SocietyUISummary(this), eventData);
        }

        public void OnEndDrag(PointerEventData eventData) {
            PrivateData.UIControl.PushEndDragEvent(new SocietyUISummary(this), eventData);
        }

        public void OnPointerClick(PointerEventData eventData) {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerEnterEvent(new SocietyUISummary(this), eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            PrivateData.UIControl.PushPointerExitEvent(new SocietyUISummary(this), eventData);
        }

        public void OnSelect(BaseEventData eventData) {
            PrivateData.UIControl.PushSelectEvent(new SocietyUISummary(this), eventData);
        }

        public void OnDeselect(BaseEventData eventData) {
            PrivateData.UIControl.PushDeselectEvent(new SocietyUISummary(this), eventData);
        }

        #endregion

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
                if(CurrentComplexity.Production[resourceType] == 0) {
                    continue;
                }
                int blobsOfTypeToProduce = CurrentComplexity.Production[resourceType];
                if(hasSatisfiedSomeWants) {
                    ++blobsOfTypeToProduce;
                }
                for(int i = 0; i < blobsOfTypeToProduce; ++i) {
                    if(PrivateData.Location.BlobSite.CanPlaceBlobOfTypeInto(resourceType)) {
                        PrivateData.Location.BlobSite.PlaceBlobInto(PrivateData.BlobFactory.BuildBlob(
                            resourceType, Location.BlobSite.transform.position)
                        );
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
                            var removedBlob = PrivateData.Location.BlobSite.ExtractBlobOfType(resourceType);
                            PrivateData.BlobFactory.DestroyBlob(removedBlob);
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
                SetCurrentComplexity(complexityAbove);
                PrivateData.Location.BlobSite.ClearContents();
                RefreshAppearance();
                RefreshBlobSitePermissionsAndCapacities();
            }else {
                throw new SocietyException("Society cannot ascend its complexity ladder");
            }
        }

        private bool CanDescendComplexityLadder() {
            var descentTransition = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
            return descentTransition != null && !NeedsAreSatisfied && Mathf.Approximately(0f, SecondsUntilComplexityDescent);
        }

        private void DescendComplexityLadder() {
            if(CanDescendComplexityLadder()) {
                var complexityBelow = ActiveComplexityLadder.GetDescentTransition(CurrentComplexity);
                SetCurrentComplexity(complexityBelow);
                PrivateData.Location.BlobSite.ClearContents();
                secondsOfUnsatisfiedNeeds = 0f;
                needsAreSatisfied = true;

                RefreshAppearance();
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
                if(CurrentComplexity.Production[resourceType] > 0) {
                    int valueInDict;
                    capacityDict.TryGetValue(resourceType, out valueInDict);
                    valueInDict += (int)(CurrentComplexity.Production[resourceType] * CurrentComplexity.ProductionCapacityCoefficient);
                    capacityDict[resourceType] = valueInDict;

                    ProductionProfile.SetPlacementPermission(resourceType, true);
                    DefaultProfile.SetPlacementPermission(resourceType, true);
                    DefaultProfile.SetExtractionPermission(resourceType, true);
                }
            }

            //Ascent Cost
            var ascentComplexity = ActiveComplexityLadder.GetAscentTransition(CurrentComplexity);
            if(ascentComplexity != null) {
                var ascentCost = ascentComplexity.CostOfAscent;
                foreach(var resourceType in ascentCost) {
                    if(ascentCost[resourceType] > 0) {
                        int valueInDict;
                        capacityDict.TryGetValue(resourceType, out valueInDict);
                        valueInDict += ascentCost[resourceType];
                        capacityDict[resourceType] = valueInDict;

                        DefaultProfile.SetPlacementPermission(resourceType, true);
                        DefaultProfile.SetExtractionPermission(resourceType, true);
                    }
                    
                }
            }

            //Needs
            foreach(var resourceType in CurrentComplexity.Needs) {
                if(CurrentComplexity.Needs[resourceType] > 0) {
                    int valueInDict;
                    capacityDict.TryGetValue(resourceType, out valueInDict);
                    valueInDict += (int)(CurrentComplexity.Needs[resourceType] * CurrentComplexity.NeedsCapacityCoefficient);
                    capacityDict[resourceType] = valueInDict;

                    ConsumptionProfile.SetPlacementPermission(resourceType, true);
                    ConsumptionProfile.SetExtractionPermission(resourceType, true);
                    DefaultProfile.SetPlacementPermission(resourceType, true);
                    DefaultProfile.SetExtractionPermission(resourceType, false);
                }
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

                    if(wantSummary[resourceType] > 0) {
                        DefaultProfile.SetPlacementPermission(resourceType, true);
                        DefaultProfile.SetExtractionPermission(resourceType, false);

                        ProductionProfile.SetPlacementPermission(resourceType, true);
                        ProductionProfile.SetExtractionPermission(resourceType, true);
                    }
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

        private void RefreshAppearance() {
            var meshRenderer = GetComponent<MeshRenderer>();
            if(meshRenderer != null) {
                meshRenderer.sharedMaterial = CurrentComplexity.MaterialForSociety;
            }
        }

        #endregion

    }

}
