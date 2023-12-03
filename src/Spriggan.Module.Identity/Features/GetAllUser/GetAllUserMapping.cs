using AutoMapper;
using Spriggan.Data.Identity.Contracts.Entities;

namespace Spriggan.Module.Identity.Features.GetAllUser;

public class GetAllUserMapping : Profile
{
    public GetAllUserMapping()
    {
        CreateMap<User, Contracts.Models.User>();

        CreateMap<Contracts.Models.User, User>();
    }
}
