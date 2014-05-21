using UnityEngine;
using System.Collections;

public class CheeseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact()
    {
        if (QuestController.questStarted(5))
        {
            QuestController.addToObjective(5, 0, 1);
            Destroy(gameObject);
        }
    }
}
