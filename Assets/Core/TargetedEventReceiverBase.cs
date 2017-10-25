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
    /// <remarks>
    /// This class exists as a way of working around C#'s type checking system in a
    /// relatively clean way. It should not be inherited from by any class other than
    /// its generic counterpart. If you want to create a class that receives events on
    /// specific types, use <see cref="TargetedEventReceiverBase{T}"/>.
    /// </remarks>
    public abstract class TargetedEventReceiverBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Accepts a BeginDrag event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushBeginDragEvent(object source, PointerEventData eventData);

        /// <summary>
        /// Accepts a Drag event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushDragEvent     (object source, PointerEventData eventData);

        /// <summary>
        /// Accepts an EndDrag event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushEndDragEvent  (object source, PointerEventData eventData);


        /// <summary>
        /// Accepts a PointerClick event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushPointerClickEvent(object source, PointerEventData eventData);


        /// <summary>
        /// Accepts a PointerEnter event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushPointerEnterEvent(object source, PointerEventData eventData);

        /// <summary>
        /// Accepts a PointerExit event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushPointerExitEvent (object source, PointerEventData eventData);


        /// <summary>
        /// Accepts a Select event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushSelectEvent        (object source, BaseEventData eventData);

        /// <summary>
        /// Accepts an UpdateSelected event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushUpdateSelectedEvent(object source, BaseEventData eventData);

        /// <summary>
        /// Accepts a Deselect event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        /// <param name="eventData">Data describing the event</param>
        public abstract void PushDeselectEvent      (object source, BaseEventData eventData);


        /// <summary>
        /// Accepts a BeginDrag event.
        /// </summary>
        /// <param name="source">The nongeneric source that originally sent the event</param>
        public abstract void PushObjectDestroyedEvent(object source);

        /// <summary>
        /// Tells the event receiver to close any displays it has open and accepting input, and tell the
        /// caller if any displays were closed.
        /// </summary>
        /// <returns>A value indicating whether any displays were closed</returns>
        public abstract bool TryCloseAllOpenDisplays();

        #endregion

    }

}
