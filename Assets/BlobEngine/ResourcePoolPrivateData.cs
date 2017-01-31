using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;

namespace Assets.BlobEngine {

    public class ResourcePoolPrivateData : MonoBehaviour {

        #region instance fields and properties

        public UIFSM TopLevelUIFSM {
            get { return _topLevelUIFSM; }
        }
        [SerializeField] private UIFSM _topLevelUIFSM;

        public uint Height {
            get { return _height; }
        }
        [SerializeField] private uint _height;

        public uint Width {
            get { return _width; }
        }
        [SerializeField] private uint _width;

        public uint Depth {
            get { return _depth; }
        }
        [SerializeField] private uint _depth;

        public Vector3 LocalNorthConnectionPoint {
            get { return new Vector3(0f, Height / 2f, ResourceBlob.DesiredZPositionOfAllBlobs); }
        }

        public Vector3 LocalSouthConnectionPoint {
            get { return new Vector3(0f, -Height / 2f, ResourceBlob.DesiredZPositionOfAllBlobs); }
        }

        public Vector3 LocalEastConnectionPoint {
            get { return new Vector3(Width / 2f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs); }
        }

        public Vector3 LocalWestConnectionPoint {
            get { return new Vector3(-Width / 2f, 0f, ResourceBlob.DesiredZPositionOfAllBlobs); }
        }

        public BlobPileCapacity Capacity {
            get {
                if(_capacity == null) {
                    _capacity = new BlobPileCapacity(new Dictionary<ResourceType, int>() {
                        { ResourceType.Red, 25 },
                    });
                }
                return _capacity;
            }
        }
        private BlobPileCapacity _capacity = null;

        public BlobPileCapacity Cost {
            get {
                if(_cost == null) {
                    _cost = new BlobPileCapacity(new Dictionary<ResourceType, int>() {
                        { ResourceType.Red, 10 },
                    });
                }
                return _cost;
            }
        }
        private BlobPileCapacity _cost = null;
        
        public float RealignmentSpeedPerSecond {
            get { return _realignmentSpeedPerSecond; }
        }
        [SerializeField] private float _realignmentSpeedPerSecond;

        #endregion

    }

}
