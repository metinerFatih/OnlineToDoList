using Microsoft.AspNetCore.Identity;

namespace OnlineToDoList.Data
{
    public class ApplicationUser : IdentityUser
    {
        public List<ToDoItem>? ToDoItems { get; set; } = new(); // default olarak boş bi liste.
    }

}
