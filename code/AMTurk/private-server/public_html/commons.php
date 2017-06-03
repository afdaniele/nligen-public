<?php
/**
 * Created by PhpStorm.
 * User: andrea
 * Date: 6/8/16
 * Time: 7:57 PM
 */



// enable test mode for certain workerIds
if ( isset($_GET['workerId']) &&  in_array( $_GET['workerId'], $TEST_WORKER_IDS ) ) {
	$TEST_MODE = true;
}





function execSELECT($query){
	global $mysql;
	try {
		$res = $mysql->query($query);
		if ($res instanceof \mysqli_result) {
			$array = array();
			$i = 0;
			while ($row = $res->fetch_assoc()) {
				$array[$i] = $row;
				$i++;
			}
			return array('success' => true, 'size' => $i, 'data' => $array);
		} else {
			return array('success' => false, 'data' => $mysql->error);
		}
	} catch (Exception $e) {
		return array('success' => false, 'data' => $mysql->error);
	}
}


function execUPDATE($query)
{
	global $mysql;
	try {
		$res = $mysql->query($query);
		if ($res) {
			return array('success' => true, 'data' => null);
		} else {
			return array('success' => false, 'data' => $mysql->error);
		}
	} catch (Exception $e) {
		return array('success' => false, 'data' => $mysql->error);
	}
}//execUPDATE


function execINSERT($query)
{
	global $mysql;
	$res = execUPDATE($query);
	if ($res['success']) {
		$res['insertID'] = $mysql->insert_id;
	}
	return $res;
}//execINSERT


function prettyPrint( $json ){
	$result = '';
	$level = 0;
	$in_quotes = false;
	$in_escape = false;
	$ends_line_level = NULL;
	$json_length = strlen( $json );

	for( $i = 0; $i < $json_length; $i++ ) {
		$char = $json[$i];
		$new_line_level = NULL;
		$post = "";
		if( $ends_line_level !== NULL ) {
			$new_line_level = $ends_line_level;
			$ends_line_level = NULL;
		}
		if ( $in_escape ) {
			$in_escape = false;
		} else if( $char === '"' ) {
			$in_quotes = !$in_quotes;
		} else if( ! $in_quotes ) {
			switch( $char ) {
				case '}': case ']':
				$level--;
				$ends_line_level = NULL;
				$new_line_level = $level;
				break;

				case '{': case '[':
				$level++;
				case ',':
					$ends_line_level = $level;
					break;

				case ':':
					$post = " ";
					break;

				case " ": case "\t": case "\n": case "\r":
				$char = "";
				$ends_line_level = $new_line_level;
				$new_line_level = NULL;
				break;
			}
		} else if ( $char === '\\' ) {
			$in_escape = true;
		}
		if( $new_line_level !== NULL ) {
			$result .= "\n".str_repeat( "\t", $new_line_level );
		}
		$result .= $char.$post;
	}

	return $result;
}//prettyPrint


function openDialog($title, $message, $buttonText, $buttonCmd, $size='', $return=false)
{
	?>
	<div class="modal fade modal-vertical-centered" id="messageModal" tabindex="-1" role="dialog">
		<div class="modal-dialog <?php echo $size ?>">
			<div class="modal-content">
				<div class="modal-header">
					<h4 class="modal-title"><?php echo $title ?></h4>
				</div>
				<div class="modal-body">
					<p><?php echo $message ?></p>
				</div>
				<div class="modal-footer">
					<?php if (!is_null($buttonCmd)) {
						?>
						<button type="button" class="btn btn-primary" style="padding: 6px 40px"
								onclick="<?php echo $buttonCmd ?>"><?php echo $buttonText ?></button>
					<?php
					} else {
						?>
						<span style="float:right; font-weight:bold"><?php echo $buttonText ?></span>
					<?php
					} ?>

				</div>
			</div>
			<!-- /.modal-content -->
		</div>
		<!-- /.modal-dialog -->
	</div><!-- /.modal -->

	<script type="application/javascript">
		$(document).ready(function () {
			$('#messageModal').modal({
				backdrop: 'static',
				keyboard: false,
				show: true
			});
			$('.modal-vertical-centered').each(centerModal);
		});
	</script>

	<?php
	if (!$return){
		die();
		exit();
	}
}

?>