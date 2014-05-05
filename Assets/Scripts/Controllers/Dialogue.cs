using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class Dialogue{
    [XmlAttribute("id")]
    public int Id;

    [XmlArray("Texts")]
    [XmlArrayItem("Text")]
    public List<string> Texts = new List<string>();
}
