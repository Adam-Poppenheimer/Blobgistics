using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Map;

namespace Assets.BlobEngine {

    public class BlobGeneratorFactory : BlobGeneratorFactoryBase {

        #region instance fields and properties


        [SerializeField] private GameObject GeneratorPrefab;
        [SerializeField] private BlobGeneratorPrivateData GeneratorPrivateData;

        #endregion

        #region instance methods

        #region from BlobGeneratorFactoryBase

        public override IBlobGenerator ConstructGenerator(MapNode location, ResourceType blobTypeGenerated) {
            var generatorObject = Instantiate(GeneratorPrefab);
            var generatorBehaviour = generatorObject.GetComponent<BlobGenerator>();
            if(generatorBehaviour != null) {
                generatorBehaviour.PrivateData = GeneratorPrivateData;
                generatorBehaviour.BlobTypeGenerated = blobTypeGenerated;
                generatorBehaviour.Location = location;
            }else {
                throw new BlobException("The ResourcePool prefab did not contain a ResourcePool component");
            }
            return generatorBehaviour;
        }

        public override IBlobGenerator ConstructGeneratorOnGyser(IResourceGyser gyser) {
            return ConstructGenerator(gyser.Location, gyser.BlobTypeGenerated);
        }

        public override Schematic BuildSchematic() {
            return new Schematic("Generator", GeneratorPrivateData.Cost, delegate(MapNode locationToConstruct) {
                var gyserOnLocation = locationToConstruct.GetComponent<IResourceGyser>();
                if(gyserOnLocation != null) {
                    ConstructGeneratorOnGyser(gyserOnLocation);
                }else {
                    ConstructGenerator(locationToConstruct, ResourceType.Red);
                }
            });
        }

        #endregion

        #endregion

    }

}
