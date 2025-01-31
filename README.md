# **Sweat Bot 1.0.0.77 Alpha**

Sweat Bot is a simple, lightweight and open Twitch bot designed to control and interact with your computer and games through Twitch chat. It’s heavily inspired by Instructbot, but it’s always free and easy to set up and requires no other connections besides Twitch and tarkov.dev. Originally designed for *Escape from Tarkov*, it now includes a **Custom Command Builder**, allowing it to work with any game.

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
- Interact with your PC/games via Twitch chat.
- Quick and easy built in commands.
- Configure custom commands for any game using the new **Custom Command Builder**.
- Works locally, requiring connections only to Twitch and tarkov.dev API.
- Heavily inspired by *Instructbot*, but forever free.
  
---

## **How it Works**

Whenever a user cheers bits in your chat, the bot will track the total and store it. Users can then use `!` commands to trigger specific actions, removing bits from their balance as they go. The bot sends virtual keypresses and mouse events directly to your PC.

> **Important**: This bot was written with the help of ChatGPT, but the majority of the code is my own. I use AI to simplify repetitive tasks and identify bugs and to help speed up my learning! The source code is sloppy but I'm simplifying everything as I go on.

---

## **Current Features (WIP)**

- **Quick Setup**: Just check a box and configure the cost/inputs for commands.
- **Trader Reset Timer**: Tracks when items are restocked for hardcore Tarkov players.
- **Twitch Integration**: Twitch chat commands trigger keypresses and mouse events.
- **Bit Integration**: Users cheer bits and spend them on in-game actions. You can make it free even!
- **Custom Command Builder**: Easily create, price, and customize commands with a simple UI.

---

## **Screenshots**

![Screenshot 2024-12-20 112414](https://github.com/user-attachments/assets/4a140306-8289-4efc-b762-6a4fbe625289)
![Screenshot 2024-12-20 112423](https://github.com/user-attachments/assets/03073685-c9b4-499e-8b2b-ba1b6d14fe20)
![Screenshot 2024-12-20 112852](https://github.com/user-attachments/assets/acccdb00-419e-4cae-92f4-5962f221f846)



---

## **Upcoming/Potential Features**
*(Not guaranteed, but under consideration)*

- Profile toggle for quick switching.
- Additional commands.
- Support for Linux/Mac by switching to another UI framework.
- Open to suggestions!

---

## **Upcoming Custom Command Functions**
*(Not guaranteed, but under consideration)*

- OpenFile
- Maybe ShutdownPc

---
## **Download**
#### [**Download Latest Release**](https://github.com/sprollucy/Tarkov-Twitch-Bot-Working/releases/tag/1.0.0.77)

---

## **Getting Started with Sweat Bot**

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


---

## **Mod/Broadcaster Commands**

- `!help` – List available commands
- `!addbits @user <amount>` – Add bits to a user
- `!refund @user` – Refund the last command a user used
- `!sbadd <commandname> <methods>` - Add a command through chat
- `!sbremove <commandname>` - Remove a command through chat

---

## **How to Update the Bot**

Typically, you only need to replace the `Sweat Bot.exe` file when updating. Custom commands and prices are saved in the **Data** folder, which won’t be overwritten unless specified.

### **Transferring Settings After an Update**

1. Run the new version once.  
2. Navigate to `C:\Users\User\AppData\Local\Sweat_Bot`.  
3. Copy the `user.config` file from the current version’s folder.  
4. Paste it into the new version’s folder, overwriting the existing `user.config`.

---

## **Adding Custom Sounds**

To add your own sounds:

- Ensure the sound files are in `.wav` format.
- Place them in the **Sound Clips** folder.
- Access them in-game with the `!playsound` command or use the Command Builder to integrate sounds into commands

---

## **Support**

Feel free to report issues via GitHub or contact me on Discord: *sprollucy*.  
If you enjoy the project and want to support my work, consider donating via [PayPal](https://www.paypal.com/donate/?business=FK2ZHM73QW3FA).

---

## **Latest Changelog**
1.0.0.76-alpha
- Spelling errors
- Added !rembit. Use '!rembit @user amount' to remove bits from a user in case of a error
- Added file for loading saved toggles to prevent them resetting on update
- Added blerp integration. If a user sends something with blerp and the message shows up in chat, it should add the bits (testing)

1.0.0.75-alpha
- Added screen pixelation mode. In testing and may be very buggy
- Spelling fixes
- Added in a way for user to directly use a command without having to cheer bits before running a command. Using ! before a cheer will look for a command with that cost, and if it matches that cost it will run the command. If the amount doesn't match a command, than it is added to the users bits stored in the bot 
- Fixed bug with !cheer that would cause a over payment
- Fixed bug with !cheer failing to store bits a user gave for overpayment

1.0.0.75-alpha
- Added expandable menu to show normal chat commands
- Made it so if you have more spent this session than last session, the number will turn green. If it is less than last session it will turn red
- Bug fixes
- Added way to directly call commands by cheering bits with ! in front. If it matches the cost of a command it will run it, if not it will add to the user bits

1.0.0.74-alpha
- Trader menu adjustments
- Added a way to add commands in chat. Using !sbadd you can add commands as long as its a valid format. Example !sbadd chatcom 100 kp=1 kh=d(100)
- Added a way to remove commands from chat using !sbremove. Be careful with this
- Added moderator toggles for these 2 new commands as well
- Added gambling command, allowing users a chance to make more bits without spending. Use !sbgamble {amount}

