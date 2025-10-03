namespace EmployeeTracker.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    public class CreateEmployeeDto
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; } = "Test@123"; // default password
        public string Role { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
    public class UpdateEmployeeDto
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
