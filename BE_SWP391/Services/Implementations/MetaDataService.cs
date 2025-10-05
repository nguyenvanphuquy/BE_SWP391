using BE_SWP391.Models.Entities;
using BE_SWP391.Models.DTOs.Request;
using BE_SWP391.Models.DTOs.Response;
using BE_SWP391.Repositories.Interfaces;
using BE_SWP391.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BE_SWP391.Services.Implementations
{
    public class MetaDataService : IMetaDataService
    {
        private readonly IMetaDataRepository _metaDataRepository;
        public MetaDataService(IMetaDataRepository metaDataRepository)
        {
            _metaDataRepository = metaDataRepository;
        }
        public MetaDataResponse? GetById(int id)
        {
            var metadata = _metaDataRepository.GetById(id);
            return metadata == null ? null : ToResponse(metadata);
        }
        public IEnumerable<MetaDataResponse> GetAll()
        {
            return _metaDataRepository.GetAll().Select(ToResponse);
        }
        public MetaDataResponse? Create(MetaDataRequest request)
        {
            var metadata = new MetaData
            {
                Title = request.Title,
                Description = request.Description,
                Keywords = request.Keywords,
                FileFormat = request.FileFormat,
                FileSize = request.FileSize,
                CreatedAt = DateTime.Now,
            };
            _metaDataRepository.Create(metadata);
            return ToResponse(metadata);
             
        }
        public MetaDataResponse? Update(int id, MetaDataRequest request)
        {
            var metadata = _metaDataRepository.GetById(id);
            if (metadata == null) return null;
            metadata.Title = request.Title;
            metadata.Description = request.Description;
            metadata.Keywords = request.Keywords;
            metadata.FileFormat = request.FileFormat;
            metadata.FileSize = request.FileSize;
            _metaDataRepository.Update(metadata);
            return ToResponse(metadata);
        }
        public bool Delete(int id)
        {
            var metadata = _metaDataRepository.GetById(id);
            if (metadata == null) return false;
            _metaDataRepository.Delete(id);
            return true;
        }
        public static MetaDataResponse ToResponse(MetaData metadata)
        {
            return new MetaDataResponse
            {
                MetadataId = metadata.MetadataId,
                Title = metadata.Title,
                Description = metadata.Description,
                Keywords = metadata.Keywords,
                FileFormat = metadata.FileFormat,
                FileSize = metadata.FileSize,
                CreatedAt = metadata.CreatedAt
            };
        }
    }
}
