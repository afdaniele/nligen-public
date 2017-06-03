<?php

include_once __DIR__.'/settings.php';
include_once __DIR__ . '/commons.php';


$message = array('200', 'Ok');
$mandatory_arguments = array('ex_id', 'who', 'label', 'data');

foreach ($mandatory_arguments as $argument) {
	if (!isset($_GET[$argument]) || strlen($_GET[$argument]) < 1) {
		$message = array('400', sprintf("Bad Request: (ERROR: the parameter '%s' is mandatory!", $argument));
	}
}

if ( $message[0] != '200' ){
	echo implode( '<br/>', $message );
	exit();
}


# collect data
$ex_id = $_GET['ex_id'];
$who = $_GET['who'];
$label = $_GET['label'];
$data = $_GET['data'];




$mysql = new \mysqli($MYSQL_HOST, $MYSQL_USERNAME, $MYSQL_PASSWORD, $MYSQL_DBNAME);


if ($mysql->connect_error != NULL ){

	$message = array('500', $mysql->connect_error);

}else{

	$query = sprintf("INSERT INTO `experiment`(`id`, `ex_id`, `who`, `label`, `data`) VALUES (DEFAULT, %d, '%s', '%s', '%s')", $ex_id, $who, $label, $data );

	$res = $mysql->query( $query );

	if( $res ){
		$message = array('200', 'Ok');
	}else{
		$message = array('500', $mysql->error);
	}

}


echo implode( '<br/>', $message )

?>