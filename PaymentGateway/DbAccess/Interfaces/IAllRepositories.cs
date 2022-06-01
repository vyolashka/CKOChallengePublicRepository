using Microsoft.EntityFrameworkCore.Storage;
using Model.Models;

namespace DbAccess.Interfaces
{
    public interface IAllRepositories
    {
        IDbContextTransaction GetTransaction();

        IRepository<Payment> PaymentRepository { get; }
    }
}
