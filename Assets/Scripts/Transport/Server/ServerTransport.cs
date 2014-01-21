using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Linq;
using UnityEngine;

public class ServerTransport
{
	private ServerProtocol _serverProtocol;
	
	private const int MaxConnections = 10;
	private const int DefaultPort = Transport.Port;
	private const int MessageSize = Transport.PacketSize;
	
	private Thread _executer;
	private bool _aborted;
	private Socket _listener;
	
	public static ManualResetEvent allDone = new ManualResetEvent(false);
	
	
	public ServerTransport (ServerProtocol serverProtocol)
	{
		_serverProtocol = serverProtocol;
	}
	
	public void StartListening()
	{
		try 
		{
			_executer = new System.Threading.Thread (StartListeningInternal);
			_executer.Start ();
			_aborted = false;			
		} 
		catch (Exception ex) 
		{
			Debug.Log(ex);
		}
	}
	
	public void StopListening()
	{
		_aborted = true;
		try 
		{
			_listener.Close();
			_executer.Abort ();
		}
		catch (Exception ex) 
		{
			Debug.Log(ex);
		}
	}
	
	private void StartListeningInternal()
	{
		byte[] bytes = new Byte[MessageSize];
		
		try 
		{
			var localEndPoint = new IPEndPoint(IpHelper.Ip, DefaultPort);
			
			_listener = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_listener.Bind(localEndPoint);
			_listener.Listen(MaxConnections);			
			
			while (!_aborted) 
			{
				Socket handler = _listener.Accept();
				var data = "";
				
				while (!_aborted) 
				{
					try 
					{
						bytes = new byte[Transport.PacketSize];
						var bytesRec = handler.Receive(bytes);
						data += Encoding.ASCII.GetString(bytes,0,bytesRec);
						
						if (data.IndexOf(Transport.EndFlag) > -1) 						
							break;
					} 
					catch (Exception ex) 
					{
						Debug.Log(ex);
					}
				}
				
				// Echo the data back to the client.
				var result = _serverProtocol.ProcessRequest(data);
				if (result!=null)
					handler.Send(Encoding.ASCII.GetBytes(result+Transport.EndFlag));	

				
				handler.Shutdown(SocketShutdown.Both);
				handler.Close();
			}
			
			_listener.Shutdown(SocketShutdown.Both);
			_listener.Close();
		} 
		catch (Exception e) 
		{
			Debug.Log(e);
		}
	}
}