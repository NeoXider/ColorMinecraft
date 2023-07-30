using UnityEngine;
using System.Collections;

public class ColorSquare : MonoBehaviour 
{

	public int _index;
    Game game;
    public AudioManager AM = AudioManager.Instance;
    public ParticleSystem particle;

    void OnMouseDown ()					
	{
        //AM.PlayEffects(AM.buttonClick);
        Nice();
        game.CheckSquare(gameObject);   
        gameObject.GetComponent<Animator>().enabled = true;
        

    }
    private void Start()
    {
        particle.Pause();
        AM = AudioManager.Instance;
        game = Game.instance.game;
    }
    public void Nice()
    {
        particle.Stop();
        particle.Play();
    }
}
