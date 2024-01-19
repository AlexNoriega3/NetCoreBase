using AutoMapper;
using Bll.FactoryServices.UOW.Interfaces;
using Dal;
using Models.Entities;

namespace Bll.FactoryServices.UOW.Repositories
{
    public class UserRepository : BaseRepository<AppUsuario>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}