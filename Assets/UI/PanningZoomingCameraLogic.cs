using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.UI {

    public class PanningZoomingCameraLogic : MonoBehaviour {

        #region instance fields and properties

        public bool IsReceivingInput { get; set; }

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

        private void Start() {
            DesiredSize = CameraToControl.orthographicSize;
            StartingSize = DesiredSize;
        }

        private void Update() {
            if(IsReceivingInput) {
                HandleMovement();
                HandleZooming();
            }
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
