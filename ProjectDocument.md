# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

Our player is trapped in the depths of a dungeon and must solve puzzles and fight monsters to advance through each room. Each room may bring them closer to freedom, but there’s no way to know what dangers might be in store for them. They may encounter mages, gnomes, and knights or a merchant who might or might not help in their journey.

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**

Player movement is controlled using WASD. The melee weapon is controlled by the mouse position, and Left Click to initiate a swing. The player have 4 spells, bounded to Right Click, Q, E, and Space, that casts a Fireball, Lighting, Water, and Blizzard respectively. Similarly to the melee weapon, the spells are also guided by mouse position, and a button hold is used to represent the cast time of the spell. Release the spell button after CASTTIME second/s to cast the spell. There are special mechanics to these spells. The Fireball spell is capable of piercing through and burning monsters, and is able to bounce off the wall 3 times. The Lightning spell summons 3 auto-targetting lightning bolts that target to the nearest monster, and damage the monsters. The Water spell summons a AOE damage circle that starts at the player and moves towards the direction of where the mouse was when the spell was casted. The Blizzard spell creates a AOE circle based on the mouse position, like Mage's Blizzard in WoW, and freezes any mob that gets hit.

The player also has an inventory system, bounded to I, where the player can see the weapons, armor, and potions they have collected. The player may click on any weapon or armor and click Equip to equip the new weapon or armor. The player may also navigate to the Items tab in order to access their potions that they may use during their adventure to heal themselves when at low health.

There are 4 types of rooms: Combat, Puzzle, Shop, and Boss. Within the Combat Rooms, the player must kill the spawned monsters in order to clear the the Combat Room to progress to the next room. Within Puzzle Rooms, there are unlit torches scattered across the walls of the room that must be hit with a Fireball spell within a set amount of time, either through direct fire or through bounces. These torches do extinguish after // seconds, so all torches must be lit at the same time in order to progress to the next room. If the player completes the puzzle early, as indicated on the chest timer, the player will be rewarded with random item drops. If the player fails to complete the Puzzle Room within the set amount of time, the player will have to defeat the activated knights before they may progress. Beware, these knights are very difficult. The Shop Room is where the player may spend their gold that they have collected during their journey. The player may buy items such as new weapons, armor, and health potions to strengthen themselves for the remaining exploration. The player may also sell any items they have collected during their adventure. Finally, the player will encounter the red Boss Room, where the player must walk to the center to be moved to start the boss fight.

There are 2 difficulties to the game, normal and hard mode, shown as portals on the top of the Starting Room. The player may interact with either portal with F to select their difficulty. The difference of choosing hard mode compared to normal at the moment is currently only a lighting change, where the game becomes much darker and harder to see. There is also a special mode that must be unlocked through a Konami-Code like system, where you must input a specific button order to unlock the mode. To use the konami-code, you need to move player to the magic-array painting in the center of start room, press "Space" to activate the system, then input/tap "S-W-A-S-W-A" to triger the unlock.

**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.


## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**

## Player Control & Combat - Honghui Li

### Movement
The player-controlled movement is done by capturing directional input and manipulating the `Player` game object's `transform` in  `FixedUpdate()` to have more control over the movement and stability with physic interactions. The direction of movement is captured using Input.GetAxis() on `Horizontal` and `Vertical` for x and y values; then normalized to ensure the player moved at the same speed in all directions. The movement itself is performed by calculating the move-offset using `speed * speedModifier * Time.fixedDeltaTime` and applying move-offset using transform.position or use transform.Translate(). The speedModifier is used for slow debuffon the player and speed-bust power-ups.

There are also non-player controlled movements when a special event is happening, such as interaction with portals. Multiple boolean values were used to ensure the player's movements corresponded with the free-move / event.

### Combat - Weapon
To separate the weapon logic from player control, I designed the weapon to function independently from PlayerController and receive the signal from PlayerController for the attack. The `CRWeapon` (Close Range Weapon) prefab is attached under `Player`, so there move together. The attack is achieved by activating the weapon `hitbox` and `sprite renderer` to show the swinging weapon and enable the hitbox detection. The swing of the weapon is controlled by a customized lerp function `Attack()` that lerp attackProgress from 0f to 1f and repositions the weapon to corresponding angle. With these controls, the weapon can show up, swing through configured attackAngle centered at the mouse position, and then disappear when finished attack.

To make sure the weapon rotation center is the (0, 0) of the weapon prefab, the `counter weight` sub-object was attached to the weapon prefab to be the same shape, not rendered, and opposite position across (0,0) to make sure the weapon prefab's center is in (0, 0) for rotation to function correctly.

When the weapon is idealing (not attacking), the `weapon follower` will show up near the player to show the equipped weapon. The logic of `weapon follower` is similar to the `PositionFolloweCamera` from (exercise 2)[]. It stays still when it's distance to the player is less than `InnerRadius`; moves at `followSpeed` that's slightly slower than the player when distance with the player in between `InnerRadius` and `OuterRadius`; and maintains the max `OuterRadius` when is reaching max leash distance.


| Weapon |   |   |
| :------------: |:------------: |:------------: |
|  <img src="https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocumentMaterial/Attack.gif" alt="Attack" width="80%">  |  <img src="https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocumentMaterial/WeaponFollow.gif" alt="WeaponFollow" width="120%">  | <img src="https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocumentMaterial/counterweight.png" alt="counterweight" width="100%"> |
| Weapon Attack | Weapon Follow | | Counter-weight Idea | 
note: the Counter-weight is compeletely transparent and collider diabled in game, here is showing idea

### Combat - Spells
The purpose of spells is to enhance the player's ranged combat ability. In the early stage, spell instantiation was included inside PlayerController, but the script soon became too large and hard to manage. So I followed the `factory pattern` from (exercise 4)[] to pack spell instantiation logics into the `ISpellMaker` interface that has `ShowRange()` and `Execute()` for indication of spell position and instantiation. The spell management structure was inspired by the (`SpellFactory`)[] participation exercise to include all Spell Execution logics in the spell prefabs so all the internal logic is customized and self-contained for easy utilization by spell makers. The utilization of the particle system for spell effect was inspired by the (`SpellFactory`)[] exercise.

The spell of 4 elements was designed to serve different roles:
- `Fire-Ball` (bound to `fire2`) is the main range dagame spell and serves puzzle rooms. It shoots a fireball toward the mouse position damage and burns the enemy it hits.
- `Water-Surge` (bound to `E`) is the AOE damage spell. It creates a circle range that slowly moves towards the mouse position and damage and stuns the enemy periodically. Inspired by the (`Lerp Playground`)[] demo, the curved particle speed of the particle system is used to show the `charging` and `burst` state.
- `Blizzard` (bound to `Space`) is the AOE control spell. It randomly generates ice-sharps in selected areas, and each ice-sharp will go through forming, falling, and exploding stages and then do a small AOE damage around the ice-sharp.
- `Thunder` (bound to `Q`) is the Monster-Targted spell. It randomly selects a few monsters to chase and then strike. It serves more on information gain in hard/dark mode which will guaranty to reveal some monsters' position in the dark.

| Spells |   |   |  |
| :------------: |:------------: |:------------: |:------------: |
|  <img src="https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocumentMaterial/Fire.gif" alt="Fire-Ball" width="100%">  |  <img src="https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocumentMaterial/Water.gif" alt="Water" width="100%">  | <img src="https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocumentMaterial/Blizzard.gif" alt="Blizzard" width="100%"> |  <img src="https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocumentMaterial/Thunder.gif" alt="Thunder" width="80%">  |
| Fire-Ball | Water-Surge | Blizzard | Thunder | 

The spell also has different level UPR 2D light effect attached to them for hard mode information gain.
The casting/ready stage of spell casting is shown by particle effect around player and is inspired by the (healing effect from classmate's `SpellFactory` exeersice)[].

The initial plan on the spell part was to make 2 spells per element for the player to choose/purchase and implement the interaction between elements to allow players to build their combat style. For example, the original design for the water spell is for a `wet` status on monsters that make them easier to be frizzed or induce lightning. We also planned to include a few special purposed spells for consumable scroll items, such as a `light scroll` that light up the entire room for hard mode combat assist. But these plans were selectively pushed back due to task transferring and time manner.

## Mobs - Orien Cheng

### Mob AI
The concept of the Mob AI was to track and move toward the player, and attack the player when the player is within the mob's attack range. There were 6 mobs in concept: Slime, Skeleton, Knight, GNome, GNelf, and Mage.

In its earliest phases, the design was very simplistic, the mob had to take damage, and the mob had to keep track, move towards, and then attack the player. There were no Mob assets to begin with so they started out with simple Hexagon prefabs so that the interaction between player and mob could be tested. Each mob was designed very simplistically, with Slime, Skeleton, and Knight all melee attacking. GNome was designed to throw a projectile and spawn GNelfs, and Mage was designed to cast either a Fireball or Frostbolt. These projectiles would have their own controllers similarly to (Exercise 4)[] and my self designed pirate command in (Exercise 1)[] . Each mob had its own controller, derived from the `IMobController` Interface, that allowed the player easy access to call the function `TakeDamage()`. The `MobManager.cs` was designed after to keep track of all the mobs in a list, with functions `Subscribe()`, `Unsubscribe()`, and `Notify()` to keep track and control all mobs that were spawned, just like in [Exercise 3](SOME LINK). There were some simple functions designed, such as `SpawnMobs()` and`GetMobs()`, so that the room manager was able spawn mobs and keep track of whether the list of mobs was empty so that the player may progress.

We wanted more variety within the mobs, as many of the mobs shared the same attack patterns or traits, so we redesigned Skeleton, and Knight and made a small rework to the Slime. The skeleton was redesigned so that it threw a bone projectile, and it needed to retrieve its bone before it can throw it again. The decision to have the Skeleton pick up its thrown bone was to differentiate from the GNome, who threw knife projectiles at the player. The Knight was redesigned so that it had a respawning shield that takes a free hit. The Slime was minorly changed so that it slowed the player.

A design bug came up while reworking Skeletons, where a different Skeleton can steal another's bone. In such case, the Skeleton that got their bone stolen ended up dancing inplace. I decided to make this bug into a feature, reworking how Skeletons would pick up thier bones, with the possibility that another Skeleton could steal a bone. During this development phase, a race condition was created where a bone could interact with two Skeleton hitboxes simultaneously, and a Skeleton could interact with two bone hitboxes simultaneously. Using knowledge of semaphores, they were implemented to prevent two Skeletons being able to gain a bone from 1 bone, and vice versa.

Each mob had a state machine system. The Slime, GNelf, and Knight have a similar system, move toward the player, if the player was within its attack range, attack the player, and repeat the cycle. Skeleton had a modification to that. Skeletons would move toward the player, and if it was within its attack range, it would throw its bone projectile. It then would retrieve the bone, and repeat the cycle. The Mage was designed to move toward the player just like the rest of the mobs, and if the player was in range of the Mage, it would start casting either a Fireball or Frostbolt and the cycle would repeat. Similarly to the Skeleton and Mage, the GNome combines both systems. It would move toward the player, and if it was within range of the player, throw a knife projectile. And similarly to Mages, if its cast cooldown was up, it would start casting on the spot until it gets inturrupted by a hit. If this cast went off, GNelfs would spawn around the GNome, and the cycle repeats.

### Status Effects
As we wanted to implement status effects such as burn, freeze, and slow, I designed a Scriptable Object, `StatusEffects.cs`, that could be used globally for statuses. This Scriptable Object would store the status prefabs and status durations for death, stun, burn, and freeze, and would contain functions that would instantiate these prefabs. A design issue came up during this, where each mob had hundreds of lines of code that were identical between all mob controllers. This prompted me to rewrite all mob controllers using the abstract class `Mob.cs` that would still inherit the interface `IMobController.cs`, and allow generic functions to be written for all mob controllers. Some of these functions were virtual functions, allowing certain mob types to override the existing function to fit into the mob design. Each of these status effects use Coroutines to control its duration, where each time a status effect would occur, the existing Coroutine for that status effect would be stopped and a new one would be started, continuing the status effect.

In the end, I realized that I could have changed this implementation so that instead of a prefab being generated every time a status effect occurred, each mob prefab would contain the status effect sprites instead so that the `SetActive()` function could be used to show different statuses on the model.

This Scriptable Object was then extended to the `playerController.cs` to allow mobs to apply statuses onto the player, with accompanying functions added for the player.

### Boss AI
The concept of the Boss AI was to take the Among Us player and create a boss fight with multiple attack patterns designed from the Among Us game.

Having Mob AI already made, the Boss AI was relatively simple in comparison. Many of the functions that were used in Mob AI were transferrable to Boss AI, however the Boss AI would need different attack patterns. As the theme of the boss was Among Us, we decided that the two attack patterns were the button slam and the laser beam. Having learned about child GameObjects and `SetActive()`, two hands representing the left and right hand, a button, and the laser beam was added into the Boss prefab as child objects. These child objects would be activated when the Boss AI is on that attack pattern and deactivated once it has been finished.

### Mob and Boss Rigging
As assets were introduced, Animators were added to all mob prefabs, and animations and animator controllers were implemented based on the design of the mob.

Due to the design of each mob, every mob had a different animation state machine. However, all mobs contained booleans for Attack, Freeze, and Stun within its animator controller that led to its attack animation or idle/still animation respectively. Each mob's controller contained a function, `spriteControl()`, that changed the direction of the sprite to either up, down, left, or right while the mob was moving. As all mobs had an attack animation, except for GNelf who just charges at you, an attack state machine was needed to be implemented on all mobs. This attack state machine would track the elapsed attack animation time, and trigger the player damage or projectile instantiation at the correct point of time during the animation. This was heavily adjusted during Game Feel testing, which I also conducted.

### TA AI
We wanted to put in an Easter Egg player spell for the game's special mode.

TA AI, or Arunpreet AI, does 1 job, attract mobs and throw attendance codes at them. When Arunpreet is spawned, a black hole effect is triggered. Similarly to my implementation of a black hole in the spell factory extra exercise, it takes the list of mobs and moves them towards Arunpreet's character model. Then he randomly throws attendance code projectiles in all directions and disappears.

## Animation and Visuals - Krystal Chau

### Concept Art
The context of the game's environment was imperative to even beginning the process of creating assets. Knowing this, I created a number of concepts for the player, mobs, and the map itself so that my teammates and I would have a clearer vision of what the game would look and feel like. This way we could continue on with a relative consensus of what the game's feel and story was like so that their individual parts would not stray too far from the concepts.<br>
<sub>*All concept art was made using the app `GoodNotes` on the iPad.*<sub><br><br>


| *Concepts* |               |
| :------------: | :------------: |
| Initial Map       | <img src="https://cdn.discordapp.com/attachments/1103896189017010186/1106839144950415426/image.png" width="70%"> | 
| Final Map         | <img src="https://cdn.discordapp.com/attachments/1103896189017010186/1106859081983066142/Game_Asset_Concept_Art-5.png" width="70%"> |
| Player | <img src="https://cdn.discordapp.com/attachments/1103896189017010186/1107156397206012013/Game_Asset_Concept_Art-2.png" width="70%"> |
| Monster Ideas | <img src="https://cdn.discordapp.com/attachments/1103896189017010186/1107156452637933648/Game_Asset_Concept_Art-3.png" width="70%"> |
| Slime | <img src="https://cdn.discordapp.com/attachments/1103896189017010186/1106839020216004608/image.png" width="70%"> |
| Skeleton | <img src="https://cdn.discordapp.com/attachments/1103896189017010186/1106839096967565332/image.png" width="70%"> |

<br>

### Asset Creation
Now that the theme and feel of the game was set, I tried my best to keep the feel the same throughout creation. Since the game did not have a set narrative, I had to make sure that all the assets, at least, had a consistent pixel sizing and palette so that the game had the symblance of being cohesive. We decided on have the animations made in a 64-bit style since it was closest to what I had done before in [Exercise 1](https://github.com/ensemble-ai/exercise-1-command-pattern-krystalchau), but I ended up just making assets that fix what my teammates had coded. With the decided medieval style dungeon, it was important for me to keep the color palette muted and almost muddy to emphasize the implied atmosphere that comes with being in a dungeon. I was mostly inspire by the "dark academia" color palette when choosing colors, using the website [coolors](https://coolors.co/d1c6ad-bbada0-a1869e-797596-0b1d51) to quickly generate colors that might work. In the cases where I needed a tint or shade of a color, I used the site [Tint and Shade Generator](https://maketintsandshades.com/). All assets were made by me (Krystal) using the free website [Piskel](https://www.piskelapp.com/) unless specified otherwise.<br>

#### Animation Rules/Style
- Dark Gray outline around the entirely of the character and major parts of design
- "Glowing" effects do not need to be outlined
- Colored using the set palette
- Each GIF much run at 12 frames per second<br>
<sub>*These rules were set after creating the first Player sprite described below.*<sub>

### The Player
Our game is a 2D top down dungeon crawler, so the first this we had to figure out was the number of directions each mob and the player could move. Originally, we had decided to do all 8 directions (up, down, left right, and the diagonals), but we later decided against it and chose to do 4 directions due to the shear number of assets I would have to create in the end and the reasurrance that it could work since the game `Don't Starve` also does not have 8 directions.<br><br>

| Style Versions |  |
| :------------: | :------------ |
| ![Player V0.1](https://cdn.discordapp.com/attachments/1103896189017010186/1107204628174606476/Armor_Player_Down_2.gif) | The original design looked almost nothing like the concept arts because I quite literally had no experience making animations from scratch. He had a bow and arrows on his back and was frankly, ugly, so I decided to try again. This design was also before we decided to switch to 64-bit, so it was also much less detailed then I wanted.|
| ![Player V0.2](https://piskel-imgstore-b.appspot.com/img/a28790d9-09ad-11ee-aa58-c98958b07512.gif) | This design is almost exactly the same as the concept art and is much nicer than the previous. He was drawn on a single layer and has a visible dark gray border around all parts of his design. He was also made to look like he was running at 12 frames per second. Since this was the first asset, it would set the rules for the rest of the assets after it. |

Once the style of animation was chosen, I animated the rest of the directions, not without problems, but with much more ease than the first.

| Up | Down | Left/Right | Idle | Dead |
| :------------: | :------------: |:------------: |:------------: |:------------: |
| ![Player Up](https://piskel-imgstore-b.appspot.com/img/08945670-09b0-11ee-8107-c98958b07512.gif) | ![Player Down](https://piskel-imgstore-b.appspot.com/img/a28790d9-09ad-11ee-aa58-c98958b07512.gif) | ![Player Left/Right](https://piskel-imgstore-b.appspot.com/img/2d67191c-09b0-11ee-b3b0-c98958b07512.gif) | ![Player Idle](https://piskel-imgstore-b.appspot.com/img/8abc51a3-09b0-11ee-b2e7-c98958b07512.gif) | ![Player Dead](https://piskel-imgstore-b.appspot.com/img/cca63966-09b0-11ee-85e2-c98958b07512.gif) |

<sub>*The idle animation and death icon where created much later in the process compared to the previous player assests.*<sub>

### Mobs
From the player onwards there were not many style issues due to the color palette and drawing style being set; the only real challenge was making the movements feasible to the mob itself.

Since we had many typical mob types, it was important for me to make them unique as unique as I could think of, even if their mechanic ends up being similar to another game.<br>
<sub>*The **mobs** are listed below in the order they were created.*<sub>

#### Slime 

| Up/Down | Left/Right | Attack |
| :------------: |:------------: |:------------: |
| ![Slime Up/Down](https://piskel-imgstore-b.appspot.com/img/7243251c-09bd-11ee-ab25-c98958b07512.gif) | ![Slime Left/Right](https://piskel-imgstore-b.appspot.com/img/cc827e54-09b2-11ee-9866-c98958b07512.gif) | <img src="https://piskel-imgstore-b.appspot.com/img/14fd77a6-09b3-11ee-ad62-c98958b07512.gif" width="70%"> |

#### Skeleton
| Type | Up | Down | Left/Right |
| :------------: |:------------: |:------------: |:------------: |
| Chase | ![Skeleton Chase Up](https://piskel-imgstore-b.appspot.com/img/a1f53f21-09b8-11ee-b065-c98958b07512.gif) | <img src="https://piskel-imgstore-b.appspot.com/img/60ac9e97-09b8-11ee-8c72-c98958b07512.gif" width="60%"> |![Skeleton Chase Left/Right](https://piskel-imgstore-b.appspot.com/img/7c11eff8-09b8-11ee-99c0-c98958b07512.gif) |
| Attack | ![Skeleton Attack Up](https://piskel-imgstore-b.appspot.com/img/38e2f587-09b8-11ee-b687-c98958b07512.gif) | <img src="https://piskel-imgstore-b.appspot.com/img/ce68a791-09b8-11ee-935f-c98958b07512.gif" width="60%"> |![Skeleton Attack Left/Right](https://piskel-imgstore-b.appspot.com/img/f37e55a3-09b8-11ee-b1e5-c98958b07512.gif) |
| Bone | ![Bone](https://piskel-imgstore-b.appspot.com/img/41ad90dc-09b9-11ee-b31d-c98958b07512.gif) | *The bone is one of the few times I forgot to follow my animation rules (forgetting the gray outline)*|  |

#### G'Nelf
| Up | Down | Left/Right |
| :------------: |:------------: |:------------: |
| ![G'Nelf Up](https://piskel-imgstore-b.appspot.com/img/16afaa66-09ba-11ee-9f03-c98958b07512.gif) | ![G'Nelf Down](https://piskel-imgstore-b.appspot.com/img/3082a4eb-09ba-11ee-b4e3-c98958b07512.gif) | ![G'Nelf Left/Right](https://piskel-imgstore-b.appspot.com/img/c1bb5838-09b9-11ee-a5ea-c98958b07512.gif) |

#### G'Nome
| Type | Up | Down | Left | Right |
| :------------: |:------------: | :------------: | :------------: | :------------: |
| Chase | ![G'Nome Chase Up](https://piskel-imgstore-b.appspot.com/img/0390504c-09bb-11ee-b1c7-c98958b07512.gif) | ![G'Nome Chase Down](https://piskel-imgstore-b.appspot.com/img/2b59b3c2-09bb-11ee-a3fc-c98958b07512.gif) |![G'Nome Chase Left](https://piskel-imgstore-b.appspot.com/img/9d10ecc0-09ba-11ee-a727-c98958b07512.gif) | ![G'Nome Chase Right](https://piskel-imgstore-b.appspot.com/img/e057590f-09ba-11ee-9659-c98958b07512.gif) |
| Attack | ![G'Nome Attack Up](https://piskel-imgstore-b.appspot.com/img/1bb479ca-09bb-11ee-a272-c98958b07512.gif) | ![G'Nome Attack Down](https://piskel-imgstore-b.appspot.com/img/8e3772a8-09ba-11ee-b8cc-c98958b07512.gif) | ![G'Nome Attack Left/Right](https://piskel-imgstore-b.appspot.com/img/eefa2b33-09ba-11ee-8a0e-c98958b07512.gif) |  |  |
| Cast | ![G'Nome Cast](https://piskel-imgstore-b.appspot.com/img/3aa15ea1-09bb-11ee-a05d-c98958b07512.gif) |
| Knife | ![G'Nome](https://piskel-imgstore-b.appspot.com/img/b7371fd7-09bd-11ee-b113-c98958b07512.gif) |  |  |

#### Mage 
| Type | Up | Down | Left/Right |
| :------------: |:------------: |:------------: |:------------: |
| Chase | ![Chase Up](https://piskel-imgstore-b.appspot.com/img/e93fe2dc-09bb-11ee-bbc1-c98958b07512.gif) | ![Chase Down](https://piskel-imgstore-b.appspot.com/img/a38f4491-09bb-11ee-9731-c98958b07512.gif ) |![Chase Left/Right](https://piskel-imgstore-b.appspot.com/img/55425478-09bb-11ee-91ef-c98958b07512.gif) |
| Cast | ![Cast Burn](https://piskel-imgstore-b.appspot.com/img/cdc3dab5-09bb-11ee-91a4-c98958b07512.gif) | ![Cast Frost](https://piskel-imgstore-b.appspot.com/img/b4bda5b0-09bb-11ee-9ba5-c98958b07512.gif) |  |

#### Knight
| Type | Up | Down | Left/Right |
| :------------: |:------------: |:------------: |:------------: |
| Chase | ![Knight Chase Up](https://piskel-imgstore-b.appspot.com/img/d18cd399-09bc-11ee-bfc1-c98958b07512.gif) | ![Knight Chase Down](https://piskel-imgstore-b.appspot.com/img/8c1509f3-09bc-11ee-bee0-c98958b07512.gif) | ![Knight Chase Left/Right](https://piskel-imgstore-b.appspot.com/img/de4f7dee-09bc-11ee-a9d7-c98958b07512.gif) |
| Attack | ![Knight Attack Up](https://piskel-imgstore-b.appspot.com/img/c21c8070-09bc-11ee-832b-c98958b07512.gif) | ![Knight Attack Down](https://piskel-imgstore-b.appspot.com/img/9d6435e8-09bc-11ee-89bf-c98958b07512.gif) | ![Knight Attack Left/Right](https://piskel-imgstore-b.appspot.com/img/ef4d7db0-09bc-11ee-bd0c-c98958b07512.gif) |
| Statue | ![Knight Statue](https://piskel-imgstore-b.appspot.com/img/368cc26e-09bd-11ee-b587-c98958b07512.gif) |  |  |
| Shield | ![Knight Shield](https://piskel-imgstore-b.appspot.com/img/0ec3b530-09bd-11ee-8453-c98958b07512.gif) |  |  |

### Weapons/Armor
Weapons and armor were created as a way for the player to have some kind of customizability to their playthrough. Although exteremly RNG, having one of the more powerful weapons/armor appear in the Merchants room can drastically change the player's experience playing the game with different stats on each item.

| All Weapons | ![Weapon All](https://piskel-imgstore-b.appspot.com/img/894ddd97-09c0-11ee-81c4-c98958b07512.gif) | All Armor | ![Armor All](https://piskel-imgstore-b.appspot.com/img/30704302-09c1-11ee-9126-c98958b07512.gif) |
| :------------: |:------------: |:------------: |:------------: |

#### Attack (Unsused)
| ![Wooden](https://piskel-imgstore-b.appspot.com/img/7799f29c-09c0-11ee-a8e6-c98958b07512.gif) | ![Iron](https://piskel-imgstore-b.appspot.com/img/552a4723-09c1-11ee-bbdb-c98958b07512.gif) | ![Ice](https://piskel-imgstore-b.appspot.com/img/5cc34ef3-09c1-11ee-bf1d-c98958b07512.gif) | ![Amethyst](https://piskel-imgstore-b.appspot.com/img/6867375c-09c1-11ee-be6b-c98958b07512.gif) | ![Ruby](https://piskel-imgstore-b.appspot.com/img/732aa259-09c1-11ee-a4b9-c98958b07512.gif) | ![Emerald](https://piskel-imgstore-b.appspot.com/img/7e90b98f-09c1-11ee-b28a-c98958b07512.gif) |  
| :------------: |:------------: |:------------: |:------------: |:------------: |:------------: |

### Effects
| Ablaze | Frozen | Stun | Death Poof |
|:------------: |:------------: |:------------: |:------------: |
| ![Ablaze](https://piskel-imgstore-b.appspot.com/img/305e0f75-09c2-11ee-92c1-c98958b07512.gif) | ![Frozen](https://piskel-imgstore-b.appspot.com/img/47ba868c-09c2-11ee-8a6f-c98958b07512.gif) | ![Stun](https://piskel-imgstore-b.appspot.com/img/66113b1e-09c2-11ee-9194-c98958b07512.gif) | ![Death Poof](https://piskel-imgstore-b.appspot.com/img/80011954-09c2-11ee-9901-c98958b07512.gif) |

### Merchant
The concept for the Merchant was to set him up to be a potential boss. However due to time constraints, he is not a boss, but his concpet remains. The merchant is a retired [Mage](https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocument.md#mage) that has now devoted his life to selling junk. Although he seems very homeless, he is extremely rich. 

His design is almost identical to that of the Mage with a recolor and some cloth patches to emphasis his homelessnes. Since a merchant like him would want to hide his richness, it is protrayed subtly through the gold buttons on the merchant UI panel.

| Merchant | Tent | Rug | Together |
|:------------: |:------------: |:------------: |:------------: |
| ![Merchant](https://piskel-imgstore-b.appspot.com/img/492f1a38-09c3-11ee-8b70-c98958b07512.gif) | ![Tent](https://piskel-imgstore-b.appspot.com/img/54bfe63d-09c3-11ee-a0ae-c98958b07512.gif) | ![Rug](https://piskel-imgstore-b.appspot.com/img/3b3afec5-09c3-11ee-a40a-c98958b07512.gif) | ![Together](https://piskel-imgstore-b.appspot.com/img/5d72148c-09c3-11ee-abd3-c98958b07512.gif) |

### Special Sprites
You (Professor or Arunpreet) may recall someone asking you what you what you usually where on a typical day. This is the result of that.

The reason for the noticable style change here is that these characters can only be **unlocked** by completing a special pattern. Since they are hidden under a code, they must also adron a special style specific to them.<br>
<sub>*I wanted to make cute sprites for once.*<sub>

#### Professor
| Running | Idle |
| :------------: |:------------: |
| ![McCoy Running](https://piskel-imgstore-b.appspot.com/img/e7f4a79e-09be-11ee-be05-c98958b07512.gif) | ![McCoy Idle](https://piskel-imgstore-b.appspot.com/img/fb6f2c54-09be-11ee-9941-c98958b07512.gif) |

#### Arunpreet
| Spin | Attendance Code |
| :------------: |:------------: |
| ![Arunpreet](https://piskel-imgstore-b.appspot.com/img/ef7f388f-09bf-11ee-9b82-c98958b07512.gif) | ![Attendance Code](https://piskel-imgstore-b.appspot.com/img/a18f19ae-09c2-11ee-beb2-c98958b07512.gif) |
#### Mode Select
| Normal | Hard | Special |
| :------------: |:------------: |:------------: |
| ![Normal Gate](https://piskel-imgstore-b.appspot.com/img/a32ed461-09c8-11ee-90e1-c98958b07512.gif) | ![Hard Gate](https://piskel-imgstore-b.appspot.com/img/97355d94-09c8-11ee-9f67-c98958b07512.gif) | ![Special Gate](https://piskel-imgstore-b.appspot.com/img/8540dfe8-09c8-11ee-82dc-c98958b07512.gif) |

| Special Code Area |  |
| :------------: |:------------ |
| <img src="https://piskel-imgstore-b.appspot.com/img/28300e8a-09c8-11ee-bada-c98958b07512.gif" width="80%"> | We wanted there to be a way to visualize entering the correct code to make the experience even more special to the player. The restoration of the pentagram a long with the glowing elements is all made in an effort to make this special event more magical, so it would make sense that the curtain would mysteriously fall off. |

| Curtain Version |  |
| :------------: |:------------ |
| ![Curtain V0.1](https://piskel-imgstore-b.appspot.com/img/8b3b5ab5-09c7-11ee-b437-c98958b07512.gif) | The purpose of the curtain is to later fall/fade off to reveal the mirror behind it, but having this type of curtain fall of makes no sense in context, so I decided to remake it. |
| ![Curtain V0.2](https://piskel-imgstore-b.appspot.com/img/9ac656d1-09c7-11ee-8aa1-c98958b07512.gif) | This made much more sense, but took me much longer than any of the other sprites beacuse I wanted the sheet to fall in a somewhat natural way. Draping the sheet was difficult in itself, but have the sheet fall in a "graeful" way was an important was to signify the "special-ness" of this mode. |

### UI
#### Inventory/Merchant
I made sure to make all the UI panels, buttons, and slots as close to the theme as possible, while also being simple enough so that the design itself would not distract from the items in the inventory too much. It is also important to keep the theme consistent in these screens because thery blocks a large portion of the play area that contains the "atmosphere" of the game; keeping the theme as closely as possible helps midigate any damage to game feel.

#### Puzzle
The puzzle buttons had be extremely obvious in order for Players to get an understanding of the puzzle quickly. I made sure to make walls that correspnded to a specific button very obivous so that there is little trial and error on the player's part regarding puzzle mechanics. 

### Miscellaneous
| Hearts | Potion | Chest | Gate | Decorations | Coin | Scroll |
| :------------: |:------------: | :------------: |:------------: | :------------: |:------------: | :------------: | 
| ![Hearts](https://piskel-imgstore-b.appspot.com/img/ee0f580a-09c4-11ee-90e5-c98958b07512.gif) | ![Health Potion](https://piskel-imgstore-b.appspot.com/img/c040c84c-09c6-11ee-9a54-c98958b07512.gif) | ![Wooden Chest](https://piskel-imgstore-b.appspot.com/img/bd02e073-09c4-11ee-80f2-c98958b07512.gif) | ![Gate](https://piskel-imgstore-b.appspot.com/img/fe459fd7-09c4-11ee-9fd5-c98958b07512.gif) | ![Decorations](https://piskel-imgstore-b.appspot.com/img/93743a66-09c5-11ee-9a9e-c98958b07512.gif) | ![Coin](https://piskel-imgstore-b.appspot.com/img/e8e78233-09c5-11ee-a117-c98958b07512.gif) | ![Scroll](https://piskel-imgstore-b.appspot.com/img/a8291100-09c4-11ee-a11a-c98958b07512.gif) |
|  |  | ![Gold Chest](https://piskel-imgstore-b.appspot.com/img/c579ee45-09c4-11ee-8254-c98958b07512.gif) |  |  |  |  |
|  |  | ![Diamond Chest](https://piskel-imgstore-b.appspot.com/img/d025c7ae-09c4-11ee-a222-c98958b07512.gif) |  |  |  |  | <br>

<sub>*As of the time writing this (6/13/2023), this is the complete list of all assets I have created excluding: UI Elements, Boss Health Bar, and a Scrapped Torch.*<sub>

## Puzzle

### Design
The main concepts behind most puzzles were made on graph paper, where I could correlate each square on the paper to a tile in game. I chose to desgin on paper because it was the best way to test many different designs quickly and without too much of a turnaround time so that the implementor(Jason) could work on the room. 

We chose to do a "bouncing angles" type of puzzle where the goal is to light all the torches in the room by bouncing fireballs of the walls and obsticals of the room to hit them under a certain about of time because our spell implementor(Honghui) ended up creating the fireball spell quite early on in the game creating process. This way the spell could be useful to not only attack, but also to solve puzzles which helped further integrate the mechanic with the game as a whole. 

Originally, the puzzles were supposed to reward the player with an upgrade to their fireball spell and to punish failure with the special knight mob that would only activate upon puzzle failure, but the upgrade was scrapped as we felt it was no longer necessary. Despite the scrapping of the previous reward, we felt that it was still important to provide an incentive for players to complete puzzles so that they would be less tempted to wait out the puzzle timer, so we provided random drops that might or might not be better than their current gear and strengthened the knights.

We ended up creating a total of 5 puzzle rooms, including the tutorial, 4 of them designed by Krystal and 1 by Jason.

### Puzzles Designed by Krystal
Most of my puzzles went through multiple design changes as our game was developed due to the screen's restriction of the player's sight. The first itteration of puzzles too large for the player to be able to see all torches on the map, so it forced the player to guess angles to bounce fireballs, rather then making more educated decisions. 

| Room 1 Version 1 | Room 2 Version 1 |
| :------------: | :------------: |
| <img src="https://cdn.discordapp.com/attachments/1112968117509959701/1112968670143070278/IMG_4571.jpg" width="50%"><br><img src="https://cdn.discordapp.com/attachments/1112968117509959701/1112968670562496525/IMG_4572.jpg" width="50%"> | <img src="https://cdn.discordapp.com/attachments/1112968117509959701/1113420536094150696/IMG_4576.jpg" width="70%">  |
| As this was the first ever puzzle made, I made sure to include multiple explainations of what each mechanic would do and what the "answer" to teh puzzle was. This room was 11 by 6 units and was reflected that way in the game when implemented. However, the player was not able to see the torches on both sides at the same time making it difficult for the player to solve rooms as intended. | This puzzle was even larger and had the same mechanics as the first puzzle room. It also had the same problems as the first room, where each room was just too large for players to see much of anything. |

Once these problems were brought to my attention, I opted to redesign each puzzle so that they would fit in the screen's 9 by 5 block limit, while trying to keep the rooms as close to the original designs.

| Room 1 Version 2 | Room 2 Version 2 |
| :------------: | :------------: |
| <img src="https://cdn.discordapp.com/attachments/1073950912801947698/1118667150979240026/IMG_4591.jpg" width="70%"> | <img src="https://cdn.discordapp.com/attachments/1073950912801947698/1118667279689863198/IMG_4591.jpg" width="70%"> |

Both of the new designs were inside the constraints of the player's field of vision, coupled with a new mechanic to lock the camera on the center of the room so that the player would always be able to see the entire map. This change effectively made each puzzle much less of blind guessing, but more of the intended educated shooting.

The 3rd room I created was made after the above observations, so it was created with only one drafting. The major difference between this puzzle and the others I created was the new mechanic of moving walls, made by Jason as he was creating his own.

| Room 3 | <img src="https://cdn.discordapp.com/attachments/1112968117509959701/1117286341957402775/IMG_4589.jpg" width="30%" style="transform:rotate(270deg);"> |
| :------------: | :------------: |

The last room to mention is the Tutorial puzzle that is supposed to teach the player the mechanics of puzzles, was actually created sometime between the first puzzle version 1 and the second puzzle version 1.

| Tutorial Puzzle |  |
| :------------: |:------------: |
|  This was the first iteration of the tutorial room, but had the same player field of view problems as the fist and second puzzles, so it was proptly resized to 9 by 5 in the current game | <img src="https://cdn.discordapp.com/attachments/1112968117509959701/1112974720434966628/IMG_4574.jpg" width="70%"> |
| Unfortunately, after testing the game at the game showcase, we have decided that the tutorial room simply does not prepare the player well enough to complete puzzles on thier own, but do not currently have the dev time for it, so it will remain as is. |  |

### Puzzle Designed by Jason

<!-- Feel free to add anything to the stuff above if you want to - Krystal -->

### Implementation

## Map Visuals, Logic and Design - Marc Paolo Yap

### Overview

I used a teleport based system in order to link several rooms together to form a cohesive map. This was done to allow for most of the map related gameobjects to be already loaded upon entering the scene. In order for the game to feel unique every run, I made it so that how the rooms are linked are based on the map configuration matrix which is procedurally generated.

In addition to this majority of the map-related assets were created by me (Paolo) using the free website [Piskel](https://www.piskelapp.com/).

## Rooms

### Setting Up the Scene

I created the initial design for a single room grid in our game environment. I needed this to be done in order to create the rest of the map.

I decided to use tile maps for room creation as this gave flexibility and ease of use when creating different layouts and design

### Design and Creation

I had different iterations of the room tile map as we weren't sure yet on how are room should look like. Either having it look isometric at a 45 degree angle or to have it completly top-down similar to The Legend of Zelda (1987) or the Binding of Isaac: Rebirth.

<img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118726997787545650/DefaultRoom_Layout.PNG width = 70%>

| _Room Tile Maps_ | | |
| :-: | :-: | :-: |
|  Initial Design for Wall   | <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118726657549811712/BridgeFloor.png>  |
| Finialized Design for Wall | <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118721230535675904/DungeonWall.png>  | <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118722437568278548/DungeonWall_6.png> |
|  Initial Design for Floor  | <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118721230321758228/DungeonFloor.png> |
| Finalized Design for Floor |  <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118721230091067412/FloorTile.png>   |

We decided on having a total of 6 room types: <br>
Start Room, Tutorial Room, Puzzle Room, Combat Room, and the End Room.

I decided to have a total of 16 rooms in order to prolong the game, this consisted of:
- 1x Start Room
- 1x Tutorial Room
- 8x Combat Rooms
- 2x Shop Rooms
- 3x Puzzle Rooms (3 are chosen out of 4 existing)
- 1x End Room

These rooms had their own respective colors and layout. Starting Room was set to the default room and color.

| Room Type |   Layouts  |
| :-------: | :-: |
|      Combat Room     |  <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118726658065703055/CombatRooms.PNG>  |
|      Tutorial Room     |  <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118750031554543636/TutorialRoom.PNG>  |
|      Puzzle Room <br>(was initially different sized but changed for consistency)    |  <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118726999066824744/PuzzleRooms.PNG>  |
|      Shop Room <br> (Additional assets and layout were created by Krystal, Jason, and I)     |  <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118726999561732226/ShopRooms.PNG>  |
|      End Room     |  <img src = https://cdn.discordapp.com/attachments/1116541534087684108/1118726997997256734/EndRoom.PNG>  |

## End Room
|||
| :-------: | :-: |
| Boss Room Entrance <br> <br> Found inside the End Room, and teleports the player to the boss room when colliding with the trigger | ![BossEntrance](https://piskel-imgstore-b.appspot.com/img/f61eb1d1-0b25-11ee-878c-b7bc24a2ee1f.gif)|

Animation and trigger is handled by the `VentDetector.cs`, and `BossEntrance.cs` script.

Teleportation to boss room is handled by the `BossTeleport.cs` script via on trigger.


## Map Layout

### Procedurally Generated
The map was procedurally generated using a heavily modified drunkard walk algortithm in `DrunkardWalk.cs` tailored to fit our layout. This algorithm could potentially make any n x m matrix map by modifying some values, but I kept these values set to follow our map constraints (specific number of total rooms, specific number of combat, puzzle, shop rooms).

I used an enum `RoomTypes` that represented different room types:
- 0 being No Room
- 1 being Starting Room
- 2 being Tutorial Room
- 3 being Shop Rooms
- 4 being Combat Rooms
- 5 being Puzzle Rooms
- 6 being End Rooms
- 7 being Temporary Rooms

The public function `GenerateMatrix` returns a n by n generated matrix that could be fed later to our `MapGenerator.cs` script.

`GenerateMatrix` first chooses a starting room randomly in the matrix by setting the matrix [n][m] value to `1`. It then calls the `GenerateTrainingRoom` which generates a training room by assiging the matrix value to `2` always adjacent to the starting room. 

After it picks both the starting room and the training room, the drunkard starts walking randomly in different directions which is stored in the `nextRoom` value for 100 steps trying to create a total of 15 rooms (end room is added at the very end). If the drunkard hasn't finished creating all 15 rooms within the amount of steps, it just regenerates the matrix again. This drunkard will assign the value of a temporary room, which is `7`, only on empty spots in the matrix. I also made a constraint that it cannot place a room adjacent to the starting room, as the tutorial room has to be the only room next to it.

After it generates all the temporary rooms, the drunkard stops walking. Multiple functions are then called to assing the different room types that are remaining. `GenerateShopLayer()` generates the shops replacing temporary rooms with its value, `3`, this makes sure that no shop rooms are placed adjacent to a shop room. `GeneratePuzzleLayer()` functions simialrly to `GenerateShopLayer()` placing its value `5` on top of temporary rooms and ensuring that they're not placed adjacent to another puzzle room. `GenerateCombatLayer()` just fills the remaining temporary rooms with the combat room value `4`. Finally, `GenerateEndRoom()` looks for a room that exist, is not completly surrounded by rooms, and is as far away (by distance not by rooms) from the start room and places the end room `6` on a random empty spot adjacent to it. 

I initially had planned to generated the end room to be the furthest amount of rooms away from the starting room but I found it too complicated to do within the remaining time. I also felt like I could've used a better map generation method but decided to go for the more simplistic route.

### Linking the Rooms Together
I used the `MapGenerator.cs` script to link all the rooms together via teleporters, esentially creating the pathway between rooms.

`MapGenerator.cs` on `Start()`, looks for all the type of room gameobjects that exist within the scene and adds them to their respective lists via the `AssignList()` method.

Then from RNG, the map is selected, there is a 25% chance of a premade `crewmate` shaped map being picked, and a 75% chance of being a randomly generated map from `DrunkardWalk.cs`.

 Then according to their respective location in the matrix, `AssignRoom()` is called to iterate throught the fed n by m matrix and uses its `Vector2Int` position in the matrix as the `roomDictionary` key and assigns the respective room gameobject as the `roomDictionary` value based on the room value in the matrix.

`AssignPortal()` is then called which iterates through the map matrix and checks the current room's right and bottom if there exists a room. If there exists a room on the right, the current room's left teleporter `PortalA1` will link to the right adjacent room's left teleporter `PortalA2`. Similarly, if there exists a room on the bottom, the current room's bottom teleporter `PortalB1` will link to the bottom adjacent room's top teleporter `PortalB2`. 

This linkage is done via the `LinkTeleporter.cs` script attached to all teleporters (via prefab) by assigning the `TargetRoom` gameobject to each others teleporter. If a player walks to the teleporter via an `OnTriggerEnter2D` it teleports the player to the targer room.

While linking these rooms, I also made sure that the teleporter sprites were not rendered via the `LinkTeleporter.cs` script by checking if `TargetRoom` was null. The wall tiles were also updated properly to reflect the map layout. This meant that if there was no target room the walls will visually get updated, this is all handled by the `TileUpdate.cs` script attached to the wall gameobject of each room.

I felt like this could've been done more efficently if I used a central teleporter controller for each room.
 
<img src= "https://cdn.discordapp.com/attachments/1116541534087684108/1118727890373181470/UpdatedRoomTiles.PNG">

### MiniMap

The minimap is generated via the `MapGenerator.cs` script. It creates a bunch of `miniRoom` gameobjects arranged in a matrix and is shown via the canvas ui. The minimap has a transparent background for better visibility. Visited rooms are colored with light grey, current room is colored with white, and unvisited adjacent rooms are colored with dark grey. The boss room has a unique icon. This minimap design was heavily inspired by the Binding of Isaac: Rebirth.

<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118726549475180594/image.png">

The icon sprites were drawn by me:
| Icon |  |
| :-------: | :-: |
| Room | <img src= "https://cdn.discordapp.com/attachments/1116541534087684108/1118776117470429294/MiniRoomTexture.png"> |
| Boss Room | <img src= "https://cdn.discordapp.com/attachments/1116541534087684108/1118721228761477200/BossIcon.png"> |

### Room Logic
Each room had their own preset conditions to complete. If the room is not completed when entered, then the teleporters (represented by the gate sprites) will be closed and kept shut. This is mostly handled by the `LinkTeleporter.cs` script found in each teleporter. For puzzle rooms/tutorial room, upon entering, the camera shifts to center on the room, and the minimap disables for better visibility. When the puzzle starts, the tiles updates via `TileUpdate.cs` to create an enclosed room with no escape until the player completes or defeats the knights that will spawn when failing (by checking the mob count). For combat rooms, the room is shut if there are mobs and completes once they all die. 

### Additional Room Related Sprite Work

| Sprites |  |
| :-------: | :-: |
| Candles | ![Candle](https://piskel-imgstore-b.appspot.com/img/8a7601dc-0b3f-11ee-b56b-b7bc24a2ee1f.gif) |
| Chain Ball <br> (unused) | <img src= "https://cdn.discordapp.com/attachments/1116541534087684108/1118722436750393394/Ball_Chain.png"> |
| Item Pedastel<br> (unused) | <img src= "https://cdn.discordapp.com/attachments/1116541534087684108/1118721229872975892/Item_Pedestal_Base.png"> |

## Input

**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**

## Game Logic

**Document what game states and game data you managed and what design patterns you used to complete your task.**

# Sub-Roles

## Project Manager - Orien Cheng

**Describe the steps you took in your role as producer. Typical items include group scheduling mechanism, links to meeting notes, descriptions of team logistics problems with their resolution, project organization tools (e.g., timelines, depedency/task tracking, Gantt charts, etc.), and repository management methodology.**

### Project Management
From my understanding of game design and game development cycles, I conducted regular weekly meetings to check on each teams progress and to make sure that the game development was on track. I scheduled our (Initial Game Plan)[https://docs.google.com/document/d/1nOKUQqh0cJJVvcR_yvxZgWWZTnJzXiF4eff2JPueFGs/edit?usp=sharing] Gantt Chart, with an emphasis on focusing on creating quick and simple systems early in the development cycle, so that certain dependencies could be tested early on to ensure cohesion throughout our game.
An (Excel Sheet)[https://docs.google.com/spreadsheets/d/1dQqFI7IdrA2Wo5TLjiHZfgli4moYHDZg/edit?usp=sharing&ouid=117100085502910190489&rtpof=true&sd=true] was used to keep track of each person's tasks with expected deadlines to ensure that certain features could be tested at specific times.

## Game Feel
Using information from personal playtesting and from the Project Game Showcase, many factors were changed or tweaked to enhance game feel.

*Puzzle Rooms* - Puzzle rooms initially felt very clunky with the existing spell designs, with high cast time and long cooldowns. Although those cast times and cooldowns were meant for combat, it massively slowed down the gameplay during puzzle rooms so much so that the tutorial room was very easily failable. Some suggested changes were lowering both the cast time and cooldown of the Fireball spell specifically for the puzzle room, so that the player can test multiple angles much faster, and to increase the torch duration once it had been hit by a Fireball. Another issue that came up during the Project Game Showcase was that certain UI elements were blocking torch visibility, so moving the timer to back to the center of the screen and disabling the minimap and zooming out more for puzzle rooms were suggested as solutions.

*Combat Rooms* - Initially we had long cast times, and not being able to cast while moving, however it was concluded that the casting felt very clunky, and too inhibiting. This was then changed to a lower cast time, and being able to cast while moving, making it less punishing on the player if they wanted to use spells. I had also initially known that mobs were very overpowered or strong, where some projectiles were not dodgeable or cast times and cooldowns were way too low, so many of the mob numbers were tweaked so that there is a slight challenge to fighting mobs.

*Tutorial Puzzle Room* - The tutorial room currently gives weapons and potions as rewards for completing it fast, however it didn't make sense for the tutorial room to give a weapon that would replace the starting weapon that had not been used at all. Thus, a suggestion for removing weapon drops for the Tutorial Puzzle Room was made.

*Player* - During personal playtests and duing the Project Game Showcase, the general consensus was that the player moved too slow. I had suggested to increase the player move speed to help with the flow of the game, however due to time constraints and required testing with new player speed, we were not able to implement the change.

## Cross-Platform

**Describe the platforms you targeted for your game release. For each, describe the process and unique actions taken for each platform. What obstacles did you overcome? What was easier than expected?**

## Audio - Krystal Chau

### Background Music
I make multiple attempts to create my own soundtracks using a free to use website called [BeepBox](https://www.beepbox.co/#9n31s0k0l00e03t2ma7g0fj07r1i0o432T7v1u41f0q011d08H_RJSIrsAArrrrrh0IaE0T1v1u62f0qwx10s811d08A0F0B0Q00adPfe39E4b761862863bT0v1u12f10s4q00d03w2h2E0T2v1u15f10w4qw02d03w0E0b4h400000000h4g000000014h000000004h400000000p16000000), but was unsuccessfull as what every I created was alway either too happy for a dunegon atmosphere or just [bad](https://www.beepbox.co/player/#song=9n31sbk7l00e0nt2ma7g0nj07r1i0o432T5v6ug6f0qE1132d42H07000000000000h2E11iT7v6u07f20h4134q012d7aHt7760Md9xb9pb9h1I8E0T7v6u41f0q011d08H_RJSIrsAArrrrrh0IaE0T3v6uf8f0q0x10p71d08S-IqiiiiiiiiiiiiE1bib000Pc0004Pc14001014y8N8y8y8Qgy8w4h4h5i4hkh5mtlh5llkh5l4h8h5jcx4kp23tFJvok18lkQvgzwF2Ezjn-5-ihRRRlW1BQ7dQ6m2CFMhVHjubY3nF-DUg7jQRf8VmwVkaqfQhM178AtsIhS4tkD9g5ldvEGWqqqqquiFFFFFFFFFFFFFFFF_b_W5ZdHYaGquQw860FGUILPU7ab-gHIJFGOznEbRd682ewzE8W2ewzE8W2ewzDjTAt17ghQ4tdQV2yCUYwnFEO2ewzE8W2fgerEer0) in general. After my failures, my goal was to find a soundtrack that not only fit the medieval fantasy dungeon vibe of the game, but was also 8-bit. My reasoning for wanting an 8-bit soundtrack is quite simply inspired by `Undertale` and it's outstanding soundtrack. Therefore, I found [`8-Bit Fantasy & Adventure Music`](https://www.youtube.com/watch?v=5bn3Jmvep1k&ab_channel=xDeviruchi), royalty free music by *xDeviruchi*, using [Track 10](https://www.youtube.com/watch?v=5bn3Jmvep1k&t=2082s) for combat/puzzle rooms and [`Persona 5 Among Us Remix`](https://www.youtube.com/watch?v=x0KRvAJDlRw&ab_channel=Pukirox) for the boss fight.

[`8-Bit Fantasy & Adventure Music` Track 10](https://www.youtube.com/watch?v=5bn3Jmvep1k&t=2082s) is used for the combat/puzzle rooms because I found the track mysterious which fit with the exploring nature of our game. It was also calm/quiet enough to hear all the sound effects at the same time. It is also a sound that is easy to listen to, so it does not matter how many times it loops and will not get immediately annoying.

[`Persona 5 Among Us Remix`](https://www.youtube.com/watch?v=x0KRvAJDlRw&ab_channel=Pukirox) is used for the boss fight
because of the boss itself. There is a huge style change, but the style change also happens in game so the change is not completly out of no where. According to, the creators of `Among Us`, on *Inner Sloth*'s [website](https://www.innersloth.com/fan-creation-policy/):
> 2. Educational Use/Teaching 
>
>   Teachers may integrate the Among Us IP in educational materials for small group classes and team-building activities, but we ask that you refrain from widespread distribution or sale of these materials.
> 
> NOTE: Innersloth will not issue licenses for educational use of the Among Us IP.
> 
> Examples – Permitted Educational/Teaching Use
> ●        Creating and playing an interactive game with your class based on Among Us IP
> 
> ●        Using Among Us IP in an interactive > slideshow or school-wide program
> 
> ●        Sharing the materials provided above with fellow teachers at your school (at no charge)

Therefore, we should be allowed to use this in our game.

### Special Sounds
There is/will be special voice lines from Arunpreet that fire in the special mode when using one of the spells. The reasoning for this was that, we thought it would be funny and would add to the "special-ness" of special mode.

### Sound Effects

[`Whoosh Sounds Effects HD (No Copyright)`](https://www.youtube.com/watch?v=TitDsqWGtxs&ab_channel=YouTubeSoundEffects) by *YouTube Sound Effects* - Sword whoosh for both the [Player](https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocument.md#the-player) and [Knight](https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocument.md#knight).

[`Flash Fire Ignite-Sound Effect (HD)`](https://www.youtube.com/watch?v=I2_DotYhuAA&ab_channel=FameFX) by *Fame FX* - Torch Igniting.

All other sound effects are created by me using [BeepBox](https://www.beepbox.co/#9n31s0k0l00e03t2ma7g0fj07r1i0o432T7v1u41f0q011d08H_RJSIrsAArrrrrh0IaE0T1v1u62f0qwx10s811d08A0F0B0Q00adPfe39E4b761862863bT0v1u12f10s4q00d03w2h2E0T2v1u15f10w4qw02d03w0E0b4h400000000h4g000000014h000000004h400000000p16000000) to emulate each sound as best as I could in the 8-bit style. 

I made sure to create sound for most attacks to that the Player could distingush the sound of being hit or the sound of an attack from an enemy. I also made sounds to indicate actions like spells or opening the inventory, so that actions would not just result in visual queues and so that these actions are more satisfying to the player. 

The sound implementation mostly consisted of sorting sounds into 4 categories: background music, room sounds, player/mob sounds, and special sounds. Then I would create a dedicated `SoundManager` prefab for the category, create a custom tag for it, and assign all sounds as a sound component. Depending on what sound it was adding, I would locate the tag and proceed to call the sound through playing the corresponding sound's array index in it's sound manager. This gave the me an easier way to find and add in audio, since I would not have to spend time counting `Audio Source` components to find the indexing of each sound. The old exceptions were sounds that were always played upon use, where I opted to directly add the `Audio Source` compontent to the game object itself and set it to "Play on Awake."

## Boss Concept and Design - Marc Paolo Yap
I thought of the boss concept, and boss stage design with the clear reference to Among Us. All of the create sprites and tiles, shown under here are all drawn by me using the free website [Piskel](https://www.piskelapp.com/). The Boss AI and scripting was handled by Orien Cheng.

Upon entering the vent in the final room, the player enters the Boss Room and will be guided upwards. At a certain point when going up, there is a trigger via `BossTrigger.cs` that starts the boss, prevents the player from backtracking, and changes the music.

|| Paint.exe Concept Art|
| :-------: | :-: |
| Boss Entrance & Design <br> (decided on the right one)| <img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118726655922421812/Boss_Final.png" width = 50%>|
| Boss 1st Phase <br> (only one we decided using) | <img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118726656417333368/Boss_Phase_1.png" width = 50%>|
| Boss 2nd Phase <br> (unused) | <img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118726656736108634/Boss_Phase_2.png" width = 50%>|

### Boss Assets
|Left Hand| Head | Right Hand|
|:-:|:-:|:-: |
|![BossLeft](https://piskel-imgstore-b.appspot.com/img/92611bca-0b1f-11ee-9f75-b7bc24a2ee1f.gif)|![BossHead](https://piskel-imgstore-b.appspot.com/img/28ca839c-0b1f-11ee-bf19-b7bc24a2ee1f.gif)|![BossRight](https://piskel-imgstore-b.appspot.com/img/7334a91c-0b1f-11ee-a2f6-b7bc24a2ee1f.gif)|
|Laser Beam| ![Beam](https://piskel-imgstore-b.appspot.com/img/345188dc-0b1f-11ee-abce-b7bc24a2ee1f.gif)|
|Button |![Button](https://piskel-imgstore-b.appspot.com/img/5d54c18f-0b1f-11ee-bea3-b7bc24a2ee1f.gif)|

### Boss Map

<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118726657247805560/BossRoom.PNG">

|Tiles for TileMap|
|:-:|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118722437006241862/Bridge_Wall.png">|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118722437245321257/BridgeFloor.png">|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118721229239623700/Bridge_Wall_2.png">|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118722786442092584/Bridge_Bottom_Side_L.png">|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118722787272572938/Bridge_Side_Corner_R.png">|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118722787809435698/Bridge_Side_L.png">|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118722788061102150/Bridge_Side_R_Top.png">|
|<img src = "https://cdn.discordapp.com/attachments/1116541534087684108/1118722676463239168/Boxes.png">|
  
## Gameplay Testing

**Add a link to the full results of your gameplay tests.**

**Summarize the key findings from your gameplay tests.**

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



