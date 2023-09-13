using AutoMapper;
using Par.CommandCenter.Application.Common.Mappings;
using Par.CommandCenter.Domain.Entities;
using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.States.Queries.GetAllStates
{
    public class GetAllStatesResponse
    {
        public List<StateModel> States { get; set; }
    }

    public class StateModel : IMap<State>
    {
        public int Id { get; internal set; }

        public string Name { get; set; }

        public string StateCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<State, StateModel>();

        }
    }
}
