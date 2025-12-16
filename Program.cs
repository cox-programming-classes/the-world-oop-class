// See https://aka.ms/new-console-template for more information

using The_World.GameData;
using The_World.GameData.Commands;
using The_World.GameData.GameMechanics;

Console.WriteLine("Enter player name:");
var player = Console.ReadLine()!;

var gameContext = WorldBuilder.BuildWorld(player);

Console.WriteLine($"Welcome, {gameContext.Player.Name}, to The World!");
Console.WriteLine($"You find yourself in the {gameContext.CurrentArea.Name}.");
Console.WriteLine(gameContext.CurrentArea.Description);

// Create game context and command parser
// var commandParser = new CommandParser();

Context currentContext = gameContext;
while (currentContext.KeepPlaying)
{
    Console.Write(">> ");
    var input = Console.ReadLine()?.Trim().ToLower();
    if (string.IsNullOrWhiteSpace(input))
        continue;
        
    var command = currentContext.Parser.Parse(input);
    currentContext = command.Execute(currentContext);
}

// TODO: Consider implementing an Event System for game events (e.g., player leveling up, creature encounters, etc.).
// TODO: Consider implementing a Save/Load system to persist game state.
// TODO: Consider implementing Unit Tests for game mechanics and logic.
// TODO: Consider implementing Logging for debugging and tracking game events.
// TODO: Consider implementing Different Kinds of Creatures and Items with unique behaviors and properties.
//       NPCs, Enemies, Quest Items, etc.
// TODO: Consider implementing Combat Mechanics for player and creature interactions.
//       Research - Turn-Based Combat, and State Machines for managing combat states.