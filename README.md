# Tarkov-Twitch-Bot 1.0.0.68alpha

This is my simple to use and open source twitch bot designed to be used with tarkov mainly but works with other games  
It features chat commands to interact with you as you play. Only a few right now but more are coming. It also includes a trader reset screen for the true sweats to track exactly when traders reset and twitch chat reminders for it also!  
Readme and other info currently incomplete. This app has zero error exception handling so if it crashes, it may lock up!  

Notice - This app was written with the assistance of chatgpt as I am still learning, but 90% of the code was writen by me. Its not fun writing the same thing 50 times and its great for finding bugs that I have missed.  


## Features
-Some bitbot cloned features  
-A trader reset timer that tracks when items are restocked for the real sweats  
-Twitch chat incorporation  
-Twitch Bits for commands 
-Easy to use UI(WIP)  
-Fun and easy commands(WIP)  
-Togglable commands to pick what commands you want

## Upcoming Features(Not promised) 
-Bot moderation  
-Customizable commands  
-Extra game setups?  
-Possibly add profiles  
-More commands  
-Follower services  

### Download Latest Here
[1.0.0.68a](https://github.com/sprollucy/Tarkov-Twitch-Bot-Working/releases/tag/1.0.0.68a)

## How To Use
Tutorial video on quick setup  
https://youtu.be/_G8fQeHlMOA  
  

1. First step is to create a account on Twitch.tv that you want to use as the bot account. I use this site but twitch also offers their own  
https://twitchtokengenerator.com/  
  
2. Go to your Twitch chat and mod the bot you have created  

3. Go to a website or use twitches tool to generate a Access Token that has all the permissions you want the bot to have and then copy the token info into the Access Token field on the app and then hit save  

WARNING - DO NOT SHARE YOUR ACCESS TOKEN WITH ANYONE AS IT ALLOWS THEM TO USE YOUR BOT TO ACCESS YOUR TWITCH CHANNEL WITH MODERATOR PERMISSIONS. I AM NOT RESPONSABLE IF YOU SHARE THE TOKEN  
   
4. Open the EXE in the folder  

5. Click on the settings wheel in the bottom left  

6. Enter the channel token for the bot, hit save, then enter the channel name you want the bot to join in the Channel Name field and hit save  

5. Next click the Command Menu button on the left hand side(hit the 3 lines button to see the names) and configure any of the commands  
	-The auto message has a way to format and send seperate messages using \\  
	-The Random Keys command has a default format of "W,A,S,D,E,Q,C,{TAB},G,2,3"  
	-Default times are set to 300 seconds which is 5 minutes  

8. Open tarkov and go a offline and open your menu. Then open the "Drop Config.exe" to configure the mouse position for the drop command. Using the DOWN ARROW key, go over each of the items you with to drop(each weapon, armor, body armor, bag ect...)  
All together there is 9 items to select. Once this is finished restart the app.  

7. Next select the Connect Menu on the left hand side and hit the connect button.  

FOR THE GOOSE COMMAND TO WORK YOU MUST GO TO THIS SITE AND DOWNLOAD THE FILE. 
https://samperson.itch.io/desktop-goose  

8. Once the game is downloaded all the files inside the folder named "Goose"  

9. Once the files are in there, delete the folder called "FOR MOD-MAKERS" as it opens a pop up everytime the command runs  


## Commands
Soon to be written. use !help while running

## How to Support  
Feel free to report issues on here or directly contact me on discord if you have any issues, or have any suggestion/criticisms: sprollucy
If you like my projects and want to help support me, you can directly donate here  
[Paypal Donate](https://www.paypal.com/donate/?business=FK2ZHM73QW3FA&no_recurring=0&item_name=Thank+you+for+helping+support+my+projects%21&currency_code=USD)

## Changelog

1.0.0.68-alpha 6-17-24  
-Added !refund command and setting to allow moderators to use it  
-Removed !grenadesound  
-Added custom audio loader. Currently it only supports wav format files. With the commands enabled, you can either do !audiolist to list all sound clips in the Sound Clips directory and then you do !audioplay <filename>. I will eventually make a way to add custom prices to each sound  
-Align command boxes  
-Fixed logbits method showing the wrong users bits  
-Fixed description error for firemode  
-Moved log methods to separate file called LogHandler  
-Changed voice line to either do f1 once or twice randomly  
-Fixed bug with goose still being able to work when chat is paused  
-Removed messages that commands are disabled for less chat clutter  
-Fixed error with trader update causing app to crash either when opening trader menu or calling it in twitch chat and then opening the trader menu  
-Fixed Kill Goose button not working. Broke with a git error  
-Fixed goose command being able to be used while it is disabled and commands are paused.  
-Added whitelist for moderators to allow certain mods of the channel to use the !refund and !addbits commands. Either use the button in the Commands menu or go to the Data folder and opon ModWhitelist.txt  
-Added bit multiplier in the Command Menu for people who give bits. So if set to 2, and someone give 10 bits, it will count as 20 to the bot  


1.0.0.67-alpha 6-12-24  
-Added log files for when bits are added and when commands are used for how much. Can be found in the Log Folder  
-Changed file directory layout  
-Added back up system for user data, which keeps a back up of the user bits from twitch and user configurations. Back up button can be found in the Quick Toggles section of the Connection Menu  
-Added restore function in the settings menu for default commands in case something goes wrong  
-Added restore functions in the settings menu to restore user bit data from back up in case json file gets corrupted  
-Added setting in the Command Menu to allow for moderators to give to users bits  
-Added in quick buttons to open your twitch channel from inside the Connection Menu and Settings Menu  
-Added button to launch the drop config exe. Still planning on writing this into the app  
-Optimized loading of data string data. Ram usage is down from 190Mb to 60-70Mb in current testing.  
-Fixed randomkeys to actually send the keys randomly  
-Potentially fixed bug where users bits show in the negatives  
-Fixed issue of potential corruption of json files from improper saving of files  

1.0.0.66 Preview-alpha 3-26-24  
-Expanded main window length  
-Added !praisesun. It makes you crouch and look straight up  
-Added !touchgrass  
-Added ability to allow mods to give bits(will add specifics later  
-Renamed 360grenadetoss to 360grenade  
-Added !knifeout  
-Fixed !dropkit not using assigned key and tweaked to be more accurate  
-Added !jump  
-Changed !bitcost to list from cheapest to most expensive   
-Added donation link in the settings menu  
-Added !walk  
-Added !hotmic  
-Added !grenadetoss  
-Added !weaponswap  
-Added a key config for dropbag   
-Renamed control menu to command menu  
-Tweaked 360 grenade to hopefully get it to go off more reliably   
-Added !firemode which changes firemode  
-Added backup and restore system in case of crashing. After you hit the backup button it will run every 10 minutes  
-Tons of bug fixes and other minor tweaks  


1.0.0.65-alpha 3-22-24  
-Added !addbits to streamer commands just in case the bot misses a cheer or you feel like giving them out usage is !mybits @username amount  
-Added !voiceline - will need feedback on this. its currently just f1  
-Added !prone  
-Added !reload  
-Added restart button to commands menu  
-Changed !bitcost to print out only which commands are active  
-Added spam protection for bitcost  
-Added !how2use to tell how to add bits and use commands  
-Added !magdump  
-Added !360magdump  
-Changed !grenade to !grenadesound  
-Changed !grenadetoss to !360grenadetoss  
-Changed !bitcost so it will only list the active commands  

1.0.0.64-alpha  
Rewrote how commands are handled again. I have opted to remove the cooldown and switch fully to a seudo bit system. Now if a user cheers bits in the chat this should pick that up and add it to a file to be used as point payment for commands. Channel owners can add points manually as well from the !addbits command or by editing the user_bits.json file. 
-Fixed error on !randomkey where it actually didnt include the correct method  
-Added a Quest menu for future potential future updates. For now enjoy the blank screen  
-Updated background color to be a little easier on the eyes and minor layout tweaks  
-Removed help button because it just showed the changelog  
-Added !grenadetoss which spins you then throws a grenade  
-Added !crouch  
-Added !magdump  
-Added !holdaim  
-Added first time chat bonus
-Added !addbits  
-Added !mybits  

1.0.0.62-alpha 10-20-23  
-All timer systems are now under a unified system and use less cpu  
-Corrected !drop timer  
-Corrected typo in !goose cooldown  
-Corrected !dropbag timer  
-Corrected !grenade timer  
-Corrected !pop timer  
-Corrected !randomkeys timer  
-Added kill counter and tracker  
-Renamed  !resetdeath to !resettoday and added kill reset to it  
-Added !about  
-Added !kill counter command  
-Added !wipestats to read stats saved for this wipe  
-Changed !stats to only show daily stats  
-Added anti spam features to some of the commands(help, traders, stats, wipestats, about)  
-Updated kit drop to accept any input key  
-Reorganized Mainbot code for sanity  
-Created a new app thats used for configuration of the drop feature. It allows for 9 seperate points on the screen to be set depending on where you select using the down arrow key. Currently it reads and loads from a seperate json and is its own exe file. Will add into the main app later  
-Added minimize button  
-Updated close and minimize look  

1.0.0.6-alpha 10-6-23
-Fixed numerous dumb errors(made more probably)
-Fixed wiggle command not always working
-Changed cooldown format to not read the float
-Updated trader ui
-Updated starting screen
-Updated how to use
-Simplified some code used
-Added bag drop command. Currently uses z as the default drop key
-Added grenade command. Currently you cannot adjust the volume but you can modify the sound file
-Modified the trader logic on load so the sound doesnt lock the application up or crash if the info cannot be retrieved

1.0.0.5-alpha 09-17-2023
-Added individual configs for each trader so you only hear the notifications for which ones you want
-Added a way to apply this configuration to the twitch bot
-Added a config under commands for random keys to configure what buttons it holds and hits. Currently it just spams buttons for 5 seconds
-Added notification mute for traders
-Added ability to turn off trader auto notifications on twitch
-Finished up a Auto Messaging command to the commands menu. The cooldown timer reads in seconds so 300 seconds = 5 minutes. Added a syntax to be able to send multiple messages at a time by typing \\ after any message you want to send. I will probably expand its funtionality to send more messages on seperate timers.
-Removed old files used for prototyping


1.0.0.03-alpha hotfix 09-11-2023
-Added catch for crashing if you connect without putting login info in
-Fixed trader menu timers to keep the timers in memory and not be cleared out as long as they run
-Started working on a automessage system - it will send and save a message, but not resend messages
-Added notification system for trader timer updates


1.0.0.3-alpha 09-10-2023
-Rebuild Commands and unified most of the timer systems
-Added in adjustable timers for each of the commands(currently you have to restart to update all the cooldowns)
-Overhauled UI to a more modern style to support expandability

-Added trader menu
	-Traders now have their own interface to track their stock resets so you dont have to run the app connected to twitch to see this info
	-Will be adding a alert system with a sound notification and some form of notification in the UI
-Reworked most of the backend to make things work with the new system

-Added Command menu
	-Added a new menu that show the commands and lets you change their cool downs and prices for bits
	-Cool downs are to be entered in how many seconds you want it to cooldown for till you can use it again. Set to 0 for no cooldown
	-Added bit payments to commands. Now when chat membered cheer with bits or pay in anyway, the bits will be added to a file with their name. When bits is enabled, the bits they have spent, can be used on a commands!
	-They can use !mybits to see how many they have
		-None of this is tested as i dont have bit payments enabled on twitch and havent enrolled in their developer thing PLEASE GIVE FEEDBACK IF THIS WORKS


1.0.0.1-alpha 09-06-2023
-Added basic command features
-Added trader reset fetching and auto updating using api.tarkov.dev
-Incorporated basic ui features
-Added some fun commands to mess with your game
-Wrote How to use - may have missed some info
-Added command list
-Added show/hide info for access token and channel
-Fixed some cooldowns accidentally triggering another when certain commands are used
-Refactored most of the original code for readability
Remember to report any bugs or new feature request on the github page!

Next feature update will include more bug fixes and hopefully I will start working on a way to adjust and edit cooldown timers
