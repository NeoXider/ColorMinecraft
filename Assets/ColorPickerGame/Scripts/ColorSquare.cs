using UnityEngine;
using System.Collections;

public class ColorSquare : MonoBehaviour 
{

	public int _index;
    Game game;
    public AudioManager AM = AudioManager.Instance;

    void OnMouseDown ()					
	{
        AM.PlayEffects(AM.buttonClick);
        game.CheckSquare(gameObject);   
        gameObject.GetComponent<Animator>().enabled = true;
        

    }
    private void Start()
    {
        AM = AudioManager.Instance;
        game = Game.instance.game;
    }
}
