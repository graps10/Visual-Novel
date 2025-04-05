using Naninovel;

public interface IMiniGameManager : IEngineService
{
    UniTask StartGameAsync(int pairsCount, AsyncToken asyncToken = default);
    UniTask<bool> WaitForGameCompletionAsync(AsyncToken asyncToken = default);
    void ForceEndGame();
}