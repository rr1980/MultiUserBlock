using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Models;
using MultiUserBlock.ViewModels;

namespace MultiUserBlock.Common.Repository
{
    public interface IUserRepository
    {
        Task<List<UserViewModel>> GetAll();
        Task<UserViewModel> GetById(int userId);
        Task<UserViewModel> GetByUserName(string userName);
        Task Remove(int userId);
        Task AddOrUpdate(UserViewModel user);
        Task<bool> HasRole(int userId, UserRoleType urt);
    }
}
