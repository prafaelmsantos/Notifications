namespace Notifications.Tests.Core.Builders
{
    public class EmailConfigBuilder
    {
        public static EmailConfig EmailConfig()
        {
            return new Faker<EmailConfig>()
                .RuleFor(e => e.Name, f => f.Person.FullName)
                .RuleFor(e => e.Address, f => f.Internet.Email())
                .RuleFor(e => e.Username, f => f.Internet.UserName())
                .RuleFor(e => e.Password, f => f.Internet.Password())
                .RuleFor(e => e.Host, f => f.Internet.DomainName())
                .RuleFor(e => e.Port, f => f.Internet.Port());
        }
    }
}
