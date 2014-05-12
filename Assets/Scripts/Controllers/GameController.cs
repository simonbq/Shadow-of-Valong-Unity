using UnityEngine;
using System.Collections;
public class GameController : MonoBehaviour {


	//Gamestate
    public enum GameState { GAME, PAUSED, MAINMENU, MAP, QUESTMENU };
    public static GameState gameState = GameState.GAME;

	// Use this for initialization
	void Start (){

	}
	
	// Update is called once per frame
	void Update (){

	}
}
