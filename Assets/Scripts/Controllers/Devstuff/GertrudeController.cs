using UnityEngine;
using System.Collections;

public class GertrudeController : MonoBehaviour {

    public Unit unitInfo;
	// Use this for initialization
	void Start () {
        unitInfo = UnitController.getUnit("Gertrude.prefab");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact(){
        DialogueController.startDialogue(0);
    }
}
