using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using static UnityEditor.Progress;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using UnityEngine.U2D;

public class Game : MonoBehaviour 
{
	public Color[] colorPalette;		//Array holding all the possible colors we can use.
	public Color curColor;				//The current color that will be used on the board for this round.
	public Color curOddColor;           //The current color that will be used as the odd square color.
    public GameObject colorSquares;	//Array holding all the color squares on the board.
	public int oddColorSquare;          //The index number for the odd color square that identifies an element in colorSquares.

	public float difficulty = 1.04f;
	public float difficultyModifier;	//The difficulty which will decrcrease over the rounds, making spotting the different color harder. This will be explained in more detail later.
	public int round;					//The number indicating how many rounds have passed.
	public int score;					//The number indicating your current score.

	public GameMode gameMode;			//The game mode that the player selected on the main menu.

	public float timer;					//The timer is used when in the Time Rush gamemode and counts up from 0.
	public float eliminationTime;       //In Time Rush, when the timer reaches this number and the player is still on the same round they lose.

	public GameObject root;
    public int _number;
    public Sprite[] _sprite;
    public float _scaleColorSquares = 1;
	private GameObject[] _colorSquares;
	public static Game instance;

	private Sprite randTexture;

    public Game game;
    private void Start()
    {
        _colorSquares = new GameObject[_number];
        Generation();
        NewRound();

    }
    private void OnValidate()
    {
		
    }
    void Awake ()
	{
		if (instance == null)
            instance = this;
        
		round = 0;																			//The round gets set to 0.
		score = 0;																			//The score gets set to 0.

		gameMode = (GameMode)PlayerPrefs.GetInt("GameMode");								//The gamemode gets set as the int identifier get loaded from the player prefs and converted to the enum element.
																				//As the game starts the NewRound() function gets called.
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
    float modif = 7;
    void NewRound ()
	{
        difficultyModifier /= difficulty; 														//Start of a new round so the difficulty modifier gets divided by 1.08.
        round++;																			//Start of a new round so the round counter goes up one.
		if(timer >= 1)
        timer -= 1.0f;                                                                       //For the Time Rush gamemode. The timer gets reset to 0 after every round.
		else 
        {
            timer = 0.0f;
        }
        modif /= difficulty;
        //curColor = colorPalette[Random.Range(0, _number - 1)];					//With Random.Range selecting a random index number, we get a random color from the palette array and is set to the cur color.
        //curColor

        float diff = ((1.0f / 255.0f) * difficultyModifier * modif);								//Creating a temp variable 'diff' which converts our difficultyModifier down to decimal scale to use with Color.
		curOddColor = new Color(curColor.r - diff, curColor.g - diff, curColor.b - diff);	//Having the curOddColor be the same as as curColor, yet modifying the r, g and b values to be subtracted by diff. This means that a lower difficulty modifdier would make the two colors more similar and harder to spot.
		//oddColorSquare = Random.Range(0, colorSquares.Length - 1);	
		oddColorSquare = Random.Range(0, _number - 1);                          //Randomly getting an index number which can be used to identify a color square in the colorSquares array. This will be the square where the color will be different.

        //for(int x = 0; x < colorSquares.Length; x++){										//Here we are looping through the colorSquares array.
        //	if(x == oddColorSquare){														//If x is the oddColorSquare number that means we make that color square the odd color.
        //		colorSquares[x].GetComponent<Image>().color = curOddColor;
        //	}else{																			//Else we just make it the normal color.
        //		colorSquares[x].GetComponent<Image>().color = curColor;
        //	}
        //}
        if (round == 1)
        {
            
            randTexture = GetRundomTexture();
        }
        else
        {
            Sprite newTexture = SetNewRandomTexture();
            if (newTexture != randTexture)
            {
                randTexture = newTexture;
            }
            else
            {
                Debug.Log("Не удалось установить новую текстуру");
            }
        }

        for (int x = 0; x < _colorSquares.Length; x++)
        {                                       //Here we are looping through the colorSquares array.
            if (x == oddColorSquare)
            {                                                       //If x is the oddColorSquare number that means we make that color square the odd color.
                _colorSquares[x].GetComponent<SpriteRenderer>().color = curOddColor;
            }
            else
            {                                                                           //Else we just make it the normal color.
                _colorSquares[x].GetComponent<SpriteRenderer>().color = curColor;
            }
            SetTexture(_colorSquares[x], randTexture);
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
        //if(colorSquares[oddColorSquare] == square){											//If the pressed square is the odd color square, then we start a new round and add 10 to the score.
        //Debug.Log(square + " Выбраный квадрат");
        //Debug.Log(_colorSquares[oddColorSquare] + " Предатель");
		if (_colorSquares[oddColorSquare] == square)
		{
			NewRound();
			score += 10;
		}
        else
        {                                                                               //Else this means the player pressed the wrong color or the time ran out and they failed.
            FailGame();                                                                     //The FailGame() function gets called and ends the game.
        }
    }	

	public void LoadMenu ()																	//This function loads the menu. Its used when the player fails the game or when the menu button gets pressed.
	{
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
		Application.LoadLevel("Menu");			
	}

	public void Generation()
	{
        for (int i = 0; i < _number; i++)
        {
			GameObject squares = Instantiate(colorSquares);
			SetParent(squares, root);
			squares.transform.localScale = new Vector3(_scaleColorSquares, _scaleColorSquares);
            //squares.GetComponent<SpriteRenderer>().size = new Vector2(30000, 30000);
            squares.GetComponent<ColorSquare>()._index = i;
            //squares.GetComponent<BoxCollider2D>()._index = i;
            _colorSquares[i] = squares;
            squares.name = "ColorSquareMine" + i.ToString();
			SetTexture(squares, randTexture);
        }
    }
    public void SetParent(GameObject parent, GameObject newParent)
    {
        parent.transform.parent = newParent.transform;
    }
    //public Sprite SetNewRandomTexture()
    //{
    //    Sprite result;
    //    result = GetRundomTexture();
    //    if (result == randTexture)
    //    {
    //        SetNewRandomTexture();
    //    }
    //    else
    //    {
    //        return result;
    //        //randTexture = result;
    //    }
    //    return null;
    //}
    int id = 0;
    public Sprite SetNewRandomTexture()
    {
        
        Sprite result;
        for(int i = 0; i < _sprite.Length; i++)
        {
            int a = Random.Range(0, _sprite.Length);
            if (a != id)
            {
                id = a;
                break;
            }
        }
        return result = _sprite[id];
    }
    Sprite GetRundomTexture()
	{
        id = Random.Range(0, _sprite.Length);
        Sprite result = _sprite[id];
        return result;
	}
    private void SetTexture(GameObject gameObject, Sprite sprite)
    {
		//gameObject.GetComponent<RawImage>().texture = SetRandomTexture();
		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}

public enum GameMode {NORMAL = 0, TIME_RUSH = 1}											//The GameMode enumurator stores the two gamemodes. Normal and Time Rush.
