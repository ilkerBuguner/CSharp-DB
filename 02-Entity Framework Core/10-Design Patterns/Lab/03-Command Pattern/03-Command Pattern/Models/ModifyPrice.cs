using _03_Command_Pattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace _03_Command_Pattern.Models
{
    public class ModifyPrice
    {
        private readonly List<ICommand> commands;
        private ICommand command;

        public ModifyPrice()
        {
            commands = new List<ICommand>();
        }

        public void SetCommand(ICommand command) => this.command = command;

        public void Invoke()
        {
            this.commands.Add(this.command);
            this.command.ExecuteAction();
        }


    }
}
