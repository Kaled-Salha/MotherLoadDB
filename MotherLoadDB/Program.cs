using MotherLoadDB.Data;

namespace MotherLoadDB
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var contexdt = new AppDbContext();
            var dbService = new DbService(contexdt);

            var company = new Company
            {
                OrgNumber = "123456",
                Name = "TestAB"
            };

            await dbService.CreateCompany(company);
            var allCompanies = await dbService.GetCompanies();

            foreach (var c in allCompanies)
            {
                Console.WriteLine($"{c.Id} {c.Name} {c.OrgNumber}");
                await dbService.DeleteCompany(c.Id);
                Console.WriteLine("Deleted the company");
            }
        }
    }
}
