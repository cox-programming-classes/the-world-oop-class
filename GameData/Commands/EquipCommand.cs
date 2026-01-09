using The_World.GameData.GameMechanics;

namespace The_World.GameData.Commands;

public class EquipCommand : ICommand
{
    private readonly string _itemName;

    public EquipCommand(string itemName = "")
    {
        _itemName = itemName?.Trim() ?? "";
    }

    public Context Execute(Context c)
    {
        if (c is not GameContext context)
        {
            Console.WriteLine("You can't do that right now.");
            return c;
        }

        if (string.IsNullOrWhiteSpace(_itemName))
        {
            Console.WriteLine("Equip what? Try 'equip [item]'");
            return context;
        }

        // Find item in inventory
        var item = context.Player.Inventory.FirstOrDefault(i => 
            i.Name.Contains(_itemName, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            Console.WriteLine($"You don't have '{_itemName}' in your inventory.");
            return context;
        }

        // Try to equip using Strategy Pattern
        var result = context.Player.Equipment.TryEquip(item, context.Player.Inventory);
        Console.WriteLine(result.Message);

        return context;
    }

    public string GetHelpText() => "equip [item] - Equip a weapon or armor";
}