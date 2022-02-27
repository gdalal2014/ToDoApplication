using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Slalom_To_Do_Application.Database;
using Slalom_To_Do_Application.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Slalom_To_Do_Application.Controllers
{
    public class HomeController : Controller
    {
        private static IContainer Container { get; set; }

        private readonly IConfiguration config;

        public HomeController(IConfiguration config)
        {
            this.config = config;            
            var dbConnectionString = config.GetValue<string>("ConnectionStrings:DatabaseConnectionString");
            var builder = new ContainerBuilder();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().WithParameter(new TypedParameter(typeof(string), dbConnectionString));
            Container = builder.Build();
        }

        public IActionResult Index()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IUnitOfWork>();
                uow.Begin();
                var usersRetrieved = uow.UserRepository.All();
                ViewData["User_Model"] = usersRetrieved;
                uow.Dispose();
                return View();
             }
            
        }
        [HttpPost]
        public ActionResult selectUser(IFormCollection result)
        {
            var _results = result;
            var userId = _results["User"].ElementAt(0).ToString();
            using (var scope = Container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IUnitOfWork>();
                uow.Begin();
                var user = uow.UserRepository.Find(userId);
                var items = uow.ToDoRepository.Find(userId);
                ViewData["User_Model"] = user;
                ViewData["ToDo_Model"] = items;
                ViewData["User_Selected"] = true;
                uow.Dispose();
                return View("Index");
            }

        }

        [HttpPost]
        public ActionResult addUser (IFormCollection result)
        {
            var _results = result;
            var firstName = _results["firstName"].ElementAt(0).ToString();
            var lastName = _results["lastName"].ElementAt(0).ToString();
            using (var scope = Container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IUnitOfWork>();
                uow.Begin();
                var newUserEntity = new UserEntity();
                newUserEntity.user_first_name = firstName;
                newUserEntity.user_last_name = lastName;
                uow.UserRepository.Add(newUserEntity);
                uow.Commit();
                var usersRetrieved = uow.UserRepository.All();
                ViewData["User_Model"] = usersRetrieved;
                uow.Dispose();
                return View("Index");
            }

        }

        [HttpPost]
        public ActionResult editUser(IFormCollection result)
        {
            var _results = result;
            var firstName = _results["firstNameEdit"].ElementAt(0).ToString();
            var lastName = _results["lastNameEdit"].ElementAt(0).ToString();
            var userId = _results["userIdHidden"].ElementAt(0).ToString();
            using (var scope = Container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IUnitOfWork>();
                uow.Begin();
                var editUserEntity = new UserEntity();
                editUserEntity.user_id = userId;
                editUserEntity.user_first_name = firstName;
                editUserEntity.user_last_name = lastName;
                uow.UserRepository.Update(editUserEntity);
                uow.Commit();
                var userValues = uow.UserRepository.All();
                ViewData["User_Model"] = userValues;
                uow.Dispose();
                return View("Index");
            }

        }

        [HttpPost]
        public ActionResult addToDoItem(IFormCollection result)
        {
            var _results = result;
            var item = _results["item"].ElementAt(0).ToString();
            var isCompleted = _results["todoCompleted"].ElementAt(0).ToString();
            var completionDate = _results["completionDate"].ElementAt(0).ToString();
            var userId = _results["userIdHidden"].ElementAt(0).ToString();
            using (var scope = Container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IUnitOfWork>();
                var newToDoEntity = new ToDoEntity();
                uow.Begin();
                newToDoEntity.user_id = userId;
                newToDoEntity.item = item;
                newToDoEntity.is_completed = isCompleted;
                newToDoEntity.completion_date = DateTime.Parse(completionDate).ToString("yyyy-MM-dd hh:mm:ss");
                uow.ToDoRepository.Add(newToDoEntity);
                uow.Commit();
                var userValue = uow.UserRepository.Find(userId);
                var itemValues = uow.ToDoRepository.Find(userId);
                ViewData["User_Model"] = userValue;
                ViewData["ToDo_Model"] = itemValues;
                uow.Dispose();
                return View("Index");
            }

        }

        [HttpPost]
        public ActionResult editToDoItem(IFormCollection result)
        {
            var _results = result;
            var item = _results["editItemToDo"].ElementAt(0).ToString();
            var isCompleted = _results["editToDoCompleted"].ElementAt(0).ToString();
            var completionDate = _results["editCompletionDate"].ElementAt(0).ToString();
            var userId = _results["editUserIdHidden"].ElementAt(0).ToString();
            var listId = _results["editListIdHidden"].ElementAt(0).ToString();
            using (var scope = Container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IUnitOfWork>();
                var editToDoEntity = new ToDoEntity();
                uow.Begin();
                editToDoEntity.user_id = userId;
                editToDoEntity.list_id = listId;
                editToDoEntity.item = item;
                editToDoEntity.is_completed = isCompleted;
                editToDoEntity.completion_date = DateTime.Parse(completionDate).ToString("yyyy-MM-dd hh:mm:ss");
                uow.ToDoRepository.Update(editToDoEntity);
                uow.Commit();
                var userValue = uow.UserRepository.Find(userId);
                var itemValues = uow.ToDoRepository.Find(userId);
                ViewData["User_Model"] = userValue;
                ViewData["ToDo_Model"] = itemValues;
                uow.Dispose();
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult completeToDoItem(IFormCollection result)
        {
            var _results = result;
            var listId = _results["todoListId"].ElementAt(0).ToString();
            var userId = _results["userIdHiddenInput"].ElementAt(0).ToString();
            using (var scope = Container.BeginLifetimeScope())
            {
                var uow = scope.Resolve<IUnitOfWork>();
                uow.Begin();
                var completeToDoEntity = new ToDoEntity();
                completeToDoEntity.user_id = userId;
                completeToDoEntity.list_id = listId;
                uow.ToDoRepository.UpdateCompletedEntry(completeToDoEntity);
                uow.Commit();
                var user = uow.UserRepository.Find(userId);
                var items = uow.ToDoRepository.Find(userId);
                ViewData["User_Model"] = user;
                ViewData["ToDo_Model"] = items;
                uow.Dispose();
                return View("Index");
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
