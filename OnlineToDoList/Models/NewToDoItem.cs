using OnlineToDoList.Data;

namespace OnlineToDoList.Models
{
    public class NewToDoItem
    {
        public string Title { get; set; } = string.Empty;
        public List<ToDoItem> ToDoItems { get; set; } = new();
    }
}
