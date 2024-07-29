namespace Notifications.Persistence.Interfaces.Services
{
    public interface IClientMessageService
    {
        Task<List<ClientMessageDTO>> GetAllClientMessagesAsync();
        Task<ClientMessageDTO> GetClientMessageByIdAsync(long clientMessageId);
        Task<ClientMessageDTO> AddClientMessageAsync(ClientMessageDTO clientMessageDTO);
        Task<ClientMessageDTO> UpdateClientMessageStatusAsync(long id, STATUS status);
        Task<List<InternalBaseResponseDTO>> DeleteClientMessagesAsync(List<long> clientMessagesIds);
    }
}
