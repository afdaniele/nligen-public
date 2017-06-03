<?php

include_once __DIR__ . '/settings.php';
include_once __DIR__ . '/commons.php';


$mandatory_arguments = array('hitId', 'workerId', 'assignmentId', 'exId');

foreach ($mandatory_arguments as $argument) {
	if (!isset($_GET[$argument]) || strlen($_GET[$argument]) < 1) {
		echo sprintf("ERROR: the parameter '%s' is mandatory!", $argument);
		exit();
	}
}

$hitId = $_GET['hitId'];
$workerId = $_GET['workerId'];
$assignmentId = $_GET['assignmentId'];
$exId = $_GET['exId'];


$mysql = new \mysqli($MYSQL_HOST, $MYSQL_USERNAME, $MYSQL_PASSWORD, $MYSQL_DBNAME);


?>

<!doctype html>
<html lang="en">

<head>
	<meta charset="utf-8">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>NLIGEN Virtual Environment Simulator</title>

	<link rel="stylesheet" href="TemplateData/style.css">

	<link rel="shortcut icon" href="TemplateData/favicon.ico"/>

	<link rel="stylesheet" href="http://<?php echo $BASE_HOST ?>/css/style.css">

	<script src="http://<?php echo $BASE_HOST ?>/TemplateData/UnityProgress.js"></script>

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>

	<!-- Latest compiled and minified CSS -->
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"
		  integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

	<!-- Latest compiled and minified JavaScript -->
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"
			integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"
			crossorigin="anonymous"></script>

	<script src="http://<?php echo $BASE_HOST ?>/js/jquery.base64.js"></script>

	<script src="http://<?php echo $BASE_HOST ?>/js/utils.js"></script>

</head>


<body>


<?php


if ($mysql->connect_error != NULL) {
	openDialog('Error!', $mysql->connect_error, 'Please, close the page!', null);
}





// verify if the user agreed the policy
$query = sprintf('SELECT * FROM `consent` c WHERE c.workerId = \'%s\' AND c.granted = TRUE', $workerId);

$res = execSELECT($query);

if (!$res['success']) {
	openDialog('Error', $res['data'], 'Please, close the page!', null);
}

if ($res['size'] == 0 && !$TEST_MODE) {
	// open Consent Dialog
	?>
	<p style="text-align: center">Please wait...</p>
	<script type="application/javascript">
		location.href = 'http://<?php echo $BASE_HOST ?>/consent.php?<?php echo $_SERVER['QUERY_STRING'] ?>';
	</script>
	<?php
	die();
	exit();
}






# check if the worker completed another task with the same ex_id
$query = sprintf('SELECT COUNT(*) as counter FROM `submission` s, `experiment` z WHERE s.experimentId = z.id AND s.workerId = \'%s\' AND z.ex_id = \'%s\'', $workerId, $exId);

$res = execSELECT($query);

if (!$res['success']) {
	openDialog('Error!', $res['data'], 'Please, close the page!', null);
}

if (intval($res['data'][0]['counter']) > 0) {
	openDialog('HIT not available', sprintf('This HIT is no longer available for the current user (workerId:%s).', $workerId), 'Please, close the page!', null);
}







/*  OLD version
# return a random experiment such that the same worker did not work on the same ex_id before
$query = sprintf('SELECT * FROM `experiment` e WHERE NOT EXISTS( SELECT * FROM `submission` s, `experiment` z WHERE s.experimentId = z.id AND  s.workerId = \'%s\' AND z.ex_id = e.ex_id ) ORDER BY RAND() LIMIT 1', $workerId);
*/

# return the experiment with ex_id=exId and smallest number of submissions
$query = sprintf('SELECT *, (SELECT COUNT(*) FROM `submission` s1 WHERE s1.experimentId = e.id) as submissionsNo FROM `experiment` e WHERE e.ex_id = \'%s\' ORDER BY submissionsNo ASC, RAND() LIMIT 1', $exId);

$res = execSELECT($query);

if (!$res['success']) {
	openDialog('HIT not available', $res['data'], 'Please, close the page!', null);
}

if ($res['size'] == 0) {
	openDialog('HIT not available', sprintf('This HIT is no longer available for the current user (workerId:%s).', $workerId), 'Please, close the page!', null);
}

$experimentId = $res['data'][0]['id'];
$experimentDataEncoded = $res['data'][0]['data'];

$experimentDataJSON = base64_decode($experimentDataEncoded);
$experiment = json_decode($experimentDataJSON, true);






// verify if it is the first time this worker performs a HIT
// $query = sprintf('SELECT count(*) as counter FROM `submission` s WHERE s.workerId = \'%s\' AND s.submissionTime > \'2016-09-10 00:00:00\'', $workerId);
$query = sprintf('SELECT count(*) as counter FROM `submission` s WHERE s.workerId = \'%s\'', $workerId);

$res = execSELECT($query);

if (!$res['success']) {
	openDialog('Error', $res['data'], 'Please, close the page!', null);
}

$tutorial = true;
if ($res['data'][0]['counter'] > 0) {
	$tutorial = false;
}






// generate data package for WebGL
$experimentDataForWebGL = array(
	'mapName' => $experiment['mapName'],
	'initialX' => $experiment['initialX'],
	'initialY' => $experiment['initialY'],
	'initialTheta' => $experiment['initialTheta'],
	'labels' => false,  // $experiment['labels'],
	'tutorial' => $tutorial
);

$experimentDataForWebGLJSON = json_encode($experimentDataForWebGL);

$experimentDataForWebGLJSONencoded = base64_encode($experimentDataForWebGLJSON);






function generateInstructions( $experiment ){
	if( is_null($experiment) ){
		$experiment = array( // tutorial instructions
			'instructions' => array( 'face the grass carpet', 'go to the chair', 'face the red brick carpet', 'move until you see yellow floor to your right', 'turn right', 'walk forward once' )
		);
	}

	?>

	<div class="input-group" style="width:100%">

		<table style="width:100%">

			<?php
			$k = 1;
			foreach ($experiment['instructions'] as $instruction) {
				?>
				<tr>
					<td>

						<div class="input-group" style="margin: 4px 0;">
											<span class="input-group-addon">
												<span
													style="font-weight:bold; font-size: 13pt;"> <?php echo $k ?></span>
											</span>

							<div class="form-control" style="font-size:10.2pt; padding:6px 8px; min-width:350px; float:none">
								<?php echo $instruction ?>
							</div>
						</div>

					</td>
				</tr>
				<?php
				$k += 1;
			}
			?>

		</table>
	</div>

	<?php
}

?>







<div class="" style="padding:10px">
	<div>
		<table style="width:100%">
			<tr>
				<td style="width:100%">
					<span style="font-weight:bold">Virtual Environment Simulator:</span>
				</td>
				<td style="width:40px">
					<img src="http://<?php echo $BASE_HOST ?>/images/separator.jpg">
				</td>
				<td style="min-width:400px; width:400px">
					<span style="font-weight:bold">Route Instructions:</span>
				</td>
			</tr>
			<tr style="height:1px">
				<td rowspan="3">
					<canvas class="emscripten" id="canvas" oncontextmenu="event.preventDefault()" style="width:100%"></canvas>
				</td>
				<td style="background-image: url('http://<?php echo $BASE_HOST ?>/images/separator.jpg'); background-repeat: repeat-y">
					<img src="http://<?php echo $BASE_HOST ?>/images/separator.jpg">
				</td>

				<td style="vertical-align:top">

					<div id="placeholder_block">

						<?php
						if( $tutorial ){

						generateInstructions( null );

						}else{
							?>

							<img src="http://<?php echo $BASE_HOST ?>/images/loader.gif" style="width:350px">

						<?php
						}
						?>

					</div>


					<div id="instructions_block" style="display:none">

						<?php
						generateInstructions( $experiment );
						?>

					</div>

				</td>
			</tr>

			<tr style="height:60px">
				<td style="background-image: url('http://<?php echo $BASE_HOST ?>/images/separator.jpg'); background-repeat: repeat-y">
					<img src="http://<?php echo $BASE_HOST ?>/images/separator.jpg">
				</td>
				<td>
					<span style="font-weight:bold">Legend:</span>
				</td>
			</tr>

			<tr>
				<td style="background-image: url('http://<?php echo $BASE_HOST ?>/images/separator.jpg'); background-repeat: repeat-y">
					<img src="http://<?php echo $BASE_HOST ?>/images/separator.jpg">
				</td>
				<td rowspan="2">

					<div id="legend" style="text-align: center">
						<img src="http://<?php echo $BASE_HOST ?>/images/legend.jpg">
					</div>

				</td>
			</tr>

			<tr style="height:1px">
				<td>

					<div id="commands">
						<table style="width:100%; margin-top:10px">
							<tr>
								<td style="width:50%; text-align:left">
									<img src="http://<?php echo $BASE_HOST ?>/images/default_commands.jpg">
								</td>
								<td style="width:50%; text-align:right">
									<img src="http://<?php echo $BASE_HOST ?>/images/finish_instructions.jpg">
								</td>
							</tr>
						</table>
					</div>

				</td>
				<td style="background-image: url('http://<?php echo $BASE_HOST ?>/images/separator.jpg'); background-repeat: repeat-y">
					<img src="http://<?php echo $BASE_HOST ?>/images/separator.jpg">
				</td>
			</tr>


		</table>
	</div>
</div>

<form
	action="http://<?php echo $BASE_HOST ?>/survey.php?<?php echo $_SERVER['QUERY_STRING'] . '&experimentId=' . $experimentId ?>"
	method="POST" target="_top" id="resultForm">
	<input type="hidden" name="data" id="data_input"/>
</form>

<script type='text/javascript'>
	var Module = {
		TOTAL_MEMORY: 268435456,
		errorhandler: null,			// arguments: err, url, line. This function must return 'true' if the error is handled, otherwise 'false'
		compatibilitycheck: null,
		dataUrl: "Release/WebGL.data",
		codeUrl: "Release/WebGL.js",
		memUrl: "Release/WebGL.mem"
	};
</script>
<script src="http://<?php echo $BASE_HOST ?>/Release/UnityLoader.js"></script>


<script type="application/javascript">
	var experimentData = '<?php echo $experimentDataForWebGLJSONencoded ?>';

	var resultData = {
		'hitId': '<?php echo $hitId ?>',
		'workerId': '<?php echo $workerId ?>',
		'assignmentId': '<?php echo $assignmentId ?>',
		'checkpoints': []
	};

	$(window).resize( function(){
		$('#canvas').css('width', '100%');
	});

</script>
<script src="js/browser_controller.js"></script>

</body>
</html>
