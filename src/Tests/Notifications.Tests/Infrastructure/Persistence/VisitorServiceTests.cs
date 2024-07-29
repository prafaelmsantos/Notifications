namespace Notifications.Tests.Infrastructure.Persistence
{
    public class VisitorServiceTests : BaseClassTests
    {
        #region Private variables

        private readonly Mock<IVisitorRepository> _visitorRepositoryMock;
        private readonly IVisitorService _visitorService;

        #endregion

        #region Constructors

        public VisitorServiceTests(ITestOutputHelper output) : base(output)
        {
            Notifications.Core.Mapper.AutoMapperProfile myProfile = new();
            MapperConfiguration configuration = new(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            _visitorRepositoryMock = new Mock<IVisitorRepository>(MockBehavior.Strict);
            _visitorService = new VisitorService(Mapper, _visitorRepositoryMock.Object);
        }

        #endregion

        #region GetVisitorCountersAsync

        [Fact]
        public async Task GetVisitorCountersAsync_GetAll_Successfully()
        {
            // Arrange
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryable(visitor)));

            // Act
            VisitorCounterDTO result = await _visitorService.GetVisitorCountersAsync();

            // Assert
            result.Should().NotBeNull();

            _visitorRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _visitorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetVisitorCountersAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _visitorService.GetVisitorCountersAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.GetVisitorCountersAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region GetAllVisitoresWithYearComparisonAsync

        [Fact]
        public async Task GetAllVisitoresWithYearComparisonAsync_GetAll_Successfully()
        {
            // Arrange
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryable(visitor)))
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryable(visitor)));

            // Act
            ResponseCompleteVisitorDTO result = await _visitorService.GetAllVisitoresWithYearComparisonAsync();

            // Assert
            result.Should().NotBeNull();

            _visitorRepositoryMock.Verify(repo => repo.GetAll(), Times.Exactly(2));
            _visitorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllVisitoresWithYearComparisonAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _visitorService.GetAllVisitoresWithYearComparisonAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.GetAllVisitorsWithYearComparisonAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task GetAllVisitoresWithYearComparisonAsync_LastGetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.SetupSequence(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryable(visitor)))
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _visitorService.GetAllVisitoresWithYearComparisonAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.GetAllVisitorsWithYearComparisonAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region GetAllVisitoresWithMonthComparisonAsync

        [Fact]
        public async Task GetAllVisitoresWithMonthComparisonAsync_GetAll_Successfully()
        {
            // Arrange
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryable(visitor)));

            // Act
            ResponseVisitorDTO result = await _visitorService.GetAllVisitoresWithMonthComparisonAsync();

            // Assert
            result.Should().NotBeNull();

            _visitorRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _visitorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllVisitoresWithMonthComparisonAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _visitorService.GetAllVisitoresWithMonthComparisonAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.GetAllVisitorsWithMonthComparisonAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion

        #region CreateOrUpdateVisitorAsync

        [Fact]
        public async Task CreateOrUpdateVisitorAsync_AddVisitorAsync_Successfully()
        {
            // Arrange   
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryableEmpty()));

            _visitorRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Visitor>()))
                .ReturnsAsync(visitor);

            // Act
            VisitorDTO result = await _visitorService.CreateOrUpdateVisitorAsync();

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(visitor.Id);
            result.Year.Should().Be(visitor.Year);
            result.Month.Should().Be(visitor.Month);
            result.Value.Should().Be(visitor.Value);

            _visitorRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _visitorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Visitor>()), Times.Once);
            _visitorRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Visitor>()), Times.Never);
            _visitorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateOrUpdateVisitorAsync_UpdateVisitorAsync_Successfully()
        {
            // Arrange   
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryable(visitor)));

            _visitorRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Visitor>()))
                .ReturnsAsync(visitor);

            // Act
            VisitorDTO result = await _visitorService.CreateOrUpdateVisitorAsync();

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(visitor.Id);
            result.Year.Should().Be(visitor.Year);
            result.Month.Should().Be(visitor.Month);
            result.Value.Should().Be(visitor.Value);

            _visitorRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _visitorRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Visitor>()), Times.Never);
            _visitorRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Visitor>()), Times.Once);
            _visitorRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CreateOrUpdateVisitorAsync_GetAllNotBreak_ThrowsExceptionAsync()
        {
            // Arrange
            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Throws(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _visitorService.CreateOrUpdateVisitorAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.CreateOrUpdateVisitorAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task CreateOrUpdateVisitorAsync_AddAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryableEmpty()));

            _visitorRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Visitor>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _visitorService.CreateOrUpdateVisitorAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.CreateOrUpdateVisitorAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        [Fact]
        public async Task CreateOrUpdateVisitorAsync_UpdateAsyncNotBreak_ThrowsExceptionAsync()
        {
            // Arrange   
            Visitor visitor = VisitorBuilder.Visitor();

            _visitorRepositoryMock.Setup(x => x.GetAll())
                .Returns(new TestAsyncEnumerable<Visitor>(VisitorBuilder.IQueryable(visitor)));

            _visitorRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Visitor>()))
                .ThrowsAsync(new Exception(ExceptionBuilder.ExceptionMessage));

            // Act & Assert
            await FluentActions.Invoking(async () => await _visitorService.CreateOrUpdateVisitorAsync()).Should()
                .ThrowAsync<Exception>()
                .WithMessage($"{DomainResource.CreateOrUpdateVisitorAsyncException} {ExceptionBuilder.ExceptionMessage}");
        }

        #endregion
    }
}
