using UnityEngine;
using System.Collections;

public class gamestate : MonoBehaviour {

    private static gamestate instance;
    private string activeLevel;
    private string name;
    private int maxHP;
    private int hp;
    private int[] inventory;

    public static gamestate Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new gamestate();
            }

            return instance;
        }
    }

    public void startState()
    {
        //load shit from storage 
        activeLevel = "";
        name = "Link";
        maxHP = 3;
        hp = 3;

        Debug.Log("Started state machine");
        Debug.Log("Active level: "+activeLevel+", name: "+name+", max hp: "+maxHP+", current hp: "+hp);
    }

    public string getName()
    {
        return name;
    }

    public int getHP()
    {
        return hp;
    }

    public int getMaxHP()
    {
        return maxHP;
    }

    public int[] getInventory()
    {
        return inventory;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
