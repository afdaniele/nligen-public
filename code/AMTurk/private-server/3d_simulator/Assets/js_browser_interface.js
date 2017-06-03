function IN_passExperimentData ( data : String ) {
	/*
	var terrain = GameObject.Find("Terrain");
	var script : tutorial_control;
	script = terrain.GetComponent("tutorial_control");
	script.loadExperimentDetails (data);
	*/
	var frame = GameObject.Find("Frame");
	var script : intro_control;
	script = frame.GetComponent("intro_control");
	script.loadExperimentDetails (data);
}