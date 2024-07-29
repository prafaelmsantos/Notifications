namespace Notifications.Persistence.Mapping
{
    public class VisitorMap : IEntityTypeConfiguration<Visitor>
    {
        public void Configure(EntityTypeBuilder<Visitor> entity)
        {
            entity.ToTable("visitors");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()
                .IsRequired(true);

            entity.Property(x => x.Year)
                .HasColumnName("year")
                .IsRequired(true);

            entity.Property(x => x.Month)
                .HasColumnName("month")
                .IsRequired(true);

            entity.Property(x => x.Value)
                .HasColumnName("value")
                .IsRequired(true);

        }

    }
}
