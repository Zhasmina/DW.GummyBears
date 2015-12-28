using GummyBears.Contracts;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Helpers
{
    public static class CreationMapper
    {
        public static CreationEntity ToEntity(this Creation creation)
        {
            return new CreationEntity
            {
                FilePath = creation.CreationPath,
                Name = creation.CreationName,
                Id = creation.CreationId,
                UserId = creation.UserId,
                Signiture = creation.Signature,
                CreationFootprint = creation.Footprint
            };
        }

        public static Creation ToContract(this CreationEntity creation)
        {
            return new Creation
            {
                CreationId = creation.Id,
                CreationName = creation.Name,
                CreationPath = creation.FilePath,
                UserId = creation.UserId
            };
        }
    }
}