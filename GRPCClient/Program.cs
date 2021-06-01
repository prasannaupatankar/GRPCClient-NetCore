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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static async Task postComapny()
        {
            try
            {

                Console.WriteLine("press enter to call the Gprc service...");
                Console.ReadLine();
                var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
                var readerClient = new Company.CompanyClient(channel);

                var emps = new EmployeeModel[]
                {
                new EmployeeModel
                {
                     EmpId = 1,
                     EmpName = "John Doe",
                     BirthDate = Timestamp.FromDateTime(DateTime.UtcNow),
                     CompanyId = 1
                },
                new EmployeeModel
                {
                     EmpId = 2,
                     EmpName = "Mark Lue",
                     BirthDate = Timestamp.FromDateTime(DateTime.UtcNow),
                     CompanyId = 1
                }
                };

                var compData = new CompanyModel
                {
                    CompanyId = 1,
                    ComapnyName = "GRPC Company"
                };
                compData.Employees.Add(emps);
                var responseData = await readerClient.PostCompanyWithEmployeesAsync(compData);
                Console.WriteLine($"Status : {responseData.Status}");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
