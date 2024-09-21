using AutoMapper;
using PhoneBookAPI.Data;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Define mapping between ContactModel and ContactEntity
            CreateMap<ContactModel, ContactEntity>().ReverseMap();
        }
    }
}
