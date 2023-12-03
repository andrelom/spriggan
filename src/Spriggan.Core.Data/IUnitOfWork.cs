using Microsoft.EntityFrameworkCore;

namespace Spriggan.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();

    void ChangeState(object model, EntityState state);
}
