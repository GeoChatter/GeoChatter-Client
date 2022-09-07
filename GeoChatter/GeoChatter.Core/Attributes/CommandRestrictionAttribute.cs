using GeoChatter.Model.Enums;
using System;

namespace GeoChatter.Core.Attributes
{
    /// <summary>
    /// Target to apply cooldown for
    /// </summary>
    public enum CooldownTarget
    {
        /// <summary>
        /// Apply individual cooldowns
        /// </summary>
        Individual,
        /// <summary>
        /// Apply global cooldown
        /// </summary>
        Global
    }

    /// <summary>
    /// Twitch command restriction attribute for limiting access
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CommandRestrictionAttribute : Attribute
    {
        /// <summary>
        /// Allow access to given viewers with given levels, use CommonUserLevel
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// Command firing cooldown in seconds. If given seconds of time hasn't passed since last command fire, event is ignored.
        /// <para>For moderator and above, this is ignored</para>
        /// </summary>
        public double CommandCooldownSeconds { get; set; }

        /// <summary>
        /// Message cooldown for sending messages within the command handlers (to be checked within handlers)
        /// <para>Make sure this value is smaller or equal to <see cref="CommandCooldownSeconds"/></para>
        /// </summary>
        public double MessageCooldownSeconds { get; set; }

        /// <summary>
        /// Application states to restrict the command use to
        /// </summary>
        public AppGameState AllowedState { get; set; } = AppGameState.ANYTIME;

        /// <summary>
        /// Target to apply <see cref="CommandCooldownSeconds"/> for after each command call
        /// </summary>
        public CooldownTarget CooldownTarget { get; set; } = CooldownTarget.Global;

        /// <summary>
        /// Wheter developers (defined by IDs in <see cref="Interfaces.ICommandBase.DeveloperIDs"/>) can bypass all access checks
        /// </summary>
        public bool CanDeveloperBypass { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandCooldown"></param>
        /// <param name="messageCooldown"></param>
        /// <param name="applyTo"></param>
        public CommandRestrictionAttribute(int commandCooldown = 0, int messageCooldown = 0, CooldownTarget applyTo = CooldownTarget.Global)
        {
            CommandCooldownSeconds = commandCooldown;
            MessageCooldownSeconds = messageCooldown;
            CooldownTarget = applyTo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLevel"></param>
        /// <param name="commandCooldown"></param>
        /// <param name="messageCooldown"></param>
        /// <param name="applyTo"></param>
        public CommandRestrictionAttribute(int userLevel, int commandCooldown = 0, int messageCooldown = 0, CooldownTarget applyTo = CooldownTarget.Global) : this(commandCooldown, messageCooldown, applyTo)
        {
            AccessLevel = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLevel"></param>
        /// <param name="commandCooldown"></param>
        /// <param name="messageCooldown"></param>
        /// <param name="applyTo"></param>
        public CommandRestrictionAttribute(object userLevel, int commandCooldown = 0, int messageCooldown = 0, CooldownTarget applyTo = CooldownTarget.Global) : this(commandCooldown, messageCooldown, applyTo)
        {
            try
            {
                AccessLevel = (int)userLevel;
            }
            catch
            {
                AccessLevel = 0;
            }
        }
    }
}
