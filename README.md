
# Bullet Tester

A program made in Godot that assists in the creation of bullet hell patterns by allowing the user to change parameters to pre-made templates in order to see the effects in performance and difficulty, and copy it to their projects.

## UI

Upon starting, the user will see the following screen:



The screen is composed of a bullet counter to the top right, an FPS counter to the bottom right, and a button at the top left.
The bullet counter keeps a count of the amount of bullets active at any moment during runtime, while the FPS counter keeps track of how many frames per second the program runs at, showing an approximation.
If the button at the top left is pressed, the pattern menu will appear, it looks as follows:



The menu is divided into two sections, the top section is where the user can change the parameters of the selected pattern. Each pattern has different parameters, some of which change in real time, such as rotation. The spinnboxes that show fractions have a maximum precision of 5 digits.
Bellow this section is the pattern selection section, in which the user can change the pattern currently being displayed by clicking on any pattern, this will not delete any changes to the parameters changed to the pattern, nor will it delete any bullets currently on screen.

Clicking on any section of the UI will keep the focus on it, it is recomended to click elsewhere after so any accidental keystrokes will not trigger any part of the UI.

## Patterns

Patterns are functions that are executed in cycles to spawn bullets based on mathematical equations.
Rather than an approach in which all bullets are previously stored in an array or an external file, this makes the building of the pattern itself faster, since the developer does not have to manually write every bullet.
This is very important in Bullet Hells, because there are too many bullets to account for. It can however, have unintended effects if the player decides to modify game variables or if an unexpected error changes them.
It was decided that for this case, these would be edge cases that can be ignored, however, if you decide to use that approach, it should not change the purpose of this tool.

Patterns follow a series of commands with variables that can be changed in the UI, all patterns share certain attributes as follows.

#### Alpha
The alpha value of all spawned bullets by this pattern.

#### Main Palette & Secondary Palette
The colors of the spawned bullets, in sets.
The "Main" Palette refers to the color of the bullets spawned every even cycle, while the "Secondary" Palette is the color of the bullet spawned every odd cycle.

All palettes are composed of two colors, which are selected at random when the bullet is spawned.
The colors of the palettes are as follow.

| Palette index | Color 1 | Color 2 |
| :------------ | :-----: | :-----: |
| 0 | `Blue` | `Green` |
| 1 | `Yellow` | `Green` |
| 2 | `Yellow` | `Red` |
| 3 | `Purple` | `Red` |
| 4 | `Black` | `White` |
| 5 | `Red` | `Blue` |

#### Timer
The amount of time in delta time that the pattern has been active.
In normal circumstances, it should be roughly equal to real time seconds.

#### Max Timer
The max value of the timer, once it reaches this value, the timer will reset and the cycle will increase.

#### Cycle
The current cycle for this pattern, patterns may behave differently depending on what cycle it's in.

#### Max Cycle
The max value for the cycle, once it reaches this value, it will reset.

#### Type
The behaviour of all bullets spawned by this pattern. Behaviours are explained in the Bullet behaviour section.

#### Ai1-3
Additional parameters for the bullet's behaviour. This is also explained in further detail in the Bullet behaviour section.

Though all patterns share these common attributes, all patterns are still unique among each other, the following will explain the patterns and what they do.

### Circle

A basic pattern in which bullets are shot from a single point outwards at equall angles from one another, rotating every frame.

| Attribute | Type | Description |
| :-------- | :--: | :---------: |
| `Position` | `Vector2` | The vector position of the point from which bullets are shot. |
| `Base Rotation` | `float` | The initial angle in radians at which bullets are shot. |
| `Rotation Per Fame` | `float` | The added angle in radians to the Base Rotation every frame. |
| `Disrotation` | `float` | The added angle in radians to the Base Rotation every time Cycle resets. |
| `Number Of Shots` | `int` | The amount of bullets shot. The angle between them will automatically change to be equal. |
| `Point Type` | `bool` | If true, Bullet sprites will change to a sprite that points towards the direction of it's movement.  |

### Multiple Points

A pattern in which bullets are shot from a single point, similar to the Circle pattern, but every cycle, the point in which they are spawned outwards and rotates it. The point goes back to it's origin when the cycles restart.
The pattern changes the Max Timer depending on what cycle it's at.

| Attribute | Type | Description |
| :-------- | :--: | :---------: |
| `Number Of Shots` | `int` | The amount of bullets shot. The angle between them will automatically change to be equal. |
| `Cooldown Timer` | `float` | The value of Max Timer to be changed between cycle resets. It will be changed when the last cycle is reached. |
| `Shoot Interval` | `float` | The value of Max Timer to be used during regular cycles. The Max Timer will be changed every frame. |
| `Bullet Speed` | `int` | The speed of all bullets shot from the pattern. |
| `Center Offset` | `float` | The distance to be added between each cycle to the point where bullets spawn relative to it's original point. |
| `Center Rotation Offset` | `float` | The angle in radians to be added between each cycle to the point where bullets spawn relative to it's original point. |

### Flower

A pattern in which bullets are shot from a single point, similar to the Circle pattern, but the bullets change their speed depending on their angle based on the sine wave.

| Attribute | Type | Description |
| :-------- | :--: | :---------: |
| `Base Rotation` | `float` | The initial angle in radians at which bullets are shot. |
| `Rotation Per Fame` | `float` | The added angle in radians to the Base Rotation every frame. |
| `Petals` | `int` | A multiplication of the sine wave function, it inceases the frequency of it, increasing the variation of the bullet's speed. |
| `Number Of Shots` | `int` | The amount of bullets shot. The angle between them will automatically change to be equal. |
| `Point Type` | `bool` | If true, Bullet sprites will change to a sprite that points towards the direction of it's movement.  |

### Line

A pattern in which bullets are shot from a single point, similar to the Circle pattern, but the point moves in a straight line from a starting point to an ending point. This pattern also has a cooldown at the end of it.

| Attribute | Type | Description |
| :-------- | :--: | :---------: |
| `Begining` | `Vector2` | The amount of bullets shot. The angle between them will automatically change to be equal. |
| `End` | `Vector2` | The value of Max Timer to be changed between cycle resets. It will be changed when the last cycle is reached. |
| `Side To Side` | `bool` | If true, the point will change Begin To End to it's opposite whenever the cycle resets. |
| `Begin To End` | `bool` | If true, the point will begin it's trajectory from Begining and move towards End, if false, it will begin from End and move towards Begining. |
| `Base Rotation` | `float` | The initial angle in radians at which bullets are shot. |
| `Rotation Per Fame` | `float` | The added angle in radians to the Base Rotation every frame. |
| `Number Of Shots` | `int` | The amount of bullets shot. The angle between them will automatically change to be equal. |
| `Interval` | `float` | The value of Max Timer to be changed between cycle resets. It will be changed when the last cycle is reached. |
| `Normal Max Timer` | `float` | The value of Max Timer to be used during regular cycles. |
| `Point Type` | `bool` | If true, Bullet sprites will change to a sprite that points towards the direction of it's movement. |

### Falling

A pattern in which bullets are shot from one or more of the screen borders.

| Attribute | Type | Description |
| :-------- | :--: | :---------: |
| `Top` | `bool` | If true, bullets will spawn from the top to the bottom. |
| `Top Range` | `float` | The percentage of the border from which bullets can spawn. Ranges from 0 to 1. |
| `Left` | `bool` | If true, bullets will spawn from the left to the right. |
| `Left Range` | `float` | The percentage of the border from which bullets can spawn. Ranges from 0 to 1. |
| `Bottom` | `bool` | If true, bullets will spawn from the bottom to the top. |
| `Bottom Range` | `float` | The percentage of the border from which bullets can spawn. Ranges from 0 to 1. |
| `Right` | `bool` | If true, bullets will spawn from the right to the left. |
| `Right Range` | `float` | The percentage of the border from which bullets can spawn. Ranges from 0 to 1. |
| `Set Position` | `bool` | If true, bullets will spawn in a position based on it's amount, making the space between each spawn point equal, if false, they will spawn at random points. 
| `Point Type` | `bool` | If true, Bullet sprites will change to a sprite that points towards the direction of it's movement. |
| `Number Of Shots` | `int` | The amount of bullets shot. The angle between them will automatically change to be equal. |
| `Speed` | `float` | The speed of all bullets shot from the pattern. |

## Bullets

The pattern is composed of bullets that follow a template for their physics, by default all bullets move in a straight line, unaltered by time or position, and all bullets, when colliding with a hitbox or going off screen, will disappear. All hitboxes are circular.
Contrary to OOP principles, bullets are not objects, rather, raw data stored in an array as to comply with DOD principles. This was a choice made as a solution to problems of performance.
Because many bullets are expected to be on screen at the same time, it can cause performance issues if every bullet is an object that needs to be initialized and later deleted, by using this approach, bullets will make full use of the RAM and improve performance drastically.
Some of the issues this presents are mainly in maintaining code, because of the nature of arrays, every bullet needs to be accessed by it's index, and every attribute of the bullet needs to be accessed by it's index as well, it is confusing to use this system.

The mouse has a hitbox of 50 by 50 that can be used to test collision, if the collision is not needed, press ctr + left click to disable the mouse hitbox as long as the mouse is held down, additionally, with right click the user can place down a static 50 by 50 hitbox, there are no limits to how many hitboxes can be placed. To delete all placed hitboxes, press ctr + right click.

### Bullet behaviour

The bullets have pre-made ais in them to simulate more complex movements that can be modified by parameters in the Pattern menu section of the ui, the following is a description of the possible behaviours a bullet can have.

| Behaviour index | Description | ai1 | ai2 | ai3 |
| :-------------- | :---------: | :-: | :-: | :-: |
| 0 | Default bullet behaviour. moves forward without changes. | `N/A` | `N/A` | `N/A` |
| 1 | Bullets are affected by gravity. | `N/A` | `N/A` | `N/A` |
| 2 | Bullets will bounce when colliding with a screen border. | The amount of times the bullet will bounce. | `N/A` | `N/A` |
| 3 | Bullets will home in to the mouse position. | The strength at which bullets will home in, from 0 to 1. | `N/A` | `N/A` |
| 4 | The bullets will oscillate their speed between a positive number and a negative number based on a sine wave function. If either ai1, ai2, or ai3 are less than or equal to 0, the bullets will have the default behaviour. | A multiplier for the max time the bullets will repeat this behaviour, leave at 1 for a single oscillation. | A multiplier for the internal bullet's timer, which dictates how fast they move through the sine function. | The bullet's top speed, it is recommended to keep it over 100. |
| 5 | The bullets will rotate based on a sine wave function, so the bullets will change the direction and strength they rotate to based on it. If ai1 is less than or equal to 0, the bullets will have the default behaviour. | A multiplier for the internal bullet's timer, which dictates how fast they move through the sine function. | The strength at which the bullets will rotate, if positive, they will start rotating clockwise, if negative, will rotate counterclockwise. | `N/A` |
| 6 | The bullets will rotate in a single direction, optionally reducing the strength of their rotation over time up to an optional minimum. | The rotation over time in radians, if positive, they will rotate clokwkwise, if negative, counterclockwise. | The reduction in rotation over time in ratians. If it's a negative number, it will add to the rotation instead, independently of the value of Ai1. | The minimum value of Ai1. Will be ignored if it's less than or equal to 0. |
