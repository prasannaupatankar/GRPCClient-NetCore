using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await postComapny();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static async Task postComapny()
        {
            try
            {

                Console.WriteLine("Press enter to initiate the Gprc service...");
                Console.ReadLine();
                //Update the URL with your local or remote URL
                var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                var readerClient = new Company.CompanyClient(channel);
                //Creating new employees
                var employees = new EmployeeModel[]
                {
                    new EmployeeModel
                    {
                        EmpId = 1,
                        EmpName = "Rama Bapat",
                        BirthDate = Timestamp.FromDateTime(DateTime.UtcNow.AddYears(-24).AddMonths(-4)),
                        CompanyId = 1
                    },
                    new EmployeeModel
                    {
                        EmpId = 2,
                        EmpName = "Krishna Nene",
                        BirthDate = Timestamp.FromDateTime(DateTime.UtcNow.AddYears(-22).AddMonths(-7)),
                        CompanyId = 1
                    }
                };
                //Creating new company
                var company = new CompanyModel
                {
                    CompanyId = 1,
                    ComapnyName = "Patankar Khauwale"
                };

                //Printing the Company & Employee details
                //This part can be removed, nothing to do with the functionality
                Console.WriteLine("----------Employee details----------");                
                foreach (EmployeeModel emp in employees)
                {
                    Console.WriteLine("Id : " + emp.EmpId);
                    Console.WriteLine("Name : " + emp.EmpName);
                    Console.WriteLine("Birthdate : " + emp.BirthDate);
                    Console.WriteLine("CompanyId : " + emp.CompanyId);
                    Console.WriteLine("----------");
                }
                Console.WriteLine("----------Company details----------");                
                Console.WriteLine("Id : " + company.CompanyId);
                Console.WriteLine("Name : " + company.ComapnyName);
                Console.WriteLine("Press enter to call the Gprc service...");
                Console.ReadLine();

                //Adding employees to company model
                company.Employees.Add(employees);
                //Calling the server channel
                var responseData = await readerClient.PostCompanyWithEmployeesAsync(company);

                if (responseData.Status == 1)
                { 
                    Console.WriteLine("Company & Employees added Successfully.");
                }
                else 
                {
                    Console.WriteLine("Company & Employees could not be added, please try again.");
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
