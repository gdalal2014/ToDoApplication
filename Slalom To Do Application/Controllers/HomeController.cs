using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Slalom_To_Do_Application.Entities;
using Slalom_To_Do_Application.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Slalom_To_Do_Application.BusinessLayer;


namespace Slalom_To_Do_Application.Controllers
{
    public class HomeController : Controller
    {
        


        private IUserActionsBusinessLayer _userActionBusinessLayer;
        private IToDoActionBusinessLayer _toDoActionBusinessLayer;

        public HomeController(IUserActionsBusinessLayer userActionBusinessLayer, IToDoActionBusinessLayer toDoActionBusinessLayer)
        {
            _userActionBusinessLayer = userActionBusinessLayer;
            _toDoActionBusinessLayer = toDoActionBusinessLayer;

        }

        public IActionResult Index()
        {

            var usersRetrieved = _userActionBusinessLayer.retriveAllUsers();
            ViewData["User_Model"] = usersRetrieved;

            return View();


        }
        [HttpPost]
        public ActionResult selectUser(IFormCollection result)
        {
            try
            {
                var _results = result;
                var userId = _results["User"].ElementAt(0).ToString();
                var user = _userActionBusinessLayer.retriveUser(userId);
                var items = _toDoActionBusinessLayer.retriveToDoItem(userId);
                ViewData["User_Model"] = user;
                ViewData["ToDo_Model"] = items;
                ViewData["User_Selected"] = true;
                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }


        }

        [HttpPost]
        public ActionResult addUser(IFormCollection result)
        {
            try
            {
                var _results = result;
                var firstName = _results["firstName"].ElementAt(0).ToString();
                var lastName = _results["lastName"].ElementAt(0).ToString();
                var newUserEntity = new UserEntity();
                newUserEntity.user_first_name = firstName;
                newUserEntity.user_last_name = lastName;
                _userActionBusinessLayer.createNewUser(newUserEntity);
                var usersRetrieved = _userActionBusinessLayer.retriveAllUsers();
                ViewData["User_Model"] = usersRetrieved;
                ViewBag.alertMessage = "New User Added Successfully";

                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult editUser(IFormCollection result)
        {
            try
            {
                var _results = result;
                var firstName = _results["firstNameEdit"].ElementAt(0).ToString();
                var lastName = _results["lastNameEdit"].ElementAt(0).ToString();
                var userId = _results["userIdHidden"].ElementAt(0).ToString();

                var editUserEntity = new UserEntity();
                editUserEntity.user_id = userId;
                editUserEntity.user_first_name = firstName;
                editUserEntity.user_last_name = lastName;
                _userActionBusinessLayer.updateExistingUser(editUserEntity);
                var userValues = _userActionBusinessLayer.retriveAllUsers();
                ViewBag.alertMessage = "Selected User Information Updated Successfully";
                ViewData["User_Model"] = userValues;
                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult addToDoItem(IFormCollection result)
        {
            try
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
                _toDoActionBusinessLayer.createNewToDoItem(newToDoEntity);
                var userValue = _userActionBusinessLayer.retriveUser(userId);
                var itemValues = _toDoActionBusinessLayer.retriveToDoItem(userId);
                ViewBag.alertMessage = "New To Do Item Created Successfully";

                ViewData["User_Model"] = userValue;
                ViewData["ToDo_Model"] = itemValues;

                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult editToDoItem(IFormCollection result)
        {
            try
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
                _toDoActionBusinessLayer.updateExistingToDoItem(editToDoEntity);
                var userValue = _userActionBusinessLayer.retriveUser(userId);
                var itemValues = _toDoActionBusinessLayer.retriveToDoItem(userId);
                ViewBag.alertMessage = "Selected To Do Item Updated Successfully";

                ViewData["User_Model"] = userValue;
                ViewData["ToDo_Model"] = itemValues;

                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult completeToDoItem(IFormCollection result)
        {
            try
            {
                var _results = result;
                var listId = _results["todoListId"].ElementAt(0).ToString();
                var userId = _results["userIdHiddenInput"].ElementAt(0).ToString();

                var completeToDoEntity = new ToDoEntity();
                completeToDoEntity.user_id = userId;
                completeToDoEntity.list_id = listId;
                _toDoActionBusinessLayer.completeToDoItem(completeToDoEntity);
                var userValue = _userActionBusinessLayer.retriveUser(userId);
                var itemValues = _toDoActionBusinessLayer.retriveToDoItem(userId);
                ViewBag.alertMessage = "Selected To Do Item Marked as Completed Successfully";

                ViewData["User_Model"] = userValue;
                ViewData["ToDo_Model"] = itemValues;

                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

