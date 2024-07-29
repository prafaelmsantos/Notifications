namespace Notifications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorsController : ControllerBase
    {
        #region Properties

        private readonly IVisitorService _visitorService;

        #endregion

        #region Constructors

        public VisitorsController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        #endregion

        #region CRUD Methods

        /// <summary> Get Line Chart </summary>
        [HttpGet("LineChart")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetLineChart()
        {
            try
            {
                return Ok(await _visitorService.GetAllVisitoresWithYearComparisonAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Get Bar Chart </summary>
        [HttpGet("BarChart")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetBarChart()
        {
            try
            {
                return Ok(await _visitorService.GetAllVisitoresWithMonthComparisonAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        /// <summary> Get Counters </summary>
        [HttpGet("Counters")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCounters()
        {
            try
            {
                return Ok(await _visitorService.GetVisitorCountersAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Create/Update Visitor </summary>
        [HttpPost()]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Post()
        {
            try
            {
                return Ok(await _visitorService.CreateOrUpdateVisitorAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        #endregion
    }
}
