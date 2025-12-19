namespace The_World.GameData;

/// <summary>
/// Represents currency in the game using Encapsulation to prevent invalid states
/// Demonstrates Single Responsibility - only manages currency operations
/// </summary>
public class Currency
{
    private int _amount;
    
    public int Amount 
    { 
        get => _amount;
        private set => _amount = Math.Max(0, value); // Encapsulation - never allow negative currency
    }
    
    public Currency(int initialAmount = 0)
    {
        Amount = initialAmount;
    }
    
    // Encapsulated operations
    public bool CanAfford(int cost) => Amount >= cost && cost > 0;
    
    public bool Spend(int cost)
    {
        if (!CanAfford(cost)) return false;
        Amount -= cost;
        return true;
    }
    
    public void Earn(int earnings)
    {
        if (earnings > 0) Amount += earnings;
    }
    
    public override string ToString() => $"{Amount} gold";
}