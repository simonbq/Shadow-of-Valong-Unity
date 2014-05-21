using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

[XmlRoot("UnitCollection")]
public class UnitContainer{
    [XmlArray("Units")]
    [XmlArrayItem("Unit")]
    public List<Unit> Units = new List<Unit>();

    public static UnitContainer Load(string path){
		var serializer = new XmlSerializer(typeof(UnitContainer));
		//Use Path.Combine(Application.streamingAssetsPath, path) with path being the name of your xml file and put the xml file in the "StreamingAssets" folder
		using(var stream = new FileStream(System.IO.Path.Combine(Application.streamingAssetsPath, path), FileMode.Open)){
			return serializer.Deserialize(stream) as UnitContainer;
		}
	}
	
	public void Save(string path){
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		var serializer = new XmlSerializer(typeof(UnitContainer));
		using (var stream = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, path))){
			serializer.Serialize(stream, this);
		}
	}
}

public class Unit{
    [XmlAttribute("id")]
    public int Id;

    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("description")]
    public string Description;

    [XmlAttribute("prefab")]
    public string Prefab;

    [XmlAttribute("alignment")]
    public string Alignment;

    [XmlAttribute("type")]
    public string Type;

    [XmlAttribute("element")]
    public string Element;

    [XmlAttribute("atk_mode")]
    public string AttackMode;

    [XmlAttribute("threat")]
    public int Threat;

    [XmlAttribute("damage")]
    public int Damage;

    [XmlAttribute("health")]
    public int Health;

    [XmlAttribute("speed")]
    public int Speed;

    [XmlAttribute("dmg_red")]
    public int DamageReduction;

    [XmlAttribute("rarity")]
    public string Rarity;

    [XmlArray("Drops")]
    [XmlArrayItem("Drop")]
    public List<Drop> Drops = new List<Drop>();

    [XmlArray("Powers")]
    [XmlArrayItem("Power")]
    public List<Power> Powers = new List<Power>();
}

public class Drop{

}

public class Power{
    [XmlAttribute("name")]
    public string Name;
}