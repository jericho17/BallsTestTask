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
	
	public static ManualResetEvent allDone = new ManualResetEvent(false);
	
	
	public ServerTransport (IGameController controller)
	{
		_controller = controller;
	}
	
	public void StartListening()
	{
		var t = new System.Threading.Thread (StartListeningInternal2);
		t.Start ();
	}
	
	private void StartListeningInternal2()
	{
		byte[] bytes = new Byte[1024];
		
		// Establish the local endpoint for the socket.
		// Dns.GetHostName returns the name of the 
		// host running the application.
		IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
		IPAddress ipAddress = ipHostInfo.AddressList[0];
		IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 843);
		
		// Create a TCP/IP socket.
		Socket listener = new Socket(AddressFamily.InterNetwork,
		                             SocketType.Stream, ProtocolType.Tcp );
		
		// Bind the socket to the local endpoint and 
		// listen for incoming connections.
		try {
			listener.Bind(localEndPoint);
			listener.Listen(10);
			
			// Start listening for connections.
			while (true) {
				Console.WriteLine("Waiting for a connection...");
				// Program is suspended while waiting for an incoming connection.
				Socket handler = listener.Accept();
				var data = "";
				
				// An incoming connection needs to be processed.
				while (true) {
					try {
						bytes = new byte[1024];
						int bytesRec = handler.Receive(bytes);
						data += Encoding.ASCII.GetString(bytes,0,bytesRec);
						if (data.IndexOf("<EOF>") > -1) {
							break;
						}	
					} catch (Exception ex) {
						Debug.Log(ex);
					}
					
				}
				
				// Show the data on the console.
				Console.WriteLine( "Text received : {0}", data);
				
				// Echo the data back to the client.
				byte[] msg = Encoding.ASCII.GetBytes(data);
				if (data.Contains("Update"))
				{
					try {
						var gameEventId = int.Parse(data.Split('#')[1]);
						var gameEvent = _controller.GameCore.ProcessedGameEvents.SingleOrDefault(x=>x.Id == gameEventId+1);
						var serialized = gameEvent!=null
							?gameEvent.Serialize()
								:"";
						
						handler.Send(Encoding.ASCII.GetBytes("Update"+gameEvent.Serialize()+"<EOF>"));	
					} catch (Exception ex) {
						Debug.Log(ex.ToString());
					}
					
				}
				
				handler.Shutdown(SocketShutdown.Both);
				handler.Close();
			}
			
		} catch (Exception e) {
			Console.WriteLine(e.ToString());
		}
	}
	
	private void StartListeningInternal()
	{
		// Establish the local endpoint for the socket.
		// The DNS name of the computer
		// running the listener is "host.contoso.com".
		IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
		IPAddress ipAddress = ipHostInfo.AddressList[0];
		IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 843);
		
		// Create a TCP/IP socket.
		Socket listener = new Socket(AddressFamily.InterNetwork,
		                             SocketType.Stream, ProtocolType.Tcp);
		
		// Bind the socket to the local endpoint and listen for incoming connections.
		try
		{
			listener.Bind(localEndPoint);
			listener.Listen(100);
			
			while (true)
			{
				// Set the event to nonsignaled state.
				allDone.Reset();
				
				// Start an asynchronous socket to listen for connections.
				Console.WriteLine("Waiting for a connection...");
				listener.BeginAccept(
					new AsyncCallback(AcceptCallback),
					listener);
				
				// Wait until a connection is made before continuing.
				allDone.WaitOne();
			}
			
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
		
		Console.WriteLine("\nPress ENTER to continue...");
		Console.Read();
		
	}
	
	public void AcceptCallback(IAsyncResult ar)
	{
		// Signal the main thread to continue.
		allDone.Set();
		
		// Get the socket that handles the client request.
		Socket listener = (Socket)ar.AsyncState;
		Socket handler = listener.EndAccept(ar);
		
		// Create the state object.
		StateObject state = new StateObject();
		state.WorkSocket = handler;
		handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
		                     new AsyncCallback(ReadCallback), state);
	}
	
	public void ReadCallback(IAsyncResult ar)
	{
		String content = String.Empty;
		
		// Retrieve the state object and the handler socket
		// from the asynchronous state object.
		StateObject state = (StateObject)ar.AsyncState;
		Socket handler = state.WorkSocket;
		
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
			Socket handler = (Socket)ar.AsyncState;
			
			// Complete sending the data to the remote device.
			int bytesSent = handler.EndSend(ar);
			Console.WriteLine("Sent {0} bytes to client.", bytesSent);
			
			handler.Shutdown(SocketShutdown.Both);
			handler.Close();
			
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
}