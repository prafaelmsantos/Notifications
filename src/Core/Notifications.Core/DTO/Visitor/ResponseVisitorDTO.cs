namespace Notifications.Core.DTO.Visitor
{
    public class ResponseVisitorDTO
    {
        public List<VisitorDTO> Visitors { get; set; } = new();
        public long Value { get; set; }
        public double ValuePerc { get; set; }
    }
}
