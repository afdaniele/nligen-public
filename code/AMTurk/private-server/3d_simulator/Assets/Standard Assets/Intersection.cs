using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Intersection {

	[XmlAttribute("x")]
	public int x;

	[XmlAttribute("y")]
	public int y;

	[XmlAttribute("item")]
	public string item;


	public Intersection(){
		this.x = -1;
		this.y = -1;
		this.item = "";
	} 

	public Intersection( int _x, int _y, string _item ){
		this.x = _x;
		this.y = _y;
		this.item = _item;
	} 
		
}
