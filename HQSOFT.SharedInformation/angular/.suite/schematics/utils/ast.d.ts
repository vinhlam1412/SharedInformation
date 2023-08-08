import * as ts from 'typescript';
import { Change } from './angular';
export declare function addProviderToModuleWithProviderMetadata(source: ts.SourceFile, ngModulePath: string, symbolName: string, importPath?: string | null): Change[];
export declare function findModuleWithProviderDeclarations(source: ts.SourceFile): ts.MethodDeclaration[];
export declare function shouldAddRouteDeclaration(source: ts.SourceFile, route?: string): boolean;
