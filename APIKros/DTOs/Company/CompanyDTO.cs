using APIKros.Models;

namespace APIKros.DTOs
{
    public class CompanyDTO : IDto<Company, CompanyDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public int? DirectorId { get; set; }

        public static CompanyDTO CreateInstance(Company company)
        {
            return new CompanyDTO
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                DirectorId = company.DirectorId
            };
        }
    }

}