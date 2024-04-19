## Purchasable Equipment Details
<details>
<summary>HandLamp</summary>

![alt text](HandlampPreview.png)
Costs 25 credits, Weighs 5

PROS - 
- Makes light in a radius around the holder that reaches about as far as a BB flashlight can reach in one direction.
- Its battery capacity is comparable to a pro flashlight.
- One-handed.

CONS - 
- The light doesn't reach as far as the pro flashlight.
- Its light can be blindingly bright if you are caught in fog or smoke.
</details>

<details>
<summary>Bandages</summary>

![alt text](BandagesPreview.png)
Costs 25 credits, Weighs 1

Bandages are a consumable with 5 charges that heal 8 health each.

Unlike the medkit, Bandages heal you instantly and are a cheaper short-term option.
</details>

<details>
<summary>Medkit</summary>

![alt text](MedkitPreview.png)
Costs 150 credits, Weighs 5

The medkit gradually heals the holder using a hidden health pool that depletes while healing.

Unlike the bandages, the medkit's base health pool can heal 6 times the health one bandage item can heal if given enough time. Additionally, its health pool can be refilled by bringing it into the ship. In the future, I plan to add a feature that will allow players to use the medkit on their teammates without having to drop it for them.
</details>

## Unique Scrap Items

<details>
<summary>Explosive Tank (dangerous scrap item)</summary>

![alt text](ExplosiveTankPreview.png)

Once the explosive tank is picked up, an internal timer begins counting down until it reaches 0 and the tank will then explode. The only way to stop the timer is by bringing the tank inside the ship.

- Hitting the tank with a melee weapon will cause it to explode immediately.
- Each time the tank is dropped, its remaining time will be reduced by a set amount. If the tank is dropped three times, it will explode immediately.
- The internal timer can start at any time between 2 and 4 minutes.
- Spawns on any moon rarely.

</details>

<details>
<summary>Walking Cane (Useful scrap item)</summary>

![alt text](WalkingCanePreview.png)

Increases your move speed when held.

- Currently spawns on dine very rarely.

</details>


## Basic Scrap Items


## Dependencies
1. LethalLib 
2. BepInExPack

---

Massive thanks to Malcolm on YouTube for their LC modding guides and examples.

Massive thanks to Evaisa and anyone else who has contributed to LethalLib

Massive thanks to everyone working on BepInEx

---

## Issues
These issues will be fixed in the future when I have time but I've only just started modding, therefore I'm still figuring things out. I will list interactions and remaining mod issues below so you can decide what mods to include or avoid if you decide to use this mod. I should use github but uh...............
<details>
<summary>Interactions</summary>

- Mods that affect flashlights may affect the handlamp.

</details>
<details>
<summary>Issues</summary>

- In vanilla, it can be difficult to tell if healing items are actually working so until I sit down and add an animation or effect I'd recommend downloading a mod that displays your health as a number or bar that updates dynamically.

- Handlamp has the normal flashlight light when pocketed. Will change this eventually.

- Item values don't seem to be generating in the bounds I have set so I have to figure that out now.

</details>
<details>
<summary>Mod Compat Issues</summary>

- "FlashlightExtendedRange" Makes the handlamp insanely bright when it's turned on.

- "Diversity" will make the handlamp brighter, I don't think it's too bad but it isn't the brightness I intended.

</details>

