<img width="1024" height="1024" alt="MysticMiner_Logo" src="https://github.com/user-attachments/assets/8cb57bf5-81b4-40ec-b058-349a7a412a7c" />
# MysticMiner

> 🌍 **A mobile RPG where you mine resources and fight monsters in an endless world**  
> ⚒️ *Inspired by Hypixel Skyblock & “뒷산에서 보석캐기”*  
> 🚧 **Currently in development (WIP)**

---

## 🎮 Overview
MysticMiner is a **Unity-based top-down RPG**,  
where players explore infinite chunk-based terrain to **mine, battle monsters, and grow their village**.

- **Platform**: Mobile (Android / iOS planned)  
- **Engine**: Unity  
- **Genre**: Top-down RPG  
- **Release**: Free on Google Play Store (planned)  

---

## 🛠 Features
- **Chunk-based Terrain Generation**  
  - Infinite expansion system powered by `ChunkTerrainManager`  
- **Mining System**  
  - `PickaxeManager` detects and mines ores within range  
- **Item System**  
  - Abstract `Item` class + interfaces like `IWeapon`, `IArtifact`  
  - Automatic resource loading for item sprites  
- **Inventory & Equipment Management**  
  - `Dictionary<Type, List<Item>>` structure for organized storage  
- **UI**  
  - Dynamic slot creation with `InventoryUI` and `GridLayoutGroup`  

---

## 📅 Progress
- ✅ Basic terrain generation & chunk management implemented  
- ✅ Mining logic foundation complete  
- 🚧 Expanding item/inventory system  
- 🚧 Village growth features under planning  
- 🚧 Combat system upcoming  

---

## 🤝 Contribution
Currently developed as a **personal project**,  
but may open for external contributions in the future.  

---

## ⚠️ Status
This project is **still under development (WIP)**.  
Expect instability and incomplete features.  

---

## 📜 License
MIT License (planned)
