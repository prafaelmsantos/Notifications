namespace Notifications.Core.DTO.Visitor
{
    public class ResponseCompleteVisitorDTO : ResponseVisitorDTO
    {
        public List<VisitorDTO> LastVisitors { get; set; } = new();
    }
}
