// modal.js
function openModal(url) {
    $.get(url, function (data) {
        $('#modalContainer').html(
            '<div class="modal-backdrop-custom"></div>' +
            '<div class="modal-content-custom">' + data + '</div>'
        );
        $('body').addClass('modal-open');
    });
}

function closeModal() {
    $('#modalContainer').html('');
    $('body').removeClass('modal-open');
}

$(document).on('click', '.modal-backdrop-custom', function () {
    closeModal();
});
