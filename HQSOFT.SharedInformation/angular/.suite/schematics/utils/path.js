"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.relativePathFromComponentToProxy = exports.relativePathFromComponentToProxyNamespace = exports.relativePathFromComponentToService = exports.relativePathFromComponentToModel = exports.relativePathFromComponentToEnum = exports.relativePathToSrcRoot = void 0;
const text_1 = require("./text");
const constants_1 = require("../constants");
function relativePathToSrcRoot(namespace) {
    const repeats = namespace ? namespace.split('.').length : 0;
    return '../../..' + '/..'.repeat(repeats);
}
exports.relativePathToSrcRoot = relativePathToSrcRoot;
function relativePathFromComponentToEnum(scope, namespace, e) {
    const path = relativePathFromComponentToProxyNamespace(scope, namespace, e.namespace);
    return path + `/${(0, text_1.kebab)(e.name)}.enum`;
}
exports.relativePathFromComponentToEnum = relativePathFromComponentToEnum;
function relativePathFromComponentToModel(scope, namespace) {
    const path = relativePathFromComponentToProxyNamespace(scope, namespace, namespace);
    return path + '/models';
}
exports.relativePathFromComponentToModel = relativePathFromComponentToModel;
function relativePathFromComponentToService(namespace, name) {
    const path = relativePathFromComponentToProxyNamespace(constants_1.DEFAULT_SCOPE, namespace, namespace);
    return path + `/${(0, text_1.kebab)(name)}.service`;
}
exports.relativePathFromComponentToService = relativePathFromComponentToService;
function relativePathFromComponentToProxyNamespace(scope, namespace, proxyNamespace) {
    return removeTrailingSlash(removeDoubleSlash(relativePathFromComponentToProxy(scope, namespace) + '/' + (0, text_1.dir)(proxyNamespace)));
}
exports.relativePathFromComponentToProxyNamespace = relativePathFromComponentToProxyNamespace;
function relativePathFromComponentToProxy(scope, namespace) {
    const repeats = namespace ? namespace.split('.').length : 0;
    const pathToProxy = '../..' + '/..'.repeat(repeats) + '/proxy';
    return scope ? `${pathToProxy}/${scope}` : pathToProxy;
}
exports.relativePathFromComponentToProxy = relativePathFromComponentToProxy;
function removeDoubleSlash(path) {
    return path.replace(/\/{2,}/g, '/');
}
function removeTrailingSlash(path) {
    return path.replace(/\/+$/, '');
}
//# sourceMappingURL=path.js.map