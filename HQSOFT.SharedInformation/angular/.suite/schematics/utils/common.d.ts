import { Template } from '../enums';
import { ProjectDefinition } from '@angular-devkit/core/src/workspace';
export declare function buildAbsolutePath(source: string): string;
export declare function interpolate(text: string, ...params: (string | number | boolean)[]): string;
export declare function isNullOrUndefined(value: any): value is null | undefined;
export declare function isString(value: any): value is string;
export declare function checkIsModule(projectTemplate: Template, project: ProjectDefinition): boolean;
export declare function checkIsMS(projectTemplate: Template): boolean;
