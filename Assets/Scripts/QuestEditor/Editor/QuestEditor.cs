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

	private int selectedQuestTreeId = 0;
	private int prevQuestTreeId = -1;
	private int selectedQuestId = 0;
	private int prevQuestId = -1;
	private string changeQuestTreeName;
	private string changeQuestName;

	[MenuItem("Edit/Game Data/Quest Editor")]

	static void Init()
	{
		var window = (QuestEditor)EditorWindow.GetWindow(typeof(QuestEditor));
		window.minSize = new Vector2 (240, 320);
		window.title = "Quest Editor";
	}

	void OnEnable()
	{
		loadQuestTrees ();
		loadQuests (0);
	}

	void OnGUI()
	{
		selectedQuestTreeId = EditorGUILayout.IntPopup (selectedQuestTreeId, questTreeNames, questTreeIds);
		if(prevQuestTreeId != selectedQuestTreeId)
		{
			changeQuestTreeName = qt.QuestTrees [selectedQuestTreeId].Name;
			prevQuestTreeId = selectedQuestTreeId;
			loadQuests (selectedQuestTreeId);
		}

		changeQuestTreeName = EditorGUILayout.TextField ("Change name", changeQuestTreeName);
		if(GUILayout.Button ("Apply"))
		{
			qt.QuestTrees[selectedQuestTreeId].Name = changeQuestTreeName;
			questTreeNames[selectedQuestTreeId] = qt.QuestTrees[selectedQuestTreeId].Name + " (Quest tree-ID " + selectedQuestTreeId + ")"; 
		}

		EditorGUILayout.Space ();
		selectedQuestId = EditorGUILayout.IntPopup (selectedQuestId, questNames, questIds);
		if(prevQuestId != selectedQuestId)
		{
			changeQuestName = q[selectedQuestId].Name;
			prevQuestId = selectedQuestId;
		}

		changeQuestName = EditorGUILayout.TextField ("Change name", changeQuestName);
		if(GUILayout.Button ("Apply"))
		{
			q[selectedQuestId].Name = changeQuestName;
			questNames[selectedQuestId] = q[selectedQuestId].Name + " (Quest-ID " + q[selectedQuestId].Id + ")"; 
		}
		
		EditorGUILayout.Space ();
	}
	
	private void loadQuestTrees()
	{
		qt = QuestContainer.Load(Path.Combine (Application.streamingAssetsPath, "quest.xml"));
		questTreeNames = new string[qt.QuestTrees.Count];
		questTreeIds = new int[qt.QuestTrees.Count];

		for(int i = 0; i < qt.QuestTrees.Count; i++)
		{
			questTreeIds[i] = i;
			questTreeNames[i] = qt.QuestTrees[i].Name + " (Quest tree-ID " + i + ")";
		}
	}

	private void loadQuests(int qtid)
	{
		q = qt.QuestTrees [qtid].Quests;
		questNames = new string[q.Count];
		questIds = new int[q.Count];

		for(int i = 0; i < q.Count; i++)
		{
			questIds[i] = i;
			questNames[i] = q[i].Name + " (Quest-ID " + q[i].Id + ")";
		}
	}
}