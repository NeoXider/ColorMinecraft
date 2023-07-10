using UnityEngine;
using System.Collections;

public class ColorSquare : MonoBehaviour 
{
	public Game game;	//The game script so we can call the CheckSquare() function.

	void OnMouseDown ()					//OnMouseDown() gets called whenever the object has been clicked on. Requires a collider.
	{
		game.CheckSquare(gameObject);	//If so, then we call the CheckSquare function and set the square perameter as this gameObject.
	}
}
