# Rythm game in basic winforms with c#

a simple rhythm game built in C# WinForms, featuring JSON-based beatmaps, scalable rendering, and real-time note spawning.

This project is a learning-focused rhythm game engine that loads beatmaps from files and renders falling notes synced to timing data.
-------

## work in progress.
Current features:

- Beatmap selection menu
- JSON beatmap loading
- Real-time note spawning
- Hit detection system
- Score system
- Fullscreen scalable gameplay
- Virtual resolution rendering
- Multi-lane input (D, F, J, K)





---------
# 📁 Project Structure

```
rythmgame/
│
├── Beatmaps/
│     test.json
│
├── Classes/
│     Beatmap.cs
│     Beatnote.cs
│     Note.cs
│     BeatmapLoader.cs
│
├── Forms/
│     Form1.cs        # Beatmap selection menu
│     GameForm.cs     # Gameplay logic
│
├── Program.cs
│
└── README.md
```
---------- 
# 🧠 How It Works

## Beatmap System

Beatmaps are stored as JSON files inside the `Beatmaps` folder.

Each note contains:

* `lane` → which column the note appears in
* `time` → when the note should hit the line (seconds)

Example:

```json
{
  "bpm": 120,
  "notes": [
    { "lane": 0, "time": 1.0 },
    { "lane": 1, "time": 2.0 },
    { "lane": 2, "time": 3.0 },
    { "lane": 3, "time": 4.0 }
  ]
}
```

Notes are spawned based on their target hit time and travel toward the hit line.

------
# 🛠 Development Goals

This project focuses on:

* Learning game architecture
* Real-time timing systems
* File-driven content
* Rendering in WinForms
* Rhythm gameplay logic

---


