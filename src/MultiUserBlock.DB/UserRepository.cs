using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiUserBlock.Common;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Common.Repository;
using MultiUserBlock.ViewModels;

namespace MultiUserBlock.DB
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<User> _db;

        public UserRepository(DataContext context)
        {
            _context = context;
            _db = context.Set<User>();
        }

        public async Task AddOrUpdate(UserViewModel user)
        {
            var ex = await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.UserId == user.UserId);
            if (ex == null)
            {
                var usr = new User()
                {
                    Username = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                };

                List<RoleToUser> rtus = new List<RoleToUser>();
                foreach (var role in user.Roles)
                {
                    var _rtu = new RoleToUser()
                    {
                        Role = role != -1 ? _context.Roles.First(r => r.UserRoleType == (UserRoleType)role) : _context.Roles.First(r => r.UserRoleType == UserRoleType.Default),
                        User = usr
                    };
                    rtus.Add(_rtu);
                    _context.RoleToUsers.Add(_rtu);
                }
                usr.RoleToUsers = rtus;
            }
            else
            {
                _delRoles(ex);
                ex.Name = user.Name;
                ex.Vorname = user.Vorname;
                ex.Username = user.Username;
                List<RoleToUser> rtus = new List<RoleToUser>();
                foreach (var role in user.Roles)
                {
                    var _rtu = new RoleToUser()
                    {
                        Role = role != -1 ? _context.Roles.First(r => r.UserRoleType == (UserRoleType)role) : _context.Roles.First(r => r.UserRoleType == UserRoleType.Default),
                        User = ex
                    };
                    rtus.Add(_rtu);
                    _context.RoleToUsers.Add(_rtu);
                }
                ex.RoleToUsers = rtus;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e); 
                throw;
            }
        }

        private User _delRoles(User user)
        {
            var rtus = _context.RoleToUsers.Where(rtu => rtu.UserId == user.UserId);
            _context.RemoveRange(rtus);
            _context.SaveChanges();
            return user;
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            return await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Select(r => _map(r)).ToListAsync();
         }

        public async Task<UserViewModel> GetById(int userId)
        {
            return _map(await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.UserId == userId));
        }

        public async Task<UserViewModel> GetByUserName(string userName)
        {
            return _map(await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.Username == userName));
        }

        public async Task<bool> HasRole(int userId, UserRoleType urt)
        {
            var result = await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.UserId == userId);

            return result.RoleToUsers.Any(rtu => rtu.Role.UserRoleType == urt);
        }

        public async Task Remove(int userId)
        {
            _db.Remove(await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.UserId == userId));
            _context.SaveChanges();
        }

        private UserViewModel _map(User user)
        {
            if (user != null)
            {
                return new UserViewModel()
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    ShowName = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    Roles = user.RoleToUsers.Select(r => r.Role).Select(r => (int)r.UserRoleType)
                };
            }
            else
            {
                return default(UserViewModel);
            }
        }

        private User _map(UserViewModel user)
        {
            if (user != null)
            {
                var usr = new User()
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                };

                List<RoleToUser> rtus = new List<RoleToUser>();
                foreach (var role in user.Roles)
                {
                    rtus.Add(new RoleToUser()
                    {
                        Role = role != -1 ? _context.Roles.First(r => r.UserRoleType == (UserRoleType)role) : _context.Roles.First(r => r.UserRoleType == UserRoleType.Default),
                        User = usr
                    });
                }

                usr.RoleToUsers = rtus;

                return usr;
            }
            else
            {
                return default(User);
            }
        }
    }
}
