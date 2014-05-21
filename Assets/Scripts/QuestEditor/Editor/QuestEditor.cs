using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class QuestEditor : EditorWindow {

	private QuestContainer qt;
	private string[] questTreeNames;
	private int[] questTreeIds;

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

    private bool changed = false;

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
            changed = true;
		}

        if (prevQuestTreeId != selectedQuestTreeId)
        {
            changeQuestTreeName = qt.QuestTrees[selectedQuestTreeId].Name;
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
            if (selectedQuestId == qt.QuestTrees[selectedQuestTreeId].Quests.Count)
			{
                addNewQuest();
                changed = true;
			}
            changeQuestName = qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Name;
            questDescription = qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Description;
		}

		changeQuestName = EditorGUILayout.TextField ("Change name", changeQuestName);
		questDescription = EditorGUILayout.TextArea (questDescription, GUILayout.Height (80));
		if(GUILayout.Button ("Apply"))
		{
            qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Name = changeQuestName;
            qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Description = questDescription;
            questNames[selectedQuestId] = qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Name + " (Quest-ID " + qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Id + ")"; 
		}

		EditorGUILayout.Space ();
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
        if (qt.QuestTrees[selectedQuestTreeId].Quests.Count > 0)
        {
            for (int i = 0; i < qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives.Count; i++)
            {
                int typeindex = new int();
                switch (qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives[i].Type)
                {
                    case "Boolean":
                        typeindex = 0;
                        break;

                    case "Counter":
                        typeindex = 1;
                        break;
                }

                typeindex = EditorGUILayout.Popup("Objective type", typeindex, objectiveTypes);
                qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives[i].Type = objectiveTypes[typeindex];

                if (qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives[i].Type == "Counter")
                {
                    qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives[i].GoalCount = EditorGUILayout.IntField("Goal count", 
                        qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives[i].GoalCount);
                }

                qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives[i].Text = EditorGUILayout.TextField("Objective", 
                    qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives[i].Text);
                if (GUILayout.Button("Delete"))
                {
                    qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives.RemoveAt(i);
                }

                EditorGUILayout.Space();
            }
        }
		EditorGUILayout.EndScrollView ();

        if (GUI.changed &&
            prevQuestId == selectedQuestId &&
            prevQuestTreeId == selectedQuestTreeId)
        {
            changed = true;
        }

        prevQuestTreeId = selectedQuestTreeId;
        prevQuestId = selectedQuestId;

		if(GUILayout.Button ("Add Objective"))
		{
            qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Objectives.Add(new Objective());
		}

        if (GUILayout.Button("Save"))
        {
            qt.Save(Path.Combine(Application.streamingAssetsPath, "quest.xml"));
            changed = false;
        }
    }

    void OnDestroy()
    {
        if (changed &&
            EditorUtility.DisplayDialog("Unsaved changes!",
                                        "You have some unsaved changes. Do you want to save?",
                                        "Save",
                                        "Quit without saving"))
        {
            qt.Save(Path.Combine(Application.streamingAssetsPath, "quest.xml"));
        }
    }
    private void addNewQuest()
    {
        qt.QuestTrees[selectedQuestTreeId].Quests.Add(new Quest());
        qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Name = "Unnamed quest";
        qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Id = getNewQuestId();
        Debug.Log("Added quest-ID " + qt.QuestTrees[selectedQuestTreeId].Quests[selectedQuestId].Id);
        loadQuests(selectedQuestTreeId);
        selectedQuestId = qt.QuestTrees[selectedQuestTreeId].Quests.Count - 1;
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
        questNames = new string[qt.QuestTrees[selectedQuestTreeId].Quests.Count + 1];
        questIds = new int[qt.QuestTrees[selectedQuestTreeId].Quests.Count + 1];

        for (int i = 0; i < qt.QuestTrees[selectedQuestTreeId].Quests.Count + 1; i++)
		{
			questIds[i] = i;
            if (i == qt.QuestTrees[selectedQuestTreeId].Quests.Count)
			{
				questNames[i] = "New quest";
			}

			else{
                questNames[i] = qt.QuestTrees[selectedQuestTreeId].Quests[i].Name + " (Quest-ID " + qt.QuestTrees[selectedQuestTreeId].Quests[i].Id + ")";
			}
		}

		selectedQuestId = 0;
        if (qt.QuestTrees[selectedQuestTreeId].Quests.Count > 0)
        {
            changeQuestName = qt.QuestTrees[selectedQuestTreeId].Quests[0].Name;
            questDescription = qt.QuestTrees[selectedQuestTreeId].Quests[0].Description;
        }

        else{
            changeQuestName = "";
            questDescription = "";
            }
	}
}