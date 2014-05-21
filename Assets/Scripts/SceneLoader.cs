using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    public string levelToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Changing level to: "+levelToLoad);
        if(other.gameObject.tag == "Player"){
            Application.LoadLevel(levelToLoad);
            
        }
        
    }

}
