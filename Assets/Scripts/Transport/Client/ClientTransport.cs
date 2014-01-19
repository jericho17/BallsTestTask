using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientTransport
{
	private IGameController _controller;
	
	// The port number for the remote device.
	private const int port = 843;
	
	// ManualResetEvent instances signal completion.
	private ManualResetEvent connectDone = new ManualResetEvent(false);
	private ManualResetEvent sendDone = new ManualResetEvent(false);
	private ManualResetEvent receiveDone = new ManualResetEvent(false);
	
	// The response from the remote device.
	private String response = String.Empty;
	
	public ClientTransport (IGameController controller)
	{
		_controller = controller;
	}
	
	public void Update()
	{
		try
		{
			var message = "Update#" + _controller.GameCore.LastEventId;
			MakeRequestInternal (message);
		}
		catch(Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	
	private void MakeRequestInternal(string message)
	{
		// Data buffer for incoming data.
		byte[] bytes = new byte[1024];
		
		// Connect to a remote device.
		try {
			// Establish the remote endpoint for the socket.
			// This example uses port 11000 on the local computer.
			IPAddress ipAddress = IpHelper.ServerIp;
			IPEndPoint remoteEP = new IPEndPoint(ipAddress,843);
			
			// Create a TCP/IP  socket.
			Socket sender = new Socket(AddressFamily.InterNetwork, 
			                           SocketType.Stream, ProtocolType.Tcp );
			
			// Connect the socket to the remote endpoint. Catch any errors.
			try {
				sender.Connect(remoteEP);
				
				byte[] msg = Encoding.ASCII.GetBytes(message+"#<EOF>");
				// Send the data through the socket.
				int bytesSent = sender.Send(msg);
				
				// Receive the response from the remote device.
				int bytesRec = sender.Receive(bytes);
				
				var data = Encoding.ASCII.GetString(bytes,0,bytesRec);
				if (data.Contains("Update"))
				{
					var serializedEvent = data.Substring(6).Split('<')[0];
					_controller.AddGameEvent(serializedEvent);
				}
				// Release the socket.
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();
				
			} catch (ArgumentNullException ane) {
				Debug.Log(ane.ToString());
			} catch (SocketException se) {
				_controller.EndGame(true);
				Debug.Log(se.ToString());
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}			
		} 
		catch (Exception e) 
		{
			_controller.EndGame(true);
			Debug.Log( e.ToString());
		}
	}
	
}