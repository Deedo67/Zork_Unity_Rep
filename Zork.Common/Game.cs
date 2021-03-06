using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Game : INotifyPropertyChanged
    {
        public event EventHandler GameStarted;
        public event EventHandler GameStopped;

        public event PropertyChangedEventHandler PropertyChanged;

        public World World { get; private set; }

        public string StartingLocation { get; set; }

        public string WelcomeMessage { get; set; }

        public string ExitMessage { get; set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; set; }

        public IInputService Input { get; set; }

        public IOutputService Output { get; set; }

        [JsonIgnore]
        public Dictionary<string, Command> Commands { get; private set; }

        public Game(World world, Player player)
        {
            World = world;
            Player = player;

            Commands = new Dictionary<string, Command>()
            {
                { "QUIT", new Command("QUIT", new string[] { "QUIT", "Q", "BYE" }, Quit) },
                { "LOOK", new Command("LOOK", new string[] { "LOOK", "L" }, Look) },
                { "NORTH", new Command("NORTH", new string[] { "NORTH", "N" }, game => Move(game, Directions.North)) },
                { "SOUTH", new Command("SOUTH", new string[] { "SOUTH", "S" }, game => Move(game, Directions.South)) },
                { "EAST", new Command("EAST", new string[] { "EAST", "E"}, game => Move(game, Directions.East)) },
                { "WEST", new Command("WEST", new string[] { "WEST", "W" }, game => Move(game, Directions.West)) },
            };
        }

        public void Start(IInputService input, IOutputService output)
        {
            Assert.IsNotNull(input);
            Input = input;
            Input.InputRecieved += InputReceivedHandler;

            Assert.IsNotNull(output);
            Output = output;

            IsRunning = true;
            GameStarted?.Invoke(this, EventArgs.Empty);
        }

        private void InputReceivedHandler(object sender, string commandString)
        {
            Command foundCommand = null;
            foreach (Command command in Commands.Values)
            {
                if (command.Verbs.Contains(commandString))
                {
                    foundCommand = command;
                    break;
                }
            }

            if (foundCommand != null)
            {
                foundCommand.Action(this);
                Player.Moves++;
            }
            else
            {
                Output.WriteLine("Unknown command.");
            }
        }

        private static void Move(Game game, Directions direction)
        {
            if (game.Player.Move(direction) == false)
            {
                game.Output.WriteLine("The way is shut!");
            }
        }

        public static void Look(Game game) => game.Output.WriteLine(game.Player.Location.Description);

        private static void Quit(Game game)
        {
            game.IsRunning = false;
            game.GameStopped?.Invoke(game, EventArgs.Empty);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) => Player = new Player(World, StartingLocation);
    }
}