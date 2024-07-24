namespace Notifications.Core.DTO
{
    public class ClientMessageDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public long PhoneNumber { get; set; }
        public string Message { get; set; } = null!;
        public STATUS Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

