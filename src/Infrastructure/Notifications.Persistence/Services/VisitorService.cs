namespace Notifications.Persistence.Services
{
    public class VisitorService : IVisitorService
    {
        #region Private variables

        private readonly IMapper _mapper;
        private readonly IVisitorRepository _visitorRepository;

        #endregion

        #region Constructors
        public VisitorService(IMapper mapper, IVisitorRepository visitorRepository)
        {
            _mapper = mapper;
            _visitorRepository = visitorRepository;
        }
        #endregion

        #region Public methods

        public async Task<VisitorCounterDTO> GetVisitorCountersAsync()
        {
            try
            {
                var visitorsDTO = await GetAllVisitoresByYear(DateTime.UtcNow.Year);

                return new()
                {
                    Total = visitorsDTO.Select(x => x.Value).Sum(),
                    TotalMonth = visitorsDTO.Where(x => (int)x.Month == DateTime.UtcNow.Month)?.FirstOrDefault()?.Value ?? 0
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResource.GetVisitorCountersAsyncException} {ex.Message}");
            }
        }

        public async Task<ResponseCompleteVisitorDTO> GetAllVisitoresWithYearComparisonAsync()
        {
            try
            {
                var currentVisitors = await GetAllVisitoresByYear(DateTime.UtcNow.Year);
                var lastVisitors = await GetAllVisitoresByYear(DateTime.UtcNow.Year - 1);

                var values = GetValuesWithYearComparison(lastVisitors, currentVisitors);

                return new()
                {
                    Visitors = currentVisitors,
                    LastVisitors = lastVisitors,
                    Value = values.Item1,
                    ValuePerc = values.Item2
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResource.GetAllVisitorsWithYearComparisonAsyncException} {ex.Message}");
            }
        }

        public async Task<ResponseVisitorDTO> GetAllVisitoresWithMonthComparisonAsync()
        {
            try
            {
                var visitorsDTO = await GetAllVisitoresByYear(DateTime.UtcNow.Year);

                var values = GetValuesWithMonthComparison(visitorsDTO);

                return new()
                {
                    Visitors = visitorsDTO,
                    Value = values.Item1,
                    ValuePerc = values.Item2
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResource.GetAllVisitorsWithMonthComparisonAsyncException} {ex.Message}");
            }
        }

        public async Task<VisitorDTO> CreateOrUpdateVisitorAsync()
        {
            try
            {
                Visitor? visitor = await _visitorRepository
                    .GetAll()
                    .Where(x => x.Year == DateTime.UtcNow.Year && (int)x.Month == DateTime.UtcNow.Month)
                    .FirstOrDefaultAsync();

                if (visitor is null)
                {
                    visitor = new Visitor();
                    await _visitorRepository.AddAsync(visitor);
                }
                else
                {
                    visitor.SetValue();
                    await _visitorRepository.UpdateAsync(visitor);
                }

                return _mapper.Map<VisitorDTO>(visitor);
            }
            catch (Exception ex)
            {
                throw new Exception($"{DomainResource.CreateOrUpdateVisitorAsyncException} {ex.Message}");
            }
        }

        #endregion

        #region Private methods

        private async Task<List<VisitorDTO>> GetAllVisitoresByYear(int year)
        {
            List<Visitor> visitors = await _visitorRepository
                     .GetAll()
                     .Where(x => x.Year == year)
                     .ToListAsync();

            List<VisitorDTO> visitorsDTO = _mapper.Map<List<VisitorDTO>>(visitors);

            var monthList = Enum.GetValues(typeof(MONTH)).Cast<MONTH>().ToList();

            var visitorsEmpty = monthList.Except(visitorsDTO.Select(x => x.Month)).ToList();

            visitorsEmpty.ForEach(month =>
                visitorsDTO.Add(new VisitorDTO() { Month = month, Year = DateTime.UtcNow.Year, Value = 0 }));

            return visitorsDTO.OrderBy(x => x.Month).ToList();
        }

        private static (long, double) GetValuesWithYearComparison(List<VisitorDTO> lastVisitors, List<VisitorDTO> currentVisitors)
        {
            long lastValue = lastVisitors.Select(x => x.Value).Sum();
            long currentValue = currentVisitors.Select(x => x.Value).Sum();
            var valuePerc = lastValue != 0 ? (double)(currentValue - lastValue) / lastValue * 100 : currentValue != 0 ? 100 : 0;

            return (currentValue, Math.Round(valuePerc, 1));
        }

        private static (long, double) GetValuesWithMonthComparison(List<VisitorDTO> visitorsDTO)
        {
            long currentMonthValue = visitorsDTO.Where(x => (int)x.Month == DateTime.UtcNow.Month)?.FirstOrDefault()?.Value ?? 0;
            long lastMonthValue = visitorsDTO.Where(x => (int)x.Month == DateTime.UtcNow.Month - 1)?.FirstOrDefault()?.Value ?? 0;
            var valuePerc = lastMonthValue != 0 ? (double)(currentMonthValue - lastMonthValue) / lastMonthValue * 100 : currentMonthValue != 0 ? 100 : 0;

            return (currentMonthValue, Math.Round(valuePerc, 1));
        }

        #endregion
    }
}
