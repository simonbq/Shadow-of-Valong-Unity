using UnityEngine;
using System.Collections;

public class Grab : MonoBehaviour {

	Transform originalParent = null;

	// Use this for initialization
	void Start () {
		if (transform.parent != null) {
			originalParent = transform.parent;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void StartGrabbing(Transform userTransform) {
		//if (transform.parent != userTransform) {
			transform.parent = userTransform;
		//}
	}
	void StopGrabbing(Transform userTransform){
		transform.parent = originalParent;
	}
}
