// See https://aka.ms/new-console-template for more information

using The_World.GameData;
using The_World.GameData.GameMechanics;

Console.WriteLine("Enter player name:");
var player = Player.CreateNewPlayer(Console.ReadLine()!, "");

var currentArea = WorldBuilder.BuildWorld();

Console.WriteLine($"Welcome, {player.Name}, to The World!");
Console.WriteLine($"You find yourself in the {currentArea.Name}.");
Console.WriteLine(currentArea.Description);

bool keepPlaying = true;
while (keepPlaying)
{
    Console.Write(">> ");
    var input = Console.ReadLine()?.Trim().ToLower();
    if (string.IsNullOrWhiteSpace(input))
        continue;
    var commandParts = input.Split(' ', 2);
    var command = commandParts[0];
    var argument = commandParts.Length > 1 ? commandParts[1] : "";
    /* Process commands
     * TODO: Expand command processing
     * For now, we only handle "look" and "quit"
     * "look" can be used as "look" or "look [target]"
     * e.g., "look sword" to look at a specific item
     * or "look" to look around the area
     * You can add more commands like "get", "attack", etc.
     * we also need:
     * "go" to move between areas
     * "help" to list commands
     * "inventory" to show player inventory (once that is implemented!)
     *
     * This is all very basic command parsing.
     * TODO: Implement a more robust command parser as a separate class.
     * Code should eventually be simply "ProcessCommand(input);" or similar.
     */
    switch (command)
    {
        case "look":
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine(currentArea.LookAround());
            }
            else
            {
                Console.WriteLine(currentArea.LookAt(argument));
            }
            break;
       
        case "help":
            Console.WriteLine("Available commands:");
            Console.WriteLine("  look - Look around the area");
            Console.WriteLine("  look [item] - Look at specific item");
            Console.WriteLine("  go [direction] - Move to connected area");
            Console.WriteLine("  get [item] - Pick up an item");
            Console.WriteLine("  inventory - Show your items");
            Console.WriteLine("  help - Show this help");
            Console.WriteLine("  quit - Exit game");
            break;
       
        case "go":
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Go where? Try 'go [direction]'");
            }
            else if (currentArea.ConnectedAreas.TryGetValue(argument, out var newArea))
            {
                currentArea = newArea;
                Console.WriteLine($"You move to the {currentArea.Name}.");
                Console.WriteLine(currentArea.Description);
            }
            else
            {
                Console.WriteLine($"You can't go '{argument}' from here.");
            }
            break;
        
        case "inventory":
            if (player.Inventory.Count == 0)
            {
                Console.WriteLine("Your inventory is empty.");
            }
            else
            {
                Console.WriteLine("You are carrying:");
                foreach (var item in player.Inventory)
                {
                    Console.WriteLine($"  {item.Name} ({item.Weight} lbs)");
                }
            }
            break;
        
        case "get":
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Get what? Try 'get [item]'");
            }
            else if (currentArea.Items.TryGetValue(argument, out var item))
            {
                // Remove item from area and add to player inventory
                currentArea.Items.Remove(argument);
                player.Inventory.Add(item);
                Console.WriteLine($"You pick up the {item.Name}.");
            }
            else
            {
                Console.WriteLine($"There is no '{argument}' here to pick up.");
            }
            break;

        case "quit":
            keepPlaying = false;
            Console.WriteLine("Thanks for playing!");
            break;
        default:
            Console.WriteLine($"Unknown command: {command}");
            break;
        
        case "attack": //add attack command
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Attack what? Try 'attack [creature]'");
            }
            else if (currentArea.Creatures.TryGetValue(argument, out var creature))
            {
                // TODO: Implement actual combat mechanics
                Console.WriteLine($"You attack the {creature.Name}!");
                Console.WriteLine("Combat system not yet implemented.");
            }
            else
            {
                Console.WriteLine($"There is no '{argument}' here to attack.");
            }
            break;
        
        case "use":
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Use what? Try 'use [item]'");
            }
            else
            {
                Console.WriteLine($"You attempt to use '{argument}'.");
                Console.WriteLine("Item usage system not yet implemented.");
            }
            break;


    }
}

// TODO: Implement game loop, movement between areas, interaction with items and creatures, etc.
// This is just a starting point!
// TODO: Consider implementing a Command Pattern for better command handling.
// TODO: Consider implementing an Event System for game events (e.g., player leveling up, creature encounters, etc.).
// TODO: Consider implementing a Save/Load system to persist game state.
// TODO: Consider implementing Unit Tests for game mechanics and logic.
// TODO: Consider implementing Logging for debugging and tracking game events.
// TODO: Consider implementing Different Kinds of Creatures and Items with unique behaviors and properties.
//       NPCs, Enemies, Quest Items, etc.
// TODO: Consider implementing Combat Mechanics for player and creature interactions.
//       Research - Turn-Based Combat, and State Machines for managing combat states.