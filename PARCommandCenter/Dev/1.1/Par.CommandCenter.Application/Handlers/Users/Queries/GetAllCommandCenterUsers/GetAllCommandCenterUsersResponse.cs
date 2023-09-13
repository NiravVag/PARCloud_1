using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities.Users;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Users.Queries.GetAllCommandCenterUsers
{
    public class GetAllCommandCenterUsersResponse
    {
        public IEnumerable<UserModel> Users { get; set; }
    }

    public class UserModel : IMap<User>
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserModel>();
        }
    }
}
