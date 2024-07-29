namespace Notifications.Tests.Infrastructure.Persistence
{
    public class ClientMessageServiceTests : BaseClassTests
    {
        #region Private variables

        private readonly Mock<IClientMessageRepository> _clientMessageRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly IClientMessageService _clientMessageService;

        #endregion

        #region Constructors

        public ClientMessageServiceTests(ITestOutputHelper output) : base(output)
        {
            _clientMessageRepositoryMock = new Mock<IClientMessageRepository>(MockBehavior.Strict);
            _emailServiceMock = new Mock<IEmailService>(MockBehavior.Strict);
            _clientMessageService = new ClientMessageService(Mapper, _clientMessageRepositoryMock.Object, _emailServiceMock.Object);
        }

        #endregion

        #region GetAllClientMessagesAsync

        [Fact]
        public async Task GetAllClientMessagesAsync_GetAll_Successfully()
        {
            // Arrange   
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Id = 0;

            IQueryable<ClientMessage> clientMessages = ClientMessageBuilder.IQueryable(dto);

            _clientMessageRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<ClientMessage>(clientMessages));

            // Act
            List<ClientMessageDTO> result = await _clientMessageService.GetAllClientMessagesAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(clientMessages);

            _clientMessageRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllClientMessagesAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            _clientMessageRepositoryMock.Setup(x => x.GetAll()).Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _clientMessageService.GetAllClientMessagesAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.GetAllClientMessagesAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region GetMarkByIdAsync

        [Fact]
        public async Task GetClientMessageByIdAsync_ValidClientMessage_Successfully()
        {
            // Arrange   
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Status = STATUS.Open;

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(ClientMessageBuilder.ClientMessage(dto));

            // Act
            ClientMessageDTO result = await _clientMessageService.GetClientMessageByIdAsync(dto.Id);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.Email.Should().Be(dto.Email);
            result.PhoneNumber.Should().Be(dto.PhoneNumber);
            result.Message.Should().Be(dto.Message);
            result.Status.Should().Be(dto.Status).And.Be(STATUS.Open);
            result.CreatedDate.Date.Should().Be(dto.CreatedDate.Date);
            result.CreatedDate.Hour.Should().Be(dto.CreatedDate.Hour);
            result.CreatedDate.Minute.Should().Be(dto.CreatedDate.Minute);

            _clientMessageRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetClientMessageByIdAsync_ClientMessageNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _clientMessageService.GetClientMessageByIdAsync(0))
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.GetClientMessageByIdAsyncException} {DomainResource.ClientMessageNotFoundException}");
        }

        [Fact]
        public async Task GetClientMessageByIdAsync_FindByIdAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _clientMessageService.GetClientMessageByIdAsync(0))
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.GetClientMessageByIdAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region AddClientMessageAsync

        [Fact]
        public async Task AddClientMessageAsync_ValidClientMessage_Successfully()
        {
            // Arrange   
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Status = STATUS.Open;

            _clientMessageRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ClientMessage>()))
                .ReturnsAsync(ClientMessageBuilder.ClientMessage(dto));

            _emailServiceMock.Setup(repo => repo.SendClientEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            ClientMessageDTO result = await _clientMessageService.AddClientMessageAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.Email.Should().Be(dto.Email);
            result.PhoneNumber.Should().Be(dto.PhoneNumber);
            result.Message.Should().Be(dto.Message);
            result.Status.Should().Be(dto.Status).And.Be(STATUS.Open);
            result.CreatedDate.Date.Should().Be(dto.CreatedDate.Date);
            result.CreatedDate.Hour.Should().Be(dto.CreatedDate.Hour);
            result.CreatedDate.Minute.Should().Be(dto.CreatedDate.Minute);

            _clientMessageRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<ClientMessage>()), Times.Once);
            _emailServiceMock.Verify(repo => repo.SendClientEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddClientMessageAsync_AddAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();

            _clientMessageRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ClientMessage>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _clientMessageService.AddClientMessageAsync(dto))
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.AddClientMessageAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region UpdateClientMessageStatusAsync

        [Fact]
        public async Task UpdateClientMessageStatusAsync_SetStatus_Successfully()
        {
            // Arrange   
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            ClientMessage clientMessage = ClientMessageBuilder.ClientMessage(dto);

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(clientMessage);

            _clientMessageRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<ClientMessage>()))
                .ReturnsAsync(clientMessage);

            // Act
            ClientMessageDTO result = await _clientMessageService.UpdateClientMessageStatusAsync(It.IsAny<long>(), dto.Status);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(dto.Name);
            result.Email.Should().Be(dto.Email);
            result.PhoneNumber.Should().Be(dto.PhoneNumber);
            result.Message.Should().Be(dto.Message);
            result.Status.Should().Be(dto.Status);
            result.CreatedDate.Date.Should().Be(dto.CreatedDate.Date);
            result.CreatedDate.Hour.Should().Be(dto.CreatedDate.Hour);
            result.CreatedDate.Minute.Should().Be(dto.CreatedDate.Minute);

            _clientMessageRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _clientMessageRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<ClientMessage>()), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateClientMessageStatusAsync_ClientMessageNotFoundException_ThrowsExceptionAsync()
        {
            // Arrange   
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null!);

            // Act & Assert
            await FluentActions.Invoking(async () => await _clientMessageService.UpdateClientMessageStatusAsync(It.IsAny<long>(), It.IsAny<STATUS>()))
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.UpdateClientMessageAsyncException} {DomainResource.ClientMessageNotFoundException}");
        }

        [Fact]
        public async Task UpdateClientMessageStatusAsync_UpdateAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(ClientMessageBuilder.ClientMessage(dto));

            _clientMessageRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<ClientMessage>(ClientMessageBuilder.IQueryableEmpty()));

            _clientMessageRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<ClientMessage>()))
                .ThrowsAsync(new Exception());

            // Act & Assert
            await FluentActions.Invoking(async () => await _clientMessageService.UpdateClientMessageStatusAsync(It.IsAny<long>(), It.IsAny<STATUS>()))
                .Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.UpdateClientMessageAsyncException} {DomainResource.ClientMessageStatusNeedsToBeSpecifiedException}");
        }

        #endregion

        #region DeleteClientMessagesAsync

        [Fact]
        public async Task DeleteClientMessagesAsync_ValidClientMessage_Successfully()
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Id = 0;
            ClientMessage clientMessage = ClientMessageBuilder.ClientMessage(dto);
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseResponseListDTOBuilder.InternalBaseResponseDTOList(null, clientMessage.Id);

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(clientMessage);

            _clientMessageRepositoryMock.Setup(repo => repo.RemoveAsync(It.IsAny<ClientMessage>())).ReturnsAsync(true);

            // Act
            List<InternalBaseResponseDTO> results = await _clientMessageService.DeleteClientMessagesAsync(new List<long> { dto.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _clientMessageRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _clientMessageRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<ClientMessage>()), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteClientMessagesAsync_InvalidClientMessage_ClientMessageNotFoundException()
        {
            // Arrange
            string errorMessage = DomainResource.ClientMessageNotFoundException;
            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseResponseListDTOBuilder.InternalBaseResponseDTOList(errorMessage);

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null!);

            // Act
            List<InternalBaseResponseDTO> results = await _clientMessageService.DeleteClientMessagesAsync(new List<long> { 0 });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _clientMessageRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteClientMessagesAsync_FindByIdAsyncNotBreak_DeleteClientMessagesAsyncException()
        {
            // Arrange
            string errorMessage = DomainResource.DeleteClientMessagesAsyncException;

            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseResponseListDTOBuilder.InternalBaseResponseDTOList(errorMessage);

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ThrowsAsync(new Exception());

            // Act
            List<InternalBaseResponseDTO> results = await _clientMessageService.DeleteClientMessagesAsync(new List<long> { 0 });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _clientMessageRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteClientMessagesAsync_RemoveAsyncNotBreak_DeleteClientMessagesAsyncException()
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Id = 0;
            ClientMessage clientMessage = ClientMessageBuilder.ClientMessage(dto);
            string errorMessage = DomainResource.DeleteClientMessagesAsyncException;

            List<InternalBaseResponseDTO> internalBaseResponseDTOs = InternalBaseResponseListDTOBuilder.InternalBaseResponseDTOList(errorMessage, clientMessage.Id);

            _clientMessageRepositoryMock.Setup(repo => repo.FindByIdAsync(It.IsAny<long>())).ReturnsAsync(clientMessage);

            _clientMessageRepositoryMock.Setup(repo => repo.RemoveAsync(It.IsAny<ClientMessage>())).ThrowsAsync(new Exception());

            // Act
            List<InternalBaseResponseDTO> results = await _clientMessageService.DeleteClientMessagesAsync(new List<long> { dto.Id });

            // Assert
            results.Should().NotBeEmpty();
            results.Should().BeEquivalentTo(internalBaseResponseDTOs);

            _clientMessageRepositoryMock.Verify(repo => repo.FindByIdAsync(It.IsAny<long>()), Times.Once);
            _clientMessageRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<ClientMessage>()), Times.Once);
            _clientMessageRepositoryMock.VerifyNoOtherCalls();
        }
        #endregion
    }
}
