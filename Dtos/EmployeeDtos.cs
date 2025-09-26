using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Dtos
{
    public class EmployeeDtos
    {
        public class EmployeeCreateDto
        {
            [Key]
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; } = string.Empty;
            [EmailAddress]
            public string Mail { get; set; } = string.Empty;
            public string? Role { get; set; } 
            public string ProfilePictureUrl { get; set; } = string.Empty;
        }

        // For Updating an existing Employee
        public class EmployeeUpdateDto
        {
            public string EmployeeName { get; set; } = string.Empty;
            public string? Role { get; set; } 
            public string ProfilePictureUrl { get; set; } = string.Empty;
        }

        // For Reading/Returning Employee Data
        public class EmployeeReadDto
        {
            public int EmployeeId { get; set; }
            public string EmployeeName { get; set; } = string.Empty;
            public string Mail { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string ProfilePictureUrl { get; set; } = string.Empty;
        }
        public class EmployeeDeleteDto
        {
            
            public int EmployeeId { get; set; }
        }

    }
}
