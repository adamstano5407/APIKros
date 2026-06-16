using APIKros.Models;
using System.Linq;


namespace APIKros.DTOs
{
    public class DivisionDTO : IDto<Division, DivisionDTO>
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public int CompanyId { get; set; }
        public int? ManagerId { get; set; }

        public static DivisionDTO CreateInstance(Division division)
        {
            return new DivisionDTO
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