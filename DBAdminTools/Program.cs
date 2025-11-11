using ClosedXML.Excel;
using MotherLoadDB;
using MotherLoadDB.Data;

namespace DBAdminTools
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var context = new AppDbContext();
            var dbService = new DbService(context);



            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Companies Database Admin Tools ===");
                Console.WriteLine("1. Upload CSV");
                Console.WriteLine("2. Create a Company");
                Console.WriteLine("3. View all companies");
                Console.WriteLine("4. Search Company");
                Console.WriteLine("5. Delete Company");
                Console.WriteLine("6. Export to CSV");
                Console.WriteLine("7. Database Statistics");
                Console.WriteLine("0. Exit");
                Console.WriteLine("Select Option: ");

                var choice = Console.ReadLine();


                switch (choice)
                {
                    case "1":
                        await UploadCSV(dbService);
                        break;
                    case "2":
                        await CreateCompany(dbService);
                        break;
                    case "3":
                        await ViewAllCompanies(dbService);
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "0":
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;

                }

                Console.WriteLine("Press any key to continue");
                Console.ReadKey();


            }
        }

        static async Task UploadCSV(DbService dbService)
        {
            Console.Write("Enter File Path: ");
            var filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found");
                return;
            }

            var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
            int count = 0;

            foreach (var row in rows)
            {
                var orgNumber = row.Cell(5).GetString();

                var company = new Company
                {
                    Name = row.Cell(1).GetString(),
                    City = row.Cell(2).GetString(),
                    Industry = row.Cell(3).GetString(),
                    OrgNumber = orgNumber,
                    Website = row.Cell(8).GetString()
                };

                await dbService.CreateCompany(company);
                count++;
                if (count % 100 == 0) Console.WriteLine($"Processed {count} companies");
            }
        }

        static async Task CreateCompany(DbService dbService)
        {
            Console.WriteLine("Create a company by filling out below");
            Console.WriteLine("Name of the company: ");
            var name = Console.ReadLine();

            Console.WriteLine("Enter the organizationnumber: ");
            var number = Console.ReadLine();

            var company = new Company
            {
                OrgNumber = number,
                Name = name
            };

            await dbService.CreateCompany(company);
        }
        static async Task ViewAllCompanies(DbService dbService)
        {
            var companies = await dbService.GetCompanies();
            foreach (var c in companies)
            {
                Console.WriteLine($"{c.Id} | {c.Name} | {c.OrgNumber}");
            }
        }




    }
}
