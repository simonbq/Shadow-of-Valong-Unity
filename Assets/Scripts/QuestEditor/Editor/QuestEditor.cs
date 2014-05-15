using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class QuestEditor : EditorWindow {

	private QuestContainer qt;
	private string[] questTreeNames;
	private int[] questTreeIds;

	private List<Quest> q;
	private string[] questNames;
	private int[] questIds;
	private string[] objectiveTypes = new string[2];

	private int selectedQuestTreeId = 0;
	private int prevQuestTreeId = -1;
	private int selectedQuestId = 0;
	private int prevQuestId = -1;
	private string changeQuestTreeName;
	private string changeQuestName;
	private string questDescription;

	private Vector2 scrollPos = new Vector2();

	[MenuItem("Edit/Game Data/Quest Editor")]

	static void Init()
	{
		var window = (QuestEditor)EditorWindow.GetWindow(typeof(QuestEditor));
		window.minSize = new Vector2 (240, 480);
		window.title = "Quest Editor";
	}

	void OnEnable()
	{
		loadQuestTrees ();
		loadQuests (selectedQuestTreeId);
		objectiveTypes [0] = "Boolean";
		objectiveTypes [1] = "Counter";
	}

	void OnGUI()
	{
		selectedQuestTreeId = EditorGUILayout.IntPopup ("Quest tree", selectedQuestTreeId, questTreeNames, questTreeIds);
		if(selectedQuestTreeId == qt.QuestTrees.Count)
		{
			qt.QuestTrees.Add(new QuestTree());
			qt.QuestTrees [selectedQuestTreeId].Name = "Unnamed quest tree";
			reloadQuestTrees ();
            loadQuests(selectedQuestTreeId);
            reloadQuestTrees();
            addNewQuest();
		}

        if (prevQuestTreeId != selectedQuestTreeId)
        {
            changeQuestTreeName = qt.QuestTrees[selectedQuestTreeId].Name;
            prevQuestTreeId = selectedQuestTreeId;
            loadQuests(selectedQuestTreeId);
        }
		
		changeQuestTreeName = EditorGUILayout.TextField ("Change name", changeQuestTreeName);
		if(GUILayout.Button ("Apply"))
		{
			qt.QuestTrees[selectedQuestTreeId].Name = changeQuestTreeName;
			questTreeNames[selectedQuestTreeId] = qt.QuestTrees[selectedQuestTreeId].Name + " (Quest tree-ID " + selectedQuestTreeId + ")"; 
		}

		EditorGUILayout.Space ();
		selectedQuestId = EditorGUILayout.IntPopup ("Quest", selectedQuestId, questNames, questIds);
		if(prevQuestId != selectedQuestId)
		{
			if(selectedQuestId == q.Count)
			{
                addNewQuest();
			}
			changeQuestName = q[selectedQuestId].Name;
			prevQuestId = selectedQuestId;
			questDescription = q[selectedQuestId].Description;
		}

		changeQuestName = EditorGUILayout.TextField ("Change name", changeQuestName);
		questDescription = EditorGUILayout.TextArea (questDescription, GUILayout.Height (80));
		if(GUILayout.Button ("Apply"))
		{
			q[selectedQuestId].Name = changeQuestName;
			q[selectedQuestId].Description = questDescription;
			questNames[selectedQuestId] = q[selectedQuestId].Name + " (Quest-ID " + q[selectedQuestId].Id + ")"; 
		}

		EditorGUILayout.Space ();
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
        if (qt.QuestTrees[selectedQuestTreeId].Quests.Count > 0)
        {
            for (int i = 0; i < q[selectedQuestId].Objectives.Count; i++)
            {
                int typeindex = new int();
                switch (q[selectedQuestId].Objectives[i].Type)
                {
                    case "Boolean":
                        typeindex = 0;
                        break;

                    case "Counter":
                        typeindex = 1;
                        break;
                }

                typeindex = EditorGUILayout.Popup("Objective type", typeindex, objectiveTypes);
                q[selectedQuestId].Objectives[i].Type = objectiveTypes[typeindex];

                if (q[selectedQuestId].Objectives[i].Type == "Counter")
                {
                    q[selectedQuestId].Objectives[i].GoalCount = EditorGUILayout.IntField("Goal count", q[selectedQuestId].Objectives[i].GoalCount);
                }

                q[selectedQuestId].Objectives[i].Text = EditorGUILayout.TextField("Objective", q[selectedQuestId].Objectives[i].Text);
                if (GUILayout.Button("Delete"))
                {
                    q[selectedQuestId].Objectives.RemoveAt(i);
                }

                EditorGUILayout.Space();
            }
        }
		EditorGUILayout.EndScrollView ();

		if(GUILayout.Button ("Add Objective"))
		{
			q[selectedQuestId].Objectives.Add (new Objective());
		}
	}

    private void addNewQuest()
    {
        q.Add(new Quest());
        q[selectedQuestId].Name = "Unnamed quest";
        q[selectedQuestId].Id = getNewQuestId();
        Debug.Log("Added quest-ID " + q[selectedQuestId].Id);
        loadQuests(selectedQuestTreeId);
        selectedQuestId = q.Count - 1;
    }

	private int getNewQuestId()
	{
		int id = 0;
		for(int i = 0; i < qt.QuestTrees.Count; i++)
		{
			for(int o = 0; o < qt.QuestTrees[i].Quests.Count; o++)
			{
				id++;
			}
		}
		id--;
		return id;
	}
	
	private void loadQuestTrees()
	{
		qt = QuestContainer.Load(Path.Combine (Application.streamingAssetsPath, "quest.xml"));
		reloadQuestTrees ();
	}

	private void reloadQuestTrees()
	{
		questTreeNames = new string[qt.QuestTrees.Count+1];
		questTreeIds = new int[qt.QuestTrees.Count+1];
		
		for(int i = 0; i < qt.QuestTrees.Count+1; i++)
		{
			questTreeIds[i] = i;
			if(i == qt.QuestTrees.Count)
			{
				questTreeNames[i] = "New quest tree";
			}
			
			else{
				questTreeNames[i] = qt.QuestTrees[i].Name + " (Quest tree-ID " + i + ")";
			}
		}
	}

	private void loadQuests(int qtid)
	{
		q = qt.QuestTrees [qtid].Quests;
		questNames = new string[q.Count+1];
		questIds = new int[q.Count+1];

		for(int i = 0; i < q.Count+1; i++)
		{
			questIds[i] = i;
			if(i == q.Count)
			{
				questNames[i] = "New quest";
			}

			else{
				questNames[i] = q[i].Name + " (Quest-ID " + q[i].Id + ")";
			}
		}

		selectedQuestId = 0;
        if (q.Count > 0)
        {
            changeQuestName = q[0].Name;
            questDescription = q[0].Description;
        }

        else{
            changeQuestName = "";
            questDescription = "";
            }
	}
}