// See https://aka.ms/new-console-template for more information

using The_World.GameData;
using The_World.GameData.Commands;
using The_World.GameData.GameMechanics;

Console.WriteLine("Enter player name:");
var player = Player.CreateNewPlayer(Console.ReadLine()!, "");

var currentArea = WorldBuilder.BuildWorld();

Console.WriteLine($"Welcome, {player.Name}, to The World!");
Console.WriteLine($"You find yourself in the {currentArea.Name}.");
Console.WriteLine(currentArea.Description);

// Create game context and command parser
var gameContext = new GameContext(player, currentArea);
var commandParser = new CommandParser();

while (gameContext.KeepPlaying)
{
    Console.Write(">> ");
    var input = Console.ReadLine()?.Trim().ToLower();
    if (string.IsNullOrWhiteSpace(input))
        continue;
        
    var command = commandParser.Parse(input);
    command.Execute(gameContext);
}

// TODO: Consider implementing an Event System for game events (e.g., player leveling up, creature encounters, etc.).
// TODO: Consider implementing a Save/Load system to persist game state.
// TODO: Consider implementing Unit Tests for game mechanics and logic.
// TODO: Consider implementing Logging for debugging and tracking game events.
// TODO: Consider implementing Different Kinds of Creatures and Items with unique behaviors and properties.
//       NPCs, Enemies, Quest Items, etc.
// TODO: Consider implementing Combat Mechanics for player and creature interactions.
//       Research - Turn-Based Combat, and State Machines for managing combat states.