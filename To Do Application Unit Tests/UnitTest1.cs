using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Slalom_To_Do_Application.BusinessLayer;
using Slalom_To_Do_Application.Controllers;
using Slalom_To_Do_Application.Entities;
using System.Collections.Generic;
using System.Linq;

namespace To_Do_Application_Unit_Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDetailsView()
        {
            var mockUserActionBusinessLayer = new Mock<IUserActionsBusinessLayer>();
            var mockToDoActionBusinessLayer = new Mock<IToDoActionBusinessLayer>();
            UserEntity _user = new UserEntity();
            _user.user_id = "1";
            _user.user_first_name = "Test";
            _user.user_last_name = "Value";
            _user.create_date = System.DateTime.Now.ToString();
            _user.modified_date = System.DateTime.Now.ToString();
            List<UserEntity> _allUsers = new List<UserEntity>();
            _allUsers.Add(_user);
            mockUserActionBusinessLayer.Setup(x => x.retriveAllUsers()).Returns(_allUsers);
            var _mockFormCollection = new Mock<IFormCollection>();
            KeyValuePair<string, string> userKVPair = new KeyValuePair <string, string>("User", "1");
            _mockFormCollection.Setup(x => x.Keys.Add(userKVPair.Key));
            var _testController = new HomeController(mockUserActionBusinessLayer.Object, mockToDoActionBusinessLayer.Object);
            var users = _testController.selectUser(_mockFormCollection.Object);
            Assert.IsNotNull(users);
        }
    }
}

