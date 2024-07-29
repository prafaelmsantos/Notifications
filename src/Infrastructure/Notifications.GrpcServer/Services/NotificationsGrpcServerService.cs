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
        public async Task SendWelcomeEmailAsync(SendWelcomeEmailRequestGrpc request, CallContext context = default)
        {
            try
            {
                await _emailService.SendWelcomeEmailAsync(request.Name, request.Address, request.Password);
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public async Task SendUserProfileUpdatedEmailAsync(SendUserProfileUpdatedEmailRequestGrpc request, CallContext context = default)
        {
            try
            {
                await _emailService.SendUserProfileUpdatedEmailAsync(request.Name, request.Address);
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public async Task SendClientEmailAsync(SendClientEmailRequestGrpc request, CallContext context = default)
        {
            try
            {
                await _emailService.SendClientEmailAsync(request.Name, request.Address);
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public async Task SendPasswordChangedEmailAsync(SendPasswordChangedEmailRequestGrpc request, CallContext context = default)
        {
            try
            {
                await _emailService.SendPasswordChangedEmailAsync(request.Name, request.Address, request.Password);
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public async Task SendPasswordResetEmailAsync(SendPasswordResetEmailRequestGrpc request, CallContext context = default)
        {
            try
            {
                await _emailService.SendPasswordResetEmailAsync(request.Name, request.Address, request.Password);
            }
            catch (RpcException ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        #endregion
    }
}
