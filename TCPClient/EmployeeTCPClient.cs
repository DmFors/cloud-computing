using System.Net.Sockets;

public class EmployeeTCPClient
{
    private static TcpClient _client;

    public static void Main(string[] args)
    {
        string serverIP = "127.0.0.1";
        int port = 8888;
        Start(serverIP, port);
    }

    public static void Start(string serverIP, int serverPort)
    {
        try
        {
            _client = new(serverIP, serverPort);

            Stream stream = _client.GetStream();
            StreamReader streamReader = new(stream);
            StreamWriter streamWriter = new(stream);
            streamWriter.AutoFlush = true;

            while (true)
            {
                Console.Write("Введите имя (или exit): ");
                string? name = Console.ReadLine();

                if (name == "exit")
                {
                    break;
                }

                streamWriter.WriteLine(name);

                Console.WriteLine(streamReader.ReadLine());
            }

            stream.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
        finally
        {
            _client?.Close();
            Console.WriteLine($"Отсоединен от сервера: {serverIP}:{serverPort}");
        }
        Console.ReadKey();
    }
}
