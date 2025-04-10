
---

## How It Works

1. Unity sends game state data via **socket (TCP, port 5005)**.
2. `BeeTrapReceiver` receives and saves each JSON object to `data_logs/`.
3. `BeeTrapAgent` scans that folder, analyzes unprocessed files, and sends them to GPT via OpenAI API.
4. Each file is moved to `processed_logs/` after analysis.
5. Only hints (if applicable) are printed to the terminal.

---

## Setup Instructions

### 1. Prerequisites

- [.NET SDK 6.0+](https://dotnet.microsoft.com/en-us/download)
- Unity app running on a tablet (must be on the **same Wi-Fi network**)
- OpenAI API key

---

### 2. Build & Run

#### 🟩 Terminal 1 – Start the receiver

cd BeeTrapReceiver
dotnet run

#### 🟩 Terminal 2 – Start the AI agent


cd BeeTrapAgent
dotnet run
