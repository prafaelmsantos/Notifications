namespace Notifications.Core.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClientMessage, ClientMessageDTO>().ReverseMap();

            CreateMap<Visitor, VisitorDTO>().ReverseMap();
        }
    }
}
