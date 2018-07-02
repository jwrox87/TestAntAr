using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Text;

[XmlRoot("SpeechBubbleCollection")]
public class SpeechBubbleContainer
{
    [XmlArray("SpeechBubbles"), XmlArrayItem("SpeechBubble")]
    public List<SpeechBubble> SpeechBubbles = new List<SpeechBubble>();


    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(SpeechBubbleContainer));
        var encoding = Encoding.GetEncoding("UTF-8");

        using (var stream = new StreamWriter(path,false,encoding))
        {
            serializer.Serialize(stream, this);
        }

    }

    public static SpeechBubbleContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(SpeechBubbleContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as SpeechBubbleContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static SpeechBubbleContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(SpeechBubbleContainer));
        return serializer.Deserialize(new StringReader(text)) as SpeechBubbleContainer;
    }


    public SpeechBubble Access(int index)
    {
        return SpeechBubbles[index];
    }
}
