using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


	//Gamestate
    public enum GameState { GAME, PAUSED, MAINMENU, MAP, QUESTMENU };
    public static GameState gameState;

	// Use this for initialization
	void Start (){
        gameState = GameState.GAME;
	}
	
	// Update is called once per frame
	void Update (){

	}
}
