export async function swalPrompt(title, confirmText, denyText) {
    var response = false;
    await Swal.fire({
        title: title,
        showDenyButton: true,
        confirmButtonText: confirmText,
        denyButtonText: denyText,
    }).then((result) => {
        if (result.isConfirmed) {
            response = true;
        } else if (result.isDenied) {
            response = false;
        }
    });
    return response;
}