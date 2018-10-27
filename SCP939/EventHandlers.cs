using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SCP939PLUGIN
{
	class EventHandlers : IEventHandlerRoundStart, IEventHandlerRoundEnd, IEventHandlerWaitingForPlayers,
		IEventHandlerPlayerHurt, IEventHandlerPlayerDie, IEventHandlerMedkitUse, IEventHandlerUpdate
	{
		private Plugin plugin;
		private Server server;

		int damageAmount = 2, 
			damageInterval = 1;
		List<int> rolesCanBleed = new List<int>();

		public EventHandlers(Plugin plugin)
		{
			this.plugin = plugin;
			this.server = plugin.pluginManager.Server;
		}

		#region PlayerSpecific

		public void OnPlayerHurt(PlayerHurtEvent ev)
		{
			int damageAmount = plugin.GetConfigInt(SCP939.swingDamage939);
			int ChanceToBleed = plugin.GetConfigInt(SCP939.infectChance);

            //Sets damage to config amount if above 0
            if ((ev.Attacker.TeamRole.Role == Role.SCP_939_89 || ev.Attacker.TeamRole.Role == Role.SCP_939_53) && damageAmount > 0)
                ev.Damage = damageAmount;

            //When SCP939 damages a player, adds them to list of bleeding players to damage
            if ((SCP939.isEnabled && ev.Attacker.TeamRole.Role == Role.SCP_939_89 || SCP939.isEnabled && ev.Attacker.TeamRole.Role == Role.SCP_939_53)
                && !SCP939.playersToDamage.Contains(ev.Player.SteamId)
				&& ChanceToBleed > 0
				&& new Random().Next(1, 100) <= ChanceToBleed)
			{
				if(rolesCanBleed == null || rolesCanBleed.Count == 0 || rolesCanBleed.First() == -1)
                    SCP939.playersToDamage.Add(ev.Player.SteamId);
				else if (rolesCanBleed.Count > 0 && rolesCanBleed.Contains((int)ev.Player.TeamRole.Role))
                    SCP939.playersToDamage.Add(ev.Player.SteamId);
			}

			
			
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			//If player dies, removes them from infected list
			if (SCP939.playersToDamage.Contains(ev.Player.SteamId))
                SCP939.playersToDamage.Remove(ev.Player.SteamId);
		}
         
        public void OnMedkitUse(PlayerMedkitUseEvent ev)
		{
			int cureChance = plugin.GetConfigInt(SCP939.cureChanceConfigKey);
			//If its enabled in config and bleeding list contains player and cure chance is more than, cure.
			if (plugin.GetConfigBool(SCP939.cureEnabledConfigKey)
				&& SCP939.playersToDamage.Contains(ev.Player.SteamId) 
				&& cureChance > 0 && plugin.GetConfigInt(SCP939.cureChanceConfigKey) >= new Random().Next(1,100))
                SCP939.playersToDamage.Remove(ev.Player.SteamId);
		}

		#endregion

		#region RoundHandlers

		public void OnRoundEnd(RoundEndEvent ev)
		{
            //Empties bleeding list
            SCP939.playersToDamage.Clear();
            //Duh.
            SCP939.roundCount++;
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
            //Empties bleeding list
            SCP939.playersToDamage.Clear();

			
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			//Checks enabled config on initial start
			if (SCP939.roundCount == 0)
                SCP939.isEnabled = plugin.GetConfigBool(SCP939.enableConfigKey);

			//Reload theese configs on each round restart
			this.damageAmount = plugin.GetConfigInt(SCP939.damageAmountConfigKey);
			this.damageInterval = plugin.GetConfigInt(SCP939.damageIntervalConfigKey);
			this.rolesCanBleed = plugin.GetConfigIntList(SCP939.rolesCanBeBleeding).ToList();
		}

		#endregion


		DateTime updateTimer = DateTime.Now;

		public void OnUpdate(UpdateEvent ev)
		{
			if (SCP939.isEnabled && updateTimer < DateTime.Now)
			{
				//Sets when the next time this code will run
				updateTimer = DateTime.Now.AddSeconds(damageInterval);

				//If the server isnt empty, run code on all players
				if (server.GetPlayers().Count > 0)
					server.GetPlayers().ForEach(p =>
					{
						//If the victim is human and the player is in the bleeding list
						if ((p.TeamRole.Team != Team.SCP && p.TeamRole.Team != Team.SPECTATOR) && SCP939.playersToDamage.Contains(p.SteamId))
						{
							//If the damage doesnt kill, deal the damage
							if (damageAmount < p.GetHealth())
								p.Damage(damageAmount, DamageType.SCP_939);
                            else if (damageAmount >= p.GetHealth())
                            {
                                //If the damage kills the human, transform
                                SCP939.playersToDamage.Remove(p.SteamId);
                                Vector pos = p.GetPosition();
                                p.ChangeRole(Role.SCP_939_53, spawnTeleport: false);
                                p.Teleport(pos);

                            }

                        }
					});
			}
		}

	}
}