//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using MultiUserBlock.Common;
//using MultiUserBlock.Common.Enums;
//using MultiUserBlock.Common.ViewModels;
//using MultiUserBlock.Db.Entitys;

//namespace MultiUserBlock.Services
//{
//    public class AccountService
//    {
//        private readonly IRepository _repository;

//        public AccountService(IRepository Repository)
//        {
//            _repository = Repository;
//        }

//        public async Task<IUserViewModel> GetByUserName(string username)
//        {
//            User user = _repository.Get<User>().SingleOrDefault(u=>u.Username == username);

//            return default(IUserViewModel);
//        }

//        public bool HasRole(int id, UserRoleType urt)
//        {
//            return _repository.Get<User>().Any(u => u.Id == id && u.RoleToUsers.Select(rtu=>rtu.Role.UserRoleType).Contains(urt));
//        }
//    }
//}
