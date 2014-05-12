using UnityEngine;
using System.Collections;

public class CalleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Interact(){
		DialogueController.startDialogue(2);
        QuestController.startQuest(5);
	}
}
