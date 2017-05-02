using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityCustomUtilities.Extensions;

namespace Assets.UI {

    public abstract class IntelligentPanel : MonoBehaviour, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        public bool MovePanelWithCamera { get; set; }

        public Vector3 DesiredWorldPosition { get; set; }

        [SerializeField] private float MinimumBufferAroundDesiredPosition;

        private bool DeactivateOnNextUpdate = false;

        private RectTransform RectTransform {
            get {
                if(_rectTransform == null) {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
        private RectTransform _rectTransform;

        #endregion

        #region events

        public event EventHandler<EventArgs> DeactivationRequested;

        protected void RaiseDeactivationRequested() { RaiseEvent(DeactivationRequested, EventArgs.Empty); }

        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs {
            if(handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region instance methods

        #region Unity event methods

        private void Update() {
            if(DeactivateOnNextUpdate) {
                Deactivate();
                DeactivateOnNextUpdate = false;
            }else {
                DoOnUpdate();
                if(MovePanelWithCamera) {
                    RepositionInCameraView();
                }
            }
        }

        #endregion

        #region Unity EventSystem interfaces

        public void OnSelect(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void OnDeselect(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        #endregion

        public void Activate() {
            gameObject.SetActive(true);
            StartCoroutine(ReselectToThis());
            UpdateDisplay();
            DoOnActivate();
            if(MovePanelWithCamera) {
                RepositionInCameraView();
            }
        }

        public void Deactivate() {
            DoOnDeactivate();
            ClearDisplay();
            gameObject.SetActive(false);
        }

        public virtual void UpdateDisplay() { }
        public virtual void ClearDisplay() { }

        protected virtual void DoOnActivate() { }
        protected virtual void DoOnDeactivate() { }

        protected virtual void DoOnUpdate() { }

        public void DoOnChildSelected(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        public void DoOnChildDeselected(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        private IEnumerator ReselectToThis() {
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        private void RepositionInCameraView() {
            var desiredWorldInScreen = Camera.main.WorldToScreenPoint(DesiredWorldPosition);
            var rectOfPanel = RectTransform.rect;
            var rectOfCamera = Camera.main.pixelRect;

            var minimumScreenX = rectOfPanel.width / 2;
            var maximumScreenX = rectOfCamera.width - rectOfPanel.width / 2;
            var minimumScreenY = rectOfPanel.height / 2;
            var maximumScreenY = rectOfCamera.height - rectOfPanel.height / 2;

            var dominantDirection = desiredWorldInScreen.GetDominantManhattanDirectionTo(rectOfCamera.center);

            float desiredXInScreenGivenOffsets = 0f, desiredYInScreenGivenOffsets = 0f;

            switch(dominantDirection) {
                case ManhattanDirection.East:
                    desiredXInScreenGivenOffsets = desiredWorldInScreen.x + rectOfPanel.width / 2f + MinimumBufferAroundDesiredPosition;
                    desiredYInScreenGivenOffsets = desiredWorldInScreen.y;
                    break;
                case ManhattanDirection.West:
                    desiredXInScreenGivenOffsets = desiredWorldInScreen.x - rectOfPanel.width / 2f - MinimumBufferAroundDesiredPosition;
                    desiredYInScreenGivenOffsets = desiredWorldInScreen.y;
                    break;
                case ManhattanDirection.North:
                    desiredXInScreenGivenOffsets = desiredWorldInScreen.x;
                    desiredYInScreenGivenOffsets = desiredWorldInScreen.y + rectOfPanel.height / 2f + MinimumBufferAroundDesiredPosition;
                    break;
                case ManhattanDirection.South:
                    desiredXInScreenGivenOffsets = desiredWorldInScreen.x;
                    desiredYInScreenGivenOffsets = desiredWorldInScreen.y - rectOfPanel.height / 2f - MinimumBufferAroundDesiredPosition;
                    break;
            }

            var desiredPositionInScreenGivenOffsets = new Vector3(
                desiredXInScreenGivenOffsets,
                desiredYInScreenGivenOffsets,
                desiredWorldInScreen.z
            );

            var actualScreenPosition = new Vector3(
                Mathf.Clamp(desiredPositionInScreenGivenOffsets.x, minimumScreenX, maximumScreenX),
                Mathf.Clamp(desiredPositionInScreenGivenOffsets.y, minimumScreenY, maximumScreenY),
                desiredWorldInScreen.z
            );

            transform.position = actualScreenPosition;
        }

        #endregion

    }
}
