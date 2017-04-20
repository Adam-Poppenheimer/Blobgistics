using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.UI {

    public abstract class IntelligentPanel : MonoBehaviour, ISelectHandler, IDeselectHandler {

        #region instance fields and properties

        private bool DeactivateOnNextUpdate = false;

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

        #endregion

    }
}
