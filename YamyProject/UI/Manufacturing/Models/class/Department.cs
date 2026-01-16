namespace YamyProject.UI.Manufacturing.Models
{
    class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AssignedEmployeeId { get; set; } // Assuming you have an employee ID for the department
    }
}