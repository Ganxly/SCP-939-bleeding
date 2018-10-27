# SCP-939-bleeding

SCP-939 is a variant of [SCP008](https://github.com/Rnen/SCP008) which transform people who bleed in SCP939.

* SCP-939 can bleed players when he attacks
* When the player dies of the bleed effect he become themself SCP-939



# Config Options :


Config Key | Default Value | Description
-------|--------|-------
 scp939_enabled |True | Enable/Disable plugin
scp939_damage_amount   | 3   | Amount of damage per interval
scp939_damage_interval   | 2   | The interval at which to apply damage
scp939_swing_damage  | 45   | The damage applied on swing
scp939_kill_bleed | True | If kills by SCP-939 should transform the players
scp939_infect_chance   | 100   | Chance to bleeds
scp939_cure_enabled   | True   | If medkit can stop the bleeding
scp939_cure_chance   | 99.99   | Cure chance of medkit
scp939_ranklist_commands   |  -------------------- | What ranks are allowed to run the commands of the plugin
scp939_roles_canbleed   | -1   | Which roles can bleed (-1 is all roles) 


# Commands : 

Command | Arguments | Description
-------|--------|-------
 scp939 / 939 | No arguments | Enable / Disable plugin
  bleed | Player / ID / Steam | Bleeds / cures player
