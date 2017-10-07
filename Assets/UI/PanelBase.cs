using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.UI {

    /// <summary>
    /// The base class for all in-game UI panels that pop in an out during runtime, performing
    /// some UI tasks while they're active.
    /// </summary>
    public class PanelBase : MonoBehaviour {

        #region instance fields and properties

        /// <summary>
        /// The AudioSource to play when the panel is activated.
        /// </summary>
        [SerializeField] protected AudioSource ActivationAudio;

        /// <summary>
        /// The AudioSource to play when the panel is deactivated.
        /// </summary>
        [SerializeField] protected AudioSource DeactivationAudio;

        #endregion

        #region events

        /// <summary>
        /// Fires whenever the panel has requested deactivation.
        /// </summary>
        public event EventHandler<EventArgs> DeactivationRequested;

        /// <summary>
        /// Fires the DeactivationRequested event.
        /// </summary>
        protected void RaiseDeactivationRequested() { RaiseEvent(DeactivationRequested, EventArgs.Empty); }

        /// <summary>
        /// Helper method for raising events.
        /// </summary>
        /// <typeparam name="T">The EventArgs type of the event</typeparam>
        /// <param name="handler">The event handler itself</param>
        /// <param name="e">The event args to be passed to the handler</param>
        protected void RaiseEvent<T>(EventHandler<T> handler, T e) where T : EventArgs {
            if(handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region instance methods

        /// <summary>
        /// Activates the panel.
        /// </summary>
        public virtual void Activate() {
            gameObject.SetActive(true);
            UpdateDisplay();
            DoOnActivate();
            if(ActivationAudio != null && !ActivationAudio.isPlaying) {
                ActivationAudio.Play();
            }
        }

        /// <summary>
        /// Deactivates the panel.
        /// </summary>
        public virtual void Deactivate() {
            DoOnDeactivate();
            ClearDisplay();
            gameObject.SetActive(false);
            if(DeactivationAudio != null && !DeactivationAudio.isPlaying) {
                DeactivationAudio.Play();
            }
        }

        /// <summary>
        /// Updates the display on the panel, usually while the panel is active.
        /// </summary>
        public virtual void UpdateDisplay() { }

        /// <summary>
        /// Clears the display of any information it has.
        /// </summary>
        public virtual void ClearDisplay() { }

        /// <summary>
        /// A series of actions to perform upon activation. Used by subclasses
        /// when they want to add additional functionality to panel activation
        /// without overriding existing behavior.
        /// </summary>
        /// <remarks>
        /// This method exists primarily to avoid the "call into base method"
        /// antipattern, though it's of debatable use when Activate is not sealed.
        /// </remarks>
        protected virtual void DoOnActivate() { }

        /// <summary>
        /// A series of actions to perform upon deactivation. Used by subclasses
        /// when they want to add additional functionality to panel deactivation
        /// without overriding existing behavior.
        /// </summary>
        /// <remarks>
        /// This method exists primarily to avoid the "call into base method"
        /// antipattern, though it's of debatable use when Deactivate is not sealed.
        /// </remarks>
        protected virtual void DoOnDeactivate() { }

        #endregion

    }

}
