using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Core {

    /// <summary>
    /// The abstract base class for the main control module of the UI. This class exists to create a layer-like
    /// boundary between the UI and the simulation code and thus decrease coupling. It remains to be seen if this
    /// is a desirable state.
    /// </summary>
    public abstract class UIControlBase : MonoBehaviour {

        #region instance methods

        /// <summary>
        /// Receives and delegates BeginDragEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushBeginDragEvent<T>(T source, PointerEventData eventData) where T : class;

        /// <summary>
        /// Receives and delegates DragEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushDragEvent     <T>(T source, PointerEventData eventData) where T : class;

        /// <summary>
        /// Receives and delegates EndDragEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushEndDragEvent  <T>(T source, PointerEventData eventData) where T : class;


        /// <summary>
        /// Receives and delegates PointerClickEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushPointerClickEvent<T>(T source, PointerEventData eventData) where T : class;


        /// <summary>
        /// Receives and delegates PointerEnterEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushPointerEnterEvent<T>(T source, PointerEventData eventData) where T : class;

        /// <summary>
        /// Receives and delegates PointerExitEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushPointerExitEvent <T>(T source, PointerEventData eventData) where T : class;


        /// <summary>
        /// Receives and delegates SelectEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushSelectEvent        <T>(T source, BaseEventData eventData) where T : class;

        /// <summary>
        /// Receives and delegates UpdateSelectedEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) where T : class;

        /// <summary>
        /// Receives and delegates DeselectEvents passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        /// <param name="eventData">Data associated with the event</param>
        public abstract void PushDeselectEvent      <T>(T source, BaseEventData eventData) where T : class;


        /// <summary>
        /// Receives and delegates ObjectDestroyedEvent passed from the simulation.
        /// </summary>
        /// <typeparam name="T">The type of the source</typeparam>
        /// <param name="source">The UI summary of the object that received the event</param>
        public abstract void PushObjectDestroyedEvent<T>(T source) where T : class;


        /// <summary>
        /// Performs the UI tasks that follow from victory.
        /// </summary>
        public abstract void PerformVictoryTasks();

        /// <summary>
        /// Perform the UI tasks that follow from defeat.
        /// </summary>
        public abstract void PerformDefeatTasks();

        #endregion

    }

}
