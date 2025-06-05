# Game Design Document & Milestones
Our game will build upon the “roll a ball” game, and takes inspiration from the game [Marble It Up! Ultra](https://store.steampowered.com/app/864060/Marble_It_Up_Ultra/).

## Gameplay
The player controls a ball and has to get to the level goal as fast as possible. 

The game will be momentum and physics-based, and introduce the player to various scenarios where they can utilize different game mechanics to reach their goal. 

Each level will be relatively short and should be beatable in less than 1 or 2 minutes if the player is really good.

## Features
### Timer
Each level will have a timer that counts up and stops when the player reaches the goal. Depending on their time they will earn a gold, silver or bronze medal, or no medal if they are really slow.

### Leaderboard
We want to add leaderboards for the levels and encourage the players to go for a better time, beat eachothers times or get a new personal best. When the player gets a spot on the leaderboard they can write their name for it.

### Checkpoints
To ensure the player doesn’t get too frustrated when losing and having to start over on longer levels, checkpoints can be added for certain parts of levels. 

### Level select menu
The level select menu should give the player an overview of the levels and their medals for each, hovering over a level will show the players current best time and the levels leaderboard.

### Skins
Allows the player to customize their marble, with different colors, textures, accessories &  trails. 

These can be unlocked when the player:
- Completes a certain amount of levels
- Achieves a certain amount gold medals

### Cheat Codes
To unlock skins, levels, and other forms of cheats like infinite powerup use, invincibility, etc. possibly using the Konami code, and codes hidden in levels.

If the player is using game altering cheats they cannot get leaderboard times.

### Surface Materials
- Sand - Player will slow down making it hard to maintain their speed, but can easily change directions.
- Ice - Low friction will make the player have difficulty changing directions, but can maintain their speed.
- Bouncy - Player will bounce much more

### Obstacles
- Ramp - Allows player to convert their speed into upwards movement
- Moving Platforms - Will make it hard for the player to position themselves
- Jump Pad - Will launch the player into the air
- Enemy (Uses Navmesh) - Will hunt down the player throughout the level or a specified area.
- Spikes/Deathblocks - trigger that “kills” the player (if the player is “killed” level will reset or they will return to a checkpoint)
- Gravity Shift Area - Where the players gravity is shifted
- Time reduction collectible - Lowers the timer by a set amount of time when collected.
- Required collectibles - The players will need to collect a certain amount of collectibles to unlock the level goal

### Powerups
- Speed Boost - Player gains a burst of speed in the direction they are currently moving when used.
- Super Jump - Allows player to jump higher than normal
- Sticky - Player can stick  to obstacles
- Gravity Shift - Shifts players gravity
- Double Jump - Player can now double jump
- Slow Time - Player and environment is slowed
- Size Change - Player is now smaller/larger

## Milestones
### First Milestone
- Focus on creating menus, and general UI
- Make a couple of levels (4-6)
- Basic skins (5-10 skins)
- Checkpoints
- Timer + goal
- Sand + Ice

### Second Milestone
- Make some more levels (4-6)
- Some power ups (3-4)
- Spike/Deathblocks
- Gravity Shift Area
- Medal System
- Leaderboards
- Skin unlocks

### Third Milestone
- Make even more levels (4-6)
- Enemy (navmesh)
- More skins (5-10 skins)
- Cheat Codes
- Collectibles
- Bouncy Surface
- Aesthetics

# Dream Features
- Tileable map segments (for auto-generation)