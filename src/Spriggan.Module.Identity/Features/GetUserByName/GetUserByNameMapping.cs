using AutoMapper;
using Spriggan.Data.Identity.Contracts.Entities;
using Spriggan.Module.Identity.Contracts.Features.GetUserByName;

namespace Spriggan.Module.Identity.Features.GetUserByName;

public class GetUserByNameMapping : Profile
{
    public GetUserByNameMapping()
    {
        CreateMap<User, GetUserByNameResponse>();

        CreateMap<GetUserByNameResponse, User>();
    }
}
