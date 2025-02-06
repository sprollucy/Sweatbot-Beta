# **Sweat Bot 1.0.0.82 Alpha**

Sweat Bot is a simple, lightweight and open Twitch bot designed to control and interact with your computer and games through Twitch chat. It’s heavily inspired by Instructbot, but it’s always free and easy to set up and requires no other connections besides Twitch and tarkov.dev. Originally designed for *Escape from Tarkov*, it now includes a **Custom Command Builder**, allowing it to work with any game.
Feel free to join the [*SweatBot Discord*](https://discord.gg/k4uH6WZTS4) to keep up with the latest news

## Table of Contents
- [Features](#features)
- [How it Works](#how-it-works)
- [Setup Guide](#getting-started-with-sweat-bot)
- [Commands](#viewer-commands)
- [Screenshots](#screenshots)
- [Download](#download)
- [Upcoming Features](#upcomingpotential-features)
- [Support](#support)
- [Changelog](#latest-changelog)

### **Features**
- **Interact with your PC/games via Twitch chat**: Move your mouse, click buttons, hold down keys, fire your gun while you're off-screen, make your screen go wacky with pixelation, trigger funky sound effects, and so much more!
- **No need to rely on a website and a bunch of sign ups**: Works locally, requiring connections only to Twitch and tarkov.dev API(Soon to be disconnectable for 1 connection out).
- **Quick Setup**: Once you get your twitch bot token set up and connected, Just enable a command or make your own and hit connect!
- **Trader Reset Timer**: Tracks when items are restocked for hardcore Tarkov players.
- **Twitch Integration**: Twitch chat commands trigger keypresses and mouse events.
- **Bit Integration**: Users cheer bits and spend them on in-game actions. Users either !<cheeramount> with a command or spend saved up currency you can name from bits they have cheered while Sweatbot is running. You can make it free even!
- **Subscriber only mode**: Turn on Sub Only mode to reward loyal followers or reward people for subscribing with a little bonus.
- **Bonuses for Chatters**: Wanna reward people for joining or following? Just set the amount and give them some freebies to get them started!
- **Custom Command Builder**: Easily create, price, and completely customize commands with a simple UI. Comes with many premade commands you can activiate with 1 click. ***Now with a per game profile system!***
- **Easy to Use Refund System**: Quickly manage refunds from the main screen without having to type !refund in chat.
  
---

## **How it Works**

Whenever a user cheers bits in your chat, the bot will track the total and store it. Users can then use `!` commands to trigger specific actions, removing bits from their balance as they go. The bot sends virtual keypresses and mouse events directly to your PC.

> **Important**: This bot was written with the help of ChatGPT, but the majority of the code is my own. I use AI to simplify repetitive tasks and identify bugs and to help speed up my learning! The source code is sloppy but I'm simplifying everything as I go on.

---

## **Screenshots**

![Screenshot 2025-02-03 053851](https://github.com/user-attachments/assets/ee6942d4-fda1-4809-87af-54a80da29135)
![Screenshot 2025-02-03 053610](https://github.com/user-attachments/assets/ac37fa28-db43-4a24-bbf5-019ef416393c)
![Screenshot 2025-02-03 053642](https://github.com/user-attachments/assets/d351de4d-c3b2-4f54-9311-5a29b4f37386)
![Screenshot 2025-02-03 053629](https://github.com/user-attachments/assets/d5fc8ba1-e3da-4f7d-8501-9d7a3d53ea2d)


---

## **Upcoming/Potential Features**
*(Not guaranteed, but under consideration)*

- Support for Linux/Mac by switching to another UI framework.
- Expanded Moderation controls.
- Disable banking(stores user currency for later use)
- Ban user from using bot
- Open to suggestions!

---

## **Upcoming Custom Command Functions**
*(Not guaranteed, but under consideration)*

- Controller support
- Open Photo
- Maybe ShutdownPc

---
## **Download**
#### [**Download Latest Release**](https://github.com/sprollucy/Tarkov-Twitch-Bot-Working/releases/tag/1.0.0.81)

---

## **Getting Started with Sweat Bot**
First download .Net 8 runtime from Microsoft
https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.12-windows-x64-installer

Watch a basic setup tutorial [here](https://youtu.be/_G8fQeHlMOA).

### **Step-by-Step Setup**

1. **Create a Bot Account**  
   Go to [Twitch](https://www.twitch.tv) and create a bot account or use your main Twitch account.

2. **Mod the Bot**  
   In your Twitch chat, type `/mod <bot_name>` to mod your bot account.

3. **Generate an Access Token**  
   Use [Twitch Token Generator](https://twitchtokengenerator.com) or another tool to generate an Access Token for your bot. Paste the token into the **Access Token** field in the app, then click **Save**.

   > **Warning**: Never share your Access Token!

4. **Launch the Bot**  
   Run the `Sweat Bot.exe` file.  
   > **Note**: If you encounter a security warning, click **More Info** and run it anyway. Windows Security may quarintine the program as it has not been submitted to Microsoft to be analyzed as major code rewrites happen regularly. You will have to open Windows Security and allow 'Sweat Bot.exe'.

5. **Configure Settings**  
   - Click the settings wheel (bottom left) and enter the channel token.
   - Enter the Twitch channel where you want the bot to join, then click **Save**.

6. **Set Up Commands**  
   - Click Command Builder Menu button to configure your bot’s commands.
   - Set the command costs and enable your desired commands, then click **Save** and **Restart**.

---

## **Viewer Commands**

- `!help` – List available commands
- `!how2use` – Instructions on how to use the bot
- `!about` – Information about the bot
- `!mybits` – Check how many bits a user has stored. 
- `!bitcost` – List available commands and their cost
- `!sbgamble <amount> ` - Gamble your bits in hopes to win more
- `!sweatbot` - Turn Sweat Bot on and off
- `!sendkey <key> ` - Send any select key input to the streamers pc
- `!<cheer>` - Users put a '!' in front of their cheer amount to activate a command with the same cost

---

## **Mod/Broadcaster Commands**

- `!help` – List available commands
- `!addbits @user <amount>` – Add bits to a user
- `!refund @user` – Refund the last command a user used
- `!sbadd <commandname> <methods>` - Add a command through chat
- `!sbremove <commandname>` - Remove a command through chat

---

## **How to Update the Bot**

Typically, you only need to replace the `Sweat Bot.exe` file when updating. Custom commands and prices are saved in the **Data** folder, which won’t be overwritten unless specified. I plan on making a installer later!

---

## **Adding Custom Sounds**

To add your own sounds:

- Ensure the sound files are in `.wav` format.
- Place them in the **Sound Clips** folder.
- Access them in-game with the `!playsound` command or use the Command Builder to integrate sounds into commands

---

## **Support**

Feel free to report issues via [*SweatBot Discord*](https://discord.gg/k4uH6WZTS4)  
If you enjoy the project and want to support my work, consider donating via [PayPal](https://www.paypal.com/donate/?business=FK2ZHM73QW3FA).

---

## **Latest Changelog**
1.0.0.82-alpha
- Hotfix for bit multiplier stacking issue
- Reduced exe size from 168mb down to 5.6mb by tweaking build settings

1.0.0.81-alpha
- Fixed In Raid Check from running multiple instances
- Fixed Settings saving and loading. All changed settings should carry over from version to version 
- Fixed blerp checkbox from not staying toggled
- Changed blerp integration system to use regex magic to find the user. This should fix it not working
- Added Test Command button to the Command Builder Menu
- Added rate limit controls. This Adds a delay between processing messages, which can help command accuracy if users are spamming commands
- Added file locking to prevent errors if users are spamming commands to rapidly 
- Added debug mode to only show commands that are processing
- Adjusted bit multiplier and sub multiplier to be percentage based. If enabled when a user cheers bits they can get a bonus. example 100 cheered bits with a 20% multiplier = 20 bonus bits
- Adjusted bit multiplier to be percentage based
- Added easier to use Moderator Controls Whitelist

1.0.0.80-alpha
- Fixed !sweatbot from showing incorrect state
- Added condition for !sweatbot and !sbgamble that if subs only is checked in the control menu, only subs can use the command
- Added Sub only mode for bot commands
- Adjusted Pixelate overlay to prevent a 'after image' after being ran more than once
- Added custom names for the bots currency
- Resize custom name
- Changed file watcher so it only runs while the refund tab is open
- Menu adjustments 
- Removed redundant property saves
- Added Game profile switching in the Command Builder menu
- Corrected !sbadd and !sbremove to write into the current profile
- Fixed bug where if console tab is clicked while on that tab causing a crash
- Added integrity check on start up that verifies necessary files and folders are in the directory
- Removed trader services from start. Now only runs if Tarkov Traders are enabled
- Increased readability of Refund Menu
- Added rudimentary In raid check for tarkov(buggy) that will pause and unpause commands in and out of raid

1.0.0.79-alpha
- Fixed refund menu taking awhile to refresh after ever refund
- Potentially fixed issue when multiple users send a message at once, causing the refund menu to crash the program
- Fixed refund menu not creating a new bitlog text and loading the previous days data
