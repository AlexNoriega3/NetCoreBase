using AutoMapper;
using Bll.FactoryServices.UOW.Interfaces;
using Dal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bll.FactoryServices.UOW.Repositories
{
    public class UOW : Iuow
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private IMapper _mapper;

        public UOW(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IRoleRepository RoleRepository => new RoleRepository(_context, _mapper);

        #region Context

        public bool Save()
        {
            var result = _context.SaveChanges();
            Dispose();

            if (result == 1)
                return true;
            else
                return false;
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            await DisposeAsync();

            if (result == 1)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task DisposeAsync()
        {
            await Task.Run(() =>
            {
                _context.Dispose();
            });
        }

        public async Task BeginTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        #endregion Context
    }
}