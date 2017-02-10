using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

using UnityCustomUtilities.UI;
using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class BuildingPlotPrivateData : MonoBehaviour {

        #region instance fields and properties

        public UIFSM TopLevelUIFSM {
            get { return _topLevelUIFSM; }
        }
        [SerializeField] private UIFSM _topLevelUIFSM;

        public BlobTubeFactoryBase TubeFactory {
            get { return _tubeFactory; }
        }
        [SerializeField] private BlobTubeFactoryBase _tubeFactory;

        public ReadOnlyCollection<Schematic> AvailableSchematics {
            get {
                if(_availableSchematics == null) {
                    _availableSchematics = new List<Schematic>();
                    foreach(var name in NamesOfAvailableSchematics) {
                        _availableSchematics.Add(SchematicRepository.GetSchematicOfName(name));
                    }
                }
                return _availableSchematics.AsReadOnly();
            }
        }
        private List<Schematic> _availableSchematics = null;

        [SerializeField] private List<string> NamesOfAvailableSchematics;
        [SerializeField] private SchematicRepository SchematicRepository;

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

        public float RealignmentSpeedPerSecond {
            get { return _realignmentSpeedPerSecond; }
        }

        public Tuple<uint, uint, uint> Dimensions {
            get { return new Tuple<uint, uint, uint>(Width, Height, Depth); }
        }

        [SerializeField] private float _realignmentSpeedPerSecond;

        #endregion

    }

}
