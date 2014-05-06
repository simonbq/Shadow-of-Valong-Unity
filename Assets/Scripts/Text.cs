using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


public class Text {
    [XmlAttribute("speakerId")]
    public int SpeakerId;

    [XmlAttribute("value")]
    public string value;

    [XmlElement("Text")]
    public string mText;
}
