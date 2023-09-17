using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static YG.InfoYG;

public class Pause : MonoBehaviour
{
    private AudioManager AM = AudioManager.Instance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        
    }

    private void OnApplicationFocus(bool focus)
    {
        PauseGame(!focus);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            AM.musicSource.Pause();
            AM.efxSource.Pause();
        }
        else
        {
            Time.timeScale = 1;
            AM.musicSource.UnPause();
            AM.efxSource.UnPause();
        }
    }
}
