

using System.ComponentModel.DataAnnotations;

namespace APIKros.Requests.Employee
{
    public class ChangeCompanyRequest
    {
        [Required]
        public int EmployeeId { get; set; }
        
        [Required]
        public int NewCompanyId { get; set; }
    
    }

    
}