using System.Net;
using System.Net.Sockets;
using System.Text;

public class EmployeeUDPClient
{
    private static UdpClient _client;

    public static void Main(string[] args)
    {
        string serverIP = "127.0.0.1";
        int port = 8888;
        _client = new(serverIP, port);
        IPEndPoint? remoteEP = null;
        while (true)
        {
            Console.Write("Введите имя (или exit): ");
            string? name = Console.ReadLine();

            if (name == "exit")
            {
                break;
            }

            byte[] sendData = Encoding.UTF8.GetBytes(name);
            _client.Send(sendData, sendData.Length);

            byte[] receiveData = _client.Receive(ref remoteEP);
            string employeeJob = Encoding.UTF8.GetString(receiveData);
            Console.WriteLine(employeeJob);
        }
    }
}