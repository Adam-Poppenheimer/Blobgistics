using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Core;

namespace Assets.Societies {

    /// <summary>
    /// The standard implementation for SocietyPrivateDataBase, which stores information
    /// and dependencies necessary for Society to function.
    /// </summary>
    public class SocietyPrivateData : SocietyPrivateDataBase {

        #region instance fields and properties

        #region from SocietyPrivateDataBase

        /// <inheritdoc/>
        public override ComplexityLadderBase ActiveComplexityLadder {
            get {
                if(_activeComplexityLadder == null) {
                    throw new InvalidOperationException("ActiveComplexityLadder is uninitialized");
                } else {
                    return _activeComplexityLadder;
                }
            }
        }

        /// <summary>
        /// The externalized Set method for ActiveComplexityLadder.
        /// </summary>
        /// <param name="value">The new value for ActiveComplexityLadder</param>
        public void SetActiveComplexityLadder(ComplexityLadderBase value) {
            if(value == null) {
                throw new ArgumentNullException("value");
            }else {
                _activeComplexityLadder = value;
            }
        }
        [SerializeField] private ComplexityLadderBase _activeComplexityLadder;

        /// <inheritdoc/>
        public override ResourceBlobFactoryBase BlobFactory {
            get {
                if(_blobFactory == null) {
                    throw new InvalidOperationException("BlobFactory is uninitialized");
                } else {
                    return _blobFactory;
                }
            }
        }

        /// <summary>
        /// The externalized Set method for BlobFactory.
        /// </summary>
        /// <param name="value">The new value for BlobFactory</param>
        public void SetBlobFactory(ResourceBlobFactoryBase value) {
            if(value == null) {
                throw new ArgumentNullException("value");
            }else {
                _blobFactory = value;
            }
        }
        [SerializeField] private ResourceBlobFactoryBase _blobFactory;

        /// <inheritdoc/>
        public override MapNodeBase Location {
            get {
                if(_location == null) {
                    throw new InvalidOperationException("Location is uninitialized");
                } else {
                    return _location;
                }
            }
        }

        /// <summary>
        /// The externalized Set method for Location.
        /// </summary>
        /// <param name="value">The new value for Location</param>
        public void SetLocation(MapNodeBase value) {
            if(value == null) {
                throw new ArgumentNullException("value");
            }else {
                _location = value;
            }
        }
        [SerializeField] private MapNodeBase _location;

        /// <inheritdoc/>
        public override UIControlBase UIControl {
            get { return _uiControl; }
        }

        /// <summary>
        /// The externalized Set method for UIControl.
        /// </summary>
        /// <param name="value">The new value for UIControl</param>
        public void SetUIControl(UIControlBase value) {
            _uiControl = value;
        }
        [SerializeField] private UIControlBase _uiControl;

        /// <inheritdoc/>
        public override SocietyFactoryBase ParentFactory {
            get { return _parentFactory; }
        }

        /// <summary>
        /// The externalized Set method for ParentFactory.
        /// </summary>
        /// <param name="value">The new value for ParentFactory</param>
        public void SetParentFactory(SocietyFactoryBase value) {
            _parentFactory = value;
        }
        [SerializeField] private SocietyFactoryBase _parentFactory;

        #endregion

        #endregion

    }

}
