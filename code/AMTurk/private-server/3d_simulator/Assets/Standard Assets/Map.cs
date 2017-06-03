using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


[XmlRoot("map")]
public class Map {

	[XmlAttribute("name")]
	public string name;

	[XmlArray("nodes")]
	[XmlArrayItem("node")]
	public List<Intersection> nodes = new List<Intersection>();


	[XmlArray("edges")]
	[XmlArrayItem("edge")]
	public List<Hallway> hallways = new List<Hallway>();


	public static Map loadMap( string map_name ){
		string path = Path.Combine ("Maps", "map-");

		path += map_name;

		TextAsset _xml = Resources.Load<TextAsset> (path);

		XmlSerializer serializer = new XmlSerializer (typeof(Map));

		StringReader reader = new StringReader (_xml.text);

		Map map = serializer.Deserialize (reader) as Map;

		reader.Close ();

		return map;
	}

}
