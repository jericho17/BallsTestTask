using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Linq;
using UnityEngine;

public class ServerProtocol
{
	private IGameController _controller;

	public ServerProtocol (IGameController controller)
	{
		_controller = controller;
	}

	public string ProcessRequest(string request)
	{
		if (request.Contains(Transport.UpdateFlag))
		{
			try 
			{
				var lastGameEventId = int.Parse(request.Split(Transport.Separator)[1]);
				var newEventId = lastGameEventId+1;
				var actualEventId = _controller.GameCore.ProcessedGameEvents.SingleOrDefault(x=>x.Id == newEventId);
				var serialized = actualEventId!=null
					?Transport.UpdateFlag+actualEventId.Serialize()
					:null;				
				return serialized;
			} 
			catch (Exception ex) 
			{
				Debug.Log(ex.ToString());
			}
			
		}
		return null;
	}
}