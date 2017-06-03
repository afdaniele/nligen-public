using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Experiment {

	public string mapName { get; set; }
	public int initialX { get; set; }
	public int initialY { get; set; }
	public int initialTheta { get; set; }
	public bool labels { get; set; }
	public bool tutorial { get; set; }
	public List<Checkpoint> checkpoints { get; set; }

	public Experiment(){}

}


public class Checkpoint {

	public float time { get; set; }
	public float x { get; set; }
	public float y { get; set; }
	public float theta { get; set; }

	public Checkpoint(){}
		
	public Checkpoint( float time, float x, float y, float theta ){
		this.time = time;
		this.x = x;
		this.y = y;
		this.theta = theta;
	}

}