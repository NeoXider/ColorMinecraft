Color Picker Game - Information 
--------------------------------
Thank you for purchasing the Color Picker Complete project. This is a game that displays a
bunch of squares with the same color. Yet there is one that is slightly different. You need
to find it and click it to progress onto the next round.

This game can be played on computers and touch devices. 

Game Modes - 
	Normal: Normal mode is where you keep playing rounds until you click on the wrong square
			and go back to the menu.
			
	Time Rush: Time Rush mode is normal mode, but each round you have a certain amount of 
				time to find the different colored square. If you fail to find it in time 
				press the wrong one you go back to the menu.
				
Scripts - 

All scripts in this project are fully documentated, yet some knowledge on C# is required as it 
does not go over how to code.

	Game.cs: This script controls the game. It loads new rounds, checks to see if the pressed 
			 square is the right one and houses most of the code.
			 
	ColorSquare.cs: This script is on each of the squares and gets notified when the square
					 has been pressed. It then calls a funtion in the Game.cs script to see
					 if it was the odd one out.
					 
	UI.cs: This script manages the UI for the game. It displays the score and timer.
	
	Menu.cs: This script manages the menu in the game. It changes the pages, quits the game,
			 resets the score, loads the game level and displays the highscore.
			 
Contact - 

If you ever need to contact me you can.

	Email: buckleydaniel101@gmail.com
	Steam: Sothern (I am on steam all the time so if i'm not replying to your email you can check there)
	Skype: daniel_buckley101 
