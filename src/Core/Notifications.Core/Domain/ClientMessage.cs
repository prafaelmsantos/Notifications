namespace Notifications.Core.Domain
{
    public class ClientMessage : EntityBase
    {
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public long PhoneNumber { get; private set; }
        public string Message { get; private set; } = null!;
        public STATUS Status { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public ClientMessage() { }

        public ClientMessage(string name, string email, long phoneNumber, string message)
        {
            name.ThrowIfNull(() => throw new Exception(DomainResource.ClientMessageNameNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            email.ThrowIfNull(() => throw new Exception(DomainResource.ClientMessageEmailNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            phoneNumber.Throw(() => throw new Exception(DomainResource.ClientMessagePhoneNumberNeedsToBeSpecifiedException))
                .IfGreaterThan(999999999)
                .IfLessThan(200000000);

            message.ThrowIfNull(() => throw new Exception(DomainResource.ClientMessageNeedsToBeSpecifiedException))
                .IfWhiteSpace();

            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Message = message;
            Status = STATUS.Open;
            CreatedDate = DateTime.UtcNow;
        }

        public void SetStatus(STATUS status)
        {
            System.Enum.IsDefined(typeof(STATUS), status)
                .Throw(() => throw new Exception(DomainResource.ClientMessageStatusNeedsToBeSpecifiedException))
                .IfFalse();

            Status = status;
        }
    }
}
