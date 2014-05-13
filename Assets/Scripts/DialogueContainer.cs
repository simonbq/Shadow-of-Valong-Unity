using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("DialogueCollection")]
public class DialogueContainer{
    //public static string path = "dialogue.xml";

    [XmlArray("Dialogues")]
    [XmlArrayItem("Dialogue")]
    public List<Dialogue> Dialogues = new List<Dialogue>();



    public static DialogueContainer Load(string path){
        var serializer = new XmlSerializer(typeof(DialogueContainer));
        //Use Path.Combine(Application.streamingAssetsPath, path) with path being the name of your xml file and put the xml file in the "StreamingAssets" folder
        using(var stream = new FileStream(System.IO.Path.Combine(Application.streamingAssetsPath, path), FileMode.Open)){
            return serializer.Deserialize(stream) as DialogueContainer;
        }
    }

    //FIX ENCODING
    public void Save(string path){
        var serializer = new XmlSerializer(typeof(DialogueContainer));
        using (var stream = new StreamWriter(System.IO.Path.Combine(Application.streamingAssetsPath, path))){
            serializer.Serialize(stream, this);
        }
    }


    public Dialogue getDialogue(int id){
        return Dialogues[id];
    }

    
}

public class Dialogue
{
    [XmlArray(ElementName = "Texts")]
    [XmlArrayItem(typeof(Text), ElementName = "Text")]
    public List<Text> Texts = new List<Text>();

    public string getText(int textId)
    {
        return Texts[textId].value;
    }

    public int getSpeakerId(int textId)
    {
        return Texts[textId].SpeakerId;
    }
}

public class Text
{
    [XmlAttribute("speakerId")]
    public int SpeakerId;

    [XmlAttribute("value")]
    public string value;

    [XmlElement("Text")]
    public string mText;
}
