<?php

include_once __DIR__.'/settings.php';
include_once __DIR__ . '/commons.php';


$mandatory_arguments = array('hitId', 'workerId', 'assignmentId');

foreach ($mandatory_arguments as $argument) {
	if (!isset($_GET[$argument]) || strlen($_GET[$argument]) < 1) {
		echo sprintf("ERROR: the parameter '%s' is mandatory!", $argument);
		exit();
	}
}

$hitId = $_GET['hitId'];
$workerId = $_GET['workerId'];
$assignmentId = $_GET['assignmentId'];


$mysql = new \mysqli($MYSQL_HOST, $MYSQL_USERNAME, $MYSQL_PASSWORD, $MYSQL_DBNAME);


?>

<!doctype html>
<html lang="en">

<head>
	<meta charset="utf-8">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>Consent Form - NLIGEN Virtual Environment Simulator</title>

	<link rel="shortcut icon" href="TemplateData/favicon.ico"/>

	<link rel="stylesheet" href="http://<?php echo $BASE_HOST ?>/css/style.css">

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>

	<!-- Latest compiled and minified CSS -->
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"
		  integrity="sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7" crossorigin="anonymous">

	<!-- Latest compiled and minified JavaScript -->
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"
			integrity="sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS"
			crossorigin="anonymous"></script>

	<script src="http://<?php echo $BASE_HOST ?>/js/utils.js"></script>

	<style type="text/css">
		.consent_p {
			margin-bottom: 20px;
			text-align: justify;
		}

		.consent_span {
			font-weight: bold;
		}
	</style>
</head>


<body>


<?php


if ($mysql->connect_error != NULL) {
	openDialog('Error!', $mysql->connect_error, 'Please, close the page!', null);
}

// verify if the argument 'consent' is present
if(isset( $_GET['consent'] )){
	// store the agreement in the DB
	if (!$TEST_MODE) {
		$query = sprintf('INSERT INTO `consent`(workerId, granted) VALUES (\'%s\',1)', $workerId);

		$res = execINSERT($query);

		if (!$res['success']) {
			openDialog('Error', $res['data'], 'Please, close the page!', null);
		}
	}
	// redirect to index.php
	?>
	<p style="text-align: center">Please wait...</p>
	<script type="application/javascript">
		location.href = 'http://<?php echo $BASE_HOST ?>/index.php?<?php echo str_replace( '&consent=1', '', $_SERVER['QUERY_STRING'] ) ?>';
	</script>
	<?php
}else{
	// ask the user to agree
	$query = sprintf('SELECT * FROM `consent` c WHERE c.workerId = \'%s\' AND c.granted = TRUE', $workerId);

	$res = execSELECT($query);

	if (!$res['success']) {
		openDialog('Error', $res['data'], 'Please, close the page!', null);
	}

	if ($res['size'] == 0) {
		// open Consent Dialog
		$filepath = __DIR__.'/consent.html';
		$consent_html = file_get_contents( $filepath );
		//
		$consent_html = '<div style="width:100%; height:400px; overflow-y:scroll; padding:0 20px 0 10px">'.$consent_html.'</div>';
		//
		//TODO: bypass legend dialog. openDialog('Consent Form', $consent_html, 'I Agree', '$(\'#messageModal\').modal(\'hide\'); $(\'#instructionsModal\').modal({backdrop:\'static\',keyboard:false,show:true});', 'modal-lg', true);
		openDialog('Consent Form', $consent_html, 'I Agree', 'location.href=\'http://'.$BASE_HOST.'/consent.php?'.$_SERVER['QUERY_STRING'].'&consent=1\'', 'modal-lg', true);
	}else{
		// redirect to index.php
		?>
		<p style="text-align: center">Please wait...</p>
		<script type="application/javascript">
			location.href = 'http://<?php echo $BASE_HOST ?>/index.php?<?php echo str_replace( '&consent=1', '', $_SERVER['QUERY_STRING'] ) ?>';
		</script>
		<?php
	}
}

?>


<div class="modal fade" id="instructionsModal" tabindex="-1" role="dialog">
	<div class="modal-dialog">
		<div class="modal-content">
			<div class="modal-header">
				<h4 class="modal-title">Instructions</h4>
			</div>
			<div class="modal-body">
				<p>
				<div style="width:100%; height:400px; overflow-y:scroll; padding:0 20px 0 10px; text-align:center">
					<img src="http://<?php echo $BASE_HOST ?>/images/instructions.jpg" style="width:80%">
				</div>
				</p>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-primary" style="padding: 6px 40px"
						onclick="<?php echo 'location.href=\'http://'.$BASE_HOST.'/consent.php?'.$_SERVER['QUERY_STRING'].'&consent=1\'' ?>">Got it!</button>
			</div>
		</div>
		<!-- /.modal-content -->
	</div>
	<!-- /.modal-dialog -->
</div><!-- /.modal -->


</body>
</html>
