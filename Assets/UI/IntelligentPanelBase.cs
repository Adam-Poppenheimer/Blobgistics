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

    /// <summary>
    /// A base class for a more complex panel that implements certain desirable behaviors.
    /// </summary>
    /// <remarks>
    /// IntelligentPanelBase does several things.
    ///     1. It attempts to keep itself near but not on top of some position in the world.
    ///     2. It may try to keep itself on the screen but in the loose direction of its intended position.
    ///     3. It remains active until it is deselected, at which point it is deactivated.
    ///     
    /// It's primarily used for displaying information about various gameplay elements after
    /// those elements have been selected.
    /// </remarks>
    public abstract class IntelligentPanelBase : PanelBase, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        /// <summary>
        /// Whether or not this panel attempts to keep itself on the screen when
        /// the camera moves.
        /// </summary>
        public bool MovePanelWithCamera { get; set; }

        /// <summary>
        /// The world position the panel is trying to remain near.
        /// </summary>
        public Vector3 DesiredWorldPosition { get; set; }

        /// <summary>
        /// The distance, in screen space, that the panel should try and keep from
        /// its desired world position. 
        /// </summary>
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

        /// <inheritdoc/>
        public void OnSelect(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        /// <inheritdoc/>
        public void OnDeselect(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        #endregion

        /// <inheritdoc/>
        public override void Activate() {
            gameObject.SetActive(true);
            StartCoroutine(ReselectToThis());
            UpdateDisplay();
            DoOnActivate();
            if(MovePanelWithCamera) {
                RepositionInCameraView();
            }
            if(ActivationAudio != null && !ActivationAudio.isPlaying) {
                ActivationAudio.Play();
            }
        }

        /// <summary>
        /// a method, called during Update, that should be used by subclasses in
        /// place of Update.
        /// </summary>
        /// <remarks>
        /// Since IntelligentPanelBase requires an Update method to work properly, it's
        /// very important that subclasses do not declare their own Update methods.
        /// This will override the Update method declared in this class and cause it
        /// to stop working.
        /// </remarks>
        protected virtual void DoOnUpdate() { }

        /// <summary>
        /// Called by selectable children of this panel to handle selection behavior.
        /// </summary>
        /// <remarks>
        /// Intelligent panels are primarily used in this codebase to display information
        /// about a selected object. It makes use of the selection semantics that Unity offers.
        /// However, selection changes every time any selectable object is selected or deselected,
        /// which may include UI elements like buttons that are part of this panel. This class solves
        /// this problem by informing the panel when its children have been selected and deselected,
        /// as well. The panel should only be deactivated  if it or one of its children has been
        /// deselected and neither it nor one of its children has been reselected. Every
        /// child object with a selectable component must call into these two events via an
        /// EventTrigger in order to handle panel selection behavior properly.
        /// </remarks>
        /// <param name="eventData">Event data from the selection</param>
        public void DoOnChildSelected(BaseEventData eventData) {
            DeactivateOnNextUpdate = false;
        }

        /// <summary>
        /// Called by selectable children of this panel to handle deselection behavior.
        /// </summary>
        /// <remarks>
        /// Intelligent panels are primarily used in this codebase to display information
        /// about a selected object. It makes use of the selection semantics that Unity offers.
        /// However, selection changes every time any selectable object is selected or deselected,
        /// which may include UI elements like buttons that are part of this panel. We solve
        /// this problem by informing the panel when its children have been selected and deselected,
        /// as well. The panel should only be deactivated if it or one of its children has been
        /// deselected and neither it nor one of its children has been reselected. Every
        /// child object with a selectable component must call into these two events via an
        /// EventTrigger in order to handle panel selection behavior properly.
        /// </remarks>
        /// <param name="eventData">Event data from the selection</param>
        public void DoOnChildDeselected(BaseEventData eventData) {
            DeactivateOnNextUpdate = true;
        }

        private IEnumerator ReselectToThis() {
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        /// <summary>
        /// This method tries to keep any of the panel's edges from going beyond the edge of the screen
        /// while also trying to keep the panel within some distance of its desired world position.
        /// It does this by operating in screen space, never letting the center of the panel get beyond
        /// minimum and maximum X and Y values determined by the camera's view rect and the panel's
        /// rect.
        /// </summary>
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

            //This implementation causes the panel to jump when it's at the midpoints between
            //the various cardinal directions (Northeast, Southeast, etc). A smoother implementation
            //might set the desired position in terms of a circle to prevent the panel from suddenly
            //snapping to a new position.
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
