# Unity URP FPS Prototype (Playable Vertical Slice)

This repository contains a complete first-person shooter prototype architecture for Unity + URP, including movement, loadouts, weapons, shield/health, crafting, UI, and basic enemy AI.

## 1) Recommended Folder Structure

```text
UnityFPSPrototype/
  Assets/
    Scenes/
      Main.unity
    Prefabs/
      Player/
      Weapons/
      Enemies/
      Pickups/
      UI/
    Materials/
    Models/
    Audio/
    ScriptableObjects/
      Weapons/
      Loadouts/
    Scripts/
      Core/
        GameManager.cs
      Player/
        PlayerMovement.cs
        MouseLook.cs
      Combat/
        HealthShieldSystem.cs
      Weapons/
        IDamageable.cs
        WeaponInstance.cs
        WeaponController.cs
      AI/
        EnemyAI.cs
        EnemyHealth.cs
      Crafting/
        CraftingSystem.cs
        ResourcePickup.cs
      UI/
        HUDController.cs
        CraftingMenuUI.cs
        LoadoutMenuController.cs
```

---

## 2) Step-by-Step Unity Setup

## Prerequisites
1. Unity 2022.3 LTS (or newer LTS)
2. Universal Render Pipeline package installed
3. TextMeshPro imported

## Create project + URP
1. Create a new **3D (URP)** project in Unity Hub.
2. Copy the `UnityFPSPrototype/Assets/Scripts` folder from this repo into your Unity project's `Assets/Scripts`.
3. Create folders in Unity that match the structure above.

## Input setup
Use Unity's default Input Manager mappings:
- `Horizontal`, `Vertical`
- `Mouse X`, `Mouse Y`
- `Jump`
- `Fire1`

## Scene setup (Main.unity)
1. Create `Player` object with:
   - `CharacterController`
   - `PlayerMovement`
   - `MouseLook`
   - `HealthShieldSystem`
   - `WeaponController`
   - `CraftingSystem`
   - Tag: `Player`
2. Player hierarchy:
   - `Player`
     - `CameraRoot` (for head bob/sway)
       - `PitchPivot` (for vertical look)
         - `Main Camera`
       - `WeaponHolder` (placeholder weapon meshes)
3. Hook references:
   - `MouseLook.playerBody` = Player
   - `MouseLook.pitchPivot` = PitchPivot
   - `PlayerMovement.cameraRoot` = CameraRoot
   - `WeaponController.fpsCamera` = Main Camera
   - `WeaponController.weaponHolder` = WeaponHolder
   - `CraftingSystem.weaponController` + `healthShieldSystem`

## Weapon ScriptableObjects
Create 4 WeaponData assets in `Assets/ScriptableObjects/Weapons`:

- `AR_WeaponData`
  - automatic = true
  - damage = 22
  - fireRate = 11
  - magazine = 30
  - spread = 0.012
- `SMG_WeaponData`
  - automatic = true
  - damage = 15
  - fireRate = 14
  - magazine = 36
  - spread = 0.02
- `Sniper_WeaponData`
  - automatic = false
  - damage = 85
  - fireRate = 1.1
  - magazine = 5
  - spread = 0.002
- `Shotgun_WeaponData`
  - automatic = false
  - damage = 11
  - pellets = 8
  - fireRate = 0.9
  - magazine = 8
  - spread = 0.06

## Loadout ScriptableObjects
Create at least 3 `LoadoutData` assets in `Assets/ScriptableObjects/Loadouts`, for example:
- Assault: AR + SMG + Shotgun
- Recon: Sniper + SMG + AR
- Breacher: Shotgun + AR + SMG

Assign your default loadout to `WeaponController.startingLoadout`.

## HUD + Menus
1. Add a Canvas with:
   - Health bar Slider
   - Shield bar Slider
   - Texts: Weapon Name, Ammo, Metal, Electronics, Energy Cells
2. Add `HUDController` on Canvas and wire references.
3. Add crafting panel UI + buttons bound to:
   - `CraftingMenuUI.CraftShield`
   - `CraftingMenuUI.CraftAmmo`
   - `CraftingMenuUI.CraftGrenade`
   - `CraftingMenuUI.CraftArmor`
4. Add a loadout panel with TMP Dropdown + Spawn button.
   - Attach `LoadoutMenuController`
   - Set loadout list and button `OnClick -> SpawnWithSelectedLoadout`

## Enemies + NavMesh
1. Create enemy prefab with:
   - `NavMeshAgent`
   - `EnemyAI`
   - `EnemyHealth`
2. Add patrol points (empty transforms) and assign to `EnemyAI.patrolPoints`.
3. Bake NavMesh (`Window > AI > Navigation`).

## Resources and crafting pickups
1. Create pickup prefabs with trigger colliders.
2. Add `ResourcePickup` and set resource type:
   - Metal
   - Electronics
   - EnergyCells
3. Place around map.

## Environment + URP quality pass
1. Use URP Lit materials on terrain, walls, props.
2. Add:
   - Directional Light (main sun)
   - Reflection Probe
   - Post-processing Volume (Bloom + Color Adjustments + Vignette)
3. Build a simple arena with cover blocks, catwalks, crates, and doorways.

---

## 3) Full C# Script List

All scripts are included under `UnityFPSPrototype/Assets/Scripts/...`

- Core: `GameManager.cs`
- Player: `PlayerMovement.cs`, `MouseLook.cs`
- Combat: `HealthShieldSystem.cs`
- Weapons: `IDamageable.cs`, `WeaponInstance.cs`, `WeaponController.cs`
- AI: `EnemyAI.cs`, `EnemyHealth.cs`
- Crafting: `CraftingSystem.cs`, `ResourcePickup.cs`
- UI: `HUDController.cs`, `CraftingMenuUI.cs`, `LoadoutMenuController.cs`
- Data/ScriptableObjects: `WeaponData.cs`, `LoadoutData.cs`

---

## 4) How to Run the Game

1. Open the Unity project.
2. Open `Main.unity` scene.
3. Ensure all references in inspector are set.
4. Press Play.
5. Controls:
   - Move: WASD
   - Look: Mouse
   - Jump: Space
   - Sprint: Left Shift
   - Crouch toggle: Left Ctrl
   - Fire: Left Mouse
   - Reload: R
   - Switch weapons: Mouse Wheel / 1-3
   - Crafting menu: Tab

---

## Build Instructions
1. `File > Build Settings > Add Open Scenes`
2. Select platform (PC/Mac/Linux)
3. `Player Settings`:
   - Color Space: Linear
   - Scripting Backend: IL2CPP (recommended)
   - API Compatibility: .NET Standard 2.1
4. Click `Build` or `Build And Run`

---

## How to Add Art/Audio Assets

1. Weapon models:
   - Put meshes in `Assets/Models/Weapons`
   - Parent under `WeaponHolder`
2. Enemy models:
   - Add rigged humanoids or robots, keep collider + NavMeshAgent.
3. Sounds:
   - Assign clips in each `WeaponData` asset.
4. Effects:
   - Add muzzle flash prefab and impact decal prefab.
5. Performance tips:
   - Use baked lighting for static geometry.
   - Use LODGroups for medium/large props.
   - Keep enemy AI update rates conservative.

This provides a modular, extendable FPS base ready for art/content replacement and further gameplay iteration.
