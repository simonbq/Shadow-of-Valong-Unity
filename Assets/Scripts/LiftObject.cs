using UnityEngine;
using System.Collections;

public class LiftObject : MonoBehaviour {

	Transform originalParent;

	// Use this for initialization
	void Start () {
		if (transform.parent != null) {
			originalParent = transform.parent;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void Lift(Transform userTransform) {
		if (transform.parent != userTransform) {
			transform.parent = userTransform;
			transform.position = new Vector3(userTransform.position.x, userTransform.position.y+renderer.bounds.size.y, 0);
		}
	}
	void Drop(Transform userTransform){
		transform.parent = originalParent;
		transform.position = new Vector3(userTransform.position.x, userTransform.position.y-renderer.bounds.size.y, 0);
	}
}
