"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.isLibrary = exports.buildConfigPath = exports.readWorkspaceSchema = void 0;
const schematics_1 = require("@angular-devkit/schematics");
const angular_1 = require("./angular");
function readWorkspaceSchema(tree) {
    const workspaceBuffer = tree.read('/angular.json') || tree.read('/workspace.json');
    if (!workspaceBuffer)
        throw new schematics_1.SchematicsException("[Workspace Not Found] Make sure you are running schematics at the root directory of your workspace and it has an angular.json file." /* Exception.NoWorkspace */);
    let workspaceSchema;
    try {
        workspaceSchema = JSON.parse(workspaceBuffer.toString());
    }
    catch (_) {
        throw new schematics_1.SchematicsException("[Invalid Workspace] The angular.json should be a valid JSON file." /* Exception.InvalidWorkspace */);
    }
    return workspaceSchema;
}
exports.readWorkspaceSchema = readWorkspaceSchema;
function buildConfigPath(project) {
    return `/${project.root}/config/src/`;
}
exports.buildConfigPath = buildConfigPath;
function isLibrary(project) {
    return project.extensions['projectType'] === angular_1.ProjectType.Library;
}
exports.isLibrary = isLibrary;
//# sourceMappingURL=workspace.js.map