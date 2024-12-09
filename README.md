# **Sweat Bot 1.0.0.72 Alpha**

Sweat Bot is a simple, lightweight and open Twitch bot designed to control and interact with your computer and games through Twitch chat. It’s heavily inspired by Instructbot, but it’s always free and easy to set up and requires no other connections besides Twitch and tarkov.dev. Originally designed for *Escape from Tarkov*, it now includes a **Custom Command Builder**, allowing it to work with any game.

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

### [**Download Latest Release**](https://github.com/sprollucy/Tarkov-Twitch-Bot-Working/releases/tag/1.0.0.70)

---

## **Getting Started with Sweat Bot**

Watch a basic setup tutorial [here](https://youtu.be/_G8fQeHlMOA).

> **Note**: Drop Config.exe is currently broken, so Kit Drop is not functional.

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
   - Click the Command Menu button to configure your bot’s commands.
   - Set the command costs and enable your desired commands, then click **Save** and **Restart**.

   > **Note**: For custom commands, check **Enable Custom Commands** and use the Command Builder.

---

## **Viewer Commands**

- `!help` – List available commands.  
- `!how2use` – Instructions on how to use the bot.  
- `!about` – Information about the bot.  
- `!mybits` – Check how many bits a user has stored.  
- `!bitcost` – List available commands and their cost.

---

## **Mod/Broadcaster Commands**

- `!help` – List available commands.  
- `!addbits @user <amount>` – Add bits to a user.  
- `!refund @user` – Refund the last command a user used.

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

## **Goose Command Setup**

**Desktop Goose** can cause performance issues, but it’s fun to use in menus!

1. Download the required files from [Samperson's Desktop Goose](https://samperson.itch.io/desktop-goose).  
2. Place all files inside a folder named "Goose".  
3. Delete the "FOR MOD-MAKERS" folder to avoid pop-ups.

---

## **Support**

Feel free to report issues via GitHub or contact me on Discord: *sprollucy*.  
If you enjoy the project and want to support my work, consider donating via [PayPal](https://www.paypal.com/donate/?business=FK2ZHM73QW3FA).

---

## **Latest Changelog**

1.0.0.72-alpha
- Removed old command system in favor of the command builder. Maintaining and changing any of it was a pain
- Updated debug system
- Fixed crash caused if the custom command json was missing
- Fixed "Disable confirmation message" toggle not saving after restart
- Adjusted menu names and layouts
- Added save button to Auto Message
- Added toggle in settings to hide Trader Menu
- Adjusted scripting language syntax and how to use file
- Added MPosAsync
- Added quick commands under the Command Builder menu
- Removed goose folder. Planning on adding a way to launch exe files

1.0.0.71-alpha
- Added toggle to print out remaining bits after using a command
- Added Async Delay
- Bugfixing for Custom commands
