 
function centerModal() {
    $(this).css('display', 'block');
    var $dialog = $(this).find(".modal-dialog");
    var offset = ($(window).height() - $dialog.height()) / 2;
    // Center modal vertically in window
    $dialog.css("margin-top", offset);
}

$(document).on('ready', function(){
    $('.modal-vertical-centered').on('show.bs.modal', centerModal);
    $(window).on("resize", function () {
	$('.modal-vertical-centered:visible').each(centerModal);
    });
});