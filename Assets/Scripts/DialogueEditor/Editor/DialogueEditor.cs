using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class DialogueEditor : EditorWindow {

	private DialogueContainer c;

	private int[] dialogueIds;
	private string[] dialogueNames;
	private int selectedId;

	[MenuItem("Edit/Dialogue/Dialogue Editor")]
	static void Init()
	{
		var window = (DialogueEditor)EditorWindow.GetWindow(typeof(DialogueEditor));
		window.minSize = new Vector2 (240, 320);
		window.title = "Dialogue Editor";
	}

	void OnEnable()
	{
		reloadDialogues ();
	}

	void OnGUI()
	{
		selectedId = EditorGUILayout.IntPopup (selectedId, dialogueNames, dialogueIds);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void reloadDialogues()
	{
		c = DialogueContainer.Load (Path.Combine (Application.streamingAssetsPath, "dialogue.xml"));
		dialogueIds = new int[c.Dialogues.Count];
		dialogueNames = new string[c.Dialogues.Count];
		for(int i = 0; i < c.Dialogues.Count; i++)
		{
			dialogueIds[i] = (int)c.Dialogues[i].Id;
			dialogueNames[i] = "Dialogue ID " + (int)c.Dialogues[i].Id;
		}
	}
}
