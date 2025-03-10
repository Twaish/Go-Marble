# Tutorial
The tutorial had us make a controllable ball and a small stage that consisted of a square with some borders. The player would move a ball around this map and pick up collectibles to score points.

# Ball Movement
The ball movement uses unity’s physics engine and is controlled by the PlayerController.cs script. 

Pressing any movement keys such as WASD or arrow keys, translates the direction into a force vector, which will be applied to the ball’s rigidbody every time the FixedUpdate() method is called. 

To make sure that the correct movement is applied regardless of framerate, Time.deltaTime or Time.fixedDeltaTime has been applied to relevant parts of the code, and the movement vector is normalized to ensure that the player moves the same distance diagonally.

Based on the conditions, this movement would get multiplied by different variables to determine how much force should be added to the rigidbody. 

Based on the magnitude of the movement vector, the camera will also gradually interpolate into a higher/lower FOV to give the effect of moving faster/slower.

Other than moving 2 dimensionally, we added a jump functionality to the ball, however you were able to move fully in the air.

This is something we didn't want, we wanted the movement to feel momentum based and to solve this we added air control restrictions to reduce but not fully remove the player's air control. 

To make sure the player couldn’t jump indefinitely, we had to determine whether the player was grounded. To do this, a ray can be casted downwards from the players position and check any collider with it’s layer set to `Ground`

# Scoring
Following the tutorial we made pickups and a scoring system. 

The pickups are a prefab of a yellow cube that has been made to spin, and will disappear and award the player a point, when the player collides with its trigger. 

When the game is running, a point-spawner object will periodically spawn pickups within the map borders.

# Extra
We worked on a first level for the game, by using cubes and stretching them to the form that was needed for the level. We also made a timer and a goal that would stop the timer.

We played around with the materials in Unity and made our ball look more metallic.

Added a trail to the ball using Unitys trail renderer.
