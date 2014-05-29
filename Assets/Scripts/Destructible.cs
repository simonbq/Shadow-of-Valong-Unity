using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

    public bool destroyParent = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact()
    {
		if(ChestController.hasStick){
	        if (!destroyParent)
	        {
	            Debug.Log("Destroying self: " + gameObject);
	            Destroy(gameObject);
	        }
	        if (destroyParent)
	        {
	            Debug.Log(gameObject + " destroying parent: " + transform.parent.gameObject);
	            Destroy(transform.parent.gameObject);
	        }
		}
    }
}
