import { workspaces } from '@angular-devkit/core';
import { Tree } from '@angular-devkit/schematics';
import { WorkspaceSchema } from './angular';
export declare function readWorkspaceSchema(tree: Tree): WorkspaceSchema;
export declare function buildConfigPath(project: workspaces.ProjectDefinition): string;
export declare function isLibrary(project: workspaces.ProjectDefinition): boolean;
