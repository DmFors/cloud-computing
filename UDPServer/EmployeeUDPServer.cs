using System.Net;
using System.Net.Sockets;
using System.Text;

public class EmployeeUDPServer
{
    private static UdpClient _client;
    private static Dictionary<string, string> _employees = new()
    {
        { "murad", "programmer" },
        { "dima", "game developer" },
        { "emil", "data scientist" },
    };

    public static void Main(string[] args)
    {
        int serverPort = 8888;
        Start(serverPort);
    }

    private static void Start(int serverPort)
    {
        _client = new(serverPort);

        Console.WriteLine($"Сервер запущен на порте {serverPort}");

        IPEndPoint? remoteEP = null;
        while (true)
        {
            byte[] rowData = _client.Receive(ref remoteEP);
            string employeeName = Encoding.UTF8.GetString(rowData);

            if (string.IsNullOrEmpty(employeeName))
            {
                break;
            }

            string response;

            if (_employees.TryGetValue(employeeName, out string? employeeJob))
            {
                response = employeeJob;
            }
            else
            {
                Console.WriteLine($"Работник не найден: {employeeName}");
                response = "Работник не найден";
            }

            byte[] sendData = Encoding.UTF8.GetBytes(response);
            _client.Send(sendData, sendData.Length, remoteEP);
        }
    }
}