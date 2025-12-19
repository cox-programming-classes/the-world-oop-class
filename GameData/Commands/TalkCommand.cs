using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class TalkCommand : ICommand
{
    private readonly string _npcName;

    public TalkCommand(string npcName = "")
    {
        _npcName = npcName?.Trim() ?? "";
    }

    public Context Execute(Context c)
    {
        if (c is not GameContext context)
        {
            Console.WriteLine("You can't do that right now.");
            return c;
        }

        if (string.IsNullOrWhiteSpace(_npcName))
        {
            Console.WriteLine("Talk to whom? Try 'talk [npc]'");
            return context;
        }

        if (context.CurrentArea.NPCs.TryGetValue(_npcName, out var npc))
        {
            Console.WriteLine(npc.Interact());
        }
        else
        {
            Console.WriteLine($"There is no '{_npcName}' here to talk to.");
        }

        return context;
    }

    public string GetHelpText() => "talk [npc] - Talk to an NPC";
}