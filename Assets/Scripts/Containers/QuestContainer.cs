using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("QuestCollection")]
public class QuestContainer{
	[XmlArray("QuestTrees")]
	[XmlArrayItem("QuestTree")]
	public List<QuestTree> QuestTrees = new List<QuestTree>();
	
	public static QuestContainer Load(string path){
		var serializer = new XmlSerializer(typeof(QuestContainer));
		//Use Path.Combine(Application.streamingAssetsPath, path) with path being the name of your xml file and put the xml file in the "StreamingAssets" folder
		using(var stream = new FileStream(System.IO.Path.Combine(Application.streamingAssetsPath, path), FileMode.Open)){
			return serializer.Deserialize(stream) as QuestContainer;
		}
	}
	
	public void Save(string path){
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		var serializer = new XmlSerializer(typeof(QuestContainer));
		using (var stream = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, path))){
			serializer.Serialize(stream, this);
		}
	}
}

public class QuestTree{
	[XmlArray("Quests")]
	[XmlArrayItem("Quest")]
	public List<Quest> Quests = new List<Quest>();
	
	[XmlAttribute("name")]
	public string Name;
}

public class Quest{
	[XmlArray("Objectives")]
	[XmlArrayItem("Objective")]
	public List<Objective> Objectives = new List<Objective>();
	
	[XmlAttribute("id")]
	public int Id;
	
	[XmlAttribute("name")]
	public string Name;
	
	[XmlAttribute("description")]
	public string Description;
	
	[XmlAttribute("started")]
	public bool Started;
}

public class Objective{
	[XmlAttribute("text")]
	public string Text;
	
	[XmlAttribute("type")]
	public string Type;
	
	[XmlAttribute("goalCount")]
	public int GoalCount;
	
	[XmlAttribute("currentCount")]
	public int CurrentCount;
	
	[XmlAttribute("completed")]
	public bool Completed;
}