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
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) && gameState == GameState.MENU && !clicked)
    //    {
    //        if (IsButton())
    //            return;

    //        AudioManager.Instance.PlayEffects(AudioManager.Instance.buttonClick);
    //        AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
    //    }
    //    else if (Input.GetMouseButtonUp(0) && clicked && gameState == GameState.MENU)
    //        clicked = false;
    //}
    //public bool IsButton()
    //{
    //    bool temp = false;

    //    PointerEventData eventData = new PointerEventData(EventSystem.current)
    //    {
    //        position = Input.mousePosition
    //    };

    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventData, results);

    //    foreach (RaycastResult item in results)
    //    {
    //        temp |= item.gameObject.GetComponent<Button>() != null;
    //    }

    //    return temp;
    //}
}
