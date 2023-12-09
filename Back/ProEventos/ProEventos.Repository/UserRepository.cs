using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Repository.Context;
using ProEventos.Repository.Contratos;

namespace ProEventos.Repository
{
    public class UserRepository : GeralRepository, IUserRepository
    {
        private readonly ProEventosContext _proEventosContext;

        public UserRepository(ProEventosContext proEventosContext) : base(proEventosContext)
        {
            _proEventosContext = proEventosContext;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _proEventosContext.Users.ToListAsync();
        }             

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _proEventosContext.Users.FindAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _proEventosContext.Users.SingleOrDefaultAsync(user => user.UserName == userName); 
        }
    }
}