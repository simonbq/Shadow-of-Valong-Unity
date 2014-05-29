using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

    public int objectId;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Interact(){
		ChestController.hasKey = true;
		Destroy(gameObject);
	}
}
