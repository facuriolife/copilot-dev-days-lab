🌐 [Português (BR)](README.pt_BR.md) | [Español](README.es.md)

<h1 align="center">Soc Ops</h1>

<p align="center">
   Social Bingo for in-person events, mixers, and workshops.<br>
   Meet new people, mark your board, and get 5 in a row.
</p>

<p align="center">
   <a href="https://dotnet-presentations.github.io/vscode-github-copilot-agent-lab/"><strong>Play Demo</strong></a>
   ·
   <a href="https://dotnet-presentations.github.io/vscode-github-copilot-agent-lab/docs/"><strong>Open Lab Guide</strong></a>
</p>

<p align="center">
   <img alt="Blazor WebAssembly" src="https://img.shields.io/badge/Blazor-WebAssembly-5C2D91?logo=blazor&logoColor=white">
   <img alt=".NET 10" src="https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white">
   <img alt="Language" src="https://img.shields.io/badge/Languages-EN%20%7C%20ES%20%7C%20PT_BR-0A7EA4">
</p>

---

## Why Soc Ops

Soc Ops turns classic bingo into a social icebreaker:

- Dynamic 5x5 board with randomized prompts
- One-tap gameplay optimized for mobile and desktop
- Instant bingo detection with visual feedback
- Persistent game state through browser local storage

---

## Experience Flow

1. Start a new round and get a fresh social bingo board.
2. Find people that match each square prompt.
3. Mark squares as you connect with others.
4. Complete any row, column, or diagonal to win.

---

## Lab Guide

Build and customize the project step-by-step:

| Part | Topic |
|------|-------|
| [00](https://dotnet-presentations.github.io/vscode-github-copilot-agent-lab/docs/step.html?step=00-overview) | Overview and Checklist |
| [01](https://dotnet-presentations.github.io/vscode-github-copilot-agent-lab/docs/step.html?step=01-setup) | Setup and Context Engineering |
| [02](https://dotnet-presentations.github.io/vscode-github-copilot-agent-lab/docs/step.html?step=02-design) | Design-First Frontend |
| [03](https://dotnet-presentations.github.io/vscode-github-copilot-agent-lab/docs/step.html?step=03-quiz-master) | Custom Quiz Master |
| [04](https://dotnet-presentations.github.io/vscode-github-copilot-agent-lab/docs/step.html?step=04-multi-agent) | Multi-Agent Development |

Offline version available in [workshop/](workshop/).

---

## Quick Start

### Prerequisite

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or newer

### Run locally

```bash
cd SocOps
dotnet run
```

### Build

```bash
cd SocOps
dotnet build
```

---

## Open in GitHub Codespaces

After creating your own repo from this template:

1. Open your repository on GitHub.
2. Select Code, then Codespaces, then Create codespace on main.
3. Wait for the dev container setup to finish.
4. Run:

```bash
cd SocOps
dotnet run
```

---

## Project Snapshot

- App entrypoint: [SocOps/Program.cs](SocOps/Program.cs)
- Main route: [SocOps/Pages/Home.razor](SocOps/Pages/Home.razor)
- UI components: [SocOps/Components/](SocOps/Components/)
- Domain services: [SocOps/Services/](SocOps/Services/)
- Question bank: [SocOps/Data/Questions.cs](SocOps/Data/Questions.cs)

---

## Deployment

This project deploys automatically to GitHub Pages on push to main.
