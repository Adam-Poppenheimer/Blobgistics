using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Core {

    public abstract class UIControlBase : MonoBehaviour {

        #region instance methods

        public abstract void PushBeginDragEvent<T>(T source, PointerEventData eventData) where T : class;
        public abstract void PushDragEvent     <T>(T source, PointerEventData eventData) where T : class;
        public abstract void PushEndDragEvent  <T>(T source, PointerEventData eventData) where T : class;

        public abstract void PushPointerClickEvent<T>(T source, PointerEventData eventData) where T : class;

        public abstract void PushPointerEnterEvent<T>(T source, PointerEventData eventData) where T : class;
        public abstract void PushPointerExitEvent <T>(T source, PointerEventData eventData) where T : class;

        public abstract void PushSelectEvent        <T>(T source, BaseEventData eventData) where T : class;
        public abstract void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) where T : class;
        public abstract void PushDeselectEvent      <T>(T source, BaseEventData eventData) where T : class;

        #endregion

    }

}
