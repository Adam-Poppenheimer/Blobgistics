using System;
using System.Collections.Generic;

using UnityEngine;

using UnityCustomUtilities.UI;
using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class BlobGeneratorPrivateData : MonoBehaviour {

        #region instance fields and properties

        public UIFSM TopLevelUIFSM {
            get { return _topLevelUIFSM; }
        }
        [SerializeField] private UIFSM _topLevelUIFSM;

        public float SecondsToGenerate {
            get { return _secondsToGenerate; }
        }
        [SerializeField] private float _secondsToGenerate;

        public uint Width {
            get { return _width; }
        }
        [SerializeField] private uint _width;

        public uint Height {
            get { return _height; }
        }
        [SerializeField] private uint _height;

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

        public int Capacity {
            get { return _capacity; }
        }
        [SerializeField] private int _capacity = 1;

        public Dictionary<ResourceType, int> Cost {
            get {
                if(_cost == null) {
                    _cost = new Dictionary<ResourceType, int>() {
                        { ResourceType.Red, 10 },
                    };
                }
                return _cost;
            }
        }
        private Dictionary<ResourceType, int> _cost;

        public ResourceBlobFactory BlobFactory {
            get { return _blobFactory; }
        }
        [SerializeField] private ResourceBlobFactory _blobFactory;

        #endregion

    }

}