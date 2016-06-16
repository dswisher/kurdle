using System;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Take the most recently generated site and sync it with the hosting service.")]
    public class SyncCommand : ISubCommand
    {
        public void Execute()
        {
            Console.WriteLine("Sync is not yet implemented.");
        }
    }
}
