using UnityEngine;
using System.Collections;

public class ColorSquare : MonoBehaviour 
{

	public int _index;
    Game game;	

	void OnMouseDown ()					
	{
		game.CheckSquare(gameObject);   
        gameObject.GetComponent<Animator>().enabled = true;

    }
    private void Start()
    {
        game = Game.instance.game;
    }
}
