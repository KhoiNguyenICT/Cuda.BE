using AutoMapper;
using Cuda.Model.Entities;
using Cuda.Service.Dtos.LoginHistory;

namespace Cuda.Service.Mapper
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<Asset, Asset>();
        }
    }
}