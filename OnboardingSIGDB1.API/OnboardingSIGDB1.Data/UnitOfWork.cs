using System;
using System.Threading.Tasks;

namespace OnboardingSIGDB1.Data
{
    public interface IUnitOfWork : IDisposable
    {
        DataContext Context { get; }
        Task Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public DataContext Context { get; }

        public UnitOfWork(DataContext context)
        {
            Context = context;
        }


        public async Task Commit()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
