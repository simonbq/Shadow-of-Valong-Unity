using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    public float visibleTime = 2.0f;

    private GUIStyle backgroundStyle = new GUIStyle();
    private Texture2D dummyTexture;
    private float currentAlpha = 1;
    private float fadeInSpeed, fadeOutSpeed;

    private int fadeDirection;

    private float fadeInTimer,fadeOutTimer,visibleTimer = 0.0f;

	// Use this for initialization
	void Start () {
        Debug.Log("Starting FadeInOut");

        fadeDirection = -1;

        fadeInSpeed = (0 - 1) / fadeInTime;
        fadeOutSpeed = (1 - 0) / fadeOutTime;

        dummyTexture = new Texture2D(1, 1);
        dummyTexture.SetPixel(0, 0, new Color(0, 0, 0, currentAlpha));
        dummyTexture.Apply();

        backgroundStyle.normal.background = dummyTexture;
	}

    public void OnGUI(){
        if(currentAlpha > 0){
            GUI.Label(new Rect(-10, -10, Screen.width + 10, Screen.height + 10), dummyTexture, backgroundStyle);
        }
    }

	
	// Update is called once per frame
	void Update () {
        if (fadeDirection == -1 && (currentAlpha > 0)){
            currentAlpha += fadeInSpeed * Time.deltaTime;
            fadeInTimer += Time.deltaTime;
        }

        if (fadeDirection == 0){
            visibleTimer += Time.deltaTime;
        }

        if (fadeDirection == 1 && (currentAlpha < 1)){
            currentAlpha += fadeOutSpeed * Time.deltaTime;
            fadeOutTimer += Time.deltaTime;
        }

        if(fadeInTimer > fadeInTime){
            fadeDirection = 0;
        }
        if(visibleTimer > visibleTime){
            fadeDirection = 1;
        }
        if (fadeOutTimer > fadeOutTime){
            Application.LoadLevel("Main");
        }

        dummyTexture.SetPixel(0, 0, new Color(0,0,0,currentAlpha));
        dummyTexture.Apply();
	}
}
