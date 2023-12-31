﻿using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour 
{
	public GameObject mainMenuPage;
	public GameObject playPage;
	public GameObject optionsPage;

    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI highscoreTimeText;

    void Start ()
	{
        highscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
        highscoreTimeText.text = PlayerPrefs.GetInt("HighscoreTime").ToString();

        SetPage("Menu");
	}

	public void SetPage (string page)
	{
		switch(page){
			case "Menu":{
                mainMenuPage.SetActive(true);
                playPage.SetActive(false);
                optionsPage.SetActive(false);
                    AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
                    break;
			}
			case "Play":{
				mainMenuPage.SetActive(false);
                playPage.SetActive(true);
				optionsPage.SetActive(false);
                    AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
                    break;
			}
			case "Options":{
				mainMenuPage.SetActive(false);
                playPage.SetActive(false);
                optionsPage.SetActive(true);
                    AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
                    break;
			}
		}
	}

	public void PlayGame (int gameMode)
	{
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
        PlayerPrefs.SetInt("GameMode", gameMode);
		Application.LoadLevel(1);
	}

	public void ResetHighscore ()
	{
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        PlayerPrefs.SetInt("Highscore", 0);
        PlayerPrefs.SetInt("HighscoreTime", 0);
        highscoreText.text = "0";
        highscoreTimeText.text = "0";
    }

	public void QuitGame ()
	{
        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
        Application.Quit();
	}
}
