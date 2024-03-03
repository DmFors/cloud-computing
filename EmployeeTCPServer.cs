using System.Net;
using System.Net.Sockets;

public class EmployeeTCPServer
{
    private const int LIMIT = 16;
    private static TcpListener _server;
    private static Dictionary<string, string> _employees = new()
    {
        { "murad", "programmer" },
        { "dima", "game developer" },
        { "emil", "data scientist" },
    };

    public static void Main(string[] args)
    {
        string serverIP = "127.0.0.1";
        int serverPort = 8888;
        Start(serverIP, serverPort);

        Console.ReadKey();
    }

    public static void Start(string serverIP, int serverPort)
    {
        if (IPAddress.TryParse(serverIP, out IPAddress? serverIPAddress))
        {
            _server = new(serverIPAddress, serverPort);
            _server.Start();

            Console.WriteLine($"Сервер {_server.LocalEndpoint} запущен");

            for (int _ = 0; _ < LIMIT; _++)
            {
                Thread t = new(new ThreadStart(ListenSocket));
                t.Start();
            }
        }
        else
        {
            Console.WriteLine("Некорректный IP адрес сервера.");
        }
    }

    private static void ListenSocket()
    {
        try
        {
            while (true)
            {
                using Socket soc = _server.AcceptSocket(); // блокируем поток выполнения до момента присоединения клиента
                Console.WriteLine($"Присоединен клиент: {soc.RemoteEndPoint}");

                try
                {
                    Stream stream = new NetworkStream(soc);
                    StreamReader streamReader = new(stream);
                    StreamWriter streamWriter = new(stream);
                    streamWriter.AutoFlush = true;

                    while (true) // считываем имена сотрудников от клиента
                    {
                        string? employeeName = streamReader.ReadLine();

                        if (string.IsNullOrEmpty(employeeName))
                        {
                            break;
                        }

                        if (_employees.TryGetValue(employeeName, out string? employeeJob))
                        {
                            streamWriter.WriteLine(employeeJob);
                        }
                        else
                        {
                            Console.WriteLine("Работник не найден");
                            streamWriter.WriteLine("Работник не найден");
                        }
                    }

                    stream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine($"Отсоединен: {soc.RemoteEndPoint}");
                    soc.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _server.Stop();
        }
    }
}
