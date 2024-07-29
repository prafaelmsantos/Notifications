namespace Notifications.Tests.Core.Builders
{
    public class ClientMessageBuilder
    {
        private static readonly Faker data = new("en");

        public static ClientMessage ClientMessage()
        {
            return new(data.Person.FullName, data.Person.Email, data.Random.Long(200000000, 999999999), data.Lorem.Text());
        }
        public static ClientMessageDTO ClientMessageDTO()
        {
            return new()
            {
                Id = data.Random.Long(1),
                Name = data.Person.FullName,
                Email = data.Person.Email,
                PhoneNumber = data.Random.Long(200000000, 999999999),
                Message = data.Lorem.Text(),
                Status = data.Random.Enum<STATUS>(),
                CreatedDate = DateTime.UtcNow
            };
        }
        public static ClientMessage ClientMessage(ClientMessageDTO dto)
        {
            return new(dto.Name, dto.Email, dto.PhoneNumber, dto.Message);
        }
        public static List<ClientMessage> ClientMessageList(ClientMessageDTO dto)
        {
            return new List<ClientMessage>() { ClientMessage(dto) };
        }
        public static List<ClientMessageDTO> ClientMessageDTO(ClientMessageDTO dto)
        {
            return new List<ClientMessageDTO>() { dto };
        }
        public static IQueryable<ClientMessage> IQueryable(ClientMessageDTO dto)
        {
            return ClientMessageList(dto).AsQueryable();
        }
        public static IQueryable<ClientMessage> IQueryableEmpty()
        {
            return new List<ClientMessage>().AsQueryable();
        }
    }
}
