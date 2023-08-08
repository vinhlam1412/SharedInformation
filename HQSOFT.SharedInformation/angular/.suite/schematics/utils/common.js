"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.checkIsMS = exports.checkIsModule = exports.isString = exports.isNullOrUndefined = exports.interpolate = exports.buildAbsolutePath = void 0;
const core_1 = require("@angular-devkit/core");
const path = require("path");
const workspace_1 = require("./workspace");
function buildAbsolutePath(source) {
    return path.join(process.cwd(), (0, core_1.normalize)(source));
}
exports.buildAbsolutePath = buildAbsolutePath;
function interpolate(text, ...params) {
    params.forEach((param, i) => {
        const pattern = new RegExp('{\\s*' + i + '\\s*}');
        text = text.replace(pattern, String(param));
    });
    return text;
}
exports.interpolate = interpolate;
function isNullOrUndefined(value) {
    return value === null || value === undefined;
}
exports.isNullOrUndefined = isNullOrUndefined;
function isString(value) {
    return typeof value === 'string';
}
exports.isString = isString;
function checkIsModule(projectTemplate, project) {
    return projectTemplate === "module-pro" /* Template.Module */ && (0, workspace_1.isLibrary)(project);
}
exports.checkIsModule = checkIsModule;
function checkIsMS(projectTemplate) {
    return projectTemplate === "microservice-pro" /* Template.MS */;
}
exports.checkIsMS = checkIsMS;
//# sourceMappingURL=common.js.map