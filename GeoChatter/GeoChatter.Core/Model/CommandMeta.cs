using GeoChatter.Model.Enums;
using System;
using System.Collections.Generic;

namespace GeoChatter.Core.Model
{
    /// <summary>
    /// Command meta data
    /// </summary>
    public sealed class CommandMeta : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Wheter command is built-in
        /// </summary>
        public bool IsBuiltIn { get; set; } = true;
        /// <summary>
        /// Time command was first created
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
        /// <summary>
        /// Time command meta was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }
        /// <summary>
        /// Time command details was last edited
        /// </summary>
        public DateTime LastEdited { get; set; }

        /// <summary>
        /// Bot type name this command belongs relates to
        /// </summary>
        public string BotName { get; set; } = string.Empty;
        /// <summary>
        /// User level display string. Use CommonUserLevel from Web project.
        /// </summary>
        public string UserLevel { get; set; } = string.Empty;
        /// <summary>
        /// Trigger character of the command
        /// </summary>
        public char TriggerChar { get; set; }
        /// <summary>
        /// Base command name
        /// </summary>
        public string CommandName { get; set; } = string.Empty;
        /// <summary>
        /// Aliases of this command
        /// </summary>
        public List<string> Aliases { get; } = new();
        /// <summary>
        /// Info about the command
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Call cooldown for command
        /// </summary>
        public double Cooldown { get; set; }
        /// <summary>
        /// Message cooldown for command
        /// </summary>
        public double MessageCooldown { get; set; }
        /// <summary>
        /// Cooldown target. Use <see cref="CooldownTarget"/>
        /// </summary>
        public string CooldownTarget { get; set; } = nameof(Attributes.CooldownTarget.Individual);
        /// <summary>
        /// App states for the command usability. Use <see cref="AppGameState"/>
        /// </summary>
        public int AllowedState { get; set; } = (int)AppGameState.ANYTIME;
        /// <summary>
        /// Wheter developer IDs can bypass checks
        /// </summary>
        public bool CanDeveloperBypass { get; set; }
        /// <summary>
        /// Wheter command is usable
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public CommandMeta()
        {
            LastUpdated = Created;
            LastEdited = Created;
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Aliases?.Clear();
                    CommandName = null;
                    Description = null;
                    CooldownTarget = null;
                    UserLevel = null;
                    BotName = null;
                    IsEnabled = false;
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
