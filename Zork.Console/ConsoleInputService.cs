using System;
using Zork.Common;

namespace Zork
{
    internal class ConsoleInputService : IInputService
    {
        public event EventHandler<string> InputRecieved;

        public void ProcessInput()
        {
            string inputString = Console.ReadLine().Trim().ToUpper();
            InputRecieved?.Invoke(this, inputString);
        }
    }
}
