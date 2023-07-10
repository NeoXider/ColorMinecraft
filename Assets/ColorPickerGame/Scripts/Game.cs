using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Game : MonoBehaviour 
{
	public Color[] colorPalette;		//Array holding all the possible colors we can use.
	public Color curColor;				//The current color that will be used on the board for this round.
	public Color curOddColor; 			//The current color that will be used as the odd square color.

	public GameObject[] colorSquares;	//Array holding all the color squares on the board.
	public int oddColorSquare;			//The index number for the odd color square that identifies an element in colorSquares.

	public float difficultyModifier;	//The difficulty which will decrcrease over the rounds, making spotting the different color harder. This will be explained in more detail later.
	public int round;					//The number indicating how many rounds have passed.
	public int score;					//The number indicating your current score.

	public GameMode gameMode;			//The game mode that the player selected on the main menu.

	public float timer;					//The timer is used when in the Time Rush gamemode and counts up from 0.
	public float eliminationTime;       //In Time Rush, when the timer reaches this number and the player is still on the same round they lose.

	public int _number;
	public Sprite[] _texture;
	void Awake ()
	{
		round = 0;																			//The round gets set to 0.
		score = 0;																			//The score gets set to 0.

		gameMode = (GameMode)PlayerPrefs.GetInt("GameMode");								//The gamemode gets set as the int identifier get loaded from the player prefs and converted to the enum element.
		NewRound();																			//As the game starts the NewRound() function gets called.
	}

	void Update ()
	{
		if(gameMode == GameMode.TIME_RUSH){													//If the gamemode is Time Rush, the timer counts up 1 every second.
			timer += 1.0f * Time.deltaTime;

			if(timer >= eliminationTime){													//If the timer is more than or equal to the eliminationTime, the player fails the game and the FailGame() function gets called.
				FailGame();
			}
		}
	}

	void NewRound ()
	{
		difficultyModifier /= 1.08f; 														//Start of a new round so the difficulty modifier gets divided by 1.08.
		round++;																			//Start of a new round so the round counter goes up one.
		timer = 0.0f;																		//For the Time Rush gamemode. The timer gets reset to 0 after every round.

		curColor = colorPalette[Random.Range(0, colorPalette.Length - 1)];					//With Random.Range selecting a random index number, we get a random color from the palette array and is set to the cur color.
		float diff = ((1.0f / 255.0f) * difficultyModifier);								//Creating a temp variable 'diff' which converts our difficultyModifier down to decimal scale to use with Color.
		curOddColor = new Color(curColor.r - diff, curColor.g - diff, curColor.b - diff);	//Having the curOddColor be the same as as curColor, yet modifying the r, g and b values to be subtracted by diff. This means that a lower difficulty modifdier would make the two colors more similar and harder to spot.
		oddColorSquare = Random.Range(0, colorSquares.Length - 1);							//Randomly getting an index number which can be used to identify a color square in the colorSquares array. This will be the square where the color will be different.

		for(int x = 0; x < colorSquares.Length; x++){										//Here we are looping through the colorSquares array.
			if(x == oddColorSquare){														//If x is the oddColorSquare number that means we make that color square the odd color.
				colorSquares[x].GetComponent<Image>().color = curOddColor;
			}else{																			//Else we just make it the normal color.
				colorSquares[x].GetComponent<Image>().color = curColor;
			}
		}
	}

	void FailGame ()
	{
		if(score > PlayerPrefs.GetInt("Highscore")){										//If the score is more than the Highscore stored in the player prefs then we set the highscore to be the current score achieved in this game.
			PlayerPrefs.SetInt("Highscore", score);
		}

		LoadMenu();																			//Then we load the menu using the LoadMenu function.
	}

	public void CheckSquare (GameObject square)												//When a square gets clicked on this public function gets called. It requires a GameObject perameter.
	{
		if(colorSquares[oddColorSquare] == square){											//If the pressed square is the odd color square, then we start a new round and add 10 to the score.
			NewRound();
			score += 10;
		}else{																				//Else this means the player pressed the wrong color or the time ran out and they failed.
			FailGame();																		//The FailGame() function gets called and ends the game.
		}
	}	

	public void LoadMenu ()																	//This function loads the menu. Its used when the player fails the game or when the menu button gets pressed.
	{	
		Application.LoadLevel("Menu");			
	}
}

public enum GameMode {NORMAL = 0, TIME_RUSH = 1}											//The GameMode enumurator stores the two gamemodes. Normal and Time Rush.
