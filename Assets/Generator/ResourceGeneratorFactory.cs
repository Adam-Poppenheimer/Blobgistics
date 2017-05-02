using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Assets.Blobs;
using Assets.Map;
using Assets.Societies;
using Assets.ResourceDepots;

namespace Assets.Generator {

    [ExecuteInEditMode]
    public class ResourceGeneratorFactory : MonoBehaviour {

        #region instance fields and properties

        [SerializeField] private ResourceBlobFactoryBase  BlobFactory;
        [SerializeField] private SocietyFactoryBase       SocietyFactory;
        [SerializeField] private ResourceDepotFactoryBase DepotFactory;

        [SerializeField] private GameObject GeneratorPrefab;

        [SerializeField] private List<ResourceGenerator> InstantiatedGenerators =
            new List<ResourceGenerator>();

        #endregion

        #region instance methods

        public bool CanConstructGeneratorAtLocation(MapNodeBase location) {
            return !SocietyFactory.HasSocietyAtLocation(location) &&
                   !DepotFactory.HasDepotAtLocation(location);
        }

        public bool ConstructGeneratorAtLocation(MapNodeBase location) {
            if(CanConstructGeneratorAtLocation(location)) {
                var clonedPrefab = Instantiate(GeneratorPrefab);
                var newGenerator = clonedPrefab.GetComponent<ResourceGenerator>();

                newGenerator.transform.SetParent(location.transform, false);

                newGenerator.Location = location;
                newGenerator.BlobFactory = BlobFactory;
                newGenerator.ParentFactory = this;

                InstantiatedGenerators.Add(newGenerator);
                return newGenerator;
            }else {
                throw new InvalidOperationException("Cannot construct a generator at this location");
            }
        }

        public void DestroyGenerator(ResourceGenerator generator) {
            UnsubscribeGenerator(generator);
            if(Application.isPlaying) {
                Destroy(generator.gameObject);
            }else {
                DestroyImmediate(generator.gameObject);
            }
        }

        public void UnsubscribeGenerator(ResourceGenerator generator) {
            InstantiatedGenerators.Remove(generator);
        }

        public void TickAllGenerators(float secondsPassed) {
            foreach(var generator in InstantiatedGenerators) {
                generator.TickProduction(secondsPassed);
            }
        }

        #endregion

    }

}
