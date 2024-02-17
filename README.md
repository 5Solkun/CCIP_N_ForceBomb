# README
* This is an add-on to Saccflight written in U#
* It's an MIT license, But i'd appreciate it if you could leave a credit when using it :D

## Features

### Ballistic calculator
BallisticCalculator.cs

* A script that calculates and displays the ballistic curve and the impact point
* Based on the gravitational acceleration and projected speed in each frame, predict as much as "Integer: Number Of Prediction Points" at intervals of "Float: interval".
* Projectiles can be predicted after launch for the time of "Number Of Predict Points" * "interval"
* If the value "interval" is too high, the error may increase
* The higher the number of "Number Of Predict Points", the higher the load
* "muzzleVelocity": You can also predict armaments such as rockets and artillery by entering the same value as "LaunchSpeed" in "SAV_BombController."
* #### Calculating drag is not functioning properly, only projectiles with zero air resistance can be calculated, So Please set "AirPhysics Strength" of "SAV_BombController" to 0

### Physical Force Bomb
SAV_BombController_Force.cs
* SAV_BombController now can push out players and RigidBody around them when a projectile explodes
### Prefab
* It includes SF-1 prefab with these features.


# Reference
## SaccFlightAndVehicles
https://github.com/Sacchan-VRC/SaccFlightAndVehicles
## Demo Videos
* https://twitter.com/5So2_/status/1715661207389261922
* https://twitter.com/5So2_/status/1715042884595151188
