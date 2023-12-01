function createNote() {
    $('#summernote').summernote();
}
function getCode() {
    return $('#summernote').summernote('code');
}
function setCode(changedCode) {
    $('#summernote').summernote('code', changedCode);
}
function destroyNote(){
    $('#summernote').summernote('destroy');
}
