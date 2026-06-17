using System.ComponentModel.DataAnnotations;

namespace APIKros.Requests.Company
{
    public class CreateCompanyRequest 
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Code { get; set; } = null!;
        
        public int? DirectorId { get; set; }

    }
}

