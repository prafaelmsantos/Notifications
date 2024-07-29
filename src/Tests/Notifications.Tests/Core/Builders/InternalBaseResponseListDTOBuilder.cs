namespace Notifications.Tests.Core.Builders
{
    public class InternalBaseResponseListDTOBuilder
    {
        public static List<InternalBaseResponseDTO> InternalBaseResponseDTOList(string? errorMessage = null, long id = 0)
        {
            return new() { new InternalBaseResponseDTO() { Id = id, Success = errorMessage is null, ErrorMessage = errorMessage } };
        }
    }
}
