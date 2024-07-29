namespace Notifications.GraphQL
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:All members as static")]
    public class Query
    {
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ClientMessage> GetClientMessages([Service] IClientMessageRepository repo)
        {
            return repo.GetAll();
        }
    }
}
