using UnityEngine;
using System.Collections;

public class QuestController : MonoBehaviour {

	private static QuestContainer quests;

	// Use this for initialization
	void Start () {
		quests = QuestContainer.Load("quest.xml");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if(GameController.gameState == GameController.GameState.QUESTMENU){
            GUI.Label(new Rect(0,0,100,20), getQuest(5).Name);
            GUI.Label(new Rect(0, 20, 350, 20), getQuest(5).Description);

			//if(questStarted(5))
			GUI.Label(new Rect(0,40,100,20), "" + getQuest(5).Started);
		}
	}
	public static void startQuest(int questId){
		Quest quest = getQuest(questId);
        if (!quest.Started){
            quest.Started = true;

        }
	}
	
	public bool questStarted(int questId){
		Quest quest = getQuest(questId);
		return quest.Started;
	}
	
	public bool questCompleted(int questId){
		bool completed = true;
		Quest quest = getQuest(questId);
		if(quest == null){
			return false;
		}
		
		for(int i=0; i < quest.Objectives.Count; i++){
			if(!quest.Objectives[i].Completed){
				completed = false;
			}
		}
		if(completed){
			return true;
		}else{
			return false;
		}
	}
	
	private static Quest getQuest(int questId){
		Quest returnQuest = null;
		for(int i=0; i < quests.QuestTrees.Count; i++){
			for(int j=0; j < quests.QuestTrees[i].Quests.Count; j++){
				if(questId == quests.QuestTrees[i].Quests[j].Id){
					returnQuest = quests.QuestTrees[i].Quests[j];
				}
			}
		}
		return returnQuest;
	}
}
