export function showToast(toastId, DotNet, options) {
    const toastHtml = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastHtml, options);
    toastHtml.addEventListener("hidden.bs.toast", () => {
        DotNet.invokeMethodAsync("OnHide", toastId);
    })
    toast.show();
}