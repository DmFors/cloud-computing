using System.Net;
using System.Net.Sockets;
using System.Text;

public class ShowIP
{
    public static void WriteIPAdress(string[] args)
    {
        string name = args.Length > 0 ? args[0] : Dns.GetHostName();

        try
        {
            IPAddress[] iPAddresses = Dns.Resolve(name).AddressList;
            foreach (var address in iPAddresses)
            {
                Console.WriteLine($"{name}/{address}");
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"{ex.Message}: {name}");
        }
    }

    public static async Task CheckConnection()
    {
        string url = "ya.ru";
        int port = 80;

        using Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            await socket.ConnectAsync(url, port);
            Console.WriteLine($"Успешное соединение с {url}:{port}");
            Console.WriteLine($"Адрес хоста: {socket.RemoteEndPoint}");
            Console.WriteLine($"Адрес приложения {socket.LocalEndPoint}");

            string message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: close\r\n\r\n";

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            int bytesSendAmount = await socket.SendAsync(messageBytes, SocketFlags.None);

            Console.WriteLine($"Хосту {socket.RemoteEndPoint} отправлено {bytesSendAmount} байт от приложения {socket.LocalEndPoint}");

            StringBuilder sb = new();
            byte[] packetResponseBytes = new byte[512];
            int packetBytesAmount;
            do
            {
                packetBytesAmount = await socket.ReceiveAsync(packetResponseBytes, SocketFlags.None);
                string packetResponse = Encoding.UTF8.GetString(packetResponseBytes, 0, packetBytesAmount);
                sb.Append(packetResponse);
            } while (packetBytesAmount > 0);

            Console.WriteLine($"Ответ от {url}:\n{sb}. Всего символов: {sb.Length}.");
        }
        catch (SocketException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
