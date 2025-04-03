using Naninovel;
using UnityEngine;

[CommandAlias("cardGame")]
public class StartCardGameCommand : Command
{
    public IntegerParameter Pairs;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        var cardGameService = Engine.GetService<IMiniGameService>();
        await cardGameService.StartGameAsync(Pairs, asyncToken);
        bool success = await cardGameService.WaitForGameCompletionAsync(asyncToken);

        if (success)
            Debug.Log("Player won the card game!");
        else
            Debug.Log("Card game was ended");
    }

}