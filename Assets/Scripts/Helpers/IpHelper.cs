using System;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;
using System.Net.Sockets;

/// <summary>
/// Helps to get machine's ip. 
/// Dns.GetHostName () returns null, when computer have gray ip.
/// </summary>
public static class IpHelper
{
	public static IPAddress Ip 
	{ 
		get
		{
			lock (locker) 
			{
				if (_ip == null)
					_ip = GetIp();
				return _ip;
			}
		}}
	private static IPAddress _ip;
	
	
	public static IPAddress ServerIp
	{
		get
		{
			return _serverIp;
		}
	}
	private static IPAddress _serverIp = null;
	
	static object locker = new object();
	
	static IPAddress GetIp ()
	{
		lock (locker) 
		{
			var hostName = Dns.GetHostName ();
			if (hostName != null) 
			{
				IPHostEntry ipHostInfo = Dns.GetHostEntry (hostName);
				return ipHostInfo.AddressList [0];
			}
			else
			{
				try 
				{
					var networkInterface = NetworkInterface.GetAllNetworkInterfaces ().First();
					var ipProperties = networkInterface.GetIPProperties();
					var uni = ipProperties.UnicastAddresses;
					var localAddress = uni.FirstOrDefault(x=>x.Address.AddressFamily == AddressFamily.InterNetwork);
					return localAddress != null 
						? localAddress.Address
							: new IPAddress( new byte[] {127, 0, 0, 1} );	
				} catch (Exception ex) {
					UnityEngine.Debug.Log(ex.ToString());
					return null;
				}
				
			}
			
		}
		
	}
	
	public static bool CheckValid(string address)
	{
		IPAddress ip;
		var successful = IPAddress.TryParse(address, out ip);
		return successful;
	}
	
	public static void SetServerIp(string ip)
	{
		IPAddress serverIp;
		var done = IPAddress.TryParse(ip, out serverIp);
		if (done)
			_serverIp = serverIp;
	}
}