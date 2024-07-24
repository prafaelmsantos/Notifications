using AutoMoreira.Base.Lib.Grpc.Base.Response;

namespace Notifications.GrpcServer.Services
{
    public class NotificationsGrpcServerService : INotificationsGrpcServerService
    {
        #region Vars
        private readonly IEmailService _emailService;
        #endregion

        #region Constructors
        public NotificationsGrpcServerService(IEmailService emailService)
        {
            _emailService = emailService;
        }
        #endregion

        #region Public methods
        public async Task<ResponseGrpc> SendWelcomeEmailAsync(SendWelcomeEmailRequestGrpc request, CallContext context = default)
        {
            await _emailService.SendWelcomeEmailAsync(request.Name, request.Address, request.Password);
            return new ResponseGrpc();
        }

        public async Task<ResponseGrpc> SendUserProfileUpdatedEmailAsync(SendUserProfileUpdatedEmailRequestGrpc request, CallContext context = default)
        {
            await _emailService.SendUserProfileUpdatedEmailAsync(request.Name, request.Address);
            return new ResponseGrpc();
        }

        public async Task<ResponseGrpc> SendClientEmailAsync(SendClientEmailRequestGrpc request, CallContext context = default)
        {
            try
            {
                //await _emailService.SendClientEmailAsync(request.Name, request.Address);
                return new ResponseGrpc();
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public async Task<ResponseGrpc> SendPasswordChangedEmailAsync(SendPasswordChangedEmailRequestGrpc request, CallContext context = default)
        {
            await _emailService.SendPasswordChangedEmailAsync(request.Name, request.Address, request.Password);
            return new ResponseGrpc();
        }

        public async Task<ResponseGrpc> SendPasswordResetEmailAsync(SendPasswordResetEmailRequestGrpc request, CallContext context = default)
        {
            await _emailService.SendPasswordResetEmailAsync(request.Name, request.Address, request.Password);
            return new ResponseGrpc();
        }

        #endregion
    }
}
