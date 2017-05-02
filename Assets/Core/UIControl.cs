using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Map;

using Assets.Highways;
using Assets.ConstructionZones;
using Assets.Societies;
using Assets.ResourceDepots;
using Assets.HighwayManager;

namespace Assets.Core {

    public class UIControl : UIControlBase {

        #region instance fields and properties

        public List<TargetedEventReceiverBase> MapNodeEventReceivers {
            get { return _mapNodeEventReceivers; }
            set { _mapNodeEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _mapNodeEventReceivers;

        public List<TargetedEventReceiverBase> HighwayEventReceivers {
            get { return _highwayEventReceivers; }
            set { _highwayEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _highwayEventReceivers;

        public List<TargetedEventReceiverBase> SocietyEventReceivers {
            get { return _societyEventReceivers; }
            set { _societyEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _societyEventReceivers;

        public List<TargetedEventReceiverBase> ConstructionZoneEventReceivers {
            get { return _constructionZoneEventReceivers; }
            set { _constructionZoneEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _constructionZoneEventReceivers;

        public List<TargetedEventReceiverBase> ResourceDepotEventReceivers {
            get { return _resourceDepotEventReceivers; }
            set { _resourceDepotEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _resourceDepotEventReceivers;

        public List<TargetedEventReceiverBase> HighwayManagerEventReceivers {
            get { return _highwayManagerEventReceivers; }
            set { _highwayManagerEventReceivers = value; }
        }
        [SerializeField] private List<TargetedEventReceiverBase> _highwayManagerEventReceivers;

        private Dictionary<Type, List<TargetedEventReceiverBase>> EventReceiversByType =
            new Dictionary<Type, List<TargetedEventReceiverBase>>();

        #endregion

        #region instance methods

        #region Unity event methods

        private void Awake() {
            EventReceiversByType[typeof(MapNodeUISummary         )] = MapNodeEventReceivers;
            EventReceiversByType[typeof(BlobHighwayUISummary     )] = HighwayEventReceivers;
            EventReceiversByType[typeof(SocietyUISummary         )] = SocietyEventReceivers;
            EventReceiversByType[typeof(ConstructionZoneUISummary)] = ConstructionZoneEventReceivers;
            EventReceiversByType[typeof(ResourceDepotUISummary   )] = ResourceDepotEventReceivers;
            EventReceiversByType[typeof(HighwayManagerUISummary  )] = HighwayManagerEventReceivers;
        }

        #endregion

        #region from UIControlBase

        public override void PushBeginDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushBeginDragEvent(source, eventData);
                }
            }
            
        }

        public override void PushDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushDragEvent(source, eventData);
                }
            }
        }

        public override void PushEndDragEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushEndDragEvent(source, eventData);
                }
            }
        }

        public override void PushPointerClickEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerClickEvent(source, eventData);
                }
            }
        }

        public override void PushPointerEnterEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerEnterEvent(source, eventData);
                }
            }
        }

        public override void PushPointerExitEvent<T>(T source, PointerEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushPointerExitEvent(source, eventData);
                }
            }
        }

        public override void PushSelectEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushSelectEvent(source, eventData);
                }
            }
        }

        public override void PushUpdateSelectedEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushUpdateSelectedEvent(source, eventData);
                }
            }
        }

        public override void PushDeselectEvent<T>(T source, BaseEventData eventData) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushDeselectEvent(source, eventData);
                }
            }
        }

        public override void PushObjectDestroyedEvent<T>(T source) {
            if(EventReceiversByType.ContainsKey(typeof(T))) {
                foreach(var receiver in EventReceiversByType[typeof(T)]) {
                    receiver.PushObjectDestroyedEvent(source);
                }
            }
        }

        #endregion

        #endregion

    }

}
