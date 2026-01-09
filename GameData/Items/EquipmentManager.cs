
// GameData/Equipment/EquipmentManager.cs

using The_World.GameData.Items;

namespace The_World.GameData.Equipment;

/// <summary>
/// Manages player equipment using Strategy Pattern
/// Single Responsibility: Only handles equipment operations
/// </summary>
public class EquipmentManager
{
    private readonly Dictionary<Type, Item?> _equippedItems = new();
    
    // Encapsulation - private fields with public methods
    private Weapon? _equippedWeapon;
    private readonly Dictionary<ArmorSlot, Armor?> _equippedArmor = new();
    
    public Weapon? EquippedWeapon => _equippedWeapon;
    public IReadOnlyDictionary<ArmorSlot, Armor?> EquippedArmor => _equippedArmor.AsReadOnly();
    
    // Strategy Pattern - different equipment types have different equip logic
    public EquipResult TryEquip(Item item, List<Item> inventory)
    {
        return item switch
        {
            Weapon weapon => EquipWeapon(weapon, inventory),
            Armor armor => EquipArmor(armor, inventory),
            _ => new EquipResult(false, $"You cannot equip {item.Name}.")
        };
    }
    
    private EquipResult EquipWeapon(Weapon weapon, List<Item> inventory)
    {
        var previousWeapon = _equippedWeapon;
        _equippedWeapon = weapon;
        inventory.Remove(weapon);
        
        if (previousWeapon != null)
        {
            inventory.Add(previousWeapon);
            return new EquipResult(true, 
                $"You equipped {weapon.Name} and unequipped {previousWeapon.Name}.");
        }
        
        return new EquipResult(true, $"You equipped {weapon.Name}.");
    }
    
    private EquipResult EquipArmor(Armor armor, List<Item> inventory)
    {
        var previousArmor = _equippedArmor.GetValueOrDefault(armor.Slot);
        _equippedArmor[armor.Slot] = armor;
        inventory.Remove(armor);
        
        if (previousArmor != null)
        {
            inventory.Add(previousArmor);
            return new EquipResult(true, 
                $"You equipped {armor.Name} and unequipped {previousArmor.Name}.");
        }
        
        return new EquipResult(true, $"You equipped {armor.Name}.");
    }
    
    public EquipResult TryUnequip(string itemName, List<Item> inventory)
    {
        // Try to unequip weapon
        if (_equippedWeapon?.Name.Contains(itemName, StringComparison.OrdinalIgnoreCase) == true)
        {
            var weapon = _equippedWeapon;
            _equippedWeapon = null;
            inventory.Add(weapon);
            return new EquipResult(true, $"You unequipped {weapon.Name}.");
        }
        
        // Try to unequip armor
        foreach (var (slot, armor) in _equippedArmor.ToList())
        {
            if (armor?.Name.Contains(itemName, StringComparison.OrdinalIgnoreCase) == true)
            {
                _equippedArmor[slot] = null;
                inventory.Add(armor);
                return new EquipResult(true, $"You unequipped {armor.Name}.");
            }
        }
        
        return new EquipResult(false, $"You don't have '{itemName}' equipped.");
    }
    
    // Calculate total defense bonus (Polymorphism in action)
    public int GetTotalDefense()
    {
        return _equippedArmor.Values
            .Where(armor => armor != null)
            .Sum(armor => armor!.DefenseValue);
    }
    
    // Calculate total attack bonus
    public int GetTotalAttack()
    {
        return _equippedWeapon?.AttackPower ?? 1; // Base attack of 1 if no weapon
    }
}

// Result object (Encapsulation)
public record EquipResult(bool Success, string Message);
