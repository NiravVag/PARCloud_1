using AutoMapper;


namespace Par.CommandCenter.Application.Common.Mappings
{
    public interface IMap<T>
    {
        void Mapping(Profile profile);
    }
}
