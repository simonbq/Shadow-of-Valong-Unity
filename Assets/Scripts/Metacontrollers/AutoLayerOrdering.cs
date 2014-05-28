using UnityEngine;
using System.Collections;

public class AutoLayerOrdering : MonoBehaviour {

    public bool isStatic = true;
    private SpriteRenderer mSpriteRenderer;
    

	// Use this for initialization
	void Start () {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mSpriteRenderer.sortingOrder = Mathf.RoundToInt(1 - (transform.position.y * 100 - 100 * mSpriteRenderer.bounds.size.y / 2))-10;

	}
	
	// Update is called once per frame
	void Update () {
        if (!isStatic)
        {
            mSpriteRenderer.sortingOrder = Mathf.RoundToInt(1 - (transform.position.y * 100 - 100 * mSpriteRenderer.bounds.size.y / 2))-10;
        }
	}

}
