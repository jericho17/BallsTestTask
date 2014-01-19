public interface IGameController
{
	GameCore GameCore{ get; }
	
	void CreateNewGame();
	void AddGameEvent(GameEvent gameEvent);
	void AddGameEvent(string serializedEvent);
	void BallClick(string ballName);
	void Update();
	void EndGame(bool hasErrors);
}