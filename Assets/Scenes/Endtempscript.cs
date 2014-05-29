using UnityEngine;
using System.Collections;

public class Endtempscript : MonoBehaviour {
	
	GUISkin skinCache;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")){
			Application.Quit();
		}
	}
	
	void OnGUI(){
		skinCache = GUI.skin;
		
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		
		GUI.skin.label.fontSize = Mathf.RoundToInt(DialogueController.getHeight(80));
		GUI.Label(new Rect(0, Screen.height/2-DialogueController.getHeight(80), Screen.width, DialogueController.getHeight(80)), "End of demo.");
		
		GUI.skin.label.fontSize = Mathf.RoundToInt(DialogueController.getHeight(30));
		GUI.Label(new Rect(0, Screen.height/2+DialogueController.getHeight(200), Screen.width, DialogueController.getHeight(50)), "Click anywhere to exit.");
		
		GUI.skin = skinCache;
	}
}
