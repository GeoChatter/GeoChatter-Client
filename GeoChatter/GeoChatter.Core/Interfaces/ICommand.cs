using GeoChatter.Core.Attributes;
using GeoChatter.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GeoChatter.Core.Interfaces
{
    /// <summary>
    /// Command interface
    /// </summary>
    /// <typeparam name="TBotType">Bot type command will be used with</typeparam>
    public interface ICommand<TBotType> : ICommandBase
    {
        /// <summary>
        /// Executable command delegate
        /// </summary>
        /// <param name="bot">Parent bot</param>
        /// <param name="command">Command instance</param>
        /// <param name="eventArgs">Event arguments</param>
        /// <param name="args">User arguments</param>
        public delegate void CommandDef(TBotType bot, ICommand<TBotType> command, object eventArgs, string[] args);

        /// <summary>
        /// Command to execute
        /// </summary>
        public CommandDef Command { get; set; }

        /// <summary>
        /// Call command with given arguments
        /// </summary>
        /// <param name="bot">Parent bot</param>
        /// <param name="eventArgs">Event arguments object</param>
        public void CallCommand(TBotType bot, object eventArgs);

        /// <summary>
        /// Create a new command from given values
        /// </summary>
        /// <typeparam name="TCommand">Command type (TwitchCommand, YoutubeCommand, etc.)</typeparam>
        /// <param name="triggers">Trigger characters to be used for message start characters</param>
        /// <param name="command">Command delegate</param>
        /// <param name="meta">Meta data attribute</param>
        /// <param name="rest">Restriction data attribute</param>
        /// <returns></returns>
        public static TCommand NewCommand<TCommand>(string[] triggers, CommandDef command, CommandAttribute meta, CommandRestrictionAttribute rest)
            where TCommand : ICommand<TBotType>, new()
        {
            return new TCommand() { Command = command, CommandNames = new(triggers), Meta = meta, Restrictions = rest };
        }

        /// <summary>
        /// Create commands from the <typeparamref name="TCommand"/> methods and return them in a list
        /// </summary>
        /// <typeparam name="TCommand">Command type (TwitchCommand, YoutubeCommand, etc.)</typeparam>
        /// <param name="t">Static class where commands are defined</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when a method has only 1 of <see cref="CommandAttribute"/> or <see cref="CommandRestrictionAttribute"/> defined</exception>
        public static List<TCommand> DiscoverMethods<TCommand>(Type t)
            where TCommand : ICommand<TBotType>, new()
        {
            List<TCommand> list = new();
            List<MethodInfo> methods = AttributeDiscovery.GetMethodsWithAttribute(t, out List<CommandAttribute> attributes, true);
            List<MethodInfo> methodrests = AttributeDiscovery.GetMethodsWithAttribute(t, out List<CommandRestrictionAttribute> restattributes, true);

            if (attributes.Count != restattributes.Count)
            {
                throw new InvalidOperationException("Command and CommandRestriction attribute count must match!");
            }

            for (int i = 0; i < methods.Count; i++)
            {
                MethodInfo method = methods[i];
                list.Add(NewCommand<TCommand>(attributes[i].Names, (CommandDef)Delegate.CreateDelegate(typeof(CommandDef), method), attributes[i], restattributes[i]));
            }
            return list;
        }
    }
}
