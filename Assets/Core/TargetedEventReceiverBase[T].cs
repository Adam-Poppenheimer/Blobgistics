using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Core {

    public abstract class TargetedEventReceiverBase<T> : TargetedEventReceiverBase where T : class {

        #region instance methods

        #region from TargetedEventRecieverBase

        public override void PushBeginDragEvent(object source, PointerEventData eventData) {
            PushBeginDragEvent(source as T, eventData);
        }

        public override void PushDragEvent(object source, PointerEventData eventData) {
            PushDragEvent(source as T, eventData);
        }

        public override void PushEndDragEvent(object source, PointerEventData eventData) {
            PushEndDragEvent(source as T, eventData);
        }

        public override void PushPointerClickEvent(object source, PointerEventData eventData) {
            PushPointerClickEvent(source as T, eventData);
        }

        public override void PushPointerEnterEvent(object source, PointerEventData eventData) {
            PushPointerEnterEvent(source as T, eventData);
        }

        public override void PushPointerExitEvent(object source, PointerEventData eventData) {
            PushPointerExitEvent(source as T, eventData);
        }

        public override void PushSelectEvent(object source, BaseEventData eventData){
            PushSelectEvent(source as T, eventData);
        }

        public override void PushUpdateSelectedEvent(object source, BaseEventData eventData){
            PushUpdateSelectedEvent(source as T, eventData);
        }

        public override void PushDeselectEvent(object source, BaseEventData eventData){
            PushDeselectEvent(source as T, eventData);
        }

        public override void PushObjectDestroyedEvent(object source){
            PushObjectDestroyedEvent(source as T);
        }

        #endregion

        public abstract void PushBeginDragEvent(T source, PointerEventData eventData);
        public abstract void PushDragEvent     (T source, PointerEventData eventData);
        public abstract void PushEndDragEvent  (T source, PointerEventData eventData);

        public abstract void PushPointerClickEvent(T source, PointerEventData eventData);

        public abstract void PushPointerEnterEvent(T source, PointerEventData eventData);
        public abstract void PushPointerExitEvent (T source, PointerEventData eventData);

        public abstract void PushSelectEvent        (T source, BaseEventData eventData);
        public abstract void PushUpdateSelectedEvent(T source, BaseEventData eventData);
        public abstract void PushDeselectEvent      (T source, BaseEventData eventData);

        public abstract void PushObjectDestroyedEvent(T source);

        #endregion

    }

}
