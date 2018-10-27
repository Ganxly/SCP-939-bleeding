using Smod2;
using Smod2.Attributes;
using System.Collections.Generic;

namespace SCP939PLUGIN
{
	[PluginDetails(
		author = "Ganxly",
		name = "SCP939",
		description = "A variant of SCP008 which transform people who bleed in SCP939",
		id = "rnen.scp.939",
		version = pluginVersion,
		SmodMajor = 3,
		SmodMinor = 1,
		SmodRevision = 19
		)]
	public class SCP939 : Plugin
	{
		public const string pluginVersion = "1.2";

		public static List<string> playersToDamage = new List<string>();
		public static bool isEnabled = true;
		public static int roundCount = 0;

		#region ConfigKeys
		public const string
			enableConfigKey = "scp939_enabled",
			damageAmountConfigKey = "scp939_damage_amount",
			damageIntervalConfigKey = "scp939_damage_interval",
			swingDamage939 = "scp939_swing_damage",
            KillBleedConfigKey = "scp939_kill_bleed",
			infectChance = "scp939_infect_chance",
			cureEnabledConfigKey = "scp939_cure_enabled",
			cureChanceConfigKey = "scp939_cure_chance",
			ranksAllowedConfigKey = "scp939_ranklist_commands",
			rolesCanBeBleeding = "scp939_roles_canbleed";
		#endregion

		public override void OnDisable() => this.Info(this.Details.name + " has been disabled.");

		public override void OnEnable() => this.Info(this.Details.name + " loaded successfully !");

		public override void Register()
		{
			#region EventRegister
			this.AddEventHandlers(new EventHandlers(this),Smod2.Events.Priority.Low);
			#endregion

			#region CommandRegister
			this.AddCommands(new string[] { "scp939", "939" }, new Command.EnableDisableCommand(this));
			this.AddCommands(new string[] { "bleed" }, new Command.InfectCommand(this));
			#endregion

			#region ConfigRegister
			this.AddConfig(new Smod2.Config.ConfigSetting(enableConfigKey, true, Smod2.Config.SettingType.BOOL, true, "Enable/Disable plugin"));
			this.AddConfig(new Smod2.Config.ConfigSetting(ranksAllowedConfigKey, new string[] { }, Smod2.Config.SettingType.LIST, true, "What ranks are allowed to run the commands of the plugin"));
			this.AddConfig(new Smod2.Config.ConfigSetting(rolesCanBeBleeding, new int[] { -1 }, Smod2.Config.SettingType.NUMERIC_LIST, true, "What roles can bleed"));

			this.AddConfig(new Smod2.Config.ConfigSetting(damageAmountConfigKey, 3, Smod2.Config.SettingType.NUMERIC, true, "Amount of damage per interval."));
			this.AddConfig(new Smod2.Config.ConfigSetting(damageIntervalConfigKey, 2, Smod2.Config.SettingType.NUMERIC, true, "The interval at which to apply damage."));
			this.AddConfig(new Smod2.Config.ConfigSetting(swingDamage939, 45, Smod2.Config.SettingType.NUMERIC, true, "The damage applied on swing."));

			this.AddConfig(new Smod2.Config.ConfigSetting(KillBleedConfigKey, true, Smod2.Config.SettingType.BOOL, true, "If kills by SCP-939 should transform the players"));
			this.AddConfig(new Smod2.Config.ConfigSetting(infectChance, 100, Smod2.Config.SettingType.NUMERIC, true, "Chance to bleed"));
			this.AddConfig(new Smod2.Config.ConfigSetting(cureEnabledConfigKey, true, Smod2.Config.SettingType.BOOL, true, "If medkit can stop the bleeding"));
			this.AddConfig(new Smod2.Config.ConfigSetting(cureChanceConfigKey, 99.99, Smod2.Config.SettingType.NUMERIC, true, "Cure chance of medkit"));

			this.AddConfig(new Smod2.Config.ConfigSetting("scp939_spawn_room", string.Empty, Smod2.Config.SettingType.STRING, true, "The room ID that scp939 will spawn."));
			#endregion
		}
	}
}