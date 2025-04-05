using Naninovel;
using UnityEngine;

[InitializeAtRuntime]
public class MiniGameManager : IMiniGameManager
{
    private IUIManager uiManager;
    private MiniGame gameInstance;

    private UniTaskCompletionSource<bool> gameCompletionSource;

    public MiniGameManager(IUIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    public virtual UniTask InitializeServiceAsync()
    {
        return UniTask.CompletedTask;
    }
    public void ResetService() => ForceEndGame();
    public void DestroyService() => ForceEndGame();

    public async UniTask StartGameAsync(int pairsCount, AsyncToken asyncToken = default)
    {
        var miniGame = uiManager.GetUI<MiniGamePanel>();

        miniGame.GetComponent<MiniGame>().Initialize(pairsCount, OnGameCompleted);
        miniGame.Show();

        // Wait for game to complete
        gameCompletionSource = new UniTaskCompletionSource<bool>();
        await gameCompletionSource.Task;

        miniGame.Hide();
    }

    public UniTask<bool> WaitForGameCompletionAsync(AsyncToken asyncToken = default)
    {
        return gameCompletionSource?.Task ?? UniTask.FromResult(false);
    }

    public void ForceEndGame()
    {
        gameCompletionSource?.TrySetResult(false);
        if (gameInstance != null)
            Object.Destroy(gameInstance.gameObject);
    }

    private void OnGameCompleted(bool success)
    {
        gameCompletionSource?.TrySetResult(success);
    }
}