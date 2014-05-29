using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    public string levelToLoad;
    public Color fadeColor = new Color(0,0,0,1);
    public float fadeTime = 1.0f;
    public int portalId;
    public bool saveOnLoad = true;

    private GUIStyle backgroundStyle = new GUIStyle();
    private Color currentColor = Color.clear;
    private Texture2D dummyTexture;
    private bool fadingOut;
    private float startTime;
	// Use this for initialization
	void Start () {
        //Create dummy texture for covering the screen
        dummyTexture = new Texture2D(1, 1);
        dummyTexture.SetPixel(0, 0, currentColor);
        dummyTexture.Apply();
        backgroundStyle.normal.background = dummyTexture;
	}
	
	// Update is called once per frame
	void Update () {
	    if(fadingOut){
            var t = (Time.time - startTime) / fadeTime;
            currentColor = Color.Lerp(Color.clear, fadeColor, t);
            dummyTexture.SetPixel(0, 0, currentColor);
            dummyTexture.Apply();
        }
	}
    void OnGUI(){
        if (currentColor != Color.clear)
        {
            GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTexture, backgroundStyle);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            if (!fadingOut){
                fadingOut = true;
                startTime = Time.time;
                Invoke("loadLevel", fadeTime);
            }
        }
    }

    void loadLevel()
    {
    	if(saveOnLoad){
	        PlayerPrefs.SetString("currentLevel", levelToLoad);
	        PlayerPrefs.Save();
	        Debug.Log("Saving level: " + levelToLoad);
        }
        Debug.Log("Changing level to: " + levelToLoad);
        Application.LoadLevel(levelToLoad);
    }

    
}
