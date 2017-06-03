<?php
/**
 * Created by PhpStorm.
 * User: andrea
 * Date: 6/7/16
 * Time: 5:39 PM
 */

include_once __DIR__ . '/settings.php';
include_once __DIR__ . '/commons.php';


// Check the GET data

$mandatory_arguments = array('hitId', 'workerId', 'assignmentId', 'experimentId');

foreach ($mandatory_arguments as $argument) {
	if (!isset($_GET[$argument]) || strlen($_GET[$argument]) < 1) {
		echo sprintf("ERROR: the parameter '%s' is mandatory!", $argument);
		exit();
	}
}

$hitId = $_GET['hitId'];
$workerId = $_GET['workerId'];
$assignmentId = $_GET['assignmentId'];
$experimentId = $_GET['experimentId'];


if (!$TEST_MODE) {

	// Check the POST data

	$mandatory_arguments = array('native', 'gender', 'age', 'difficulty', 'backtrack', 'who', 'information', 'confidence', 'suggestions');

	foreach ($mandatory_arguments as $argument) {
		if (!isset($_POST[$argument])) {
			echo sprintf("ERROR: the parameter '%s' is mandatory!", $argument);
			exit();
		}
	}

	# demographic information
	$native = $_POST['native'];
	$gender = $_POST['gender'];
	$age = $_POST['age'];
	# information about the user experience
	$difficulty = $_POST['difficulty'];
	$backtrack = $_POST['backtrack'];
	$who = $_POST['who'];
	$information = $_POST['information'];
	$confidence = $_POST['confidence'];
	$suggestions = base64_encode( $_POST['suggestions'] );

}


// ================================================================================


$mysql = new \mysqli($MYSQL_HOST, $MYSQL_USERNAME, $MYSQL_PASSWORD, $MYSQL_DBNAME);


?>


<!doctype html>
<html lang="en">

<head>
	<meta charset="utf-8">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>NLIGEN Virtual Environment Simulator</title>

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>

	<!-- Latest compiled and minified CSS -->
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"
		  integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

	<!-- Latest compiled and minified JavaScript -->
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"
			integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"
			crossorigin="anonymous"></script>

	<style type="text/css">
		input.text-area:disabled {
			background-color: white;
		}

		input.text-area:hover {
			cursor: text;
		}
	</style>

</head>

<body>


<br><br>

<?php


if ($mysql->connect_error != NULL) {
	openDialog('Error!', $mysql->connect_error, 'Please, close the page!', null);
}


$query = 'INSERT INTO survey(workerId, experimentId, hitId, assignmentId, submissionTime, native, gender, age, difficulty, backtrack, who, information, confidence, suggestions) ' .
	sprintf("VALUES ('%s', %d, '%s', '%s', DEFAULT, %d, %d, %d, %d, %d, %d, %d, %d, '%s')", $workerId, $experimentId, $hitId, $assignmentId, $native, $gender, $age, $difficulty, $backtrack, $who, $information, $confidence, $suggestions);

if (!$TEST_MODE) {
	$res = execINSERT($query);
	//
	if (!$res['success']) {
		openDialog('Error!', $res['data'], 'Please, close the page!', null);
	}
}

//
if ($SYSTEM == 'local') {
	// local system, load a new HIT
	?>
		<p style="text-align: center">Please wait...</p>
		<script type="application/javascript">
			location.href = 'http://<?php echo $BASE_HOST ?>/index.php?hitId=<?php echo $_GET['hitId'] ?>&assignmentId=<?php echo $_GET['assignmentId'] ?>&workerId=<?php echo $_GET['workerId'] ?>&exId=<?php echo $_GET['exId'] ?>';
		</script>
	<?php
} else {
	// amturk system, thanks and provide the secret code
	?>

	<br><br>

	<div class="container">
		<div class="jumbotron">
			<h1>Thank you!</h1>

			<p>Thank you so much for your contribution.<br>Copy the secret key in the box below and paste it in Amazon
				Mechanical Turk to receive credit.</p>

			<p style="text-align: center">
				<br>

			<div class="input-group" style="margin-left: 240px;">
			<span class="input-group-addon" id="basic-addon3"
				  style="height: 50px; background-color:rgba(128, 128, 128, 0.32); font-weight:bold">Your secret key:</span>
				<input type="text" class="form-control text-area" id="basic-url" aria-describedby="basic-addon3"
					   style="width: 420px; font-size: 16pt; height: 50px;"
					   value="<?php echo md5(md5($assignmentId)) ?>">
			</div>
			</p>
		</div>
	</div>

<?php
}
?>

</body>
</html>