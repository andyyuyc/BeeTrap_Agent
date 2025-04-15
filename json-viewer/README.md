
# 🐝 BeeTrap JSON Viewer Dashboard

This is a local JSON dashboard viewer designed to visualize state data output by Unity (e.g., BeeTrap) via socket into the `data_logs/` folder.

---

## ✅ Features

- 📂 Automatically reads all `.json` files from the `data_logs/` folder
- 📊 Visualizes all fields over time using line/bar charts
- 🎯 Field selector for multi-field overlay visualization
- ♻️ Auto-refresh every 0.1 seconds when new files are detected
- 🔢 Boolean fields are converted to 0/1; arrays use the last value
- 🧭 Scrollable X-axis viewport using sliders to define visible window
- 🚀 Incremental load without needing a full page refresh

---

## 📁 Project Structure

```
json-viewer/
├── data_logs/              # Folder where Unity writes JSON state files
├── public/
│   └── index.html          # Frontend dashboard UI
├── server.js               # Node.js Express backend
├── start.sh                # One-click startup script
└── README.md               # Documentation (this file)
```

---

## 🚀 Getting Started

### First-time Setup

```bash
npm install express
chmod +x start.sh
```

### Start the Server

```bash
./start.sh
```

Then open your browser at:

```
http://localhost:3000
```

---

## 📂 JSON File Format

Each `.json` file should contain a valid object with numeric, boolean, or array fields. Example:

```json
{
  "click": 4,
  "PCRadius": 1.8,
  "isBee": true,
  "UserProfile": [1.2, 3.4, 5.6]
}
```

Nested objects are currently ignored (e.g., position coordinates).

---

## 🧭 Viewport Control

- Two sliders control the X-axis range displayed
- X-axis window changes without resizing the chart
- Realtime update of visible data across selected fields

---

## 🔄 Auto Refresh

- The dashboard checks for new files every 100ms
- If new JSON files are added, they are loaded and displayed automatically
- The current viewport (X range) is preserved

---

## 🛠️ Tech Stack

- Node.js + Express
- Chart.js
- HTML / CSS / Vanilla JS

---

## 📌 Recommended

- Browser: Chrome / Edge / Firefox
- Node.js: version 14 or newer
- Runs fully offline; no internet required

---

Feel free to extend the system with export buttons, snapshot capture, remote sync, or WebSocket-based data streaming.
