namespace APIKros.Requests.Company
{
    public class CreateCompanyRequest 
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int? DirectorId { get; set; }

    }
}

