using AutoMapper;
using PickupAnnouncerLegacy.Models.DAO.Config;
using PickupAnnouncerLegacy.Models.DAO.Data;
using PickupAnnouncerLegacy.Models.DTO;
using PickupAnnouncerLegacy.Models.Requests;

namespace PickupAnnouncerLegacy.Mappings
{
    public class BaseProfile : Profile
    {
        public BaseProfile()
        {
            CreateMap<StudentDAO, StudentDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<GradeLevelRequest, GradeLevel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
