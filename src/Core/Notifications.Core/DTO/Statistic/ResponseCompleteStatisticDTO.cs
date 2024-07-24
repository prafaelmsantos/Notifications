namespace Notifications.Core.DTO.Statistic
{
    public class ResponseCompleteStatisticDTO : ResponseStatisticDTO
    {
        public List<StatisticDTO> LastStatistics { get; set; } = new();
    }
}
