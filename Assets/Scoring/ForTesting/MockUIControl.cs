using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Core;
using UnityEngine.EventSystems;

namespace Assets.Scoring.ForTesting {

    public class MockUIControl : UIControlBase {

        #region events

        public event EventHandler<EventArgs> PerformVictoryTasksCalled;
        public event EventHandler<EventArgs> PerformDefeatTasksCalled;

        #endregion

        #region instance methods

        #region from UIControlBase

        public override void PerformDefeatTasks() {
            if(PerformDefeatTasksCalled != null) {
                PerformDefeatTasksCalled(this, EventArgs.Empty);
            }
        }

        public override void PerformVictoryTasks() {
            if(PerformVictoryTasksCalled != null) {
                PerformVictoryTasksCalled(this, EventArgs.Empty);
            }
        }

        public override void PushBeginDragEvent<T>(T source, PointerEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushDeselectEvent<T>(T source, BaseEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushDragEvent<T>(T source, PointerEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushEndDragEvent<T>(T source, PointerEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushObjectDestroyedEvent<T>(T source) {
            throw new NotImplementedException();
        }

        public override void PushPointerClickEvent<T>(T source, PointerEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushPointerEnterEvent<T>(T source, PointerEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushPointerExitEvent<T>(T source, PointerEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushSelectEvent<T>(T source, BaseEventData eventData) {
            throw new NotImplementedException();
        }

        public override void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) {
            throw new NotImplementedException();
        }

        #endregion

        #endregion
        
    }

}
