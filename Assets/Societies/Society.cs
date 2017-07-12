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
                if(PrivateData != null) {
                    return PrivateData.ActiveComplexityLadder;
                }else {
                    return null;
                }
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
            RaiseCurrentComplexityChanged(_currentComplexity);
        }
        [SerializeField, HideInInspector] private ComplexityDefinitionBase _currentComplexity;

        public override bool NeedsAreSatisfied {
            get { return _needsAreSatisfied; }
            protected set {
                _needsAreSatisfied = value;
                if(UnsatisfiedNeedsIcon == null) {
                    return;
                }else if(_needsAreSatisfied) {
                    UnsatisfiedNeedsIcon.gameObject.SetActive(false);
                }else {
                    UnsatisfiedNeedsIcon.gameObject.SetActive(true);
                }
            }
        }
        [SerializeField, HideInInspector] private bool _needsAreSatisfied = true;

        public override float SecondsOfUnsatisfiedNeeds {
            get { return _secondsOfUnsatisfiedNeeds; }
            set { _secondsOfUnsatisfiedNeeds = value; }
        }
        [SerializeField, HideInInspector] private float _secondsOfUnsatisfiedNeeds;

        public override float SecondsUntilComplexityDescent {
            get {
                if(!NeedsAreSatisfied) {
                    return Math.Max(0f, CurrentComplexity.ComplexityDescentDuration - SecondsOfUnsatisfiedNeeds);
                }else {
                    return -1;
                }
            }
        }

        public override bool AscensionIsPermitted {
            get { return _ascensionIsPermitted; }
            set {
                _ascensionIsPermitted = value;
                RefreshBlobSitePermissionsAndCapacities();
            }
        }
        private bool _ascensionIsPermitted = false;

        public override MapNodeBase Location {
            get { return PrivateData.Location; }
        }

        #endregion

        public SocietyPrivateDataBase PrivateData {
            get { return _privateData; }
            set { _privateData = value; }
        }
        [SerializeField, HideInInspector] private SocietyPrivateDataBase _privateData;

        [SerializeField] private SpriteRenderer ForegroundRenderer;
        [SerializeField] private SpriteRenderer BackgroundRenderer;

        [SerializeField] private RectTransform UnsatisfiedNeedsIcon;

        [SerializeField] private AudioSource ComplexificationAudio;
        [SerializeField] private AudioSource DecomplexificationAudio;

        [SerializeField, HideInInspector] private float CurrentProductionTimer  = 0f;
        [SerializeField, HideInInspector] private float CurrentNeedConsumptionTimer = 0f;

        private BlobSitePermissionProfile ConsumptionProfile = new BlobSitePermissionProfile();
        private BlobSitePermissionProfile ProductionProfile = new BlobSitePermissionProfile();
        private BlobSitePermissionProfile DefaultProfile = new BlobSitePermissionProfile();

        private Dictionary<ComplexityDefinitionBase, bool> AscensionPermissionsForComplexity =
            new Dictionary<ComplexityDefinitionBase, bool>();

        #endregion

        #region instance methods

        #region Unity event methods

        #if UNITY_EDITOR

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

        #endif

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
            if(PrivateData != null) {
                if(PrivateData.ParentFactory != null) {
                    PrivateData.ParentFactory.UnsubscribeSociety(this);
                }
                if(PrivateData.UIControl != null) {
                    PrivateData.UIControl.PushObjectDestroyedEvent(new SocietyUISummary(this));
                }
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
                    NeedsAreSatisfied = true;
                    SecondsOfUnsatisfiedNeeds = 0f;
                    RaiseNeedsAreSatisfiedChanged(NeedsAreSatisfied);
                }else if(NeedsAreSatisfied){
                    NeedsAreSatisfied = false;
                    SecondsOfUnsatisfiedNeeds = 0;
                    RaiseNeedsAreSatisfiedChanged(NeedsAreSatisfied);
                }
                CurrentNeedConsumptionTimer -= CurrentComplexity.SecondsToFullyConsumeNeeds;
            }

            if(!NeedsAreSatisfied) {
                SecondsOfUnsatisfiedNeeds += secondsPassed;
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

        public override bool GetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity) {
            bool retval = false;
            AscensionPermissionsForComplexity.TryGetValue(complexity, out retval);
            return retval;
        }

        public override void SetAscensionPermissionForComplexity(ComplexityDefinitionBase complexity, bool isPermitted) {
            AscensionPermissionsForComplexity[complexity] = isPermitted;
        }

        #endregion

        private ComplexityDefinitionBase GetBestAscentCandidate() {
            foreach(var complexity in ActiveComplexityLadder.GetAscentTransitions(CurrentComplexity)) {
                if( 
                    complexity.PermittedTerrains.Contains(Location.Terrain) &&
                    complexity.CostToAscendInto.IsContainedWithinBlobSite(PrivateData.Location.BlobSite) &&
                    GetAscensionPermissionForComplexity(complexity)
                ){
                    return complexity;
                }
            }
            return null;
        }

        private bool HasResourcesInCommon(IntPerResourceDictionary resourcesOne, IntPerResourceDictionary resourcesTwo) {
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                if(resourcesOne[resourceType] != 0 && resourcesTwo[resourceType] != 0) {
                    return true;
                }
            }
            return false;
        }

        private ComplexityDefinitionBase GetBestDescentCandidate() {
            foreach(var complexity in ActiveComplexityLadder.GetDescentTransitions(CurrentComplexity)) {
                if(complexity.PermittedTerrains.Contains(PrivateData.Location.Terrain)) {
                    return complexity;
                }
            }
            return null;
        }

        private bool CanAscendComplexityLadder() {
            var complexitiesAbove = ActiveComplexityLadder.GetAscentTransitions(CurrentComplexity);
            if(AscensionIsPermitted && SecondsOfUnsatisfiedNeeds <= 0) {
                return GetBestAscentCandidate() != null;
            }
            return false;
        }

        private void AscendComplexityLadder() {
            if(CanAscendComplexityLadder()) {
                SetCurrentComplexity(GetBestAscentCandidate());
                PrivateData.Location.BlobSite.ClearContents();
                RefreshAppearance();
                RefreshBlobSitePermissionsAndCapacities();
                if(ComplexificationAudio != null) {
                    ComplexificationAudio.Play();
                }
            }else {
                throw new SocietyException("Society cannot ascend its complexity ladder");
            }
        }

        private bool CanDescendComplexityLadder() {
            var descentCandidate = GetBestDescentCandidate();
            return !NeedsAreSatisfied && Mathf.Approximately(0f, SecondsUntilComplexityDescent);
        }

        private void DescendComplexityLadder() {
            if(CanDescendComplexityLadder()) {
                var descentCandidate = GetBestDescentCandidate();

                if(descentCandidate != null) {
                    SetCurrentComplexity(GetBestDescentCandidate());

                    PrivateData.Location.BlobSite.ClearContents();
                    SecondsOfUnsatisfiedNeeds = 0f;
                    NeedsAreSatisfied = true;
                    RaiseNeedsAreSatisfiedChanged(NeedsAreSatisfied);

                    RefreshAppearance();
                    RefreshBlobSitePermissionsAndCapacities();

                    if(DecomplexificationAudio != null) {
                        DecomplexificationAudio.Play();
                    }
                }else {
                    PrivateData.ParentFactory.DestroySociety(this);
                }
            }else {
                throw new SocietyException("Society cannot descend its ComplexityLadder");
            }
        }

        private void RefreshBlobSitePermissionsAndCapacities() {
            Location.BlobSite.ClearPermissionsAndCapacity();
            ConsumptionProfile.Clear();
            ProductionProfile.Clear();
            DefaultProfile.Clear();

            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {

                ProductionProfile.SetPlacementPermission(resourceType, true);
                ProductionProfile.SetExtractionPermission(resourceType, true);
                ConsumptionProfile.SetExtractionPermission(resourceType, true);

                if(AscensionIsPermitted && DoesSomeValidAscensionRequireResource(resourceType)) {
                    DefaultProfile.SetPlacementPermission(resourceType, true);
                    DefaultProfile.SetExtractionPermission(resourceType, false);

                    int capacityForResource = GetGreatestAscensionStockpileOfResource(resourceType);

                    DefaultProfile.SetCapacity(resourceType, capacityForResource);
                    DefaultProfile.TotalCapacity += capacityForResource;
                    ProductionProfile.SetCapacity(resourceType, capacityForResource);
                    ProductionProfile.TotalCapacity += capacityForResource;

                }else if(DoesNeedOrSomeWantRequireResource(resourceType)){
                    DefaultProfile.SetPlacementPermission(resourceType, true);
                    DefaultProfile.SetExtractionPermission(resourceType, false);

                    int capacityForResource = GetGreatestNeedOrWantStockpileOfResource(resourceType);

                    DefaultProfile.SetCapacity(resourceType, capacityForResource);
                    DefaultProfile.TotalCapacity += capacityForResource;
                    ProductionProfile.SetCapacity(resourceType, capacityForResource);
                    ProductionProfile.TotalCapacity += capacityForResource;

                }else if(CurrentComplexity.Production[resourceType] > 0) {
                    DefaultProfile.SetPlacementPermission(resourceType, false);
                    DefaultProfile.SetExtractionPermission(resourceType, true);

                    int capacityForResource = CurrentComplexity.Production[resourceType] * (int)CurrentComplexity.ProductionCapacityCoefficient;

                    DefaultProfile.SetCapacity(resourceType, capacityForResource);
                    DefaultProfile.TotalCapacity += capacityForResource;

                    ProductionProfile.SetCapacity(resourceType, capacityForResource);
                    ProductionProfile.TotalCapacity += capacityForResource;
                }

            }

            DefaultProfile.InsertProfileIntoBlobSite(Location.BlobSite);
        }

        private bool DoesSomeValidAscensionRequireResource(ResourceType resourceType) {
            foreach(var ascension in ActiveComplexityLadder.GetAscentTransitions(CurrentComplexity)) {
                if(ascension.PermittedTerrains.Contains(Location.Terrain) && ascension.CostToAscendInto[resourceType] > 0) {
                    return true;
                }
            }
            return false;
        }

        private int GetGreatestAscensionStockpileOfResource(ResourceType resourceType) {
            int retval = 0;

            foreach(var ascension in ActiveComplexityLadder.GetAscentTransitions(CurrentComplexity)) {
                if(ascension.PermittedTerrains.Contains(Location.Terrain)) {
                    retval = Math.Max(retval, ascension.CostToAscendInto[resourceType]);
                }
            }

            return retval;
        }

        private bool DoesNeedOrSomeWantRequireResource(ResourceType resourceType) {
            if(CurrentComplexity.Needs[resourceType] > 0) {
                return true;
            }else {
                foreach(var want in CurrentComplexity.Wants) {
                    if(want[resourceType] > 0) {
                        return true;
                    }
                }
                return false;
            }
        }

        private int GetGreatestNeedOrWantStockpileOfResource(ResourceType resourceType) {
            int retval = CurrentComplexity.Needs[resourceType] * (int)CurrentComplexity.NeedsCapacityCoefficient;

            foreach(var want in CurrentComplexity.Wants) {
                retval = Math.Max(retval, want[resourceType] * (int)CurrentComplexity.WantsCapacityCoefficient);
            }

            return retval;
        }

        private void RefreshAppearance() {
            if(ForegroundRenderer != null) {
                ForegroundRenderer.sprite = CurrentComplexity.SpriteForSociety;
                ForegroundRenderer.color = CurrentComplexity.ColorForSociety;
            }
            if(BackgroundRenderer != null) {
                BackgroundRenderer.color = CurrentComplexity.ColorForBackground;
            }
        }

        #endregion

    }

}
