# SyncraEngine

A social VR platform and engine focused on collaborative content creation.

Check out the Discord server to learn more: https://discord.gg/yxMagwQx9A

## What's this project about?

The goal here is to do the whole "social VR sandbox" idea properly. A lot of sandbox games either don't support VR, have limited editing/scripting tools, don't handle UGC safely/performantly, and are generally just poorly implemented. Syncra's architecture and feature set is focused on modularity, extensibility, safety, performance, and simplicity. The main objectives here are the following:

- Full process/world sandboxing
- Scriptable components/systems
- Entity-component-system architecture
- Built-in git version control for content
- Engine-level permissions for everything
- Aggressive multithreading
- Pipelined/asynchronous rendering
- Official and user package management
- Content discovery
- Scriptable renderer
- And a bunch of other stuff that couldn't possibly be fully enumerated here

## Why is the repo mostly empty?

There's been a lot of design changes and architectural revisions. There's a bunch of stuff in the commit history, but right now things are being reworked.

The architectural draft should be fairly stable at this point, so the goal at this time is to rework the core ideas using Rust.

Lastly, this project is a recent idea of mine. It's only been around for a few weeks, so expect things to be volatile for now.
