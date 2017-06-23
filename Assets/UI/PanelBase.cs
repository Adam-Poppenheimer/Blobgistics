using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.UI {

    public class PanelBase : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] protected AudioSource ActivationAudio;
        [SerializeField] protected AudioSource DeactivationAudio;

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

        public virtual void Activate() {
            gameObject.SetActive(true);
            UpdateDisplay();
            DoOnActivate();
            if(ActivationAudio != null && !ActivationAudio.isPlaying) {
                ActivationAudio.Play();
            }
        }

        public virtual void Deactivate() {
            DoOnDeactivate();
            ClearDisplay();
            gameObject.SetActive(false);
            if(DeactivationAudio != null && !DeactivationAudio.isPlaying) {
                DeactivationAudio.Play();
            }
        }

        public virtual void UpdateDisplay() { }
        public virtual void ClearDisplay() { }

        protected virtual void DoOnActivate() { }
        protected virtual void DoOnDeactivate() { }

        #endregion

    }

}
