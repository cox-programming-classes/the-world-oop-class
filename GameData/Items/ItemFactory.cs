using The_World.GameData.Items;

namespace The_World.GameData.GameMechanics;

public class ItemFactory
{
    public static Item BuildSwordArchetype(
        string name = "Sword",
        string description = "You could stab something with that",
        double weight = 3.5)
    {
        return Item.CreateNewItem(
            name,
            description,
            weight
        );
    }
    
    
}