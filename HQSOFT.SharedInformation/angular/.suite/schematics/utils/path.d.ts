import { Enum } from '../models';
export declare function relativePathToSrcRoot(namespace: string): string;
export declare function relativePathFromComponentToEnum(scope: string, namespace: string, e: Enum): string;
export declare function relativePathFromComponentToModel(scope: string, namespace: string): string;
export declare function relativePathFromComponentToService(namespace: string, name: string): string;
export declare function relativePathFromComponentToProxyNamespace(scope: string, namespace: string, proxyNamespace: string): string;
export declare function relativePathFromComponentToProxy(scope: string, namespace: string): string;
