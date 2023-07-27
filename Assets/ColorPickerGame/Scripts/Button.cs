using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    [Header("GUI Components")]

    public GameState gameState;
    private AudioManager _audioManager = AudioManager.Instance;

    void Start()
    {

    }
    public void ButtonClick()
    {
        _audioManager.PlayEffects(_audioManager.buttonClick);
    }
    public void ButtonMenu()
    {
        ButtonClick();
        _audioManager.PlayMusic(_audioManager.menuMusic);
    }
}
