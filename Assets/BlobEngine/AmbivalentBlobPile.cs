using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityCustomUtilities.Extensions;

namespace Assets.BlobEngine {

    public class AmbivalentBlobPile : BlobPileBase {

        #region instance fields and properties

        #region from BlobPileBase

        public override IEnumerable<ResourceBlob> Contents {
            get { return contents; }
        }
        private HashSet<ResourceBlob> contents = new HashSet<ResourceBlob>();

        #endregion

        public int Capacity {
            get { return _capacity; }
        }
        private int _capacity;

        #endregion

        #region constructors

        public AmbivalentBlobPile(int capacity) {
            _capacity = capacity;
        }

        #endregion

        #region instance methods

        #region from BlobPileBase

        public override bool CanPlaceBlobInto(ResourceBlob blob){
            return contents.Count < Capacity;
        }

        public override bool CanPlaceBlobOfTypeInto(ResourceType type){
            return contents.Count < Capacity;
        }

        public override void PlaceBlobInto(ResourceBlob blob){
            if(CanPlaceBlobInto(blob)) {
                contents.Add(blob);
            }else {
                throw new BlobException("Cannot place this blob into this BlobPile");
            }
        }    

        public override bool CanExtractAnyBlob(){
            return contents.Count > 0;
        }

        public override ResourceBlob ExtractAnyBlob(){
            if(CanExtractAnyBlob()) {
                var retval = contents.Last();
                contents.Remove(retval);
                return retval;
            }else {
                throw new BlobException("Cannot extract a blob from this BlobPile");
            }
            
        }

        public override bool CanExtractBlobOfType(ResourceType type){
            return contents.Where((blob) => blob.BlobType == type).Count() > 0;
        }

        public override ResourceBlob ExtractBlobOfType(ResourceType type){
            if(CanExtractBlobOfType(type)) {
                var retval = contents.Where((blob) => blob.BlobType == type).Last();
                contents.Remove(retval);
                return retval;
            }else {
                throw new BlobException("Cannot extract a blob of this type from this BlobPile");
            }
        }

        public override bool TryExtractBlobFrom(ResourceBlob blob){
            return contents.Remove(blob);
        }

        public override void Clear(){
            contents.Clear();
        }

        public override IEnumerable<ResourceType> GetAllTypesWithin(){
            var retval = new HashSet<ResourceType>();
            foreach(var blob in contents) {
                retval.Add(blob.BlobType);
            }
            return retval;
        }

        public override IEnumerable<ResourceBlob> GetAllBlobsOfType(ResourceType type){
            return contents.Where((blob) => blob.BlobType == type);
        }

        public override Dictionary<ResourceType, IEnumerable<ResourceBlob>> GetAllBlobsOfAllTypes(){
            var retval = new Dictionary<ResourceType, IEnumerable<ResourceBlob>>();
            foreach(var resourceType in EnumUtil.GetValues<ResourceType>()) {
                retval[resourceType] = contents.Where((blob) => blob.BlobType == resourceType);
            }
            return retval;
        }

        public override int GetSpaceLeftForBlobOfType(ResourceType type){
            return Capacity - contents.Count;
        }

        public override bool IsAtCapacity() {
            return contents.Count >= Capacity;
        }

        #endregion

        #endregion

    }

}
