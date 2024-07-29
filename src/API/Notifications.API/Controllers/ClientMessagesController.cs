namespace Notifications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientMessagesController : ControllerBase
    {
        #region Properties

        private readonly IClientMessageService _clientMessageService;

        #endregion

        #region Constructors
        public ClientMessagesController(IClientMessageService clientMessageService)
        {
            _clientMessageService = clientMessageService;
        }
        #endregion

        #region CRUD Methods

        /// <summary> Get All Client Messages </summary>
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _clientMessageService.GetAllClientMessagesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Get Client Message </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                return Ok(await _clientMessageService.GetClientMessageByIdAsync(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Create Client Message </summary>
        /// <param name="clientMessageDTO"></param>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] ClientMessageDTO clientMessageDTO)
        {
            try
            {
                return Ok(await _clientMessageService.AddClientMessageAsync(clientMessageDTO));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Update Client Message Status </summary>
        /// <param name="status"></param>
        /// <param name="id"></param>
        [HttpPut("Status/{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> PutStatus([FromRoute] int id, [FromBody] STATUS status)
        {
            try
            {
                return Ok(await _clientMessageService.UpdateClientMessageStatusAsync(id, status));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        /// <summary> Delete Client Messages </summary>
        /// <param name="clientMessagesIds"></param>
        [HttpPost("Delete")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete([FromBody] List<long> clientMessagesIds)
        {
            try
            {
                return Ok(await _clientMessageService.DeleteClientMessagesAsync(clientMessagesIds));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        #endregion
    }
}
