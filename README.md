# LiDAR-game
Create point clouds to navigate cryptic spaces! 

We can create and render point clouds using a compute shader, 
shoot out rays from the camera, both randomly along a disc and a wide area scan.
So far it's efficient, functional and fun! 

The most interesting tidbits:
See [this](https://github.com/Wessl/LiDAR-game/blob/main/Assets/Scripts/LiDARShooter.cs) for how the "LiDAR" points are created relative to the player's view, featuring one regular circular scan and one "super scan" that covers the screen.  See [this](https://github.com/Wessl/LiDAR-game/blob/main/Assets/Scripts/DrawCircles.cs) file for how some fancy buffers are created to render points efficiently in space using the GPU. 


### play the game here!
https://wesslo.itch.io/lidar-from-afar

![alt text](https://img.itch.zone/aW1hZ2UvMTY5OTQ1My8xMDA1MDM5Ny5wbmc=/original/vV65HW.png)

