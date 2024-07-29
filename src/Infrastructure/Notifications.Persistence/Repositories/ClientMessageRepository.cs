namespace Notifications.Persistence.Repositories
{
    public class ClientMessageRepository : Repository<ClientMessage>, IClientMessageRepository
    {
        public ClientMessageRepository(AppDbContext context) : base(context) { }
    }
}
