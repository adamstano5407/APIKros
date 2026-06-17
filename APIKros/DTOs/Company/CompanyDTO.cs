namespace APIKros.DTOs.Company
{
    public class CompanyDto : IDto<Models.Company, CompanyDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public int? DirectorId { get; set; }

        public static CompanyDto CreateInstance(Models.Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Code = company.Code,
                DirectorId = company.DirectorId
            };
        }
    }

}