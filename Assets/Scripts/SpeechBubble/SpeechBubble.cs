using System.Xml;
using System.Xml.Serialization;

public class SpeechBubble
{
    [XmlAttribute("text")]
    public string text;
    [XmlElement("delay")]
    public int delay; //showtime

    [XmlElement("identifier")]
    public string identifier; //identifier for audio (audio name)
}
