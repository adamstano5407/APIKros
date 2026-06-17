namespace APIKros.DTOs.Division
{
    public class DivisionDto : IDto<Models.Division, DivisionDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public int CompanyId { get; set; }
        public int? ManagerId { get; set; }

        public static DivisionDto CreateInstance(Models.Division division)
        {
            return new DivisionDto
            {
                Id = division.Id,
                Name = division.Name,
                Code = division.Code,
                CompanyId = division.CompanyId,
                ManagerId = division.ManagerId
            };
        }
    }

}