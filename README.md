### Finally playtesting stuff in multiplayer so a TON of fixes will come in the next update along with new content. Keep an eye out.

![Preview](https://i.imgur.com/HRaqFR0.png)

## Purchasable Equipment
<details>
<summary>Handlamp (20C)</summary>

Costs 25 credits, Weighs 5

Compared to vanilla flashlights...

PROS

- The lamp lights up the area around the holder in a radius that is larger than a baby flashlight's light reaches but not as large as a pro-flashlight reaches in one direction.

- The lamp has a noticeably larger battery capacity than the pro-flashlight (It's battery can last almost the entire day when left on constantly).

CONS 

- The light produced by the lamp isn't as clear at long ranges as the pro-flashlight.

- The lamp's light is pretty bright if used in fog, gas, smoke, dust storms, etc..

Mod Issue - Mods messing with global lighting can drastically affect the Hand Lamp making it basically unusable. Haven't found a universal way to fix this issue unfortunately.

</details>

<details>
<summary>Bandages(20C)</summary>

Costs 20 credits, Weighs 1

Bandages are a 3 use consumable item that heals 20 health per use.

Compared to the Medical Kit, Bandages heal 60 health instantly, weighs less, and are cheap and disposable.

</details>

<details>
<summary>Medical Kit(90C)</summary>

Costs 90 credits, Weighs 3

The Medical Kit heals the user overtime when used. It has a limited amount of health it can heal at once (240) but will replenish its availbe health overtime.

Compared to Bandages, the Medical Kit can heal 2x the health (120) Bandages can heal (60) and can heal infinitely when given time to replenish.

</details>

<details>
<summary>Emergency Injector(45C)</summary>

Costs 45 credits, Weighs 1

The Emergency Injector instantly gives you a speed boost, jump boost, and a small amount of health.

- Taking more than one emergency injector in a short time will cause the overdose effect which will deal damage to you for a short time.
- After the Injector turns red it is empty.

</details>


<details>
<summary>Displacement Controller(240C)</summary>

Costs 240 credits, Weighs 4

The Displacement Controller is a portable teleporter that saves your coordinates and can then teleport you back there as long as it has battery.

- The Displacement Controller's saved coordinates are set when used the first time, the second use will teleport you back at the cost of some of its battery.
- The Displacement Controller consumes battery slowly when your coordinates have been set but haven't been returned to yet.
- Your saved coordinates are wiped when the controller is recharged.

Displacement controller connectivity mechanic:
 - The Displacement Controller's connectivity is based on how far you are from the saved location, the farther you are the lower the connectivity. With full connectivity the controller works as expected but when connectivity starts to lower the controller will begin to experience glitches. Glitches will wipe your saved location and prevent you from using the controller for a short time.

</details>


<details>
<summary>Toolbox(115C)</summary>

Costs 115 credits, Weighs 5

The Tool Box can be used to dismantle landmines and turrets and will produce scrap when done successfully.

- To dismantle a trap, look at it, press and hold the Left Click button, and listen for the sound effect playing, the sound means it's working. It is a bit wonky when crouching, when around weird geometry, or when the vanilla traps are changed (like bigger landmines).
- Landmines produce 1 piece of scrap and take 6 seconds to dismantle.
- Turrets produce 2-3 pieces of scrap and take 12 seconds to dismantle.

</details>

<details>
<summary>Defibrillator(300c)</summary>

Costs 300 credits, Weighs 3

The defibrillator uses power to revive terminated employees.

- Currently only revives one player before needing to be recharged (May increase to two later depending on it's performance in testing)
- Whether a defibrillator has power or not can be easily seen by the glow of the battery compartment on its model.

</details>

## Obtainable Scrap Items

<details>
<summary>Explosive Tank</summary>

High value, Weighs 32, Spawns anywhere rarely

Once the explosive tank is picked up, an internal timer will begin counting down to 0 which will then cause the tank to explode. The only way to deactivate the timer is by bringing the tank back to your ship safely.

- The internal timer can be any time around 3 minutes, better get moving!
- Hitting the tank with a melee weapon will cause it to explode. immediately.
- Each time the tank is dropped, its remaining time will be reduced by a percentage; On the third drop, the Tank will explode immediately.
- After being brought to the ship the Tank will be in a inactive state, stopping the timer. In the inactive state, Hitting it will still cause it to explode and it can be reactivated by dropping it a few times (Dropping it only causes a explosion while it is active).

Tips for survival - Take it first or take it last and know the way back to the ship, the last thing you want to do while carrying the tank is waste time being lost or wandering.

</details>

<details>
<summary>Radioactive Cell</summary>

High value, Weighs 18, Spawns anywhere rarely

The Radioactive Cell produces a sickly green light in a radius around it infinitely. When the cell is held, the holder will regularly take damage until they drop it or perish.

- The Cell inflicts ramping damage on a rough curve going from 5 to 20.

Tips for survival - The damage ramps based on how long you hold it continuously so just drop it to reset the damage ramp to take minimum damage, sometimes it's just safer to leave it behind.

</details>

<details>
<summary>Crowbar</summary>

Average value, Weighs 6.5, Spawns anywhere uncommonly

The Crowbar is a melee weapon that does normal damage but can open locked and unlocked doors by hitting them.

</details>

<details>
<summary>Padlock</summary>

Low Value, Weighs 1, Spawns anywhere uncommonly

The Padlock locks doors open or closed, nothing more.

</details>

<details>
<summary>Walking Cane</summary>

High value, Weighs 1, Spawns on S and above difficulty moons rarely

Increases your movement speed when held.

</details>

<details>
<summary>Sizable Scissors</summary>

High Value, Weighs 16, Spawns on S and above difficulty moons rarely

Sizable Scissors are a two handed scrap item that randomly damages it's holder only if they are sprinting.

- Every second or two while running a 4 sided dice is rolled that will deal 30 damage if a 1 is rolled.
</details>

<details>

<summary>Candy Dispenser</summary>

Average value, Weighs 12, Spawns on S and above difficulty moons rarely

The Candy Dispenser is a melee weapon that does 2x damage and will rarely drop a piece of candy when swinging it (1/25 chance each swing).

</details>

<details>

<summary>Pocket Watch</summary>

Average value, Weighs 3, Spawns on S and above difficulty moons rarely

The Broken Pocket Watch poorly tells the time.

</details>


<details>

<summary>Rose</summary>

Average value, Weighs 1, Spawns on S and above difficulty moons rarely

The rose damages you a little when picked up or equipped.

</details>

<details>

<summary>Tickets of Exchange</summary>

Low value/High Value, Weighs 1, Spawns on S and above difficulty moons

The Regular Ticket of Exchange is a common low value item that can be used ONCE to transform any item into a gift box, essentially exchanging it for something else.

The Golden Ticket of Exchange is extremely rare and high value item that does the same thing it predecessor does but can do it FIVE times instead of once.

</details>

## Dependencies
1. LethalLib 
2. BepInExPack
3. Latest Version of Lethal Company for latest mod versions
---

Massive thanks to Malcolm on YouTube for their LC modding guides and examples.

Massive thanks to Evaisa and anyone else who has contributed to LethalLib

Massive thanks to everyone working on BepInEx

And the biggest thanks to Zeekers for creating Lethal Company

---

### Report issues on my GitHub! - https://github.com/EtherealEmil/UsualScrap.git
 I will list issues/Limitations below for easy viewing on mod-managers.

<details>
<summary>Issues</summary>

- Last I checked, in the vanilla game the player health UI doesn't change back to white when healing because there aren't any ways to heal back to full health outside of respawning, The only thing I will do is recommend you use a mod that displays your health as a number so you can easily tell you're health is actually increasing.

- Mods that mess with the world lighting can make some of the items insanely bright. I'll try to account for it when choosing the item effects but I can't fix it.

</details>

