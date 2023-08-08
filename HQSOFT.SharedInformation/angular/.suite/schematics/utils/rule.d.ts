import { Rule, SchematicContext, Source, Tree } from '@angular-devkit/schematics';
export declare function absoluteUrl(urlString: string): (context: SchematicContext) => Tree | any;
export declare function applyWithOverwrite(source: Source, rules: Rule[]): Rule;
export declare function deleteFileIfExists(path: string): Rule;
export declare const noopRule: (tree: Tree, _context: SchematicContext) => import("@angular-devkit/schematics/src/tree/interface").Tree;
export declare function overwriteFileIfExists(tree: Tree): Rule;
