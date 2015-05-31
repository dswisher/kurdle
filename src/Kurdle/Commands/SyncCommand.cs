using System;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Take the most recently generated site and sync it with the cloud.")]
    public class SyncCommand : ISubCommand
    {
        public void Execute()
        {
            // TODO
            Console.WriteLine("Sync is not yet implemented.");
        }
    }
}
