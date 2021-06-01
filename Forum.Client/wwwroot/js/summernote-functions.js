function createNote() {
    $('#summernote').summernote();
}
function getCode() {
    return $('#summernote').summernote('code');
}
function destroyNote(){
    $('#summernote').summernote('destroy');
}
