let localStorage = window.localStorage

export function setItem(key, value) {
    localStorage.setItem(key, value);
}

export function getItem(key) {
    return localStorage.getItem(key);
}

export function deleteItem(key) {
    localStorage.removeItem(key);
}