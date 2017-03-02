﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiUserBlock.Common.Enums;

namespace MultiUserBlock.Tests.Test_Controller
{
    [TestClass, Area("Rene")]
    public class Test_Controller_Admin : Test_Base
    {
        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_GET_Admin()
        {
            var browser = new TestServerBrowser(_testServer);
            var expectedName = "rr1980";
            var credentials = new Dictionary<string, string>
                        {
                            {"username", expectedName},
                            {"password", "12003"},
                            {"ReturnUrl", "/"}
                        };

            var signInResponse = await browser.Post("/Account/Login", credentials);

            await browser.FollowRedirect(signInResponse);

            var frontPageResponse = await browser.Get("/Admin");
            Assert.AreEqual(HttpStatusCode.OK, frontPageResponse.StatusCode);
            Assert.AreEqual(frontPageResponse.RequestMessage.RequestUri.AbsolutePath, "/Admin");

            var user = _httpContexts.Last().User;
            Assert.IsNotNull(user);

            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role);
            Assert.IsTrue(roles.Count() == 2);

            var role_adnin = roles.FirstOrDefault(r => r.Value == UserRoleType.Admin.ToString());
            Assert.IsNotNull(role_adnin);

            var role_default = roles.FirstOrDefault(r => r.Value == UserRoleType.Default.ToString());
            Assert.IsNotNull(role_default);
        }

    }
}
