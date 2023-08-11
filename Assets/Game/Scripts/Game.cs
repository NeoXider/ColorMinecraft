using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//using static UnityEditor.Progress;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using UnityEngine.U2D;
using Assets.SimpleLocalization;
using YG;
public class Game : MonoBehaviour 
{
	public Color[] colorPalette;        
    [SerializeField] private Color curColor;              
    [SerializeField] private Color curOddColor;          
    [SerializeField] private GameObject colorSquares;
	public int oddColorSquare;       

	public float difficulty = 1.04f;
	public float difficultyModifier;	
	public int round;					
	public int score;				

	public GameMode gameMode;	

	public float timer;					
	public float eliminationTime;       

    [SerializeField] private GameObject root;
    [SerializeField] private int _number;
    [SerializeField] private Sprite[] _sprite;
    [SerializeField] private float _scaleColorSquares = 1;
    private float modif = 7;
    private GameObject[] _colorSquares;
	public static Game instance;

	private Sprite randTexture;

    public Game game;
    private int id = 0;
    public AudioManager AM = AudioManager.Instance;
    private void Start()
    {
        AM = AudioManager.Instance;
        _colorSquares = new GameObject[_number];
        FullscreenShowYG();
        Generation();
        NewRound();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += GetDate;
        
    }
    private void OnDisable()
    {
        YandexGame.GetDataEvent -= GetDate;
    }
    public void GetDate()
    {
        LocalizationManager.Read();
        LocalizationManager.Language = YandexGame.savesData.language;
        //YandexGame.SaveProgress();
    }
    void Awake ()
	{
		if (instance == null)
            instance = this;
        
		round = 0;																			
		score = 0;																			

		gameMode = (GameMode)PlayerPrefs.GetInt("GameMode");								
		if(YandexGame.SDKEnabled == true)
        {
            GetDate();
        }
	}

	void Update ()
	{
		if(gameMode == GameMode.TIME_RUSH){													
			timer += 1.0f * Time.deltaTime;

			if(timer >= eliminationTime){													
				FailGame();
			}
		}
	}
    
    void NewRound ()
	{
        difficultyModifier /= difficulty; 														
        round++;
		if(timer >= 1)
        timer -= 1.0f;
		else 
        {
            timer = 0.0f;
        }
        modif /= difficulty;
        float diff = ((1.0f / 255.0f) * difficultyModifier * modif);	
		curOddColor = new Color(curColor.r - diff, curColor.g - diff, curColor.b - diff);
		oddColorSquare = Random.Range(0, _number - 1);         
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
        {                                       
            if (x == oddColorSquare)
            {                                                       
                _colorSquares[x].GetComponent<SpriteRenderer>().color = curOddColor;
            }
            else
            {                                                                           
                _colorSquares[x].GetComponent<SpriteRenderer>().color = curColor;
            }
            SetTexture(_colorSquares[x], randTexture);
        }   
    }

	void FailGame ()
	{
		if(score > PlayerPrefs.GetInt("Highscore")){										
			PlayerPrefs.SetInt("Highscore", score);
		}
        if(score >= 100) FullscreenShowYG();
        LoadMenu();																			
	}

	public void CheckSquare (GameObject square)												
	{
		if (_colorSquares[oddColorSquare] == square)
		{
			NewRound();
			score += 10;
            AM.PlayEffects(AM.trueCulor);
        }
        else
        {
            AM.PlayEffects(AM.gameOver);
            FailGame();                                                                   
        }
    }	

	public void LoadMenu ()																	
	{
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.menuMusic);
        FullscreenShowYG();
        Application.LoadLevel(0);
        

    }

	public void Generation()
	{
        for (int i = 0; i < _number; i++)
        {
			GameObject squares = Instantiate(colorSquares);
			SetParent(squares, root);
			squares.transform.localScale = new Vector3(_scaleColorSquares, _scaleColorSquares);
            squares.GetComponent<ColorSquare>()._index = i;
            _colorSquares[i] = squares;
            squares.name = "ColorSquareMine" + i.ToString();
			SetTexture(squares, randTexture);
        }
    }
    public void SetParent(GameObject parent, GameObject newParent)
    {
        parent.transform.parent = newParent.transform;
    }

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
		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void VideoYG() => StartCoroutine(CoroPause());
    IEnumerator CoroPause()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            YG.YandexGame.RewVideoShow(0);
            yield break;
        }
    }
    public void FullscreenShowYG() => StartCoroutine(CoroPauseScreen());
    IEnumerator CoroPauseScreen()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            YG.YandexGame.FullscreenShow();
            yield break;
        }
    }
}

public enum GameMode {NORMAL = 0, TIME_RUSH = 1}										
