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

	if (!isset($_POST['data'])) {
		echo sprintf("ERROR: the parameter POST['%s'] is mandatory!", 'data');
		exit();
	}

	$resultData = $_POST['data'];

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
		label.radio-checked:hover {
			background-color: #ffefc5;
			border-color: #eea236;
		}

		label.radio-checked.active:hover {
			background-color: #e5a54a;
			border-color: #eea236;
			color: white;
		}

		label.radio-checked.active {
			background-color: #f0ad4e;
			border-color: #eea236;
			color: white;
		}

		.darkg-bordered{
			border: 1px solid darkgray;
		}

		.darkg-bordered>.panel-heading{
			border-bottom: 1px solid darkgray;
			background-color: #EAE6E6;
		}
	</style>
</head>

<body>


<?php


if ($mysql->connect_error != NULL) {
	openDialog('Error!', $mysql->connect_error, 'Please, close the page!', null);
}

if (!$TEST_MODE) {
	$query = 'INSERT INTO submission(workerId, experimentId, hitId, assignmentId, submissionTime, resultData) ' .
		sprintf("VALUES ('%s', %d, '%s', '%s', DEFAULT, '%s')", $workerId, $experimentId, $hitId, $assignmentId, $resultData);
	//
	$res = execINSERT($query);
	//
	if (!$res['success']) {
		openDialog('Error!', $res['data'], 'Please, close the page!', null);
	}
}


?>


<br>

<div class="container">
	<div class="jumbotron" style="padding:40px">
		<h1 style="margin:0">Brief survey</h1>

		<p>
			Please, provide information about yourself and answer some questions about your experience.
		</p>

		<div>
			<br>

			<form
				action="http://<?php echo $BASE_HOST ?>/thanks.php?<?php echo $_SERVER['QUERY_STRING'] . '&experimentId=' . $experimentId ?>"
				method="POST" target="_top" id="resultForm">


			<?php
			$questions = array(
				1 => array(
					'name' => 'native',
					'question' => 'Are you a native English speaker? <span style="font-weight:normal"> &nbsp;(this will NOT affect your compensation for the HIT)</span>',
					'answers' => array('Yes', 'Do not disclose', 'No')
				),
				2 => array(
					'name' => 'gender',
					'question' => 'What is your gender?',
					'answers' => array('Male', 'Do not disclose', 'Female')
				),
				3 => array(
					'name' => 'age',
					'question' => 'How old are you?',
					'answers' => array('Do not disclose', '18-24', '25-29', '30-39', '40-49', '50-59', '60-69', '70-79', '80+')
				)
			);

			$sizes = array(
				3 => array(40, 20, 40),
				9 => array(20, 10, 10, 10, 10, 10, 10, 10, 10)
			);

			$default = array(
				1 => -1,
				2 => -1,
				3 => -1
			);

			// check if exists something about the current worker in our DB
			$query = sprintf('SELECT DISTINCT `native`, `gender`, `age` FROM `survey` WHERE `workerId`=\'%s\'', $workerId);

			$res = execSELECT($query);

			if ($res['success']) {
				if ($res['size'] == 1) {
					$default = array(
						1 => $res['data'][0]['native'],
						2 => $res['data'][0]['gender'],
						3 => -1// $res['data'][0]['age']
					);
				}
			}

			?>


			<div class="panel panel-primary" style="margin-bottom:40px;">
				<div class="panel-heading">
					<h3 class="panel-title"
						style="font-weight:bold">Information about yourself</h3>
				</div>
				<div class="panel-body" style="padding-bottom:0">

					<?php
					foreach ($questions as $k => $q) {
						$nanswers = sizeof($q['answers']);
						?>

						<div class="panel panel-default darkg-bordered" style="margin-bottom:40px;">
							<div class="panel-heading">
								<h3 class="panel-title"
									style="font-weight:bold"><?php echo $k . ' . ' . $q['question'] ?></h3>
							</div>
							<div class="panel-body">

								<div class="btn-group" data-toggle="buttons" style="width:100%">

									<?php
									$j = 0;
									$active_no = in_array( $k, array_keys($default) ) ? $default[$k] : intval($nanswers / 2);
									for ($j = 0; $j < $nanswers; $j++) {
										?>
										<label
											class="btn btn-default radio-checked <?php echo ($j == $active_no) ? 'active' : '' ?>"
											style="width:<?php echo $sizes[$nanswers][$j] ?>%">
											<input type="radio" name="<?php echo $q['name'] ?>"
												   id="<?php echo $q['name'] . '_' . $j ?>" value="<?php echo $j ?>"
												   autocomplete="off" required <?php echo ($j == $active_no) ? 'checked' : '' ?>>
											<?php echo $q['answers'][$j] ?>
										</label>
									<?php
									}
									?>

								</div>


							</div>
						</div>

					<?php
					}
					?>

				</div>
			</div>



				<?php
				$questions = array(
					4 => array(
						'name' => 'difficulty',
						'question' => 'How would you evaluate the task in terms of difficulty?',
						'answers' => array('Very Hard', 'Hard', 'Not so hard', 'Easy', 'Very Easy')
					),
					5 => array(
						'name' => 'backtrack',
						'question' => 'How many times did you have to backtrack?',
						'answers' => array('Very often', 'Often', 'A few times', 'Rarely', 'Never')
					),
					6 => array(
						'name' => 'who',
						'question' => 'Who do you think generated the instructions?',
						'answers' => array('A Computer', 'I don\'t know', 'A Human')
					),
					7 => array(
						'name' => 'information',
						'question' => 'How would you define the amount of information provided by the instructions?',
						'answers' => array('Too little', 'Enough', 'Too much')
					),
					8 => array(
						'name' => 'confidence',
						'question' => 'How confident are you that you followed the desired path?',
						'answers' => array('Not confident', 'Slightly unconfident', 'Neutral', 'Slightly confident', 'Confident')
					)
				);

				$sizes = array(
					3 => array(40, 20, 40),
					5 => array(20, 20, 20, 20, 20)
				);

				$default = array(
					4 => -1,
					5 => -1,
					6 => -1,
					7 => -1,
					8 => -1
				);

				?>


				<div class="panel panel-primary" style="margin-bottom:40px;">
					<div class="panel-heading">
						<h3 class="panel-title"
							style="font-weight:bold">Questions about your experience</h3>
					</div>
					<div class="panel-body" style="padding-bottom:0">

					<?php
					foreach ($questions as $k => $q) {
						$nanswers = sizeof($q['answers']);
						?>

						<div class="panel panel-default darkg-bordered" style="margin-bottom:40px;">
							<div class="panel-heading">
								<h3 class="panel-title"
									style="font-weight:bold"><?php echo $k . ' . ' . $q['question'] ?></h3>
							</div>
							<div class="panel-body">

								<div class="btn-group" data-toggle="buttons" style="width:100%">

									<?php
									$j = 0;
									$active_no = in_array( $k, array_keys($default) ) ? $default[$k] : intval($nanswers / 2);
									for ($j = 0; $j < $nanswers; $j++) {
										?>
										<label
											class="btn btn-default radio-checked <?php echo ($j == $active_no) ? 'active' : '' ?>"
											style="width:<?php echo $sizes[$nanswers][$j] ?>%">
											<input type="radio" name="<?php echo $q['name'] ?>"
												   id="<?php echo $q['name'] . '_' . $j ?>" value="<?php echo $j ?>"
												   autocomplete="off" required>
											<?php echo $q['answers'][$j] ?>
										</label>
									<?php
									}
									?>

								</div>


							</div>
						</div>

					<?php
					}
					?>

					</div>
				</div>


				<div class="panel panel-primary" style="margin-bottom:40px;">
					<div class="panel-heading">
						<h3 class="panel-title" style="font-weight:bold">
							Comments or suggestions ( Optional )</h3>
					</div>
					<div class="panel-body">

						<div class="btn-group" data-toggle="buttons" style="width:100%">

							<textarea name="suggestions" id="suggestions_0" rows="3"
									  style="resize:none; width:100%"></textarea>

						</div>


					</div>
				</div>


				<p style="text-align:right">
					<button type="submit" class="btn btn-warning btn-lg"><span class="glyphicon glyphicon-send"
																			   aria-hidden="true"></span> &nbsp; Submit
					</button>
				</p>

			</form>

		</div>
	</div>
</div>


</body>
</html>
