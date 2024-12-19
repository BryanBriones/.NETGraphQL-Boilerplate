using AutoMapper;
using Data;
using Schema.Queries;

using System.Globalization;

namespace Schema.AutoMapper;

public class MappingProfile : Profile
{

    public MappingProfile()
    {

        CreateMap<UserLogin, UserLoginType>();
        
    }
}
