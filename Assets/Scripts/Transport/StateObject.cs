using System.Text;
using System.Net.Sockets;

public class StateObject
{
	public Socket WorkSocket = null;
	public const int BufferSize = 1024;
	public byte[] Buffer = new byte[BufferSize];
	public StringBuilder Sb = new StringBuilder();
}