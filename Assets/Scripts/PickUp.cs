using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

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
	void Interact (Transform userTransform) {
		if (transform.parent != userTransform) {
			transform.parent = userTransform;
			transform.position = new Vector3(userTransform.position.x, userTransform.position.y+renderer.bounds.size.y, 0);
		}else{
			transform.parent = originalParent;
			transform.position = new Vector3(userTransform.position.x, userTransform.position.y-renderer.bounds.size.y, 0);
		}
	}
}
