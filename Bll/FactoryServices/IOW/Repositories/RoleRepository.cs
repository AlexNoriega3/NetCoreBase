using AutoMapper;
using Bll.FactoryServices.UOW.Interfaces;
using Dal;
using Models.Entities;

namespace Bll.FactoryServices.UOW.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}