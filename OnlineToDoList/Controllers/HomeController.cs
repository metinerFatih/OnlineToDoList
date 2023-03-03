using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineToDoList.Data;
using OnlineToDoList.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace OnlineToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var vm = new NewToDoItem()
            {
                ToDoItems = _db.ToDoItems.Where(x => x.AuthorId == UserId).OrderBy(x => x.Done).ToList()
            };
            return View(vm);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Index(NewToDoItem vm)
        {

            if (ModelState.IsValid)
            {
                ToDoItem toDoItem = new ToDoItem()
                {
                    Title = vm.Title,
                    Done = false,
                    AuthorId = UserId
                };

                _db.ToDoItems.Add(toDoItem);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            vm.ToDoItems = _db.ToDoItems.Where(x => x.AuthorId == UserId).OrderBy(x => x.Done).ToList();
            return View(vm);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SwapDone(int id)
        {
            var todoItem = _db.ToDoItems.Find(id);

            if (todoItem == null)
                return NotFound();

            if (todoItem.AuthorId != UserId)
                return Unauthorized();

            todoItem.Done = !todoItem.Done;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var todoItem = _db.ToDoItems.Find(id);

            if (todoItem == null)
                return NotFound();

            if (todoItem.AuthorId != UserId)
                return Unauthorized();

            _db.Remove(todoItem);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}