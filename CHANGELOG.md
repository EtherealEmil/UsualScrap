<details>

<summary></summary>



</details>

---

<details>

<summary>Version 1.9.3</summary>

- Removed much of the number values from the homepage so I can balance/change item's values without needing to update the page constantly.
- Buffed the healing items slightly.

</details>

---

<details>

<summary>Version 1.9.2</summary>

Quick follow up!

- Removed terminalconflictfix dependency since dawnlib has it's own native iteration.
- The Doom Bell has been disabled, it may return later.
- Slightly reduced the time to use the toolbox overall.
- Defib should now work correctly when the battery is disabled.

</details>


---

<details>

<summary>Version 1.9.1</summary>

Hello hard working employees! I'm in the modding mood again after 3 whole months! Anyway, this update is just some updates and a few additions some of y'all have asked for. New stuff is next!

- Updated methods for newest dawnlib changes.
- Fixed and changed the golden tickets function to curb its infinite use issue. It will now simply convert up to 5 nearby items all at once when used,  the normal ticket remains the same.
- Added check to the crowbar to avoid pointless errors.
- Added terminalconflictfix as a dependency. This fixes issues buying some of the items caused by vanilla's poor terminal name checks.
- Added a config to disable all this mod's item icons for use with runtime icons or similar mods.
- Added a config to disable the defib's battery usage.

Upcoming (Any future update):
- Option to disable item lights in the ship.
- Scissors melee option/new item variation.
- Improvements to items for max health altering mods
- Toolbox modded trap compatability.
- Config presets like only scrap items and only store items.
- Item use saving.
- Mod page renovation.
- Old item code improvements/rewrites.
- More scrap pack items.
- Modded moon spawn weights.
- Scrap pack item spawn weights.

</details>


---

<details>

<summary>Version 1.9.0</summary>

Version update, Fixes.

- Updated to v73.
- Updated netcode patcher version.
- Updated Unity version.
- Small fixes, nothing effecting gameplay.

Upcoming (Any future update):
- Option to disable item lights in the ship.
- Icon toggle / compat with runtime icons.
- Scissors melee option/new item variation.
- Toolbox modded trap compatability.
- Config presets like only scrap items and only store items.
- Item use saving.
- Mod page redo.
- Old item code improvements/rewrites.
- More scrap pack items.
- Modded moon spawn weights.
- Scrap pack item spawn weights.

</details>

---

<details>

<summary>Version 1.8.6</summary>

Small changes/fixes.

- Added US_ to all my item names.
- Added forgotten doomsayer bell code.
- Inverted all the item icons to white as they should've been and smoothed some rough icons.
- Fixed candy dispenser.
- Added scrap packs and the items included to the mod page.
- Removed leftover logging.

</details>

---

<details>

<summary>Version 1.8.5</summary>

Complete config and item loading rewrite making use of dawnlib, do NOT update if you plan to finish previous ongoing save games!

- Fixed double door detection issue with the padlock.
- Radioactive handlamp removed to make room for a future item.
- Infernal Capsule renamed to Noxious Capsule, function added, and enabled by default. 
- Sacrificial Capsule renamed to Bloody Capsule and changed to drain health from nearby players instead of corpses. Masked spawning disabled pending fixes meaning it will charge and then idle.
- Added Doomsayer Bell that rings when used or dropped. Ringing the bell on a moon with time has a chance to cause the time to jump forward an hour.
- Added config to disable tickets functioning on items with a value below 15 meaning you'd no longer be able to use it on cheap or worthless equipment items like flashlights.
- Added config to enable debug logging.
- Frigid capsule slow no long slows animations. It will affect player fov when I figure that out.
- Hopefully fixed radioactive cell sometimes not damaging non-host players.
- Store item scan nodes set to be blue.
- Renamed and redesigned the Emergency Injector -> Productivity Autoinjector. The Productivity Autoinjector grants a weak speed boost, jump boost, and nullifies the effects of weight for three minutes.
- Various model tweaks, particle tweaks,  and all icons redone.
- Updated mod page and added recommended mod section.

- Configs and item registration almost completely rewritten using dawn to allow item weight spawns to be configured per moon (Currently includes only Vanilla moons and is not available for scrap packs).
- Item registration and asset loading is now completely dependant on the set configs.
- Store and Scrap versions of the same item are now completely separate items differentiated by the shop version being being called "salvaged" and having a blue scan node.
- Scrap packs added to the configs (disabled by default). These packs will include scrap items that have little to no functions and just fit a specific theme, the current available packs are the Facility pack and the Mansion Pack Containing two items each.
- Added error checks and passes all round.

I'm prepared for bugs to arise as I couldn't test as much as I should have, please report issues to my github or mod page in the lethal company modding discord! Thanks.

Upcoming (Any future update):
- Still learning dawn so be prepared for more code changes.
- Option to disable item lights in the ship.
- Scissors melee option/new item variation.
- Still want to add toolbox modded trap compatability.
- Config presets like only scrap items and only store items.
- defib & injector use saving.
- Mod page redo.

</details>

---

<details>

<summary>Version 1.8.4</summary>

This update will likely break with previous saves, you've been warned. Compiled fixes and changes to prepare for the next update.

- Accidently disabled perma death for the defib last update, this is now fixed.
- Possibly fixed the defibrillator being able to revive any amount of corpses using one charge if the corpses are close together.
- Smoothed out the defibrillator's charge animation.
- Emergency injector now functions as intended.
- Fixed golden ticket sparkle from stopping randomly and lowered its default spawn rate to 5.
- Fixed various sounds not playing & new sounds.
- Fixed various broken configs.
- Renamed Sanguine capsule --> Sacrificial capsule.
- Renamed lollipop --> piece of candy, remodeled it, and gave it two randomized effects including a short but drastic speed boost or jump boost.
- Renamed unstable fuel cylinder --> Fuel Cylinder
- Slowed the frigid and gloomy capsule charge rate.
- The fuel cylinder now has a small chance to not activate when picked up.
- Rewrote walking cane code.
- Moved the handlamp's pocket light down below the player's camera to avoid blinding the user and improved the pocket light range, this applies to the new handlamp also.
- Fixed capsule lights staying on when being pocketed.
- Two new items and the sacrifical capsule are in the process of being tested in multiplayer, fixed, or changed and so they are disabled by default. This is how I will approach releasing/fixing items from now on.
- Added warning about downloading this mod on any other sites to the mod page.
- Updated all information on the mod page.

Upcoming (Any future update):
- Moon specific item spawn weights.
- Option to disable item lights in the ship.
- Scissors melee option/new item variation.
- Still want to add toolbox modded trap compatability.
- Really want to implement scrap spawns based on interior.
- Config presets like only scrap items and only store items. Considering making item packs in the configs to add items to so that I can make vastly different items like medieval, horror, etc. based items all in one mod.
- Capsule function tweaks & fixes.
- defib & injector use saving.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" WITH a space. Balance feedback is also very welcome as I haven't actually played lethal company in ages, Thanks.

</details>

---

<details>

<summary>Version 1.8.3</summary>

Another Hotfix.

- Reverted and redid defib fixes as they broke more than they fixed. Tested working with two players and no other mods.
- Tweaked some models and sounds.

This is hopefully maybe possibly the last hotfix update so I can start work on the next content update!

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" WITH a space. Balance feedback is also very welcome as I haven't actually played lethal company in ages, Thanks.

</details>

---

<details>

<summary>Version 1.8.2</summary>

More Hotfixes.

- Hopefully worked out the final kinks with the shift controller.
- Fixed sanguine capsule having the wrong scrap value.
- Fixed defib duplicating dead bodies if the positions were desynced too much.
- Fixed sanguine capsule changing position after reloading the game.
- Fixed pocketwatch script not functioning correctly.
- Hopefully fixed bandages, defib, and emergency injector not saving their uses upon reload without breaking other stuff.

This is the last hotfix update so I can start work on the next content update!

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" WITH a space. Balance feedback is also very welcome as I haven't actually played lethal company in ages, Thanks.

</details>

---

<details>

<summary>Version 1.8.1</summary>

Hotfixes.

- Fixed candy dispenser broken reference.
- Added option to refill defibrillators upon going to orbit.
- Lowercase Rpcs.
- Fixed being able to pocket scissors after swinging.
- Removed old unused methods, references, and logging.
- Enabled the pocketwatch I disabled by accident..
- Increased the charge rate of the sanguine capsule.
- Fixed misc code oversights.


If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" WITH a space. Balance feedback is also very welcome as I haven't actually played lethal company in ages, Thanks.

</details>

---

<details>

<summary>Version 1.8.0</summary>

Hey, here's an update. Sorry for the wait.

- Reduced roses vertexs from around 8k to around 4k, model redone completely.
- Removed chance for the controller to lose connection in the average connection range, this makes it significantly more usable as a consistent short range teleport option and more worth it's price.
- The emergency injector now has two uses.
- The candy dispenser no longer does double damage and the sizable scissors are now a melee weapon that does double damage instead.
- Fixed small hud bug with bandages.
- Frigid capsule now activates at night instead of the day.
- Flipped changelog and changed It's formatting a bit.
- Bandages can now be enabled to spawn as scrap.
- Added the sanguine capsule.
- Added a subtle but clear sound and effect to the toolbox's dismantle function.
- Candy is now a single item with random colors and possibly random effects in the future.
- Decent chunk of code consolidation, improvements, and fixes.
- Playtested all the items and everything seemed to be working correctly on the surface WITHOUT other mods.

I was notified of a bug with the defibrillator corpses a few months ago or so and tried to fix it at the time but have since forgot if I did it successfully. If you experience bugs with it please report it and how to reproduce it if possible and I'll get it fixed asap.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" WITH a space. Balance feedback is also very welcome as I haven't actually played lethal company in ages, Thanks.

</details>

---

<details>

<summary>Version 1.7.8</summary>

Hello fellow employees, it's been a while. While I am still not back to modding often like before, I am here as a "part time employee" for a while. No new content this update but there may be some new stuff in the near future.

- Added permadeath config to the defibrillator.
- Candy speed effect x 2 but speed effect duration / 2 and candy value halved.
- Emergency injector function changed for the final time and price lowered.
- Toolbox redone, removed the sound but it definitely works for all players and doesn't have the previous trap detection issues. Trying to add an animation for it and new sounds to finish it off.
- Went through and updated item descriptions on the mod page.
- Haven't touched the code in over a month so I forgot some early changes.

Planned - 

- Toolbox animation and sound.
- Toolbox functioning on modded traps.
- Toolbox functioning on spike traps, spike traps need altered code for whatever reason.
- Injector visual info and final couple additions.
- Capsule item changes.
- New stuff.

Sorry if you expected more of this update but I've been busy and haven't touched the mod or the game since last update.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" WITH a space. Balance feedback is also very welcome as I haven't actually played lethal company in ages, Thanks.

</details>

---

<details>

<summary>Version 1.7.7</summary>

Didn't mean to leave the capsule broken so long, things got busy.

- Fixed weather continuing to play when teleported indoors with the gloomy capsule.
- Optimized walking cane code a bit.
- Got frigid capsule in a working state so I will no longer touch it ever again, some configs are planned.
- Reduced the slow severity of the frigid capsule massively and prevented it from unintentionally scaling with the amount of players in the lobby.
- Happy mistake but the frigid capsule now slows you less if you're not holding it and only running through it.
- Some store items now have a config option to spawn as scrap items. 
- Updated modpage descriptions.

If you have an ideas for changes that you'd be happy to share for the toolbox, I'd love to hear them! Contacts below. Either way, the toolbox changes are coming next update!

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.7.6</summary>

1.7.5 Follow-up 

- Fixed there being way too many snow particles.
- Medical kit now updates the health hud of only the player being healed. Thanks Xu Xiaolan!
- Defib should now only remove the body of the person it has revived.
- Removed rose slow.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.7.5</summary>

Modding week ends here. Next update will be a while as I planned to potentially double the amount of unique items and complete the toolbox update, till next time.

- Frigid capsule permanent frosty feet particles fixed hopefully.
- Frigid capsule slow effect lightened.
- Handlamp head light moved up a bit so you aren't partially blinded anymore.
- Added config for buying the crowbar in the store.
- Fixed pocketwatch not showing up issue hopefully.
- Emergency Injector changes.
- Something else I've forgotten.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.7.4</summary>

Modding week continues!

- When using the medical kit to heal fellow employees the rate of healing is now increased! (2x Healing Speed)
- When the gloomy capsule is charging but in your pocket a new vfx will play around your player to attempt to warn players it is actively about to do something.
- Defib body removal range shrunk, will rewrite this portion of code to only do the revived body when I have time.
- Medical kit code optimized and consolidated.
- Medical kit self healing speed increased very slightly.
- Healing particles moved slightly and attached to the player so they can still be seen when moving quickly.
- Fixed medical kit using more of its charge than intended resulting in it running out very quickly.
- Shift controller battery increased slightly and teleport delay lowered.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.7.3</summary>

Quick follow-up follow-up update to 1.7.1, check out 1.7.1 for the other important changes.

- Actually removed defib double use.
- Disabled leftover logging.
- You can now heal other employees with the medical kit directly.
- Fixed melee multi-hit for good.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.7.2</summary>

Quick follow-up update to 1.7.1, check out 1.7.1 for the more important changes ^

- Added ticket cruiser fix to the golden ticket since I forgot to before.
- Removed double use on the defib. Increased the defib's battery usage due to this.
- Added configs back for the pocketwatch.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.7.1</summary>

Happy Holidays.

- Pocketwatch readded. Just a normal item.
- Rose now also damages you when you pocket it.
- Lowered the audio range of the frigid capsule by 50%.
- Lowered most default spawn rates.
- Some prices altered.
- Medical kit no longer replenishes while actively being used.
- Frigid capsule freezing removed completely along with many of it's particles, It now just applies a stacking slow the longer you remain in the aoe. Freezing had too many factors, wasn't as fun as I thought, buggy, probably laggy, etc. No more staring at a wall for 15 seconds.
- Possibly fixed double hit issue, did some changes and light testing and it seemed fixed. Let me know.
- Added uselimit and useslimited configs for the defibrillator. uselimit is based on the amount of players the defib has been used to revive not general uses of the item.
- Defibrillator now flashes red when used on a corpse that cannot be revived due to it being dismembered. It will also turn red permanently and play a sound when out of uses completely if that is enabled.
- You now have to use the defib on a corpse twice to revive the deceased player.
- Reverted the golden ticket back to having 5 uses instead of one use that spawns 5 gifts.
- Added more checks for tickets and tickets no longer work on items that are inside the company cruiser. This cruiser bug was a very weird bug that I still don't understand but I've found a method to fixing it.
- The Handlamp now remains on when pocketed.
- Readded csync as a dependency.

Bugs I hoped to fix with this update -

- Melee item Double hit.
- Ticket use on cruiser bug.
- Frigid capsule several bugs.

Possible future improvements/additions -

- Medical kit long awaited function addition.
- Toolbox long awaited redo.
- Emergency injector additions.
- Refinements and QOL.
- New Items.

Modding interest regained, new items inbound.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.7.0</summary>

I don't think I've said enough thanks and really didn't expect the mod to have nearly as many downloads/users as it's received so I just wanted to say thanks everyone for viewing and trying the mod. I really appreciate it.

- New modpage image.
- Updated many textures and some model tweaks.
- Explosive Tank -> Unstable Fuel Cylinder , Changed the name so it's less obvious what it'll do.
- Improved effects for some items, mostly the capsules.
- Frigid capsule redone from scratch. It now creates a temporary localized snow storm that builds up frost stacks rapidly instead of firing 3 waves.
- Frigid capsule knocks you off of ladders if you are frozen while on one to avoid getting you stuck on them permanently.
- If a frozen player is damaged they will be unfrozen.
- Shift controller fixes.
- Pocketwatch removed.
- Hopefully stopped gloomy capsule from sometimes teleporting players inside objects.
- Fixed gloomy capsule grab/new day bug.
- Further increased capsules value due to the danger they can pose.
- Capsules staying active in orbit and on the company moon has been fixed. A config will be added to alter this behavior further.
- Added Info on the first two capsules I added last update.
- Added a short teleport delay to the shift controller.
- Added shift controller connection range configs.
- All configs changed to be categorized by item instead of type of config.

- Fixed visual bugs and other minor bugs not worth recording.

Here's a list of recent known and confirmed bugs -

- Rose can invert your controls (may be fixed)
- Crowbar 2 shots only hoarding bugs for some reason, I looked for hours and I can't figure out why this happens.
- Frigid capsule can perma freeze you on ladders (may be fixed)

And some possible future improvements/additions -

- Item names tweaked for better compatibility.
- Toolbox changes.
- Golden ticket changes.
- Handlamp changes.
- Emergency injector changes.
- New items.

If you have any config issues after this update I recommend deleting your config file so it can be remade. I completely reorganized it this update.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.6.9</summary>

### Lost name Update

I don't like going weeks without updating if I can help it so if an item taking long to make (frigid capsule in this case) i'm just going to update with what I have and the item will cook until it's done.

- Removed leftover logging.
- Fixed injector draining stamina instead of restoring stamina initially.
- Removed injector changing ui colors because of problems, will figure something else out.
- Shift controller no longer glitches from just being dropped inside the ship.
- Fixed harmless ticket error.
- Increased the great connection range of the shift controller by around 30%.
- Pocketwatch disabled.
- Hopefully fixed defibrillator bodies being duplicated or remaining upon revival for good.
- Made some light attempts at fixing a inconsistent invisible model issue upon revival (I tried some light fixes because I don't want to break other stuff doing random changes).
- Added check so slowing effects won't go negative and reverse your controls.
- Added Gloomy Capsule.
- Added Frigid Capsule (Still a bit unstable).
- Items that spawned on hard moons only now also spawn on any modded moons. Looking into spawning items based on the interior instead of the moon if possible.

Upcoming - 

- Toolbox rework.
- golden ticket changes.
- some new configs.
- New items.

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the "Lethal Company Modding" discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.6.8</summary>

### 1.6.6 Quick follow up fixes #2

- Fixed crowbar doing it's functions multiple times when used in multiplayer resulting in it one-shotting stuff sometimes and open/closing doors rapidly.
- Updated content image.

</details>

---

<details>

<summary>Version 1.6.7</summary>

### 1.6.6 Quick follow up fixes

- Fixed defibrillator working without being fully charged.
- Possibly fixed duplicate dead bodies when using the defibrillator.
- In addition to being cut in half any death that removes your head is also impossible to revive.
- Ticket use on corpses is now disabled.
- Fixed some typos.

</details>

---

<details>

<summary>Version 1.6.6</summary>

### The mass fix update

# VERY SURE THIS WILL BREAK WITH PREVIOUS SAVES.

It's been a little bit but I've not stopped working on the mod in my free time, fixes galore.

- Medical kit replenish rate slowed so that 2 or 3 of them will increase the amount of available health more drastically instead of one giving basically infinite health to the entire crew.
- Medical kit Healthpool is now set to max at the start of each planet.
- Fixed a couple instances where the handlamp would use battery when it shouldn't.
- Defibrillator now has sounds and works correctly.
- Tickets now work as intended.
- The rose will now apply a small slowness debuff each time it damages you.
- Brought back a few of my candies as special drops from the candy dispenser. The rest were lost to the void. The candies all do the same thing, they restore 10 health and give a very minor speed boost.
- Padlocks locking doors is now synced between players.
- Candy dispenser no longer spawns candy if the ship is in space, is currently in motion, or the current moon doesn't have a time cycle.
- The injector no longer heals, increases your sprint meter capacity, gives more speed, gives a jump buff, and changes your sprint meter color while active.
- The injector also drains your sprint meter and gives you a short slow debuff when the speed expires. Your sprint meter will also change to a seperate color while the slow is active.
- When used by clients the crowbar will now properly open the doors when unlocking locked doors.
- Model changes, new color variations for some objects, and improved effects for some objects.
- Fixed scissors not correctly being registered as a scrap item.
- Renamed displacement controller to shift controller to avoid terminal issues.
- Halved the glitch chance of the shift controller in the average connection range.
- Added a cushion to when the glitchs start randomly rolling in the average connection range so it can't roll to glitch as soon as you leave the great connection range.
- Spent like 5 hours trying to make a simple screen effect and have nothing to show for it. Maybe another time.
- Switched out most of the objects networking to my own networking and tested it so I ended up redoing most of them several times to get them working correctly and synced. This is what delayed this update so long.
- The most noticable fixes are above but I fixed an insane amount of bugs that I did not bother recording here.

Planned Additions/Changes - 

- Finish new items.
- Rework the toolbox's function.

May be a couple fix updates after this if needed but otherwise I'll be taking a short break for now. 

If there are ANY issues, the link to my github is at the top of the page! The mod is also on the modding discord under the name "Usual Scrap" with a space.

</details>

---

<details>

<summary>Version 1.6.5</summary>

### Small Update.

- Shrunk the handlamp model by around 1/3.
- Tried to make the candy spawned by the dispenser update the prices on the clients so everyone can see the price. If it works I will do this to all the other items that spawn other items.
- Smoothed out displacement controller code and added more checks. It should be close if not complete code wise unless some random bugs crop up.
- Medical kit and bandages now update the hurt overlay and ui as you are healed. You also won't be stuck limping when healing over 20 with either of them as that is also updated.
- Tried to make defib work v2.

Planned Additions/Changes - 

- Work on Defib's function.
- More bug fixes
- Brainstorming/creating some new dangerous scrap items.

If there are ANY issues, the link to my github is at the top of the page! I am also now on the modding discord.

</details>

---

<details>

<summary>Version 1.6.4</summary>

### Bugfixes, consolidation.

- Fixed Handlamp staying on when running out of battery.
- Reduced light intensity of the controller's screen so it might not be so bright on modded moons.
- Removed unused assets and references.
- Reduced the glitch time of the controller a bit, extended the first connectivity stage range a bit.
- Changed the teleport method of the controller so it hopefully works for clients now.

This'll be all for this weekend! See y'all next weekend! 

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.6.2 & 1.6.3</summary>

### Coat of paint update 2/2  (THIS UPDATE WILL BREAK SAVES WITH PREVIOUS MOD VERSIONS, THERE ARE LOTS OF INTERNAL AND EXTERNAL CHANGES)

- Fixed an OLD major bug that caused the medical kit and bandages to stop working after a day or reset.
- Fixed ticket uses not registering.
- Gave candy dispenser it's damage back, that change wasn't intentional.
- When items are destroyed (Tank exploding, Ticket used, etc.) their radar icons are now destroyed with them.
- Many new/redone item icons.
- Added configs for spawn rate and store cost. I will not be adding anymore configs anytime soon, this was tedious.
- Renamed some items. Lifeline -> Displacement Controller, Adrenaline Injector -> Emergency Injector, Broken Pocket Watch -> Pocket Watch.
- Displacement Controller heavily polished code wise and balanced so for now I will enable it by default.
- Added missing sounds.
- Tweaked models some more.
- Fixed pocket watch scan node not being where it's supposed to be.

- 1.6.3 -> Forgot to update the README with updated information :)

Plans for next update - 

- New items.
- Polish defibrillator.

Thanks for 100,000 downloads I really appreciate it! If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.6.1</summary>

### Small follow-up update

- Undid some changes I made that just broke more stuff.
- Various fixes.
- Fixed defib and lifeline not having battery initially.
- Made defib and lifeline disabled by default like I intended.
- Increased the toolbox price.
- Fixed plenty of incorrect code.
- I'm redoing most icons so those will be coming soon but I've added temporary icons to the new items.
- Tweaked some of the models.

Planned Additions/Changes - 
- Polish Defib more.
- Rename Lifeline
- More bug fixes.


Pretty tired so the next update will take a while unless a bug is critical, see y'all then.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.6.0</summary>

### Coat of paint update 1/2

- Tried to sync the padlock's door locking mechanic between players so you can lock your friends in rooms. You asked for it.
- Added the Defibrillator (disabled by default for testing. If you'd like to try it, enable it in the configs, but expect bugs).
- Added the Lifeline (disabled by default for testing. If you'd like to try it, enable it in the configs, but expect bugs).
- Added the Pocket Watch (Was supposed to have a function but I gave it's function to the Lifeline Device after I had already made its model. If you have any ideas what it should do I'd love to hear them).
- Added the adrenaline shot
- Removed candies.
- Restored some textures that mysteriously disappeared.
- Reduced the handlamp's battery capacity slightly so I could boost it's light reach slightly
- Made dropping the explosive tank deduct less time so new users have more time to think about picking it back up and so dropping it the first couple times isn't as punishing.
- The explosive tank's dropping mechanics are now completely disabled when dropped within the ship. Hitting it still blows it up.
- Messed with the spawn values again, one day maybe they'll feel correct.
- Worked on the spawn method for everything again.
- Increased the chance to spawn candy with the candy dispenser slightly.
- Tweaked the toolbox's trap detection to be more stable and accurate because it turns out landmines just have a weird collider or something. It also works more consistently while crounching now.
- Increased the value of scrap produced by the toolbox and slightly reduced it's cost because everytime I buy a toolbox it feels like traps mysteriously cease to exist.
- Changed some item's weights.
- Stuff I forgot about.


Planned Additions/Changes -

- Use medkit on teamates to heal them functionality

Report ANY and ALL issues if possible, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.9</summary>

#### V60 compatibilty confirmation update.

- Increased crowbar's spawnrate to account for other item spawn increases.
- Made any tools or scrap tools grabbable in orbit.
- Lowered candy spawn rates.
- Lowered the golden ticket conversions to 5 instead of 10 and increased it's spawn rate to 5.
- Tried to fix spawned scrap values only showing for the host, like the candy from the dispenser.
- Updated some internal stuff to the latest versions.

Planned Additions/Changes -

- IMPLEMENT DEFIB.
- 2-3 new items.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.8</summary>

### An update of all time.

- Tweaked radioactive cell damage to put you on critical health before killing you instead of killing you instantly at 25 health sometimes.
- Crowbar takes two-three hits to unlock a locked door instead of 1 and will knock it open.
- Crowbar can knock open unlocked closed doors by hitting them once.
- Reduced time to heal with the medical kit.
- The medical kit now replenishes its healthpool overtime instead of being restocked at the ship, it'll take a bit to get it balanced right but it's a start.
- Possibly fixed spawning issues with all items including the tickets, dispenser, and toolbox. Or made it worse, we shall see.
- Tweaked spawn rates a bit.
- Model tweaks.

Planned Additions/Changes -

- Implement Defib.

I've been working on a few more interesting items but with my new job and needing to learn new code stuff for them it's taking a bit longer, so I just wanted to get this update out.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.7</summary>

### The ..stable? update. again.

- Completely rewrote how the handlamp code functions.
- Consolidated some code, fixed some bugs.
- Disabled candies in configs by default for now because they aren't too useful and bloat the spawning pool with lower value scrap items. If I don't find a use for them I may scrap them.
- Added the candy dispenser.
- Made the crowbar able to open doors by hitting them.
- Messed with the medical kit, hopefully it works correctly now. probably not.
- Forgot to import..

Planned Additions/Changes -

- Continue work on Defib.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.6</summary>

### The ..stable? update.

- Completely rewrote how the handlamp code functions.
- Consolidated some code, fixed some bugs.
- Disabled candies in configs by default for now because they aren't too useful and bloat the spawning pool with lower value scrap items. If I don't find a use for them I may scrap them.
- Added the candy dispenser.
- Made the crowbar able to open doors by hitting them.
- Messed with the medical kit, hopefully it works correctly now. probably not.

Planned Additions/Changes -

- Continue work on Defib.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.5</summary>

### The "It's technically next week" update.

- Handlamp bulb now properly changes whether it's on or off.
- Explosive tank sound was playing when it wasn't yet enabled. don't know how this started but I fixed it.
- Nerfed the radioactive cell's damage intervals even further to try and give more time when eyeless dogs force you to hold it and when the map is a longer one.
- Following the previous change, I've increased the explosive tanks timer a bit to give more time.
- Rewrote a chunk of old code to fix instances where the radioactive cell continued to hurt its holder from the grave when they are killed while holding it.

Planned Additions/Changes -

- Start work on Defib and Crowbar functions.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.4</summary>

### Was supposed to be the last update for this week but...mistakes were made.

- Was testing some new teleporter item code using chocolate and I forgot to put chocolate back to how it's suppose to be last update. OOPS.
- Explosive tank and rad cell value increased.
- Rad cell damage nerfed.
- Added tips to the mod page for surviving only the most dangerous scrap items.
- Candies that gave minor speed now also refill your stamina.

Planned Additions/Changes -

- Start work on Defib and Crowbar functions.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.3</summary>

### The final update of my week long update spree.

- Various model tweaks.
- Stopped the rose from hurting you twice when picked up initially.
- Removed leftover logging from testing.
- Dimmed the hand lamp EVEN FURTHER.
- Fixed the padlock floating when first spawned.
- Fixed instances where the explosive tank being created inside the ship room causes issues.
- I don't know if this actually needs csync so I removed the dependency for now.
- Crowbar is now a melee weapon that weighs a bit less than the usual shovel. opening doors with it will come later.
- Tweaked spawnrates a bit.
- Buffed healing a little.
- Added a bunch of missing sounds
- Simplified some code.

Planned Additions -

- Start work on Defib and Crowbar functions.

I can never playtest enough so if you encounter ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.2</summary>

### The "how many updates?" update.

- Dimmed the hand lamp's bulb glow even further.
- Some more toolbox fixes.
- Gave exchange tickets the icons I forgot to add last update.
- Fixed explosive tank drop counter getting stuck and slightly lowered the time before exploding.
- Edits to the mod description.

Planned Additions - 

- Add more sounds.
- Think of more interesting candy effects.
- Start work on Defib and Crowbar functions.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.1</summary>

### New stuff, more fixes.

- Added the rose scrap Item.
- Added ticket of exchange and golden ticket of exchange scrap items.
- Fixed some tool box and medical kit issues.
- Fixed tangled configs issue.
- Redid my latest icon changes because they looked terrible.
- Code consolidation.
- Reduced tool box turret dismantle time to 12, increased mine dismantle time to 6.
- Added various sounds to items.
- Rearranged the candy effects.


Planned Additions -

- Add more sounds.
- Think of more interesting candy effects.
- Start work on Defib and Crowbar functions.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.5.0</summary>

### So many changes so little time. update.

- Changed some names and fixed inconsistent names.
- Added tags for all items to prevent item conflicts.
- Increased Radioactive Cell's price very slightly and dimmed its light a bit.
- Radioactive Cell's damage now works differently and its damage is based on how long you've held it.
- Fixed crash and syncing issue with the padlock.
- Removed healing sounds because they were very annoying to listen to.
- Scissors are more consistently dangerous.
- Reverted Tool Box to hold to use instead of spam clicking.
- Tried to fix Tool Box rewards being desynced.
- Tool Box can now dismantle active turrets after a lengthy dismantle time (5 seconds for landmines, 15 for turrets).
- Reduced the candies spawn rates further.
- Increased the candies values.
- Tweaked the Explosive Tank's functions.
- Model and sprite tweaks.
- Added control tooltips.
- Added configs for whether an item is loaded and if it is scrap or a store item (Pretty messy, took hours, but works).
- Added sound for deconstructing with the toolbox. I wanted UI but my mind refuses to sit through any more hours of confusion.
- And as always random minor things I've forgotten about.

Planned Additions -

- Add more sounds.
- 3 new items.
- Think of more interesting candy effects.
- Start work on Defib and Crowbar functions.

If there are ANY issues, the link to my github is at the top of the page!

</details>

---

<details>

<summary>Version 1.4.9</summary>

### Updated to v55. again?

- Fixed the weight issues introduced in v55 that some of the items had. No more 120 lb lollipops.
- Fixed Explosive tank being completely broken in v55.
- Tried adding an image to the mod page. hopefully it works.
- Definitely didn't mess up something on the modpage in version 1.4.8 and updated again in two minutes to fix it.

Planned Additions -

- Add more sounds.
- 2 new items.
- Think of more interesting candy effects.
- Start work on Defib and Crowbar functions.

Updates will continue to be slow, sorry. Leave feedback, changes, or additions on my Github. Really wish there was a comments section somewhere but I guess not.

</details>

---

<details>

<summary>Version 1.4.8</summary>

### Updated to v55

- Fixed the weight issues introduced in v55 that some of the items had. No more 120 lb lollipops.
- Fixed Explosive tank being completely broken in v55.
- Tried adding an image to the mod page. hopefully it works.

Planned Additions -

- Add more sounds.
- 2 new items.
- Think of more interesting candy effects.
- Start work on Defib and Crowbar functions.

Updates will continue to be slow, sorry. Leave feedback, changes, or additions on my Github. Really wish there was a comments section somewhere but I guess not.

</details>

---

<details>

<summary>Version 1.4.7</summary>

### Small changes.

- Explosive tank can be reactivated by dropping it a couple times after it's been deactivated.
- Explosive tank effect and sounds are now indicators of whether it is active or not (no more sound effect in the ship).
- Shuffled the candy effects around and added a couple new ones.
- Lowered candies spawn rates to account for there being more of them.

Planned Additions -

- Add more sounds.
- More mod options.
- 2 new items.
- Think of more interesting candy effects.
- Start work on Defib and Crowbar functions.

Updates will continue to be slow, sorry. Leave feedback, changes, or additions on my Github. Really wish there was a comments section somewhere but I guess not.

</details>

---

<details>

<summary>Version 1.4.6</summary>

### Refinement strikes twice. update.

- Added sounds to the Explosive Tank, Radioactive Cell, Toolbox, Medkit, Bandages, and Sizable Scissors.
- Added the Lollipop scrap item.
- Added the Padlock scrap item.
- Readded the crowbar (Just a normal scrap items for now).

Leave feedback, changes, or additions on my Github. Really wish there was a comments section somewhere but I guess not.

</details>

---

<details>

<summary>Version 1.4.5</summary>

### Quick update

- Tweaked explosive tank holding position
- Removed glitchlist the wishlist (wishlist)

</details>

---

<details>

<summary>Version 1.4.4</summary>

### The First in a line of refinement updates.

- The Handlamp now uses it's own script so it shouldn't be affected by mods that change the flashlight but will still be affected by mods that change world lighting.
- The Handlamp no longer shows the flashlight's headlight when pocketed.
- The Handlamp is the first and only item to receive sounds. More items will have sounds added gradually.
- Fixed the radioactive cells model clipping from some angles and changed it's holding position.
- Fix for the harmless toolbox use error.
- Fix for the radioactive cell keeping it's previous name when scanned.
- Lowered the amount of clicks for the toolbox slightly and removed the click cooldown so clicking too quickly won't only count some clicks.
- Drastically improved the icons for everything.
- Candy jar disabled for now while I decide what to do with it.

#### That's all for now. If any issues arise from the Handlamp's new script (as I only tested it in singleplayer) report the issue on the GitHub and I'll fix it asap.

##### I'm looking to change the name of the mod without having to post this as a new mod so if anyone knows how, I could really use the advice. I was considering just changing the name in the files but I don't want to break user's games by accident.

</details>

---

<details>

<summary>Version 1.4.3</summary>

### It's been a while since I started this update so I'll just note all the changes I can recall.

- The Toolbox is now button presses instead of holding to use.
- The Medkit is now hold to heal instead of toggle.
- Replaced the Gift wrap with the Wish list.
- "radioactive mineral cell" is now named "radioactive cell" because name too long.
- Added the sizable scissors scrap item.
- Various model tweaks or redoes.
- Fixed a bunch of bugs I found that weren't too serious so I'm assuming nobody experienced them enough to report them.
- A ton of code changes and improvements probably.
- Replenished motivation.

#### And now for balance tweaks that I can recall.

- The Medkit costs less and heals slightly faster.
- Bandages heal more with less charges (20 x 3) and now have a .5 second cooldown between uses.
- When dismantling turrets with the toolbox it has a chance of dropping a high value laser pointer.
- Explosive tank timer can be any time between 2-4 minutes.
- Walking cane nerfed again because I felt literally untouchable running from monsters with it in hand so now it's speed boost is 2x.

#### The next update shouldn't take as long, and as always, please report any issues on the GitHub page.

</details>

---

<details>

<summary>Version 1.4.2</summary>

- Shrunk the handlamp so it doesn't cover as much screen space when held.
- Fixed random inconsistencies.
- Hopefully fixed the toolbox's syncing (forgot one word!).

</details>

---

<details>

<summary>Version 1.4.1</summary>

New batch of items including: the toolbox, radioactive mineral cell, and gift wrap all with a new unique use or effect, new icons again. As always, this was tested in singleplayer and while I did account for multiplayer if any inconsistencies or bugs occur let me know on my github.

</details>

---

<details>

<summary>Version 1.4.0</summary>

Cleaned up a ton of beginner code, first try syncing the explosive tank's explosion (If you're reading this please report issues on github, multiplayer issues are difficult to test solo), fixed the models and cleaned up their textures, fixed some of the items floating when spawning the first time, more stuff probably. The items I planned to add are on hold to see if this implementation of the explosive tank works, If everything seems to be working I will add them next update.

</details>

---

<details>

<summary>Version 1.3.9</summary>

Buffed the handlamp, buffed the walking cane slightly, and the walking cane now properly spawns on titan, dine, and rend. The 4.0.0 update may take a bit longer as I have quite a few items I'm creating that will require more attention and testing to work properly.

</details>

---

<details>

<summary>Version 1.3.8</summary>

Fixed the scrap values not being the correct value in-game, nerfed the walking cane speed, added effects to using the medkit and bandages, fixed bandages not giving enough uses, fixed medkit logic, probably fixed some other stuff.

</details>

---

<details>

<summary>Version 1.3.7</summary>

Fixed for v50

</details>

---

<details>

<summary>Version 1.3.6</summary>

Added a github I think. New textures and fixes, nothing major.

</details>

---

<details>

<summary>Version 1.3.5</summary>

- Changed how the explosive tank works and changed the testing spawn rate I had forgot to change (my bad).
- Disabled the crowbar and nail because I'm not happy with them.
- Worked on the defibrillator some more.
- Added the walking cane.
- A whole bunch of random fixes.

</details>

---

<details>

<summary>Version 1.3.4</summary>

Added placeholder audio clips to stop the log spam and I think lag caused by it, sorry about that.

</details>

---

<details>

<summary>Version 1.3.3</summary>

fixed/updated some more homepage text.

</details>

---

<details>

<summary>Version 1.3.2</summary>

fixed some homepage text.

</details>

---

<details>

<summary>Version 1.3.1</summary>

Small code tweaks, added bandages and the medkit, started work on a defibrillator and giving the crowbar a unique function.

</details>

---

<details>

<summary>Version 1.3.0</summary>

Small code tweaks, starting work on a couple new items, and found some wonky mod interactions that I may or may not fix in the future but I will note them for now :D

</details>

---

<details>

<summary>Version 1.2.0</summary>

Figured out how this website works and made some name changes to avoid conflicts with other mods.

</details>

---

<details>

<summary>Version 1.0.1 and 1.1.0</summary>

How does this website work?

</details>

---

<details>

<summary>Version 1.0.0</summary>

Mod released with 3 scrap items and one equipment item.

</details>



