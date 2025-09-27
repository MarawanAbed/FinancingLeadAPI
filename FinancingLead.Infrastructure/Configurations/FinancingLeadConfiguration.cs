

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Financing = FinancingLead.Domain.Entities.FinancingLead;
namespace FinancingLead.Infrastructure.Configurations;

public class FinancingLeadConfiguration : IEntityTypeConfiguration<Financing>
{
    public void Configure(EntityTypeBuilder<Financing> builder)
    {
        builder.ToTable("FinancingLeads");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever(); 
        
        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(x => x.ReviewedAt)
            .HasColumnType("datetime2")
            .IsRequired(false);

        
        builder.OwnsOne(x => x.Contact, contact =>
        {
            contact.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            contact.Property(c => c.Email)
                .HasMaxLength(254) 
                .IsRequired();

            contact.Property(c => c.PhoneE164)
                .HasMaxLength(16)
                .IsRequired();

            contact.Property(c => c.PreferredContactMethod)
                .HasConversion<int>()
                .IsRequired(false);

            contact.HasIndex(c => c.Email)
                .HasDatabaseName("IX_FinancingLeads_Contact_Email");

            contact.HasIndex(c => c.PhoneE164)
                .HasDatabaseName("IX_FinancingLeads_Contact_PhoneE164");
        });

        builder.OwnsOne(x => x.Business, business =>
        {
            business.Property(b => b.TypeOfActivity)
                .HasMaxLength(200)
                .IsRequired();

            business.Property(b => b.CommercialRegisterType)
                .HasConversion<int>()
                .IsRequired();

            business.Property(b => b.AnnualIncome)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);
        });

        builder.Property(x => x.ReviewStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ReviewReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_FinancingLeads_CreatedAt")
            .IsDescending(); 

        builder.HasIndex(x => x.ReviewStatus)
            .HasDatabaseName("IX_FinancingLeads_ReviewStatus");

        builder.HasIndex(x => new { x.CreatedAt, x.ReviewStatus })
            .HasDatabaseName("IX_FinancingLeads_CreatedAt_ReviewStatus")
            .IsDescending(true, false);

        builder.Ignore(x => x.DomainEvents);

        builder.Property<byte[]>("RowVersion")
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}