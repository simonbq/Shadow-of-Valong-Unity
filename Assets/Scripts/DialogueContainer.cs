using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

    public Dialogue getDialogue(int id){
        return Dialogues[id];
    }

    
}
