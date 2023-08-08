"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.addProviderToNgModule = exports.addProviderToModuleWithProvider = void 0;
const schematics_1 = require("@angular-devkit/schematics");
const ts = require("typescript");
const angular_1 = require("./angular");
const ast_1 = require("./ast");
const common_1 = require("./common");
function addProviderToModuleWithProvider(modulePath, _providerName, _providerPath) {
    return (tree, _context) => {
        const source = readModuleSource(tree, modulePath);
        const changes = (0, ast_1.addProviderToModuleWithProviderMetadata)(source, modulePath, _providerName, _providerPath);
        const recorder = tree.beginUpdate(modulePath);
        for (const change of changes)
            if (change instanceof angular_1.InsertChange)
                recorder.insertLeft(change.pos, change.toAdd);
        tree.commitUpdate(recorder);
        return tree;
    };
}
exports.addProviderToModuleWithProvider = addProviderToModuleWithProvider;
function addProviderToNgModule(modulePath, providerName, providerPath) {
    return (tree, _context) => {
        const source = readModuleSource(tree, modulePath);
        const changes = (0, angular_1.addProviderToModule)(source, modulePath, providerName, providerPath);
        const recorder = tree.beginUpdate(modulePath);
        for (const change of changes)
            if (change instanceof angular_1.InsertChange)
                recorder.insertLeft(change.pos, change.toAdd);
        tree.commitUpdate(recorder);
        return tree;
    };
}
exports.addProviderToNgModule = addProviderToNgModule;
function readModuleSource(tree, modulePath) {
    const text = tree.read(modulePath);
    if (text === null)
        throw new schematics_1.SchematicsException((0, common_1.interpolate)("[File Not Found] There is no file at \"{0}\" path." /* Exception.FileNotFound */, modulePath));
    const sourceText = text.toString('utf-8');
    return ts.createSourceFile(modulePath, sourceText, ts.ScriptTarget.Latest, true);
}
//# sourceMappingURL=provider.js.map