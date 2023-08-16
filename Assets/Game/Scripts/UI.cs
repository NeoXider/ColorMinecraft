using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour 
{
	public Game _game;

	public Text _score;	
	public Text _timer; 	

	void Start ()
	{
		if(_game._gameMode == GameMode.TIME_RUSH)									
			_timer.gameObject.SetActive(true);
		else
            _timer.gameObject.SetActive(false);
        
	}

	void Update ()
	{
		_score.text = _game._score.ToString();													

		if(_game._gameMode == GameMode.TIME_RUSH){											
			_timer.text = (_game._eliminationTime - (int)_game._timer).ToString();
		}
	}

}
