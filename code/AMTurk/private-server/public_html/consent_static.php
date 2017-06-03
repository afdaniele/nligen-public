<!doctype html>
<html lang="en">

<head>
	<meta charset="utf-8">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<title>Consent Form - NLIGEN Virtual Environment Simulator</title>

	<style type="text/css">
		.consent_p {
			margin-bottom: 46px;
			text-align: justify;
		}

		.consent_span {
			font-weight: bold;
		}
	</style>
</head>


<body>


<?php


// open Consent Dialog
$filepath = __DIR__.'/consent.html';
$consent_html = file_get_contents( $filepath );
echo $consent_html;

?>

</body>
</html>
