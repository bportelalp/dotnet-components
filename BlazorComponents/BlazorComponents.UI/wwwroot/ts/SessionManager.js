"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getItem = exports.setItem = void 0;
localStorage = window.localStorage;
function setItem(key, value) {
    localStorage.setItem(key, value);
}
exports.setItem = setItem;
function getItem(key) {
    return localStorage.getItem(key);
}
exports.getItem = getItem;
//# sourceMappingURL=SessionManager.js.map