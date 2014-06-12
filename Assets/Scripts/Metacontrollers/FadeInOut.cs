using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    public float visibleTime = 2.0f;
    public Color fadeColor;
    public string levelToLoad;

    private GUIStyle backgroundStyle = new GUIStyle();
    private Texture2D dummyTexture;

    private float startTime;
    private Color currentColor=new Color(0,0,0,1);
    
    enum States { FADINGIN, VISIBLE, FADINGOUT, LOADNEXT };
    States currentState = States.FADINGIN;
    private float fadeInTimer, fadeOutTimer, visibleTimer = 0.0f;
	// Use this for initialization
	void Start () {
        currentState = States.FADINGIN;

        //Create dummy texture for covering the screen
        dummyTexture = new Texture2D(1, 1);
        dummyTexture.SetPixel(0, 0, currentColor);
        dummyTexture.Apply();
        backgroundStyle.normal.background = dummyTexture;

        startTime = Time.time;
        if(PlayerPrefs.GetString("currentLevel") != ""){
            levelToLoad = PlayerPrefs.GetString("currentLevel");
            Debug.Log("Loading level from savefile: " + levelToLoad);
        }
	}

    public void OnGUI(){
        GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTexture, backgroundStyle);
    }

	
	// Update is called once per frame
	void Update () {
        if(currentState == States.FADINGIN){
            var t = (Time.time - startTime) / fadeInTime;
            currentColor = Color.Lerp(fadeColor, Color.clear, t);
            fadeInTimer += Time.deltaTime;
            if(fadeInTimer > fadeInTime && fadeOutTime > 0){
                currentState = States.VISIBLE;
                Debug.Log("Start visible time");
            }
        }

        if(currentState == States.VISIBLE){
            if(visibleTimer <= visibleTime){
                visibleTimer += Time.deltaTime;
            }
            if (visibleTimer > visibleTime){
                currentState = States.FADINGOUT;
                startTime = Time.time;
                Debug.Log("Start fadeout");
            }
        }

        if(currentState == States.FADINGOUT){
            var t = (Time.time - startTime) / fadeOutTime;
            currentColor = Color.Lerp(Color.clear, fadeColor, t);
            fadeOutTimer += Time.deltaTime;
            if(fadeOutTimer > fadeOutTime){
                currentState = States.LOADNEXT;
            }
        }

        if (currentState == States.LOADNEXT){
            print("Starting game");

            DontDestroyOnLoad(gamestate.Instance);
            gamestate.Instance.startState();

            Application.LoadLevel(levelToLoad);
        }

        dummyTexture.SetPixel(0, 0, currentColor);
        dummyTexture.Apply();

    }
}
