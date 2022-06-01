using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace DbAccess
{
    public class PaymentGatewayDbContext : DbContext
    {
        public PaymentGatewayDbContext(DbContextOptions<PaymentGatewayDbContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
    }
}
