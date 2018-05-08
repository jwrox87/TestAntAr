using System.Xml;
using System.Xml.Serialization;

public class SpeechBubble
{
    [XmlAttribute("text")]
    public string text;
    [XmlElement("fadeTime")]
    public int fadeTime;

    public string Text
    {
        get { return text; }
    }
    public int FadeTime
    {
        get { return fadeTime; }
    }
}
