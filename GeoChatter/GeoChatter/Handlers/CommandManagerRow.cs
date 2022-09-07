namespace GeoChatter.Handlers
{
    /// <summary>
    /// Command manager data table row model
    /// </summary>
    public class CommandManagerRow
    {
        /// <summary>
        /// Bot name
        /// </summary>
        public string Bot { get; }
        /// <summary>
        /// Command name
        /// </summary>
        public string CommandName { get; }
        /// <summary>
        /// Aliases
        /// </summary>
        public string Aliases { get; set; }
        /// <summary>
        /// Trigger character
        /// </summary>
        public char Trigger { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Command call cooldown
        /// </summary>
        public double Cooldown { get; set; }
        /// <summary>
        /// Command message cooldown
        /// </summary>
        public double MessageCooldown { get; set; }
        /// <summary>
        /// Cooldown target
        /// </summary>
        public string CooldownTarget { get; set; } = nameof(Core.Attributes.CooldownTarget.Individual);
        /// <summary>
        /// Available game stages from <see cref="Model.Enums.AppGameState"/>
        /// </summary>
        public int AvailableStages { get; set; }
        /// <summary>
        /// User level required from <see cref="Web.CommonUserLevel"/>
        /// </summary>
        public string UserLevel { get; set; }
        /// <summary>
        /// Wheter command is enabled
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// Wheter developers can bypass checks
        /// </summary>
        public bool DeveloperBypass { get; set; }
    }
}
