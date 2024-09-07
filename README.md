# Sweat Bot 1.0.0.69alpha

This is my simple to use and open source twitch bot designed to control and interact with your computer/games by Twitch chat. Heavily inspired by instructbot, but forever free and easy to set up. Designed to be used locally without needing to connect to anything but twitch and tarkov.dev api. Originally designed to work with Escape from Tarkov, but now easily configurable to work with any game thanks to the new Custom Command Builder.  
It features chat commands to interact with you as you play. Set your price, controls, and watch as chatters annoy you!. 
Readme and other info currently incomplete. This app has zero error exception handling so if it crashes, it may lock up!  

## How it Works

Whenever a user cheers bits in your chat, the bot will automatically record and store how many they have cheer'd as long as the bot is running. Users can then use commands with the prefix "!" to activate enabled commands. When they use a command it will automatically remove their "bits" from a file.

For the more interested:
The program works by mainly sending virtual keypresses and mouse events directly to window. When a user uses a "!" command the bot in the chat will listen for that command, if that command exist it will execute through the command.

Notice - This app was written with the assistance of chatgpt as I am still learning, but 90% of the code was writen by me. Its not fun writing the same thing 50 times and its great for finding bugs that I have missed.  


## Features
### All features are a work in progress and considered incomplete  

-Quick and easy to set up commands. Just check the box and configure the cost/inputs  
-A trader reset timer that tracks when items are restocked for the real sweats  
-Twitch chat incorporation allowing them to send keypresses and mouse movements to your pc  
-Twitch Bit incorporation. Users cheer bits and chat and the bot stores it in a "bank" they can spend from
-Easy to use UI  
-Custom Command builder interface for creating your own custom commands. Name them, price them, and use the simple to use scripting language. Either type the commands or use the buttons to quickly add them in. No need to type(much)!  

## Upcoming/Potential Features(Not promised) 

-Quick toggle profiles  
-More commands  
-Possibly switch to another UI framework for Linux/Mac support  
-Open to suggestions for more!  

### Download Latest Here
[1.0.0.69a](https://github.com/sprollucy/Tarkov-Twitch-Bot-Working/releases/tag/1.0.0.69a)
  
## How to Get Started with Sweat Bot

Tutorial video on quick setup(not great)  
https://youtu.be/_G8fQeHlMOA  

Note: Drop Config.exe is broken at the moment so Kit Drop does not work  

1. Create a Bot Account  

Go to Twitch.tv and create the account you want to use as the bot.  

2. Mod the Bot  

In your Twitch chat, type /mod <bot_name> to mod the bot you created.  

3. Generate an Access Token  

Use a tool like twitchtokengenerator.com or Twitch’s tool to generate an Access Token with the permissions your bot needs.  
Copy the token into the "Access Token" field in the app, then hit "Save".  

WARNING: Never share your Access Token with anyone. Your account may be compromised if you do.  

4. Open the Bot  

Launch the Sweat Bot.exe file.  

Note: Currently my exe is unsigned so a warning "Windows protected your pc" may pop up. Click either more info and then run anyway. If Windows Security detects it as a dangerous file you will have to open Windows Defender and find it under the quarantine area and allow it  

5. Configure Settings  

Click the settings wheel in the bottom left.  
Enter the channel token for the bot, click "Save".  
Enter the channel name where you want the bot to join and click "Save".  

6. Set Up Commands  

Click the Command Menu (the button with three lines) to configure the bot’s commands.  
Set command costs and which ones you want enabled(see first note). hit "Save", then "Restart".  
To enable custom commands (still a WIP), check "Enable Custom Commands" and open the Command Builder(see notes below)  


### Notes:  
The commands were original thought about with Escape from Tarkov in mine, But using the Custom commands you can name them anything you want and tailor them to whatever game you play  

For custom commands to show up on twitch, you have to restart the program  

You can adjust the price or add to the command input and it will update the current command.  
Changing the name will write it to a new command.  

To remove commands, you’ll need to either delete them manually or edit the CustomCommands.json file in the "Data" folder.  
Default commands are listed in the Command List, which you can use as templates to create your own. Just click on one of the items on the Command List and it will show in the fields above.  

For testing custom commands, you can type them into the console in the Connect Menu. To test in the game put a Delay command at the start of your custom command so you have time to tab into the game before the rest of the command start  

You can format and send separate messages in auto messages using \\.  

7. Connect the Bot  

Open the Connect Menu and click "Connect".  
Then, open Twitch and type !help in the chat to verify the bot is working.  
Use !bitcost to see what commands are active.  


## Viewer Commands

help - List available commands  

how2use - How to use  

about - About the app  

mybits - Check how many bits a user has stored  

bitcost - List all available Sweat Bot commands and their cost in chat  


## Mod/Broadcaster Commands

help - List available commands  

addbits - Adds bits to a user. Username must be a exact match to target user(Must be enabled for moderators to use in Command Menu)  
	!addbits @user amount eg. !addbits @Sprollucy 500  

refund - Refund user for last command they used. Username must be a exact match to target user(Must be enabled for moderators to use in Command Menu)  
	!refund @user eg. !refund @Sprollucy  


## Updating the App

Usually, you only need to replace the Sweat Bot.exe when updating, unless otherwise specified. Custom commands and prices are saved in the "Data" folder which will stay if not overwritten.  

### To transfer your settings after an update

Run the new version once.  
Go to C:\Users\User\AppData\Local\Sweat_Bot.  
Find the current version folder and copy user.config.  
Paste it into the newest version’s folder, overwriting the existing user.config.  

## Adding Custom Sounds

Note: All sound files must be in .wav format  
Take the sound clips you want to add and put them in the Sound Clips folder. That's it! Files can be accessed with the !playsound  

## Goose Command Setup

Goose does cause performance issues. Good for in menus  
Download the required files from this site: samperson.itch.io/desktop-goose  
Once downloaded, place all the files inside a folder named "Goose".  
Delete the "FOR MOD-MAKERS" folder, as it will cause a pop-up to appear each time the command runs.  

## How to Support  
Feel free to report issues on here or directly contact me on discord if you have any issues, or have any suggestion/criticisms: sprollucy
If you like my projects and want to help support me, you can directly donate here  
[Paypal Donate](https://www.paypal.com/donate/?business=FK2ZHM73QW3FA&no_recurring=0&item_name=Thank+you+for+helping+support+my+projects%21&currency_code=USD)

## Latest Changelog

### 1.0.0.69-alpha  
-Restructured refund logging. Previously didn't save the user who did the command  
-Added custom command system! Now you can build out custom commands if the ones provided aren't enough. Currently exposed commands are HitKey, HoldKey, LeftClick, LeftClickHold, RightClick, RightClickHold, Delay, MuteVolume, TurnMouse, PlaySoundClip, HitKeyAsync, HoldKeyAsync, LeftClickAsync, LeftClickHoldAsync, RightClickAsync, RightClickHoldAsync, TurnMouseAsync, PlaySoundClipAsync. There are example and descriptions of the commands inside the Data folder in a file called CustomCommands.json. You can modify the cost and chain multiple methods together! Currently supports all English keys.  
-Updated Connect Menu text box to let you use your custom commands offline for testing purposes  
-Changed !bitcost to print out Custom commands.  
-Added a text box that shows the loaded custom commands on the connect menu. Ugly but functional for now  
-Added (experimental)Custom Command Builder menu. This should make it easier to create new commands with less formatting errors. Will need feedback.  
-Updated backups to not write over themselves anymore. Now they have a time stamp for each backup  
-Adjusted how !help works  
-Updated styling slightly  
-Added debug mode for custom commands. Can be toggled on in the Settings Menu. 
-Fixed trader menu configuration so it only prints the ones enabled when you do the !traders command  
-A ton of bug fixing  
