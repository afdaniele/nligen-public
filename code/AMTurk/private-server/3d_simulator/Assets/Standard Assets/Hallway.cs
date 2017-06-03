using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Hallway {

	[XmlAttribute("node1")]
	public string from;

	[XmlAttribute("node2")]
	public string to;

	[XmlAttribute("wall")]
	public string wall;

	[XmlAttribute("floor")]
	public string floor;


	public Intersection getNode1(){
		Intersection res = new Intersection ();
		string[] components = from.Split (',');
		res.x = int.Parse( components[0] );
		res.y = int.Parse( components[1] );
		//
		return res;
	}

	public Intersection getNode2(){
		Intersection res = new Intersection ();
		string[] components = to.Split (',');
		res.x = int.Parse( components[0] );
		res.y = int.Parse( components[1] );
		//
		return res;
	}

}
