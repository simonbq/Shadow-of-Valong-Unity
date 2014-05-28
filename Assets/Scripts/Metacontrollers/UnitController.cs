using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

    private static UnitContainer units = new UnitContainer();

    private static string filePath = "units.xml";

	// Use this for initialization
	void Start () {
        units = UnitContainer.Load(filePath);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static string getName(int unitId){
        string name = null;
        for(int i=0; i < units.Units.Count; i++){
			if(unitId == units.Units[i].Id){
                name = units.Units[i].Name;
			}
		}
        return name;
    }

    public static int getUnitCount(){
        return units.Units.Count;
    }

    public static Unit getUnit(string prefabName){
        Unit unit = null;
        for (int i = 0; i < units.Units.Count; i++)
        {
            if (prefabName == units.Units[i].Prefab)
            {
                unit = units.Units[i];
            }
        }
        return unit;
    }
}
