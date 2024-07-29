namespace Notifications.Persistence.Mapping
{
    public class ClientMessageMap : IEntityTypeConfiguration<ClientMessage>
    {
        public void Configure(EntityTypeBuilder<ClientMessage> entity)
        {
            entity.ToTable("client_messages");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired(true);

            entity.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired(true);

            entity.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired(true);

            entity.Property(x => x.Message)
                .HasColumnName("message")
                .IsRequired(true);

            entity.Property(x => x.CreatedDate)
                .HasColumnName("created_date")
                .IsRequired(true);

            entity.Property(x => x.Status)
                .HasColumnName("status")
                .IsRequired(true);

        }

    }
}
