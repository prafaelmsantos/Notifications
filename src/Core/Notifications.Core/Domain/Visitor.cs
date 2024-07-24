namespace Notifications.Core.Domain
{
    public class Visitor : EntityBase
    {
        public int Year { get; private set; }
        public MONTH Month { get; private set; }
        public long Value { get; private set; }

        public Visitor()
        {
            Year = DateTime.UtcNow.Year;
            Month = (MONTH)DateTime.UtcNow.Month;
            Value = 1;
        }

        public void SetValue()
        {
            Value++;
        }
    }
}
