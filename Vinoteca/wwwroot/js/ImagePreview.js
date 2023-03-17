$(document).ready(() => {
    if (profileImage.Length > 0){
    $("#image-preview").removeAttr("hidden");
    $("#img-container").removeAttr("hidden");
    $("#image-preview").attr("src", profileImage);
}
        })


$("#ImageProfile").change(evt => {
    const [file] = document.getElementById("ImageProfile").files

    if (file) {
        $("#image-preview").removeAttr("hidden");
        $("#img-container").removeAttr("hidden");
        $("#image-preview").removeAttr("src").attr("src", URL.createObjectURL(file));

    }
});