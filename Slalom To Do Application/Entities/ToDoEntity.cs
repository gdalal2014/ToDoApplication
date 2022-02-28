namespace Slalom_To_Do_Application.Entities
{
    public class ToDoEntity
    {
        public string list_id { get; set; }
        public string user_id { get; set; }
        public string item { get; set; }
        public string create_date { get; set; }
        public string modified_date { get; set; }
        public string is_completed { get; set; }
        public string completion_date { get; set; }

    }
}
