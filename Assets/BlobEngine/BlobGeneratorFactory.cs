using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.BlobEngine {

    public class BlobGeneratorFactory : BlobGeneratorFactoryBase {

        #region instance fields and properties


        [SerializeField] private GameObject GeneratorPrefab;
        [SerializeField] private BlobGeneratorPrivateData GeneratorPrivateData;

        #endregion

        #region instance methods

        #region from BlobGeneratorFactoryBase

        public override IBlobGenerator ConstructGenerator(Transform parent, Vector3 localPosition,
            ResourceType blobTypeGenerated) {
            var generatorObject = Instantiate(GeneratorPrefab);
            var generatorBehaviour = generatorObject.GetComponent<BlobGenerator>();
            if(generatorBehaviour != null) {
                generatorBehaviour.PrivateData = GeneratorPrivateData;
                generatorBehaviour.BlobTypeGenerated = blobTypeGenerated;
                generatorBehaviour.transform.SetParent(parent);
                generatorBehaviour.transform.localPosition = localPosition;
            }else {
                throw new BlobException("The ResourcePool prefab did not contain a ResourcePool component");
            }
            return generatorBehaviour;
        }

        public override IBlobGenerator ConstructGeneratorOnGyser(IResourceGyser gyser) {
            return ConstructGenerator(gyser.transform.parent, gyser.transform.localPosition, gyser.BlobTypeGenerated);
        }

        public override Schematic BuildSchematic() {
            return new Schematic("Generator", GeneratorPrivateData.Cost, delegate(Transform locationToConstruct) {
                var gyserOnLocation = locationToConstruct.GetComponent<IResourceGyser>();
                if(gyserOnLocation != null) {
                    ConstructGeneratorOnGyser(gyserOnLocation);
                }else {
                    ConstructGenerator(locationToConstruct.parent, Vector3.zero, ResourceType.Red);
                }
            });
        }

        #endregion

        #endregion

    }

}
