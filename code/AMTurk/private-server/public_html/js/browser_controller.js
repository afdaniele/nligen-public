// refreshPage function
function refreshPage( arg ){
    console.log( 'Reload required!' );
    //TODO: location.reload();
}


// playerReady function
function playerReady( arg ){
    // send data
    SendMessage("Frame", "IN_passExperimentData", experimentData);
    // fix the canvas width
    $('#canvas').css('width', '100%');
}


// cancelButtonPressed function
function cancelButtonPressed( arg ){
    // remove the canvas from the page
    $('#canvas').remove();
    // ask the user to close the page
    alert('Now you can close the page!');
}


// finishButtonPressed function
function finishButtonPressed( arg ){
    result = $.base64.atob( arg );

    console.log( result );

    jsonRes = $.parseJSON( result );

    resultData['checkpoints'] = jsonRes['checkpoints'];

    resultDataJson = JSON.stringify( resultData );

    resultDataJsonEncoded = $.base64.btoa( resultDataJson );

    $('#data_input').val( resultDataJsonEncoded );
    jQuery('#resultForm').submit();
}


// showInstructions function
function showInstructions( arg ){
    $('#placeholder_block').css('display', 'none');
    $('#instructions_block').css('display', '');
}