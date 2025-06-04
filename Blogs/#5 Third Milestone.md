# Aesthetics ✓ 
Simple visuals using #9502f5 as the primary color for most of the GUI. We wanted UI component shapes to be something similar to PS1/PS2 racing games that had sharp edges/corners and a futuristic cyberpunk aesthetic with neon lights.

## Skybox
Star shader material from a tutorial is used as a skybox to make it look like the player is in space.

## Object Texture
Shader graphs to make materials that are used on the obstacles in the levels and moving surface textures.

# VIA arcade ✓
The game is functional and is playable on VIAs arcade machine. Each input action has been mapped to corresponding keybindings on a gamepad as well as keyboard.

# Power ups ✓
Power up system has been added to the game allowing users to pick up 2 different power ups currently implemented. Power ups can be activated through a custom action in Unity’s new input system.
## Speed boost
Applies an instant boost to the player’s current moving direction.
## Jump boost
Applies an upwards boost to the player allowing them to jump higher.

# Deathblocks ✓
We modified the dead plane script to also work on collision (instead of only as a trigger) so it could be used with obstacles. The script now does not reset the scene itself, it calls the player death effect and after a second or two it calls the levelmanager to reset. 

# Medal System ✓
Each level is represented by scriptable objects, each of which has medal thresholds that can be defined. These values determine which medal to view and allows for easy editing and dynamic change.

# Updated Jumping mechanics ✓
Updated player jumping to use the surface normal they are touching so jumping mechanics feels natural on slanted surfaces. There’s also coyote time, so players can jump a bit after they have left the ground. This is important since our game is physics based, so sometimes the player bounces which would make it annoying if the player had to touch the ground to jump. To achieve this with the new jumping, we store the normal vector in a variable every time the player touches a surface.

# Add Trails and accessories to the skin selection ✓
Trails and accessories have been added to the game. They are represented using scriptable objects and contain a prefab of the trail/accessory which are positioned such that they appear correctly on the player when directly set as a child. They can be applied to the player through the customization menu like the marble skin.

# Final levels ✓
After we added power ups we needed some levels that used them. 

We made 2 levels:
- Player boosts their speed with the speed boost to jump over large gaps. 
- Player ascends a wall with the jump boost.

The speed boost level used our “sand” tag, but it works unintuitively, and if we had more time we would change it to work better.

The jump level uses a lot of walls. To make it feel better to play we didn’t want the player to be able to jump off of them, which the new jumping mechanic allowed. So to avoid that we separated the “walls” and “floor”, so that we could assign the ground tag to the “floor” but not have it on the “walls”. Now the player can jump right next to the wall without being pushed away.

# + Persistence ✓
Persistence has been added to save the player’s marble customization, as well as their best times for each level and settings.
