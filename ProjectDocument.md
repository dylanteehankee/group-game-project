# Game Basic Information #

## Summary ##

**A paragraph-length pitch for your game.**

## Gameplay Explanation ##

**In this section, explain how the game should be played. Treat this as a manual within a game. It is encouraged to explain the button mappings and the most optimal gameplay strategy.**


**If you did work that should be factored in to your grade that does not fit easily into the proscribed roles, add it here! Please include links to resources and descriptions of game-related material that does not fit into roles here.**

# Main Roles #

Your goal is to relate the work of your role and sub-role in terms of the content of the course. Please look at the role sections below for specific instructions for each role.

Below is a template for you to highlight items of your work. These provide the evidence needed for your work to be evaluated. Try to have at least 4 such descriptions. They will be assessed on the quality of the underlying system and how they are linked to course content. 

*Short Description* - Long description of your work item that includes how it is relevant to topics discussed in class. [link to evidence in your repository](https://github.com/dr-jam/ECS189L/edit/project-description/ProjectDocumentTemplate.md)

Here is an example:  
*Procedural Terrain* - The background of the game consists of procedurally-generated terrain that is produced with Perlin noise. This terrain can be modified by the game at run-time via a call to its script methods. The intent is to allow the player to modify the terrain. This system is based on the component design pattern and the procedural content generation portions of the course. [The PCG terrain generation script](https://github.com/dr-jam/CameraControlExercise/blob/513b927e87fc686fe627bf7d4ff6ff841cf34e9f/Obscura/Assets/Scripts/TerrainGenerator.cs#L6).

You should replay any **bold text** with your relevant information. Liberally use the template when necessary and appropriate.

## Producer

**Describe the steps you took in your role as producer. Typical items include group scheduling mechanism, links to meeting notes, descriptions of team logistics problems with their resolution, project organization tools (e.g., timelines, depedency/task tracking, Gantt charts, etc.), and repository management methodology.**

## User Interface

**Describe your user interface and how it relates to gameplay. This can be done via the template.**

## Movement/Physics

**Describe the basics of movement and physics in your game. Is it the standard physics model? What did you change or modify? Did you make your movement scripts that do not use the physics system?**

## Animation and Visuals

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

#### Attack
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

## Input

**Describe the default input configuration.**

**Add an entry for each platform or input style your project supports.**

## Game Logic

**Document what game states and game data you managed and what design patterns you used to complete your task.**

# Sub-Roles

## Cross-Platform

**Describe the platforms you targeted for your game release. For each, describe the process and unique actions taken for each platform. What obstacles did you overcome? What was easier than expected?**

## Audio

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

### Sound Effects

[`Whoosh Sounds Effects HD (No Copyright)`](https://www.youtube.com/watch?v=TitDsqWGtxs&ab_channel=YouTubeSoundEffects) by *YouTube Sound Effects* - Sword whoosh for both the [Player](https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocument.md#the-player) and [Knight](https://github.com/oycheng/McDungeon/blob/MapPlayerPuzzle/ProjectDocument.md#knight).

All other sound effects are created by me using [BeepBox](https://www.beepbox.co/#9n31s0k0l00e03t2ma7g0fj07r1i0o432T7v1u41f0q011d08H_RJSIrsAArrrrrh0IaE0T1v1u62f0qwx10s811d08A0F0B0Q00adPfe39E4b761862863bT0v1u12f10s4q00d03w2h2E0T2v1u15f10w4qw02d03w0E0b4h400000000h4g000000014h000000004h400000000p16000000) to emulate each sound as best as I could in the 8-bit style. 

I made sure to create sound for most attacks to that the Player could distingush the sound of being hit or the sound of an attack from an enemy. I also made sounds to indicate opening the inventory so that opening is not just a visual queue. 

## Gameplay Testing

**Add a link to the full results of your gameplay tests.**

**Summarize the key findings from your gameplay tests.**

## Narrative Design

**Document how the narrative is present in the game via assets, gameplay systems, and gameplay.** 

## Press Kit and Trailer

**Include links to your presskit materials and trailer.**

**Describe how you showcased your work. How did you choose what to show in the trailer? Why did you choose your screenshots?**



## Game Feel

**Document what you added to and how you tweaked your game to improve its game feel.**
