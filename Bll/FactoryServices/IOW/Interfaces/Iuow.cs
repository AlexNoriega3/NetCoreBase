namespace Bll.FactoryServices.UOW.Interfaces
{
    public interface Iuow : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }

        bool Save();

        Task<bool> SaveAsync();

        Task BeginTransaction();

        void Commit();

        void Rollback();
    }
}