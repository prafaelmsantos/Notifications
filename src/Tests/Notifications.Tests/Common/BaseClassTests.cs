namespace Notifications.Tests.Common
{
    public class BaseClassTests
    {
        #region private variables
        public readonly Faker Faker;
        public readonly ITestOutputHelper Output;
        public readonly string Culture;
        public readonly IMapper Mapper;

        #endregion
        public BaseClassTests(ITestOutputHelper output)
        {
            Culture = "en";
            Faker = new Faker(Culture);
            Output = output;

            Notifications.Core.Mapper.AutoMapperProfile myProfile = new();
            MapperConfiguration configuration = new(cfg => cfg.AddProfile(myProfile));
            Mapper = new Mapper(configuration);
        }
    }
}
