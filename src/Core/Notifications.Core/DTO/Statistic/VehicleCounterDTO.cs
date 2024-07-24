namespace Notifications.Core.DTO.Statistic
{
    public class VehicleCounterDTO
    {
        public CounterDTO TotalSales { get; set; } = null!;
        public CounterDTO TotalSalesMonth { get; set; } = null!;
        public CounterDTO TotalStock { get; set; } = null!;
    }
}
