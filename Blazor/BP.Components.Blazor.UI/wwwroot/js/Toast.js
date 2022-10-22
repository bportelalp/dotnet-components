//const toastTrigger = document.getElementById('liveToastBtn')
//const toastLiveExample = document.getElementById('liveToast')
//if (toastTrigger) {
//    toastTrigger.addEventListener('click', () => {
//        const toast = new bootstrap.Toast(toastLiveExample)

//        toast.show()
//    })
//}

export function showToast(toastId, DotNet) {
    const toastHtml = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastHtml);
    toastHtml.addEventListener("hidden.bs.toast", () => {
        DotNet.invokeMethodAsync("OnHide", toastId);
    })
    toast.show();
}