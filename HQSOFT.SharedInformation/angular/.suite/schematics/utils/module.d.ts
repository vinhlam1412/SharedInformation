import { Path, workspaces } from '@angular-devkit/core';
import { Rule, Tree } from '@angular-devkit/schematics';
import { ModuleOptions } from '../utils/angular/find-module';
export interface ABPModuleOptions extends ModuleOptions {
    project?: string;
    route?: string;
    selector?: string;
}
export declare function addRoutingToModule(options: ABPModuleOptions, isModuleTemplate?: boolean): Rule;
export declare function addRouteDeclarationToNgModule(options: ABPModuleOptions, routingModulePath: Path | undefined, isModuleTemplate?: boolean): Rule;
export declare function buildRelativeModulePath(options: ABPModuleOptions, modulePath: string): string;
export declare function buildSelector(name: string, prefix?: string): string;
export declare function getProject(tree: Tree, name: string): Promise<{
    name: string;
    definition: workspaces.ProjectDefinition;
}>;
export declare function getRoutingModulePath(tree: Tree, modulePath: string): Path | undefined;
export declare function resolveModuleOptions(tree: Tree, options: ABPModuleOptions): Promise<{
    options: ABPModuleOptions;
    project: ProjectDefinition;
}>;
declare type ProjectDefinition = workspaces.ProjectDefinition;
export {};
