"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.overwriteFileIfExists = exports.noopRule = exports.deleteFileIfExists = exports.applyWithOverwrite = exports.absoluteUrl = void 0;
const schematics_1 = require("@angular-devkit/schematics");
const url_1 = require("url");
function absoluteUrl(urlString) {
    const url = (0, url_1.parse)(process.cwd() + '/' + urlString);
    return (context) => context.engine.createSourceFromUrl(url, context)(context);
}
exports.absoluteUrl = absoluteUrl;
function applyWithOverwrite(source, rules) {
    return (tree, _context) => {
        const rule = (0, schematics_1.mergeWith)((0, schematics_1.apply)(source, [...rules, overwriteFileIfExists(tree)]));
        return rule(tree, _context);
    };
}
exports.applyWithOverwrite = applyWithOverwrite;
function deleteFileIfExists(path) {
    return (tree, _context) => {
        if (tree.exists(path))
            tree.delete(path);
    };
}
exports.deleteFileIfExists = deleteFileIfExists;
const noopRule = (tree, _context) => tree;
exports.noopRule = noopRule;
function overwriteFileIfExists(tree) {
    return (0, schematics_1.forEach)(fileEntry => {
        if (!tree.exists(fileEntry.path))
            return fileEntry;
        tree.overwrite(fileEntry.path, fileEntry.content);
        return null;
    });
}
exports.overwriteFileIfExists = overwriteFileIfExists;
//# sourceMappingURL=rule.js.map