using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Core {

    /// <summary>
    /// An abstract base class for receiving UI events from UIControl.
    /// </summary>
    /// <typeparam name="T">The type of the source events should be received from</typeparam>
    public abstract class TargetedEventReceiverBase<T> : TargetedEventReceiverBase where T : class {

        #region instance methods

        #region from TargetedEventRecieverBase

        /// <inheritdoc/>
        public override void PushBeginDragEvent(object source, PointerEventData eventData) {
            PushBeginDragEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushDragEvent(object source, PointerEventData eventData) {
            PushDragEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushEndDragEvent(object source, PointerEventData eventData) {
            PushEndDragEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushPointerClickEvent(object source, PointerEventData eventData) {
            PushPointerClickEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushPointerEnterEvent(object source, PointerEventData eventData) {
            PushPointerEnterEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushPointerExitEvent(object source, PointerEventData eventData) {
            PushPointerExitEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushSelectEvent(object source, BaseEventData eventData){
            PushSelectEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushUpdateSelectedEvent(object source, BaseEventData eventData){
            PushUpdateSelectedEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushDeselectEvent(object source, BaseEventData eventData){
            PushDeselectEvent(source as T, eventData);
        }

        /// <inheritdoc/>
        public override void PushObjectDestroyedEvent(object source){
            PushObjectDestroyedEvent(source as T);
        }

        #endregion

        /// <summary>
        /// Generic version of PushBeginDragEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushBeginDragEvent(T source, PointerEventData eventData);

        /// <summary>
        /// Generic version of PushDragEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushDragEvent     (T source, PointerEventData eventData);

        /// <summary>
        /// Generic version of PushEndDragEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushEndDragEvent  (T source, PointerEventData eventData);


        /// <summary>
        /// Generic version of PushPointerClickEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushPointerClickEvent(T source, PointerEventData eventData);

        /// <summary>
        /// Generic version of PushPointerEnterEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushPointerEnterEvent(T source, PointerEventData eventData);

        /// <summary>
        /// Generic version of PushPointerExitEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushPointerExitEvent (T source, PointerEventData eventData);


        /// <summary>
        /// Generic version of PushSelectEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushSelectEvent        (T source, BaseEventData eventData);

        /// <summary>
        /// Generic version of PushUpdateSelectedEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushUpdateSelectedEvent(T source, BaseEventData eventData);

        /// <summary>
        /// Generic version of PushDeselectEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushDeselectEvent      (T source, BaseEventData eventData);


        /// <summary>
        /// Generic version of PushObjectDestroyedEvent.
        /// </summary>
        /// <param name="source">The generic source that originally sent the event</param>
        public abstract void PushObjectDestroyedEvent(T source);

        #endregion

    }

}
