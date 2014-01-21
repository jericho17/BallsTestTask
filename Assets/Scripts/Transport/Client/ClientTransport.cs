using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientTransport
{
	private ClientProtocol _clientProtocol;
	
	// The port number for the remote device.
	private const int port = Transport.Port;

	// The response from the remote device.
	private String response = String.Empty;
	
	public ClientTransport (ClientProtocol clientProtocol)
	{
		_clientProtocol = clientProtocol;
	}
	
	public void Update()
	{
		try
		{
			var message = _clientProtocol.GetUpdateRequest();
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
		byte[] bytes = new byte[Transport.PacketSize];
		
		// Connect to a remote device.
		try 
		{
			// Establish the remote endpoint for the socket.
			IPAddress ipAddress = IpHelper.ServerIp;
			IPEndPoint remoteEP = new IPEndPoint(ipAddress,port);
			
			// Create a TCP/IP  socket.
			Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );
			
			// Connect the socket to the remote endpoint. Catch any errors.
			try
			{
				sender.Connect(remoteEP);
				
				byte[] msg = Encoding.ASCII.GetBytes(message+Transport.EndFlag);
				// Send the data through the socket.
				int bytesSent = sender.Send(msg);
				
				// Receive the response from the remote device.
				int bytesRec = sender.Receive(bytes);
				
				var data = Encoding.ASCII.GetString(bytes,0,bytesRec);
				_clientProtocol.ProcessResponse(data);

				// Release the socket.
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();
				
			}
			catch (ArgumentNullException ane) 
			{
				_clientProtocol.NotifyError();
				Debug.Log(ane.ToString());
			} 
			catch (SocketException se) 
			{
				_clientProtocol.NotifyError();
				Debug.Log(se.ToString());
			}
			catch (Exception e) 
			{
				_clientProtocol.NotifyError();
				Debug.Log(e.ToString());
			}			
		} 
		catch (Exception e) 
		{
			_clientProtocol.NotifyError();
			Debug.Log( e.ToString());
		}
	}
	
}