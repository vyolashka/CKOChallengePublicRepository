using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Enums;
using Model.Models;

namespace DbAccess.ModelConfigurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");

            // Primary Key
            builder.HasKey(t => t.Id);

            // Properties
            
            // For simplicity's sake I will assume that the payment can be done only via a Visa card, which has a 16-digit number
            builder.Property(t => t.CardNumber)
                .HasMaxLength(16)
                .IsRequired();

            builder.Property(t => t.CardHolderNames)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(t => t.CardExpirationMonth)
                .IsRequired();

            builder.Property(t => t.CardExpirationYear)
                .IsRequired();

            // Due to the time constraints for this test application the available values for
            // the "Status" property will come from a simple enum with only two options.
            builder.Property(t => t.Status)
                .IsRequired()
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50)
                .HasConversion(new EnumToStringConverter<StatusesEnum>());

            // Due to the time constraints for this test application the available values for
            // the "Crrency" property will come from a simple enum with only two options.
            builder.Property(t => t.Currency)
                .IsRequired()
                .HasColumnType("nvarchar(15)")
                .HasMaxLength(15)
                .HasConversion(new EnumToStringConverter<CurrenciesEnum>());

            builder.Property(t => t.PaymentAmount)
                .IsRequired()
                .HasColumnType("decimal(28,2)");

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2(3)");

            builder.Property(t => t.LastModifiedAt)
                .IsRequired()
                .HasColumnType("datetime2(3)");
        }
    }
}
