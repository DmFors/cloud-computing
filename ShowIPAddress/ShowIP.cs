using System.Net;
using System.Net.Sockets;

public class ShowIP
{
    public static void Main(string[] args)
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
}
