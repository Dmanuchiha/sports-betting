# Unity URP FPS Prototype

This repository contains a complete **first-person shooter prototype architecture** for Unity using the **Universal Render Pipeline (URP)**.

It includes:
- First-person movement (walk, sprint, crouch, jump, momentum acceleration)
- Mouse look, head bob, camera sway
- Multi-weapon loadouts with ScriptableObjects
- Firing/reload/ammo/recoil/spread/weapon switching
- Halo-style shield + health system with delayed shield regen
- Resource collection and crafting system
- HUD for health/shield/ammo/current weapon/resources
- Basic enemy AI (patrol, detect, shoot, take damage, die)

---

## 1) Folder Structure

```text
Assets/
  Scripts/
    AI/
      EnemyAI.cs
    Combat/
      HealthShieldSystem.cs
    Core/
      DamageInfo.cs
      IDamageable.cs
    Crafting/
      CraftingRecipe.cs
      CraftingSystem.cs
      ResourceInventory.cs
      ResourceNode.cs
      ResourceType.cs
    Player/
      FPSPlayerController.cs
      LoadoutSelectionCache.cs
      PlayerLoadoutSpawner.cs
    UI/
      CraftingMenuUI.cs
      HUDController.cs
      LoadoutMenuUI.cs
    Weapons/
      LoadoutData.cs
      WeaponData.cs
      WeaponInstance.cs
      WeaponManager.cs
  ScriptableObjects/
    Weapons/
    Loadouts/
    Crafting/
```

---

## 2) Step-by-step Unity Setup (URP)

1. **Create project**
   - Open Unity Hub → New Project → **3D (URP)**.
   - Recommended version: Unity 2022 LTS or newer.

2. **Import scripts**
   - Copy the repository's `Assets/Scripts` folder into your Unity project `Assets/`.

3. **Create layers and tags**
   - Tag player object as `Player`.
   - Optional: create layers like `Player`, `Enemy`, `Environment`, `Hitbox`.

4. **Create player prefab**
   - GameObject `Player` with:
     - `CharacterController`
     - `FPSPlayerController`
     - `HealthShieldSystem`
     - `ResourceInventory`
     - `CraftingSystem`
     - `WeaponManager`
     - `PlayerLoadoutSpawner`
   - Add a child camera (`Main Camera`) and assign it to `FPSPlayerController` and `WeaponManager`.
   - Add child transform `CameraHolder` and assign it to `FPSPlayerController` for head bob/sway.
   - Add child transform `WeaponSocket` and assign it to `WeaponManager`.

5. **Create weapon ScriptableObjects**
   - Right-click in `Assets/ScriptableObjects/Weapons` → Create → FPS → Weapon Data.
   - Create: AssaultRifle, SMG, SniperRifle, Shotgun.
   - Tune fields:
     - Rifle: high fire rate, moderate spread
     - SMG: very high fire rate, higher spread
     - Sniper: low fire rate, high damage, low spread
     - Shotgun: shotgun mode, pellets 8-12, short range

6. **Create weapon prefabs**
   - Create placeholder models (capsule/cube meshes) for each weapon.
   - Add `WeaponInstance` component to each prefab.
   - Set `firePoint` transform at muzzle.
   - Assign prefab reference in matching `WeaponData`.

7. **Create loadouts**
   - Right-click in `Assets/ScriptableObjects/Loadouts` → Create → FPS → Loadout Data.
   - Example loadouts:
     - Recon: SMG + Sniper
     - Assault: AssaultRifle + Shotgun
     - Heavy: AssaultRifle + Shotgun + Sniper
   - Assign one as default in `PlayerLoadoutSpawner`.

8. **Build HUD canvas**
   - Create Canvas (Screen Space - Overlay).
   - Add:
     - Health Slider
     - Shield Slider
     - Ammo TMP_Text
     - Current Weapon TMP_Text
     - Crafting Inventory TMP_Text
   - Add `HUDController` component and wire references.

9. **Create crafting recipes**
   - Right-click in `Assets/ScriptableObjects/Crafting` → Create → FPS → Crafting Recipe.
   - Create four recipes:
     - Shield Pack
     - Ammo Pack
     - Grenade
     - Armor Upgrade
   - Set costs using Metal / Electronics / Energy Cells.

10. **Create crafting UI panel**
    - Add panel with buttons for each recipe.
    - Add `CraftingMenuUI` script.
    - Hook each button `OnClick` to `CraftByIndex(index)`.
    - Toggle menu with `Tab`.

11. **Create resource pickups**
    - Place objects around map.
    - Add trigger collider + `ResourceNode`.
    - Configure resource type and amount.
    - Player collects with `E`.

12. **Create enemy prefab**
    - Capsule + `NavMeshAgent` + `HealthShieldSystem` + `EnemyAI`.
    - Add child transform `Eyes`.
    - Configure patrol point transforms in scene.
    - Ensure `visibilityMask` includes world + player layers.

13. **Bake NavMesh**
    - Window → AI → Navigation.
    - Set floor/ground static and bake.

14. **URP visuals pass**
    - Use URP Lit materials for walls/ground/props.
    - Add a Directional Light (soft shadows).
    - Enable post-processing via URP Volume:
      - Bloom (subtle)
      - Color Adjustments
      - Ambient Occlusion
      - Vignette (very light)

15. **Create simple level layout**
    - Blockout with ProBuilder or cubes:
      - Arena with cover
      - Elevated sniper line
      - Resource placement zones
      - Enemy patrol routes

---

## 3) How to Add Assets

- **Weapon models**: Replace placeholder meshes in weapon prefabs; keep `WeaponInstance` and `firePoint`.
- **Animations**: Add animator to weapons for fire/reload and trigger from `WeaponInstance` methods.
- **Audio**: Add AudioSource per weapon; play fire/reload clips on actions.
- **VFX**: Add muzzle flashes and hit particles from raycast hit point.
- **Environment assets**: Import modular sci-fi or military packs; keep colliders and bake NavMesh after placement.

---

## 4) Controls

- Move: `WASD`
- Look: Mouse
- Fire: Left Click
- Reload: `R`
- Sprint: `Left Shift`
- Crouch: `Left Ctrl`
- Jump: `Space`
- Weapon switch: `1/2/3` or Mouse Wheel
- Interact (pickup): `E`
- Crafting menu: `Tab`

---

## 5) Build and Run

1. File → Build Settings.
2. Add current scene to build.
3. Select platform (PC/Mac/Linux Standalone).
4. Click Build And Run.

For quick iteration, press Play in editor.

---

## 6) Prototype Notes / Extensibility

- All weapon tuning is data-driven through `WeaponData` ScriptableObjects.
- Loadouts are fully modular via `LoadoutData` ScriptableObjects.
- Crafting outputs can be expanded by extending `CraftableType` and adding logic in `CraftingSystem`.
- Enemy AI is intentionally basic and can be upgraded with behavior trees, cover logic, and squads.
