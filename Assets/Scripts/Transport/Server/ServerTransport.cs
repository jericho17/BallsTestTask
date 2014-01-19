using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Linq;
using UnityEngine;

public class ServerTransport
{
	private IGameController _controller;
	
	private const int MaxConnections = 10;
	private const int DefaultPort = 843;
	private const int MessageSize = 1024;
	
	private Thread _executer;
	private bool _aborted;
	private Socket _listener;
	
	public static ManualResetEvent allDone = new ManualResetEvent(false);
	
	
	public ServerTransport (IGameController controller)
	{
		_controller = controller;
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
						bytes = new byte[1024];
						var bytesRec = handler.Receive(bytes);
						data += Encoding.ASCII.GetString(bytes,0,bytesRec);
						
						if (data.IndexOf("<EOF>") > -1) 						
							break;
					} 
					catch (Exception ex) 
					{
						Debug.Log(ex);
					}
				}
				
				// Echo the data back to the client.
				byte[] msg = Encoding.ASCII.GetBytes(data);
				if (data.Contains("Update"))
				{
					try 
					{
						var lastGameEventId = int.Parse(data.Split('#')[1]);
						var actualEventId = _controller.GameCore.ProcessedGameEvents.SingleOrDefault(x=>x.Id == lastGameEventId+1);
						var serialized = actualEventId!=null
							?"Update"+actualEventId.Serialize()
								:"";
						
						handler.Send(Encoding.ASCII.GetBytes(serialized+"<EOF>"));	
					} 
					catch (Exception ex) 
					{
						Debug.Log(ex.ToString());
					}
					
				}
				
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
	
	public void AcceptCallback(IAsyncResult ar)
	{
		// Signal the main thread to continue.
		allDone.Set();
		
		// Get the socket that handles the client request.
		var listener = (Socket)ar.AsyncState;
		var handler = listener.EndAccept(ar);
		
		// Create the state object.
		var state = new StateObject();
		state.WorkSocket = handler;
		handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
		                     new AsyncCallback(ReadCallback), state);
	}
	
	public void ReadCallback(IAsyncResult ar)
	{
		var content = String.Empty;
		var state = (StateObject)ar.AsyncState;
		var handler = state.WorkSocket;
		
		// Read data from the client socket. 
		int bytesRead = handler.EndReceive(ar);
		
		if (bytesRead > 0)
		{
			// There  might be more data, so store the data received so far.
			state.Sb.Append(Encoding.ASCII.GetString(
				state.Buffer, 0, bytesRead));
			
			// Check for end-of-file tag. If it is not there, read 
			// more data.
			content = state.Sb.ToString();
			if (content.IndexOf("<EOF>") > -1)
			{
				if (content.Contains("Update"))
				{
					var gameEventId = int.Parse(content.Split('#')[1]);
					var gameEvent = _controller.GameCore.ProcessedGameEvents.Single(x=>x.Id == gameEventId);
					Send(handler, "Update"+gameEvent.Serialize()+"<EOF>");
				}
			}
			else
			{
				// Not all data received. Get more.
				handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
				                     new AsyncCallback(ReadCallback), state);
			}
		}
	}
	
	private void Send(Socket handler, String data)
	{
		// Convert the string data to byte data using ASCII encoding.
		byte[] byteData = Encoding.ASCII.GetBytes(data);
		
		// Begin sending the data to the remote device.
		handler.BeginSend(byteData, 0, byteData.Length, 0,
		                  new AsyncCallback(SendCallback), handler);
	}
	
	private void SendCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.
			var handler = (Socket)ar.AsyncState;
			
			// Complete sending the data to the remote device.
			int bytesSent = handler.EndSend(ar);
			
			handler.Shutdown(SocketShutdown.Both);
			handler.Close();			
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}
}