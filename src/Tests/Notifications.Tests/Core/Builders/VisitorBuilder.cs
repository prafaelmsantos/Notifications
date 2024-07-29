namespace Notifications.Tests.Core.Builders
{
    public class VisitorBuilder
    {
        private static readonly Faker data = new("en");

        public static Visitor Visitor()
        {
            return new();
        }
        public static VisitorDTO VisitorDTO()
        {
            return new()
            {
                Id = data.Random.Int(1),
                Year = data.Date.RecentDateOnly().Year,
                Month = (MONTH)data.Date.RecentDateOnly().Month,
                Value = data.Random.Int(1)
            };
        }
        public static VisitorDTO VisitorDTO(Visitor visitor)
        {
            return new()
            {
                Id = visitor.Id,
                Year = visitor.Year,
                Month = visitor.Month,
                Value = visitor.Value
            };
        }
        public static List<Visitor> VisitorList(Visitor visitor)
        {
            return new List<Visitor>() { visitor };
        }
        public static List<VisitorDTO> VisitorDTOList(VisitorDTO dto)
        {
            return new List<VisitorDTO>() { dto };
        }
        public static IQueryable<Visitor> IQueryable(Visitor visitor)
        {
            return VisitorList(visitor).AsQueryable();
        }
        public static IQueryable<Visitor> IQueryableEmpty()
        {
            return new List<Visitor>().AsQueryable();
        }
    }
}
