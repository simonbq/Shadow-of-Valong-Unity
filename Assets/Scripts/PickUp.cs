﻿using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact() {
        QuestController.addToObjective(5, 0, 1);
    }
}
