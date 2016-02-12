using System.Net;
using System.Net.Sockets;
using System.Text;
public class Sender
{
    public void Send()
    {
        UdpClient client = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 15000);
        byte[] bytes = Encoding.ASCII.GetBytes("Foo");
        client.Send(bytes, bytes.Length, ip);
        client.Close();
    }
}