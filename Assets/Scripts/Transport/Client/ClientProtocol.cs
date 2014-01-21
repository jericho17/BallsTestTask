using System;

public class ClientProtocol
{
	private IGameController _controller;

	public ClientProtocol (IGameController controller)
	{
		_controller = controller;
	}

	public string GetUpdateRequest()
	{
		var request = Transport.UpdateFlag + _controller.GameCore.LastEventId;
		return request;
	}

	public void ProcessResponse(string response)
	{
		if (response.Contains(Transport.UpdateFlag))
		{
			var serializedEvent = response.Substring(Transport.HeaderSize).Split(Transport.Separator)[0];
			_controller.AddGameEvent(serializedEvent);
		}
	}

	public void NotifyError()
	{
		_controller.EndGame (true);
	}
}