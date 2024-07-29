namespace Notifications.Tests.Core.Domains
{
    public class ClientMessageTests : BaseClassTests
    {
        public ClientMessageTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();

            // Act
            ClientMessage clientMessage = ClientMessageBuilder.ClientMessage(dto);

            // Assert
            clientMessage.Name.Should().Be(dto.Name).And.NotBeNullOrWhiteSpace();
            clientMessage.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            clientMessage.PhoneNumber.Should().Be(dto.PhoneNumber).And.BeGreaterThanOrEqualTo(200000000).And.BeLessThanOrEqualTo(999999999);
            clientMessage.Message.Should().Be(dto.Message).And.NotBeNullOrWhiteSpace();
            clientMessage.Status.Should().Be(STATUS.Open);
            clientMessage.CreatedDate.Date.Should().Be(dto.CreatedDate.Date);
            clientMessage.CreatedDate.Hour.Should().Be(dto.CreatedDate.Hour);
            clientMessage.CreatedDate.Minute.Should().Be(dto.CreatedDate.Minute);
        }

        [Fact]
        public void TestMap_InitializesProperties()
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Status = STATUS.Open;
            ClientMessage clientMessage = ClientMessageBuilder.ClientMessage(dto);

            // Act
            ClientMessageDTO result = Mapper.Map<ClientMessageDTO>(clientMessage);

            // Assert
            result.Name.Should().Be(dto.Name).And.NotBeNullOrWhiteSpace();
            result.Email.Should().Be(dto.Email).And.NotBeNullOrWhiteSpace();
            result.PhoneNumber.Should().Be(dto.PhoneNumber).And.BeGreaterThanOrEqualTo(200000000).And.BeLessThanOrEqualTo(999999999);
            result.Message.Should().Be(dto.Message).And.NotBeNullOrWhiteSpace();
            result.Status.Should().Be(dto.Status);
            result.CreatedDate.Date.Should().Be(dto.CreatedDate.Date);
            result.CreatedDate.Hour.Should().Be(dto.CreatedDate.Hour);
            result.CreatedDate.Minute.Should().Be(dto.CreatedDate.Minute);
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidName_ThrowsArgumentException(string? name)
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Name = name!;

            // Act & Assert
            FluentActions.Invoking(() => ClientMessageBuilder.ClientMessage(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResource.ClientMessageNameNeedsToBeSpecifiedException);
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidEmail_ThrowsArgumentException(string? email)
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Email = email!;

            // Act & Assert
            FluentActions.Invoking(() => ClientMessageBuilder.ClientMessage(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResource.ClientMessageEmailNeedsToBeSpecifiedException);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(10000000000)]
        public void Constructor_WithInvalidPhoneNumber_ThrowsArgumentException(long phoneNumber)
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.PhoneNumber = phoneNumber;

            // Act & Assert
            FluentActions.Invoking(() => ClientMessageBuilder.ClientMessage(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResource.ClientMessagePhoneNumberNeedsToBeSpecifiedException);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_WithInvalidMessage_ThrowsArgumentException(string? message)
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            dto.Message = message!;

            // Act & Assert
            FluentActions.Invoking(() => ClientMessageBuilder.ClientMessage(dto)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResource.ClientMessageNeedsToBeSpecifiedException);
        }

        [Fact]
        public void SetStatus_WithValidParameters()
        {
            // Arrange
            ClientMessageDTO dto = ClientMessageBuilder.ClientMessageDTO();
            ClientMessage clientMessage = ClientMessageBuilder.ClientMessage(dto);

            // Act
            clientMessage.SetStatus(dto.Status);

            // Assert
            clientMessage.Status.Should().Be(dto.Status).And.BeOneOf(STATUS.Open, STATUS.Closed);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        public void SetName_WithInvalidStatus_ThrowsArgumentException(int status)
        {
            // Arrange
            ClientMessage clientMessage = ClientMessageBuilder.ClientMessage();

            // Act & Assert
            FluentActions.Invoking(() => clientMessage.SetStatus((STATUS)status)).Should()
                .Throw<Exception>()
                .WithMessage(DomainResource.ClientMessageStatusNeedsToBeSpecifiedException);
        }
    }
}
