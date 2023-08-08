"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.resolveModuleOptions = exports.getRoutingModulePath = exports.getProject = exports.buildSelector = exports.buildRelativeModulePath = exports.addRouteDeclarationToNgModule = exports.addRoutingToModule = void 0;
const core_1 = require("@angular-devkit/core");
const schematics_1 = require("@angular-devkit/schematics");
const ts = require("typescript");
const ast_utils_1 = require("./angular/ast-utils");
const change_1 = require("./angular/change");
const find_module_1 = require("./angular/find-module");
const parse_name_1 = require("./angular/parse-name");
const workspace_1 = require("./angular/workspace");
const ast_1 = require("./ast");
const common_1 = require("./common");
const workspace_2 = require("./workspace");
function addRoutingToModule(options, isModuleTemplate = false) {
    return (tree) => {
        const parsedPath = (0, parse_name_1.parseName)(options.path, options.name);
        options.name = parsedPath.name;
        options.path = parsedPath.path;
        const routingModulePath = getRoutingModulePath(tree, options.module);
        return addRouteDeclarationToNgModule(options, routingModulePath, isModuleTemplate);
    };
}
exports.addRoutingToModule = addRoutingToModule;
function addRouteDeclarationToNgModule(options, routingModulePath, isModuleTemplate = false) {
    return (tree) => {
        if (!options.module)
            return tree;
        const modulePath = routingModulePath ?? options.module;
        const text = tree.read(modulePath);
        if (!text)
            throw new Error((0, common_1.interpolate)("[Module Not Found] Please check if a module called {0} exists in the {1} project." /* Exception.NoModule */, modulePath, String(options.project)));
        const sourceText = text.toString();
        const source = ts.createSourceFile(modulePath, sourceText, ts.ScriptTarget.Latest, true);
        if ((0, ast_1.shouldAddRouteDeclaration)(source, options.route)) {
            const moduleName = `${options.name}Module`;
            const relativePath = buildRelativeModulePath(options, options.module);
            const changes = [];
            let loadChildren;
            if (isModuleTemplate) {
                loadChildren = `load${moduleName}AsChild`;
                changes.push((0, ast_utils_1.insertImport)(source, modulePath, loadChildren, relativePath));
            }
            else {
                loadChildren = `() => import('${relativePath}').then(m => m.${moduleName})`;
            }
            const route = `{ path: '${options.route}', loadChildren: ${loadChildren} }`;
            changes.push((0, ast_utils_1.addRouteDeclarationToModule)(source, modulePath, route));
            const recorder = tree.beginUpdate(modulePath);
            for (const change of changes) {
                if (change instanceof change_1.InsertChange)
                    recorder.insertLeft(change.pos, change.toAdd);
            }
            tree.commitUpdate(recorder);
        }
        return tree;
    };
}
exports.addRouteDeclarationToNgModule = addRouteDeclarationToNgModule;
function buildRelativeModulePath(options, modulePath) {
    const nameKebabCase = core_1.strings.dasherize(options.name);
    const importModulePath = (0, core_1.normalize)(`/${options.path}/${nameKebabCase}/${nameKebabCase}.module`);
    return (0, find_module_1.buildRelativePath)(modulePath, importModulePath);
}
exports.buildRelativeModulePath = buildRelativeModulePath;
function buildSelector(name, prefix) {
    const nameKebabCase = core_1.strings.dasherize(name);
    return prefix ? `${prefix}-${nameKebabCase}` : nameKebabCase;
}
exports.buildSelector = buildSelector;
async function getProject(tree, name) {
    const workspace = await (0, workspace_1.getWorkspace)(tree);
    let definition;
    try {
        definition = workspace.projects.get(name);
    }
    catch (_) { }
    if (!definition)
        try {
            name = core_1.strings.dasherize(name);
            definition = workspace.projects.get(name);
        }
        catch (_) { }
    if (!definition)
        try {
            name = core_1.strings.camelize(name);
            definition = workspace.projects.get(name);
        }
        catch (_) { }
    if (!definition)
        try {
            name = (0, workspace_2.readWorkspaceSchema)(tree).defaultProject;
            definition = workspace.projects.get(name);
        }
        catch (_) { }
    if (!definition)
        throw new schematics_1.SchematicsException("[Project Not Found] A project matching entity solution name or a default project does not exist in your Angular workspace." /* Exception.NoProject */);
    return { name, definition };
}
exports.getProject = getProject;
function getRoutingModulePath(tree, modulePath) {
    const routingModulePath = modulePath.endsWith(find_module_1.ROUTING_MODULE_EXT)
        ? modulePath
        : modulePath.replace(find_module_1.MODULE_EXT, find_module_1.ROUTING_MODULE_EXT);
    return tree.exists(routingModulePath) ? (0, core_1.normalize)(routingModulePath) : undefined;
}
exports.getRoutingModulePath = getRoutingModulePath;
async function resolveModuleOptions(tree, options) {
    let project;
    ({ name: options.project, definition: project } = await getProject(tree, options.project));
    options.path = options.path ?? (0, core_1.normalize)((0, workspace_1.buildDefaultPath)(project) + `/${options.module || ''}`);
    options.route = options.route ?? core_1.strings.dasherize(options.name);
    options.module = (0, find_module_1.findModuleFromOptions)(tree, options);
    options.selector = options.selector ?? buildSelector(options.name, project.prefix);
    return { options, project };
}
exports.resolveModuleOptions = resolveModuleOptions;
//# sourceMappingURL=module.js.map