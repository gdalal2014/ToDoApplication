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


        private IUserRepository _userRepository;
        private IToDoRepository _todoRepository;

        public HomeController(IUserRepository userRepository, IToDoRepository todoRepository)
        {
            _userRepository = userRepository;
            _todoRepository = todoRepository;
        }

        public IActionResult Index()
        {

            var usersRetrieved = _userRepository.All();
            ViewData["User_Model"] = usersRetrieved;

            return View();


        }
        [HttpPost]
        public ActionResult selectUser(IFormCollection result)
        {
            var _results = result;
            var userId = _results["User"].ElementAt(0).ToString();

            var user = _userRepository.Find(userId);
            var items = _todoRepository.Find(userId);
            ViewData["User_Model"] = user;
            ViewData["ToDo_Model"] = items;
            ViewData["User_Selected"] = true;

            return View("Index");


        }

        [HttpPost]
        public ActionResult addUser(IFormCollection result)
        {
            var _results = result;
            var firstName = _results["firstName"].ElementAt(0).ToString();
            var lastName = _results["lastName"].ElementAt(0).ToString();


            var newUserEntity = new UserEntity();
            newUserEntity.user_first_name = firstName;
            newUserEntity.user_last_name = lastName;
            _userRepository.Add(newUserEntity);

            var usersRetrieved = _userRepository.All();
            ViewData["User_Model"] = usersRetrieved;
            return View("Index");

        }

        [HttpPost]
        public ActionResult editUser(IFormCollection result)
        {
            var _results = result;
            var firstName = _results["firstNameEdit"].ElementAt(0).ToString();
            var lastName = _results["lastNameEdit"].ElementAt(0).ToString();
            var userId = _results["userIdHidden"].ElementAt(0).ToString();

            var editUserEntity = new UserEntity();
            editUserEntity.user_id = userId;
            editUserEntity.user_first_name = firstName;
            editUserEntity.user_last_name = lastName;
            _userRepository.Update(editUserEntity);

            var userValues = _userRepository.All();
            ViewData["User_Model"] = userValues;
            return View("Index");


        }

        [HttpPost]
        public ActionResult addToDoItem(IFormCollection result)
        {
            var _results = result;
            var item = _results["item"].ElementAt(0).ToString();
            var isCompleted = _results["todoCompleted"].ElementAt(0).ToString();
            var completionDate = _results["completionDate"].ElementAt(0).ToString();
            var userId = _results["userIdHidden"].ElementAt(0).ToString();

            var newToDoEntity = new ToDoEntity();

            newToDoEntity.user_id = userId;
            newToDoEntity.item = item;
            newToDoEntity.is_completed = isCompleted;
            newToDoEntity.completion_date = DateTime.Parse(completionDate).ToString("yyyy-MM-dd hh:mm:ss");
            _todoRepository.Add(newToDoEntity);

            var userValue = _userRepository.Find(userId);
            var itemValues = _todoRepository.Find(userId);
            ViewData["User_Model"] = userValue;
            ViewData["ToDo_Model"] = itemValues;

            return View("Index");

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

            var editToDoEntity = new ToDoEntity();

            editToDoEntity.user_id = userId;
            editToDoEntity.list_id = listId;
            editToDoEntity.item = item;
            editToDoEntity.is_completed = isCompleted;
            editToDoEntity.completion_date = DateTime.Parse(completionDate).ToString("yyyy-MM-dd hh:mm:ss");
            _todoRepository.Update(editToDoEntity);

            var userValue = _userRepository.Find(userId);
            var itemValues = _todoRepository.Find(userId);
            ViewData["User_Model"] = userValue;
            ViewData["ToDo_Model"] = itemValues;

            return View("Index");

        }

        [HttpPost]
        public ActionResult completeToDoItem(IFormCollection result)
        {
            var _results = result;
            var listId = _results["todoListId"].ElementAt(0).ToString();
            var userId = _results["userIdHiddenInput"].ElementAt(0).ToString();


            var completeToDoEntity = new ToDoEntity();
            completeToDoEntity.user_id = userId;
            completeToDoEntity.list_id = listId;
            _todoRepository.UpdateCompletedEntry(completeToDoEntity);

            var user = _userRepository.Find(userId);
            var items = _todoRepository.Find(userId);
            ViewData["User_Model"] = user;
            ViewData["ToDo_Model"] = items;

            return View("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

