using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.UI {

    /// <summary>
    /// The logic for the game's camera, which pans with WASD or the right/middle mouse buttons,
    /// and zooms with the scroll wheel.
    /// </summary>
    /// <remarks>
    /// This class assumes the existence of a mouse with three buttons on it and accesses those
    /// buttons directly. One might consider a refactor that makes more thorough use of virtual
    /// buttons.
    /// </remarks>
    public class PanningZoomingCameraLogic : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The bounds that constrain how far the camera can move.
        /// </summary>
        public Rect Bounds { get; set; }

        [SerializeField] private Camera CameraToControl;
        [SerializeField] private float  KeyboardMovementCoefficient = 1f;

        [SerializeField] private float SmallestWindowSize = 2f;
        [SerializeField] private float LargestWindowSize  = 10f;

        [SerializeField] private float BaseSecondsToZoom = 1f;
        [SerializeField] private float ScrollWheelZoomCoefficient = 10f;

        [SerializeField] private float ScreenCenterOffsetFromBoundsX = 0f;
        [SerializeField] private float ScreenCenterOffsetFromBoundsY = 0f;

        private float CurrentScrollVelocity = 0f;
        private float DesiredSize;
        private float StartingSize;

        #endregion

        #region instance methods

        #region Unity event methods

        private void OnEnable() {
            DesiredSize = CameraToControl.orthographicSize;
            StartingSize = DesiredSize;
        }

        private void Update() {
            HandleMovement();
            HandleZooming();
        }

        #endregion

        private void HandleMovement() {
            float horizontalInput = 0f, verticalInput = 0f;
            if(Input.GetMouseButton(1) || Input.GetMouseButton(2)) {
                horizontalInput = -Input.GetAxis("Mouse X");
                verticalInput   = -Input.GetAxis("Mouse Y");
            }else {
                horizontalInput = Input.GetAxis("Horizontal") * KeyboardMovementCoefficient;
                verticalInput   = Input.GetAxis("Vertical")   * KeyboardMovementCoefficient;
            }
            horizontalInput *= DesiredSize / StartingSize;
            verticalInput   *= DesiredSize / StartingSize;

            CameraToControl.transform.Translate(new Vector3(horizontalInput, verticalInput, 0f));

            ConstrainCameraToBounds();
        }

        private void HandleZooming() {
            DesiredSize -= Input.GetAxis("Mouse ScrollWheel") * ScrollWheelZoomCoefficient;
            DesiredSize = Mathf.Clamp(DesiredSize, SmallestWindowSize, LargestWindowSize);
            
            CameraToControl.orthographicSize = Mathf.SmoothDamp(CameraToControl.orthographicSize, DesiredSize,
                ref CurrentScrollVelocity, BaseSecondsToZoom);
        }

        //The bounds to not take into account the zoom level of the camera, which means that
        //a zoomed out camera can see more outside of the camera bounds than one that is
        //zoomed in.
        private void ConstrainCameraToBounds() {
            var cameraPosition = CameraToControl.transform.position;

            var cameraXMin = Bounds.xMin - ScreenCenterOffsetFromBoundsX;
            var cameraXMax = Bounds.xMax + ScreenCenterOffsetFromBoundsX;

            var cameraYMin = Bounds.yMin - ScreenCenterOffsetFromBoundsY;
            var cameraYMax = Bounds.yMax + ScreenCenterOffsetFromBoundsY;

            cameraPosition.x = Mathf.Clamp(cameraPosition.x, cameraXMin, cameraXMax);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, cameraYMin, cameraYMax);

            CameraToControl.transform.position = cameraPosition;
        }

        #endregion

    }

}
