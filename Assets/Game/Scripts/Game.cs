using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using Assets.SimpleLocalization;
using YG;
public class Game : MonoBehaviour 
{
    [Header("Score")]
    public int _score;
   

    [Header("Game")]
    public static Game _instance;
    public Color[] _colorPalette;        
    [SerializeField] private Color _curColor;              
    [SerializeField] private Color _curOddColor;          
    [SerializeField] private ColorSquare _colorSquare;
	public int _oddColorSquare;

    [SerializeField] private float _difficulty = 1.04f;
    [SerializeField] private float _difficultyModifier = 10;
    public int _round;

    public float _timer { get; private set; }
    public float _eliminationTime;

    public GameMode _gameMode;
    
    [SerializeField] private GameObject root;
    [SerializeField] private int _countScuares = 25;
    [SerializeField] private Sprite[] _sprite;
    [SerializeField] private float _scaleColorSquares = 1;
    private GameObject[] _colorSquares;
	
	private Sprite _randTexture;

    [SerializeField] private int _idRandomSprite = 0;
    public AudioManager _am = AudioManager.Instance;
    [Header("LeaderBoard")]
    [SerializeField] private string _nameLeaderBoard;
    [SerializeField] private string _nameLeaderBoardTimeRush;

    private void Start()
    {
        SPlayerPrefsSave();

        _am = AudioManager.Instance;
        _colorSquares = new GameObject[_countScuares];
        FullscreenShowYG();
        Generation();
        NewRound();
    }

    private void SPlayerPrefsSave()
    {
        if (PlayerPrefs.HasKey("Highscore"))
            _score = PlayerPrefs.GetInt("Highscore");
        else
            PlayerPrefs.SetInt("Highscore", 0);

        if (PlayerPrefs.HasKey("HighscoreTime"))
            Debug.Log("");
        else
            PlayerPrefs.SetInt("HighscoreTime", 0);

        PlayerPrefs.Save();
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
		if (_instance == null)
            _instance = this;
        
		_round = 0;																			
		_score = 0;																			

		_gameMode = (GameMode)PlayerPrefs.GetInt("GameMode");								
		if(YandexGame.SDKEnabled == true)
        {
            GetDate();
        }
	}

	void Update ()
	{
		if(_gameMode == GameMode.TIME_RUSH){													
			_timer += 1.0f * Time.deltaTime;

			if(_timer >= _eliminationTime){													
				FailGame();
			}
		}
    }
    
    void NewRound()
    {
        _round++;
        if (_timer >= 1)
            _timer -= 1.0f;
        else
        {
            _timer = 0.0f;
        }
        SetDifficultyCollor();
        SetRundomTextureInRound();
        SetOddScuaresAndSprite();
    }

    private void SetRundomTextureInRound()
    {
        if (_round == 1)
        {
            _randTexture = GetRundomTexture();
        }
        else
        {
            Sprite newTexture = SetNewRandomTexture();
            if (newTexture != _randTexture)
            {
                _randTexture = newTexture;
            }
            else
            {
                Debug.Log("Не удалось установить новую текстуру");
                newTexture = SetNewRandomTexture();
                _randTexture = newTexture;
            }
        }
    }

    private void SetDifficultyCollor()
    {
        _difficultyModifier /= _difficulty;
        float diff = ((1.0f / 255.0f) * _difficultyModifier);
        _curOddColor = new Color(_curColor.r - diff, _curColor.g - diff, _curColor.b - diff);
        Debug.Log(diff);
    }

    private void SetOddScuaresAndSprite()
    {
        _oddColorSquare = Random.Range(0, _colorSquares.Length - 1);
        for (int x = 0; x < _colorSquares.Length; x++)
        {
            if (x == _oddColorSquare)
            {
               // _colorSquares[x].GetComponent<SpriteRenderer>().color = Color.black;
                _colorSquares[x].GetComponent<SpriteRenderer>().color = _curOddColor;
            }
            else
            {
                _colorSquares[x].GetComponent<SpriteRenderer>().color = _curColor;
            }
            SetTexture(_colorSquares[x], _randTexture);
        }
    }

    void FailGame ()
	{
        if (_gameMode == GameMode.NORMAL)
        {
            if (_score > PlayerPrefs.GetInt("Highscore"))
            {
                PlayerPrefs.SetInt("Highscore", _score);
                YandexGame.NewLeaderboardScores(_nameLeaderBoard, _score);
            }
        }
        else if (_gameMode == GameMode.TIME_RUSH)
        {
            if (_score > PlayerPrefs.GetInt("HighscoreTime"))
            {
                PlayerPrefs.SetInt("HighscoreTime", _score);
                YandexGame.NewLeaderboardScores(_nameLeaderBoardTimeRush, _score);
            }
        }
        PlayerPrefs.Save();
        if (_score >= 350) VideoYG();
        else if (_score >= 100) FullscreenShowYG();
        
        LoadMenu();																			
	}

	public void CheckSquare (GameObject square)												
	{
		if (_colorSquares[_oddColorSquare] == square)
		{
			NewRound();
			_score += 10;
            if (_am != null)
                _am.PlayEffects(_am.trueCulor);
        }
        else
        {
            if(_am != null)
                _am.PlayEffects(_am.gameOver);
            FailGame();                                                                   
        }
    }	

	public void LoadMenu ()																	
	{
        if(_am != null)
        {
            _am.PlayEffects(_am.buttonClick);
            _am.PlayMusic(_am.menuMusic);
        }  
        FullscreenShowYG();
        Application.LoadLevel(0);
    }

	public void Generation()
	{
        for (int i = 0; i < _countScuares; i++)
        {
			GameObject squares = Instantiate(_colorSquare.gameObject);
			SetParent(squares, root);
			squares.transform.localScale = new Vector3(_scaleColorSquares, _scaleColorSquares);
            squares.GetComponent<ColorSquare>()._index = i;
            _colorSquares[i] = squares;
            squares.name = "ColorSquareMine" + i.ToString();
			SetTexture(squares, _randTexture);
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
            if (a != _idRandomSprite)
            {
                _idRandomSprite = a;
                break;
            }
        }
        return result = _sprite[_idRandomSprite];
    }
    Sprite GetRundomTexture()
	{
        _idRandomSprite = Random.Range(0, _sprite.Length);
        Sprite result = _sprite[_idRandomSprite];
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
