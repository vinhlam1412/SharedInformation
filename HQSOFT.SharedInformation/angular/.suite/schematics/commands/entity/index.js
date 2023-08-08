"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular-devkit/core");
const schematics_1 = require("@angular-devkit/schematics");
const constants_1 = require("../../constants");
const enums_1 = require("../../enums");
const paths = require("../../utils/path");
const cases = require("../../utils/text");
const templateUtils = require("../../utils/template");
const utils_1 = require("../../utils");
function crateLibIfNotExist(name) {
    return async (tree) => {
        const workspace = await (0, utils_1.getWorkspace)(tree);
        if (!workspace.projects.get(cases.kebab(name))) {
            return (0, schematics_1.chain)([
                (0, schematics_1.externalSchematic)(constants_1.ABP_SCHEMATICS_PATH, enums_1.ABPSchematics.CreateLib, {
                    packageName: name,
                    isModuleTemplate: true,
                    isSecondaryEntrypoint: false,
                    override: false,
                }),
            ]);
        }
    };
}
function default_1(params) {
    const isMS = (0, utils_1.checkIsMS)(params.template);
    const entity = (0, utils_1.readSourceJson)(params.source);
    const solution = params.target;
    const projectName = solution.split('.').pop(); // BookStore in Acme.BookStore
    const name = entity.Name;
    const namespace = entity.Namespace;
    let createLib = (0, schematics_1.noop)();
    if (isMS) {
        createLib = crateLibIfNotExist(projectName);
    }
    return (0, schematics_1.chain)([
        createLib,
        async (tree, _context) => {
            const { options, project } = await (0, utils_1.resolveModuleOptions)(tree, {
                project: projectName,
                name: entity.Name,
                route: core_1.strings.dasherize(entity.NamePlural),
                selector: params.selector,
            });
            if (namespace)
                options.path = `${options.path}/${cases.dir(namespace)}`;
            const isModule = (0, utils_1.checkIsModule)(params.template, project);
            const createEntityRoute = (0, utils_1.addRoutingToModule)(options, isModule);
            let providerPath = `./${cases.kebab(name)}`;
            if (isModule || isMS) {
                providerPath = `.`;
            }
            else if (namespace) {
                providerPath = `./${cases.dir(namespace)}/${cases.kebab(name)}`;
            }
            const injectProvider = isModule || isMS ? utils_1.addProviderToModuleWithProvider : utils_1.addProviderToNgModule;
            const addRouteProvider = injectProvider((0, utils_1.findModule)(tree, (0, core_1.normalize)(isModule || isMS ? (0, utils_1.buildConfigPath)(project) : (0, utils_1.buildDefaultPath)(project))), `${cases.macro(namespace)}_${cases.macro(name)}_ROUTE_PROVIDER`, `${providerPath}/providers/${cases.kebab(name)}-route.provider`);
            const resourceName = isModule || isMS ? projectName : '';
            const enums = (0, utils_1.getEnumsFromEntity)(solution, entity);
            const targetPath = (0, core_1.normalize)(options.path + paths.relativePathToSrcRoot(namespace));
            const entityTemplateOptions = new EntityTemplateOptions({
                solution,
                project: projectName,
                resourceName,
                selector: options.selector,
                entity,
                enums,
                projectTemplate: params.template,
                projectDefinition: project,
            });
            const createEntityFiles = (0, schematics_1.chain)([
                (0, utils_1.applyWithOverwrite)((0, schematics_1.url)('./files'), [
                    (0, schematics_1.applyTemplates)(entityTemplateOptions),
                    (0, schematics_1.move)(targetPath),
                ]),
            ]);
            const apiName = isModule || isMS ? projectName : 'default';
            const moduleName = isModule || isMS ? cases.camel(projectName) : 'app';
            const proxyJsonParams = new utils_1.ProxyJsonParams({
                project,
                solution,
                moduleName,
                apiName,
                entity,
            });
            const generateProxyJson = (0, utils_1.createProxyJsonGenerator)(proxyJsonParams);
            const sourceName = isModule || isMS ? '__default' : options.project;
            const serviceType = 'application';
            const generateProxy = (0, schematics_1.externalSchematic)(constants_1.ABP_SCHEMATICS_PATH, enums_1.ABPSchematics.Api, {
                module: moduleName,
                apiName: apiName,
                source: sourceName,
                target: options.project,
                serviceType: serviceType,
            });
            const generateIndex = (0, schematics_1.externalSchematic)(constants_1.ABP_SCHEMATICS_PATH, enums_1.ABPSchematics.ProxyIndex, {
                target: options.project,
            });
            const formatFilesFn = (_tree) => (0, utils_1.formatFiles)(_tree);
            return (0, schematics_1.branchAndMerge)((0, schematics_1.chain)([
                createLib,
                createEntityRoute,
                addRouteProvider,
                createEntityFiles,
                formatFilesFn,
                generateProxyJson,
                generateProxy,
                generateIndex,
            ]));
        },
    ]);
}
exports.default = default_1;
class EntityTemplateOptions {
    constructor(options) {
        Object.assign(this, cases, core_1.strings, paths, templateUtils, options);
        this.isModule = (0, utils_1.checkIsModule)(this.projectTemplate, this.projectDefinition);
        this.checkConcurrency = options.entity.CheckConcurrency;
        this.isMS = (0, utils_1.checkIsMS)(this.projectTemplate);
        this.scope = constants_1.DEFAULT_SCOPE;
        this.namespace = this.entity.Namespace;
        this.name = this.entity.Name;
        this.namePlural = this.entity.NamePlural;
        this.requiredPolicy = this.project + '.' + this.namePlural;
        this.menuIcon = this.entity.MenuIcon;
        this.navPropsSuffix =
            this.entity.NavigationProperties?.length || this.entity.NavigationConnections?.length
                ? constants_1.NAVIGATION_PROPERTY_SUFFIX
                : '';
        this.entityNamePrefix = this.navPropsSuffix ? cases.camel(this.name) + '.' : '';
        const namespacePath = cases.dir(this.namespace);
        const relativePath = `${namespacePath}/${cases.kebab(this.name)}`;
        if (this.isModule || this.isMS) {
            this.modulePath = `src/lib/${relativePath}`;
            this.configPath = 'config/src';
        }
        else {
            this.modulePath = `src/app/${relativePath}`;
            this.configPath = this.modulePath;
        }
        this.props = (0, utils_1.getPropsFromEntity)(this.solution, this.entity);
        this.navProps = (0, utils_1.getNavPropsFromEntity)(this.solution, this.entity);
        this.navConnections = (0, utils_1.getNavConnectionsFromEntity)(this.solution, this.entity);
        this.navEntities = (0, utils_1.getEntityRefsFromProps)(this.navProps);
    }
}
//# sourceMappingURL=index.js.map