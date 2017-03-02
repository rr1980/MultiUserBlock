using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Common.Repository;

namespace MultiUserBlock.Tests.Test_Services
{
    [TestClass, Area("Rene")]
    public class Test_UserRepository : Test_Base
    {
        private IUserRepository _userRepository;

        [TestInitialize]
        [TestCategory("Smoke")]
        public new void TestInitialize()
        {
            base.TestInitialize();

            _userRepository = (IUserRepository)_testServer.Host.Services.GetService(typeof(IUserRepository));
            Assert.IsNotNull(_userRepository);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GetAllUsers()
        {
            var allUsers = await _userRepository.GetAll();
            Assert.IsNotNull(allUsers);
            Assert.IsTrue(allUsers.Count() >= 2);
        }


        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GetUserById()
        {
            await Task.Run(async () =>
            {
                var user = await _userRepository.GetById(1);

                Assert.IsNotNull(user);
                Assert.AreEqual("Riesner", user.Name);
                Assert.AreEqual("rr1980", user.ShowName);
                Assert.AreEqual(1, user.UserId);
                Assert.AreEqual("rr1980", user.Username);
                Assert.AreEqual("Rene", user.Vorname);
            });
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HasRole()
        {
            await Task.Run(async () =>
            {
                var user = await _userRepository.GetById(2);
                Assert.IsNotNull(user);

                var hasRole = await _userRepository.HasRole(2, UserRoleType.Default);
                Assert.IsTrue(hasRole);
                hasRole = await _userRepository.HasRole(2, UserRoleType.Admin);
                Assert.IsFalse(hasRole);
            });
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GetUserByName()
        {
            await Task.Run(async () =>
            {
                //var user = await (Task<UserViewModel>)typeof(AccountService).GetMethod("GetUserByNameAndPw", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_accountService, new object[] { "rr1980", "123" });
                var user = await _userRepository.GetByUserName("rr1980");
                Assert.IsNotNull(user);
                Assert.AreEqual("Riesner", user.Name);
                Assert.AreEqual("rr1980", user.ShowName);
                Assert.AreEqual(1, user.UserId);
                Assert.AreEqual("rr1980", user.Username);
                Assert.AreEqual("Rene", user.Vorname);

                user = await _userRepository.GetByUserName("abc");
                Assert.IsNull(user);

            });
        }
    }
}





