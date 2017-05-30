using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.Societies {

    public abstract class SocietyFactoryBase : MonoBehaviour {

        #region instance fields and properties

        public abstract ComplexityLadderBase StandardComplexityLadder { get; }
        public abstract ComplexityDefinitionBase DefaultComplexityDefinition { get; }

        public abstract ReadOnlyCollection<SocietyBase> Societies { get; }

        #endregion

        #region events

        public event EventHandler<SocietyEventArgs> SocietySubscribed;
        public event EventHandler<SocietyEventArgs> SocietyUnsubscribed;

        protected void RaiseSocietySubscribed  (SocietyBase newSociety) { RaiseEvent(SocietySubscribed,   new SocietyEventArgs(newSociety)); }
        protected void RaiseSocietyUnsubscribed(SocietyBase oldSociety) { RaiseEvent(SocietyUnsubscribed, new SocietyEventArgs(oldSociety)); }

        protected void RaiseEvent<T>(EventHandler<T> callback, T e) where T : EventArgs {
            if(callback != null) {
                callback(this, e);
            }
        }

        #endregion

        #region instance methods

        public abstract SocietyBase GetSocietyOfID(int id);

        public abstract bool HasSocietyAtLocation(MapNodeBase location);
        public abstract SocietyBase GetSocietyAtLocation(MapNodeBase location);

        public abstract bool        CanConstructSocietyAt(MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity);
        public abstract SocietyBase ConstructSocietyAt   (MapNodeBase location, ComplexityLadderBase ladder, ComplexityDefinitionBase startingComplexity);

        public abstract void DestroySociety(SocietyBase society);

        public abstract void SubscribeSociety  (SocietyBase society);
        public abstract void UnsubscribeSociety(SocietyBase society);

        public abstract ComplexityDefinitionBase GetComplexityDefinitionOfName(string name);
        public abstract ComplexityLadderBase     GetComplexityLadderOfName    (string name);

        public abstract void TickSocieties(float secondsPassed);

        #endregion

    }

}
