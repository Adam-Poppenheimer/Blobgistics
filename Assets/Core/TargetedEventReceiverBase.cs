using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Core {

    public abstract class TargetedEventReceiverBase : MonoBehaviour {

        #region instance methods

        public abstract void PushBeginDragEvent(object source, PointerEventData eventData);

        public abstract void PushDragEvent     (object source, PointerEventData eventData);
        public abstract void PushEndDragEvent  (object source, PointerEventData eventData);

        public abstract void PushPointerClickEvent(object source, PointerEventData eventData);

        public abstract void PushPointerEnterEvent(object source, PointerEventData eventData);
        public abstract void PushPointerExitEvent (object source, PointerEventData eventData);

        public abstract void PushSelectEvent        (object source, BaseEventData eventData);
        public abstract void PushUpdateSelectedEvent(object source, BaseEventData eventData);
        public abstract void PushDeselectEvent      (object source, BaseEventData eventData);

        public abstract void PushObjectDestroyedEvent(object source);

        #endregion

    }

}
