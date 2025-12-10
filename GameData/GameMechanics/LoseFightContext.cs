namespace The_World.GameData.GameMechanics;

public class LoseFightContext : Context
{
    public GameContext Game { get; private set; }

    public LoseFightContext(Player player,  GameContext game, Context context)
    {
        player.NumberOfLives -= 1;
        if (player.NumberOfLives == 0)
        {
            Console.WriteLine("You've passed away for real this time. Bye! ");
            context.KeepPlaying = false;
        }
        Console.WriteLine($"You've died! You have {Player.NumberOfLives} lives left.");
        Console.WriteLine("Keep playing? Yes/No");
        Game = game;
    }
}