import { workspaces } from '@angular-devkit/core';
import { Tree } from '@angular-devkit/schematics';
import { Entity, NavigationProperty, PropertyDef, Type } from '../models';
export declare function createProxyJsonGenerator(params: ProxyJsonParams): (tree: Tree) => void;
export declare class ProxyJsonParams {
    solution: string;
    apiName: string;
    moduleName: string;
    projectPath: string;
    namespace: string;
    namePlural: string;
    name: string;
    entityBaseClass: string;
    entityPrimaryKey: string;
    entityPrimaryKeySimple: string;
    entityPath: string;
    enums: Record<string, Type>;
    props: PropertyDef[];
    navProps: PropertyDef[];
    shouldExportExcel: boolean;
    navPropsSuffix: string;
    navConnectionProps: PropertyDef[];
    navigationConnections: NavigationProperty[];
    checkConcurrency: boolean;
    constructor({ project, entity, ...options }: ProxyJsonParamOptions);
    private createProps;
    private createNavigationConnectionProps;
}
declare type ProxyJsonParamOptions = Pick<ProxyJsonParams, 'apiName' | 'moduleName' | 'solution'> & {
    project: workspaces.ProjectDefinition;
    entity: Entity;
};
export {};
