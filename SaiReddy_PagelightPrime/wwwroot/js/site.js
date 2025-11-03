function showToast(message, type = 'success') {
    const toastEl = document.getElementById('appToast');
    const messageEl = document.getElementById('toastMessage');
    const toast = new bootstrap.Toast(toastEl);

    toastEl.classList.remove('text-bg-success', 'text-bg-danger', 'text-bg-warning');
    if (type === 'error') toastEl.classList.add('text-bg-danger');
    else if (type === 'warning') toastEl.classList.add('text-bg-warning');
    else toastEl.classList.add('text-bg-success');

    messageEl.textContent = message;
    toast.show();
}