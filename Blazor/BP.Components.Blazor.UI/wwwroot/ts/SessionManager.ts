localStorage = window.localStorage;

export function setItem(key: string, value: string): void {
    localStorage.setItem(key, value);
}

export function getItem(key: string) : string {
    return localStorage.getItem(key);
}