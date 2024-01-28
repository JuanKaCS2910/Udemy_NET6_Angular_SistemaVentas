namespace POS.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Declaración o matrícula de nuestra interfaces a nivel de repository
        ICategoryRepository Category { get; }

        void SaveChanges();
        Task SaveChangesAsync();

    }
}
