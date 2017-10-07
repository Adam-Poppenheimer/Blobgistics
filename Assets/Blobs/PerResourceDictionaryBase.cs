using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.Extensions;

namespace Assets.Blobs {

    /// <summary>
    /// An abstract base class for a dictionary on the ResourceType enum that plays
    /// well with Unity's serialization.
    /// </summary>
    /// <remarks>
    /// Since Unity doesn't serialize Dictionaries but I needed to establish several
    /// mappings between ResourceType and other values, I decided to use the equivalence
    /// of enums and ints to fudge a dictionary-like object into existence.
    /// 
    /// This implementation is not robust to modifications on ResourceType. Changing
    /// the ResourceType enum will likely break any instance of this class or any of
    /// its subclasses.
    /// </remarks>
    /// <typeparam name="T">The value type of the dictionary</typeparam>
    public abstract class PerResourceDictionaryBase<T> : MonoBehaviour, IEnumerable<ResourceType> {

        #region instance fields and properties

        /// <summary>
        /// Retrieves the value keyed to the given ResourceType.
        /// </summary>
        /// <remarks>
        /// In order to prevent IndexOutOfRange exceptions from being thrown,
        /// this class makes sure to initialize ValueList with a number of
        /// values equal to the number of elements in ResourceType.
        /// </remarks>
        /// <param name="type">The ResourceType to be used as a key</param>
        /// <returns>the value associated with the key</returns>
        public T this[ResourceType type] {
            get {
                return ValueList[(int)type];
            }
            set {
                ValueList[(int)type] = value;
                if(DictionaryRepresentation != null) {
                    DictionaryRepresentation[type] = value;
                }
            }
        }
        /// <summary>
        /// The list used to store and simulate the class's dictionary-like properties.
        /// </summary>
        /// <remarks>
        /// Keep in mind that this list's length is equal to the number of distinct values
        /// defined in ResourceType, and should be modified only with great care.
        /// </remarks>
        [SerializeField] protected List<T> ValueList = new List<T>();

        /// <summary>
        /// The default value that every entry in the ValueList will be initialized to.
        /// </summary>
        /// <remarks>
        /// Note that the PerResourceDictionary defines some value for every single ResourceType.
        /// There are no empty entries.
        /// </remarks>
        protected abstract T DefaultValue { get; }

        private Dictionary<ResourceType, T> DictionaryRepresentation;

        #endregion

        #region instance methods

        #region Unity event methods

        //It's very important that our ValueList is initialized properly, so that our
        //indexing operations don't throw IndexOutOfRange exceptions. This class hedges
        //extensively against this possibility in ways that may be redundant.
        private void Awake() {
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = ValueList.Count; i < resourceTypeCount; ++i) {
                ValueList.Add(DefaultValue);
            }
        }

        private void OnValidate() {
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = ValueList.Count; i < resourceTypeCount; ++i) {
                ValueList.Add(DefaultValue);
            }
        }

        private void Reset() {
            ValueList.Clear();
            int resourceTypeCount = EnumUtil.GetValues<ResourceType>().Count();
            for(int i = ValueList.Count; i < resourceTypeCount; ++i) {
                ValueList.Add(DefaultValue);
            }
        }

        #endregion

        #region from IEnumerable<ResourceType>

        /// <inheritdoc/>
        public IEnumerator<ResourceType> GetEnumerator() {
            return EnumUtil.GetValues<ResourceType>().GetEnumerator();
        }

        #endregion

        #region from IEnumerable

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() {
            return ValueList.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Presents a safer way to access values. Ideally this method should be unnecessary, but
        /// since certain operations require revealing the ValueList to subclasses, it's still
        /// possible that it won't have a value for a given key.
        /// </summary>
        /// <param name="key">The resource type whose value should be retrieved</param>
        /// <param name="value">The value associated with the key</param>
        /// <returns>True if there was a value associated with the key, and false otherwise</returns>
        public bool TryGetValue(ResourceType key, out T value) {
            value = DefaultValue;
            if(ValueList.Count > (int)key) {
                value = this[key];
                return true;
            }else {
                return false;
            }
        }

        /// <summary>
        /// Retrieves a ReadOnlyDictionary whose values are equivalent to those in the
        /// PerResourceDictionary, unless ValueList has been manipulated.
        /// </summary>
        /// <returns>An equivalent ReadOnlyDictionary</returns>
        public ReadOnlyDictionary<ResourceType, T> ToReadOnlyDictionary() {
            if(DictionaryRepresentation == null) {
                DictionaryRepresentation = new Dictionary<ResourceType, T>();
                foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                    DictionaryRepresentation[resourceType] = this[resourceType];
                }
            }
            return new ReadOnlyDictionary<ResourceType, T>(DictionaryRepresentation);
        }

        #endregion

    }

}
