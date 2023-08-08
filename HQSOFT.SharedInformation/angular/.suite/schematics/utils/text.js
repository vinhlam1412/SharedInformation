"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.dir = exports.macro = exports.snake = exports.kebab = exports.pascal = exports.camel = exports.upper = exports.lower = void 0;
const core_1 = require("@angular-devkit/core");
const lower = (text) => text.toLowerCase();
exports.lower = lower;
const upper = (text) => text.toUpperCase();
exports.upper = upper;
const camel = (text) => toCamelCase(_(text));
exports.camel = camel;
const pascal = (text) => core_1.strings.classify(_(text));
exports.pascal = pascal;
const kebab = (text) => core_1.strings.dasherize(_(text));
exports.kebab = kebab;
const snake = (text) => core_1.strings.underscore(_(text));
exports.snake = snake;
const macro = (text) => (0, exports.upper)((0, exports.snake)(text));
exports.macro = macro;
const dir = (text) => core_1.strings.dasherize(text.replace(/\./g, '/').replace(/\/\//g, '/'));
exports.dir = dir;
function _(text) {
    return text.replace(/\./g, '_');
}
// https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Utilities/StringUtils.cs#L155
function toCamelCase(str) {
    if (!str || !isUpperCase(str[0]))
        return str;
    const chars = str.split('');
    const { length } = chars;
    for (let i = 0; i < length; i++) {
        if (i === 1 && !isUpperCase(chars[i]))
            break;
        const hasNext = i + 1 < length;
        if (i > 0 && hasNext && !isUpperCase(chars[i + 1])) {
            if (isSeparator(chars[i + 1])) {
                chars[i] = toLowerCase(chars[i]);
            }
            break;
        }
        chars[i] = toLowerCase(chars[i]);
    }
    return chars.join('');
}
function isSeparator(str = '') {
    return /[\s\u2000-\u206F\u2E00-\u2E7F\\'!"#$%&()*+,\-.\/:;<=>?@\[\]^_`{|}~]+/.test(str);
}
function isUpperCase(str = '') {
    return /[A-Z]+/.test(str);
}
function toLowerCase(str = '') {
    return str.toLowerCase();
}
//# sourceMappingURL=text.js.map