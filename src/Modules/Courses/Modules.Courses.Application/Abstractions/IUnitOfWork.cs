namespace Modules.Courses.Application.Abstractions;

public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken token = default);
}
