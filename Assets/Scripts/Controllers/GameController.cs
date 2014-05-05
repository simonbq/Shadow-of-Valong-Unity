using UnityEngine;
using System.Collections;
public class GameController : MonoBehaviour {

    private DialogueContainer dialogues;

	// Use this for initialization
	void Start (){
        dialogues = DialogueContainer.Load("dialogue.xml");
        Debug.Log(dialogues.Dialogues.Count);
	}
	
	// Update is called once per frame
	void Update (){
	
	}

    void OnGui(){
        for (int i = 0; i < dialogues.Dialogues.Count; i++){
            for (int j = 0; j < dialogues.Dialogues[i].Texts.Count; j++){
                GUI.Label((new Rect(10+i*10, 10+j*10, 100, 20)), dialogues.Dialogues[i].Texts[j]);
                Debug.Log(dialogues.Dialogues[i].Texts[j]);
            }
        }
    }
    
}
