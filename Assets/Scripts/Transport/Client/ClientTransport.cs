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
		try{
			Debug.Log("asking update");
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
			IPAddress ipAddress = IPAddress.Parse("109.111.179.111");
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
				Debug.Log(se.ToString());
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
			
		} catch (Exception e) {
			Debug.Log( e.ToString());
		}
	}
	
	public void StartClient(string message)
	{
		// Connect to a remote device.
		try
		{
			// Establish the remote endpoint for the socket.
			// The name of the 
			// remote device is "host.contoso.com".
			IPAddress ipAddress = IPAddress.Parse("109.111.179.111");
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
			
			// Create a TCP/IP socket.
			Socket client = new Socket(AddressFamily.InterNetwork,
			                           SocketType.Stream, ProtocolType.Tcp);
			
			// Connect to the remote endpoint.
			client.BeginConnect(remoteEP,
			                    new AsyncCallback(ConnectCallback), client);
			connectDone.WaitOne();
			
			// Send test data to the remote device.
			var data = message+"#<EOF>";
			Send(client, data);
			sendDone.WaitOne();
			
			// Receive the response from the remote device.
			Receive(client);
			receiveDone.WaitOne();
			
			// Write the response to the console.
			Console.WriteLine("Response received : {0}", response);
			
			// Release the socket.
			client.Shutdown(SocketShutdown.Both);
			client.Close();
			
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
	
	private void ConnectCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.
			Socket client = (Socket)ar.AsyncState;
			
			// Complete the connection.
			client.EndConnect(ar);
			
			Console.WriteLine("Socket connected to {0}",
			                  client.RemoteEndPoint.ToString());
			
			// Signal that the connection has been made.
			connectDone.Set();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
	
	private void Receive(Socket client)
	{
		try
		{
			// Create the state object.
			StateObject state = new StateObject();
			state.WorkSocket = client;
			
			// Begin receiving the data from the remote device.
			client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
			                    new AsyncCallback(ReceiveCallback), state);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
	
	private void ReceiveCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the state object and the client socket 
			// from the asynchronous state object.
			StateObject state = (StateObject)ar.AsyncState;
			Socket client = state.WorkSocket;
			
			// Read data from the remote device.
			int bytesRead = client.EndReceive(ar);
			
			if (bytesRead > 0)
			{
				// There might be more data, so store the data received so far.
				state.Sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
				
				// Get the rest of the data.
				client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
				                    new AsyncCallback(ReceiveCallback), state);
			}
			else
			{
				// All the data has arrived; put it in response.
				if (state.Sb.Length > 1)
				{
					var result = state.Sb.ToString();
					if (result.Contains("Update"))
					{
						var serializedEvent = result.Substring(6).Split('<')[0];
						_controller.AddGameEvent(serializedEvent);
					}
					response = state.Sb.ToString();
				}
				// Signal that all bytes have been received.
				receiveDone.Set();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
	
	private void Send(Socket client, String data)
	{
		// Convert the string data to byte data using ASCII encoding.
		byte[] byteData = Encoding.ASCII.GetBytes(data);
		
		// Begin sending the data to the remote device.
		client.BeginSend(byteData, 0, byteData.Length, 0,
		                 new AsyncCallback(SendCallback), client);
	}
	
	private void SendCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.
			Socket client = (Socket)ar.AsyncState;
			
			// Complete sending the data to the remote device.
			int bytesSent = client.EndSend(ar);
			Console.WriteLine("Sent {0} bytes to server.", bytesSent);
			
			// Signal that all bytes have been sent.
			sendDone.Set();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
	}
}