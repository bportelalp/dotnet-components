export function showModal(modalId) {
    const modalHtml = document.getElementById(modalId);
    const modal = new bootstrap.Modal(modalHtml);
    modal.show(modalHtml);
}

export function hideModal(modalId) {
    const modalHtml = document.getElementById(modalId);
    const modal = new bootstrap.Modal(modalHtml);
    modal.hide(modalHtml);
}