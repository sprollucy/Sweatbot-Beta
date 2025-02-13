# **Sweat Bot 1.0.06 Beta**

Sweat Bot is a simple, lightweight and open Twitch bot designed to control and interact with your computer and games through Twitch chat. It’s heavily inspired by Instructbot, but it’s always free and easy to set up and requires no other connections besides Twitch and tarkov.dev. Originally designed for *Escape from Tarkov*, it now includes a **Custom Command Builder**, allowing it to work with any game.
Feel free to join the [*SweatBot Discord*](https://discord.gg/k4uH6WZTS4) to keep up with the latest news

## Table of Contents
- [Features](#features)
- [How it Works](#how-it-works)
- [*Setup Guide*](#getting-started-with-sweat-bot)
- [Commands](#viewer-commands)
- [Screenshots](#screenshots)
- [*Download*](#download)
- [Upcoming Features](#upcomingpotential-features)
- [*Support*](#support)
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
- Open to suggestions!

---

## **Upcoming Custom Command Functions**
*(Not guaranteed, but under consideration)*

- Open Photo

---
## **Download**
#### [**Download Latest Release**](https://github.com/sprollucy/Sweatbot-Beta/releases/tag/1.0.06)

First download .Net 8 runtime from Microsoft
> https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.12-windows-x64-installer

> *Important*: The files have not been verified by Microsoft and may be detected as *Trojan:Script/Wacatac.B!ml* by Windows Security and deleted when downloading. I will be resubmitting the files once in the beta stages

>### How to Allow It:  
>- Open **Windows Security** (search for it in the Start menu).  
>- Go to **Virus & threat protection**.  
>- Click **Protection history**.  
>- Find the downloaded file that was detected as *Trojan:Script/Wacatac.B!ml*.  
>- Click **Restore** if you are confident it is safe.  
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
- `!<cheer>` - Users put a '!' in front of their cheer amount to activate a command with the same cost

---

## **Mod/Broadcaster Commands**

- `!help` – List available commands
- `!addbits @user <amount>` – Add bits to a user
- `!refund @user` – Refund the last command a user used
- `!sbadd <commandname> <methods>` - Add a command through chat
- `!sbremove <commandname>` - Remove a command through chat
- `!cdebug <commandname>` - Prints command functions to chat
- `!sbban @user` - Ban a user from using the bot
- `!sbunban @user` - Unban a user from using the bot

---

## **How to Update the Bot**

Typically, you only need to replace the `Sweat Bot.exe` file when updating. Custom commands and prices are saved in the **Data** folder, which won’t be overwritten unless specified. I plan on making a installer later!

---

## **Adding Custom Sounds**

To add your own sounds:

- Ensure the sound files are in `.wav` format.
- Place them in the **Sound Clips** folder.
- Use the Command Builder Menu to make a command using the play sound button, just change filename.wav on the Command input line to the name of your file and you are all set!

---

## **Support**

Feel free to report issues via [*SweatBot Discord*](https://discord.gg/k4uH6WZTS4)  
If you enjoy the project and want to support my work, consider donating via [PayPal](https://www.paypal.com/donate/?business=FK2ZHM73QW3FA).

---

## **Latest Changelog**
1.0.06-beta
- Added a random up or down for turn mouse commands
- Added OR function to command builder. The OR operator in your script is used to randomly choose between two functions. When the script encounters OR, it will pick one of the next two methods at random and skip the other

1.0.05-beta
- Fixed white box issue on the pixelate overlay
- More pixelated overlay. It wasn't bad enough
- Update window now plays a notification sound
- Updated !help 
- Updated !about to the new GitHub link
- Fixed issue with blerp integration not working
- Fixed "You don't have enough bits to execute the command" not replacing bits with your currency name
- Updated admin/moderator chat commands to print out your currency name instead of just bits
  
1.0.04-beta
- Finished moderator permissions, now you can give certain mods access to help smooth over controlling chat while you focus on gaming
- Mod permissions include refunds, give or remove currency, add or remove a command, or ban and unban a user from using the bot
- Updated "Rate Delay" setting to properly reflect what it does which is add a cooldown per use so they can't spam commands. Added a message that alerts the user of how much time is remaining
- Fixed potential issue of "you have the latest version installed" popping up on start
