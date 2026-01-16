namespace YamyProject.UI.Manufacturing.Models
{
    public class EmployeeModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }

        public override string ToString()
        {
            //return Name;
            return $"{Name} - {Department} ({Position})";
        }
    }
}