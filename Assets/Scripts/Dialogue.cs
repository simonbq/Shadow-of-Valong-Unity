using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Dialogue{
    [XmlAttribute("id")]
    public int Id;

    [XmlArray(ElementName = "Texts")]
    [XmlArrayItem(typeof(Text), ElementName = "Text")]
    public List<Text> Texts = new List<Text>();

}
