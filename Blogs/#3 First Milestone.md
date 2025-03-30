# Focus on creating menus, and general UI ✓
We planned which menus we were going to make and made a couple of wireframes for each of them:
- Main Menu
- Pause Menu
- Level Select
- Level Complete
- Customization
- Cheats
Although no functionality has been added to the menus, all of them have been made. We also realized that certain menus were missing, such as the options menu, which we indirectly planned to add to the game by including it into some of our wireframes.

# Make a couple of levels (4-6) ✓
Have learned pro builder and made some prefabs that can be used to build levels.
made 4 levels but the 4th is not finished yet. 

Creating levels takes time, it’s not hard coming up with an idea for a level since we just need to pick one mechanic or focus like “Turns, Moving-platform, half-pipe or a specific power-up” to get started. But making a full level that feels good to play and does feel too long or too short is the hard part.

While making the levels we noticed that our jump mechanics felt unresponsive on slopes. This is because how the game checked if you were allowed to jump is a raycast from the middle of the player to under them. This is fine for flat ground but for slopes the steeper the angle the higher the player's middle would be from the ground.
The jump adds vertical velocity to the player which also feels weird when jumping on a slope.

For the next milestone we would like to change the jump mechanics, so that the player can jump when in contact with a surface and push away from that surface, not just upward.
With some leniency so the player still can jump if they just left the surface.

# Basic skins (5-10 skins) X
From the planned 5-10 skins to be added, only 3 made it into the game, this being 2 marble skins, and 1 trail. We were busy focusing on the other milestone entries, and skins weren’t the most important thing to add for us, resulting in only 3 being added.

# Checkpoints X
Scrapping checkpoint for now. If we implement checkpoints we would need to make some levels bigger so it would be relevant to use a checkpoint. it already takes some time to create a level so checkpoints would only make that process take even longer.

# Timer + goal ✓
Both have been implemented but no visuals for the goal.
The timer starts when the scene starts and counts up, and then stops if the player touches the goal.
We still need to make the level complete ui appear when reaching the goal, and we need to stop the player from triggering the death plane after winning since that resets the level.

# Sand + Ice ✓
Both sand and ice has been implemented. These change the player controller parameters to simulate movement on that material.

# Updated Second milestone
Make some more levels (5-7)<br>
Some power ups (2-3)<br>
Spike/Deathblocks<br>
Gravity Shift Area<br>
Medal System<br>
Leaderboards<br>
Skin unlocks<br>
**\+ Update Jumping Mechanics**<br>
**\+ Moving platforms**<br>
**\+ Basic skins (2-7 skins)**<br>
**\+ UI functionality**<br>
**\+ Gameplay UI (Powerup slot, Timer, etc.)**
