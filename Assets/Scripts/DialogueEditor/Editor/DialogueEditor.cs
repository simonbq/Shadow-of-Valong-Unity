using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DialogueEditor : EditorWindow {

	private DialogueContainer c;

	private int[] dialogueIds;
	private string[] dialogueNames;
	private int selectedId;

	//FULKOD :(
	private string[] speakerNames = new string[4];
	private int[] speakerIds = new int[4];

	private bool changed = false;
	private Vector2 scrollPos = new Vector2();
	private DialogueContainer prevC;

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

		EditorGUILayout.Space ();

		EditorGUILayout.BeginScrollView (scrollPos);
		for(int i = 0; i < c.getDialogue(selectedId).Texts.Count; i++)
		{
			c.getDialogue(selectedId).Texts[i].SpeakerId = EditorGUILayout.IntPopup(c.getDialogue(selectedId).Texts[i].SpeakerId,
			                                                                        speakerNames,
			                                                                        speakerIds);
			c.getDialogue(selectedId).Texts[i].value = EditorGUILayout.TextField("Text", c.getDialogue(selectedId).Texts[i].value);
			if(prevC.getDialogue(selectedId).Texts[i] != c.getDialogue(selectedId).Texts[i])
			{
				changed = true;
				prevC.getDialogue(selectedId).Texts[i] = c.getDialogue(selectedId).Texts[i];
			}

			if(GUILayout.Button("Delete"))
			{
				c.getDialogue(selectedId).Texts.RemoveAt(i);
				changed = true;
			}
			EditorGUILayout.Space ();
		}
		EditorGUILayout.EndScrollView ();

		if(GUILayout.Button ("Add line"))
		{
			c.getDialogue(selectedId).Texts.Add (new Text());
			changed = true;
		}

		if(GUILayout.Button ("Save"))
		{
			c.Save (Path.Combine (Application.streamingAssetsPath, "dialogue.xml"));
			changed = false;
		}
	}

	void OnDestroy()
	{
		if(changed &&
			EditorUtility.DisplayDialog ("Unsaved changes!",
		                                "You have some unsaved changes. Do you want to save?",
		                                "Save",
		                                "Quit without saving"))
		{
			c.Save (Path.Combine (Application.streamingAssetsPath, "dialogue.xml"));
		}
	}

	private void reloadDialogues()
	{
		c = DialogueContainer.Load (Path.Combine (Application.streamingAssetsPath, "dialogue.xml"));
		prevC = c;
		dialogueIds = new int[c.Dialogues.Count];
		dialogueNames = new string[c.Dialogues.Count];
		for(int i = 0; i < c.Dialogues.Count; i++)
		{
			dialogueIds[i] = i;
			dialogueNames[i] = "Dialogue ID " + i;
		}

		//let the fulkod commence
		for(int i = 0; i < 4; i++)
		{
			speakerIds[i] = i;
			speakerNames[i] = GameController.replaceSpeakerId(i);
		}
	}
}
