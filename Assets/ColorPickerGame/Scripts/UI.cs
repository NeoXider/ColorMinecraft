using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour 
{
	public Game game;	//The game script so that we can access its variables.

	public Text score;	//The text object that displays the players score.
	public Text timer; 	//The text object that displays the time lefts for the round if the gamemode is Time Rush.

	void Start ()
	{
		if(game.gameMode == GameMode.TIME_RUSH){											//If the gamemode is Time Rush, the timer gets enabled, else it is disabled.
			timer.gameObject.active = true;
		}else{
			timer.gameObject.active = false;
		}
	}

	void Update ()
	{
		score.text = game.score.ToString();													//Setting the text of the score to be the score variable located in Game.cs and converting it to a string.

		if(game.gameMode == GameMode.TIME_RUSH){											//If the gamemode is Time Rush, the timer text gets set.
			timer.text = "Time Left: " + (game.eliminationTime - (int)game.timer).ToString();
		}
	}
}
