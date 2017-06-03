<?php

include_once __DIR__.'/settings.php';
include_once __DIR__ . '/commons.php';


$message = array('200', 'Ok', 'Ok', null);
$mandatory_arguments = array( 'workerId', 'experimentId' );

foreach ($mandatory_arguments as $argument) {
	if (!isset($_GET[$argument]) || strlen($_GET[$argument]) < 1) {
		$message = array('400', 'Bad Request', sprintf("ERROR: the parameter '%s' is mandatory!", $argument), null);
	}
}

$prettyPrint = false;
if ( $message[0] == '200' ){

	// collect data
	$workerId = $_GET['workerId'];
	$experimentId = $_GET['experimentId'];

	if( isset($_GET['prettyPrint']) and $_GET['prettyPrint'] == '1' ){
		$prettyPrint = true;
	}


	$mysql = new \mysqli($MYSQL_HOST, $MYSQL_USERNAME, $MYSQL_PASSWORD, $MYSQL_DBNAME);

	if ($mysql->connect_error != NULL ){

		$message = array('500', 'Internal Error', $mysql->connect_error, null);

	}else{

		$query = "SELECT e.ex_id, e.who, e.label, e.data as experimentData, ".
			"s.workerId, s.experimentId, s.hitId, s.assignmentId, s.submissionTime, s.resultData, ".
			"z.native, z.gender, z.age, z.difficulty, z.backtrack, z.who as guessWho, z.information, z.confidence, z.suggestions ".
			"FROM `experiment` e, `submission` s, `survey` z ".
			sprintf( "WHERE e.id = s.experimentId AND s.workerId = '%s' AND s.experimentId = '%s' AND ", $workerId, $experimentId ).
			sprintf( "e.id = z.experimentId AND z.workerId = '%s'", $workerId );

			$res = execSELECT($query);

			// verify that a survey record exists
			if ( $res['success'] and sizeof($res['data']) == 0 ) {
				// try to retrieve info about everything but survey
				$query = "SELECT e.ex_id, e.who, e.label, e.data as experimentData, ".
					"s.workerId, s.experimentId, s.hitId, s.assignmentId, s.submissionTime, s.resultData, ".
					"-1 as native, -1 as gender, -1 as age, -1 as difficulty, -1 as backtrack, -1 as guessWho, -1 as information, -1 as confidence, '' as suggestions ".
					"FROM `experiment` e, `submission` s ".
					sprintf( "WHERE e.id = s.experimentId AND s.workerId = '%s' AND s.experimentId = '%s'", $workerId, $experimentId );

				$res = execSELECT($query);
			}


		if (!$res['success']) {

			$message = array('500', 'Internal Error', $res['data'], null);

		}else{
			// create message
			$message = array('200', 'Ok', 'Ok', $res['data']);
		}

	}

}



$data = array();

$data['code'] = $message[0];
$data['status'] = $message[1];
$data['message'] = $message[2];
$data['data'] = $message[3];

$data = json_encode( $data );
if( $prettyPrint ){
	$data = prettyPrint( json_encode( $data ) );
}

//
ob_clean();
//
header('HTTP/1.x 200 OK');
header('Connection: close');
header('Cache-Control: no-cache, no-store, must-revalidate');
header('Pragma: no-cache');
header('Expires: 0');
header('Content-Type: application/json; charset=UTF-8');
header("Content-Length: " . strlen($data) );
//
echo $data;
//
die();
exit;



?>