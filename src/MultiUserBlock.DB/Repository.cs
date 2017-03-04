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
    public class Repository : IRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<User> _db;

        public Repository(DataContext context)
        {
            _context = context;
            _db = context.Set<User>();
        }

        public async Task AddOrUpdate(UserViewModel user)
        {
            var ex = await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).SingleOrDefaultAsync(u => u.Id == user.UserId);
            if (ex == null)
            {
                var usr = new User()
                {
                    Username = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    LayoutTheme = await _context.LayoutThemes.SingleOrDefaultAsync(lt=>lt.Name=="default")
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
                ex.LayoutTheme = await _context.LayoutThemes.SingleOrDefaultAsync(lt => lt.ThemeId == user.LayoutThemeViewModel.Id);
                ex.Password = user.Password;
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

        public async Task<List<UserViewModel>> GetAll()
        {
            return await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt=>lt.LayoutTheme).Select(r => _map(r)).ToListAsync();
        }

        public async Task<UserViewModel> GetById(int userId)
        {
            return _map(await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Id == userId));
        }

        public async Task<UserViewModel> GetByUserName(string userName)
        {
            return _map(await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Username == userName));
        }

        public async Task<bool> HasRole(int userId, UserRoleType urt)
        {
            var result = await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Id == userId);

            return result.RoleToUsers.Any(rtu => rtu.Role.UserRoleType == urt);
        }

        public async Task Remove(int userId)
        {
            _db.Remove(await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Id == userId));
            _context.SaveChanges();
        }

        public async Task ResetPassword(int userId)
        {
            var ex = await _db.Include(u => u.RoleToUsers).ThenInclude(r => r.Role).Include(lt => lt.LayoutTheme).SingleOrDefaultAsync(u => u.Id == userId);
            if (ex == null)
            {
                return;
            }
            else
            {
                ex.Password = "";
                _context.SaveChanges();
            }
        }

        public async Task<List<LayoutThemeViewModel>> GetAllThemes()
        {
            return await _context.LayoutThemes.Select(lt=>_map(lt)).ToListAsync();
        }

        // Privates...

        private LayoutThemeViewModel _map(LayoutTheme lt)
        {
            return new LayoutThemeViewModel()
            {
                Id = lt.ThemeId,
                Name = lt.Name,
                Link = lt.Link
            };
        }

        private UserViewModel _map(User user)
        {
            if (user != null)
            {
                return new UserViewModel()
                {
                    UserId = user.Id,
                    Username = user.Username,
                    ShowName = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    Roles = user.RoleToUsers.Select(r => r.Role).Select(r => (int)r.UserRoleType),
                    LayoutThemeViewModel = new LayoutThemeViewModel()
                    {
                        Id = user.LayoutTheme.ThemeId,
                        Name = user.LayoutTheme.Name,
                        Link = user.LayoutTheme.Link
                    }
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
                    Id = user.UserId,
                    Username = user.Username,
                    Name = user.Name,
                    Vorname = user.Vorname,
                    Password = user.Password,
                    LayoutTheme = _context.LayoutThemes.SingleOrDefaultAsync(lt => lt.ThemeId == user.LayoutThemeViewModel.Id).Result
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

        private User _delRoles(User user)
        {
            var rtus = _context.RoleToUsers.Where(rtu => rtu.UserId == user.Id);
            _context.RemoveRange(rtus);
            _context.SaveChanges();
            return user;
        }

    }
}
