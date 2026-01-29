# Space Kitsune

A Starfox-style 3D space shooter prototype made in Unity.

## Unity Version
- 6000.0.47f1

## Overview
Space Kitsune is a fast-paced, on-rails space shooter inspired by classic games like Starfox. Control your ship, dodge obstacles, and shoot down enemies as you fly through space!

## Controls
- **Move Reticle:** Arrow Keys, WASD, or Gamepad Left Stick (moves the aiming reticle; the ship follows the reticle with smoothing)
- **Shoot:** Spacebar or Gamepad A button (or system-dependent button, see below)

## Gamepad Setup
- The project supports gamepads via Unity's Input System.
- You may need to set up or remap buttons for your specific controller in the Settings.
- Common mappings:
  - **Move Reticle:** Left Stick
  - **Shoot:** A (Xbox), Cross (PlayStation), or equivalent (mapped to "Fire1" or "Attack")
- If your controller does not work out of the box, open the Input Actions asset and assign the correct buttons for your device.

## Gameplay
- Control a ship named `Player` that always moves forward in the positive Z direction.
- Move the reticle with input; the ship smoothly follows the reticle's position (Starfox-style controls).
- Shoot projectiles to destroy randomly spawning enemies.
- The camera follows the player from behind and always stays level (no tilting).
- A reticle appears in front of the ship to indicate where shots will go.

## Player Ship and Reticle Controls
- The ship moves forward automatically and is always kept on screen using the `shipScreenBounds` variable in `PlayerController.cs`.
- The reticle can be moved freely within its own bounds (`reticleBounds`), allowing for wide aiming, but the ship's position is clamped so it cannot go off screen, even when aiming at the bottom or edges.
- The ship tilts and aims toward the reticle, but its body is always visible.
- The Y-axis can be inverted using the `invertYAxis` boolean.

## Enemy Spawning
- Enemies are spawned within a world-aligned area defined by `spawnAreaTotalWidth` and `spawnAreaTotalHeight` in `GameManager.cs`.
- This ensures enemies always appear within the area the player can reach and shoot.

## Customization
- Adjust `shipScreenBounds` in `PlayerController.cs` to change how far the ship can move from the center.
- Adjust `reticleBounds` for reticle movement freedom.
- Adjust `spawnAreaTotalWidth` and `spawnAreaTotalHeight` in `GameManager.cs` to control the enemy spawn area.

## Project Structure
- `Player`: The player ship GameObject.
- `Projectile`: Prefab for the player's shots.
- `Enemy`: Prefab for enemy ships.
- `PlayerController.cs`: Handles player movement, shooting, and reticle.
- `CameraController.cs`: Makes the camera follow the player from behind.
- `GameManager.cs`: Spawns enemies at random positions in the player's field of view.
- `Enemy.cs`: Handles enemy behavior (to be implemented).

## How to Play
1. Open the project in Unity.
2. Open the `Assets/SampleScene`
3. Press Play to start the prototype.