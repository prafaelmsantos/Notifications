using Notifications.Core.DTO.Visitor;

namespace Notifications.Persistence.Interfaces.Services
{
    public interface IVisitorService
    {
        Task<VisitorCounterDTO> GetVisitorCountersAsync();
        Task<ResponseVisitorDTO> GetAllVisitoresWithMonthComparisonAsync();
        Task<ResponseCompleteVisitorDTO> GetAllVisitoresWithYearComparisonAsync();
        Task<VisitorDTO> CreateOrUpdateVisitorAsync();
    }
}
