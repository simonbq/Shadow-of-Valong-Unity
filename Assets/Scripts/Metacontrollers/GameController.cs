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
        //DEBUGGING ONLY; REMEMBER TO REMOVE
        if (Input.GetButtonDown("LeftTrigger") && Input.GetButtonDown("RightTrigger"))
        {
            Debug.Log("CLEARED ALL PLAYER PREFS");
            PlayerPrefs.DeleteAll();
        }
	}
}
