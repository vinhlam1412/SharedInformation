"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.shouldAddRouteDeclaration = exports.findModuleWithProviderDeclarations = exports.addProviderToModuleWithProviderMetadata = void 0;
const ts = require("typescript");
const angular_1 = require("./angular");
function addProviderToModuleWithProviderMetadata(source, ngModulePath, symbolName, importPath = null) {
    let providerInsertion;
    const metadataField = 'providers';
    const [forRoot] = findModuleWithProviderDeclarations(source); // handle multiple declarations?
    if (!forRoot)
        return [];
    let [node] = (0, angular_1.findNodes)(forRoot, ts.SyntaxKind.ObjectLiteralExpression);
    if (!node || !ts.isObjectLiteralExpression(node))
        return [];
    const [field] = (0, angular_1.getMetadataField)(node, metadataField) || [];
    if (!field) {
        providerInsertion = createMetadataInsertionChange(node, source, ngModulePath, `${metadataField}: [${symbolName}]`);
    }
    else {
        const { initializer } = field;
        if (!ts.isArrayLiteralExpression(initializer))
            return [];
        if (initializer.elements.length) {
            const { elements } = initializer;
            const symbols = elements.map(expr => expr.getText());
            if (symbols.includes(symbolName))
                return []; // symbol exists
            node = elements[elements.length - 1];
        }
        else {
            node = initializer;
        }
        let position = node.getEnd();
        if (ts.isObjectLiteralExpression(node)) {
            providerInsertion = createMetadataInsertionChange(node, source, ngModulePath, symbolName);
        }
        else if (ts.isArrayLiteralExpression(node)) {
            // providers array is empty, position should be before `]`
            providerInsertion = new angular_1.InsertChange(ngModulePath, --position, symbolName);
        }
        else {
            providerInsertion = createMetadataInsertionChange({ properties: [node] }, source, ngModulePath, symbolName);
        }
    }
    return importPath === null
        ? [providerInsertion]
        : [
            providerInsertion,
            (0, angular_1.insertImport)(source, ngModulePath, symbolName.replace(/\..*$/, ''), importPath),
        ];
}
exports.addProviderToModuleWithProviderMetadata = addProviderToModuleWithProviderMetadata;
function createMetadataInsertionChange(expression, source, ngModulePath, metadata) {
    let position;
    let insertion;
    const { properties } = expression;
    if (properties?.length) {
        const property = properties[properties.length - 1];
        position = property.getEnd();
        const text = property.getFullText(source);
        const [indentation] = text.match(/^\r?\n(\r?)\s*/) || [' '];
        insertion = `,${indentation}${metadata}`;
    }
    else {
        position = expression.getEnd() - 1;
        insertion = `,  ${metadata}\n`;
    }
    return new angular_1.InsertChange(ngModulePath, position, insertion);
}
function findModuleWithProviderDeclarations(source) {
    return (0, angular_1.getSourceNodes)(source).reduce((acc, node) => {
        if (!ts.isMethodDeclaration(node))
            return acc;
        if (node.getText().startsWith('static forRoot'))
            acc.push(node);
        return acc;
    }, []);
}
exports.findModuleWithProviderDeclarations = findModuleWithProviderDeclarations;
function shouldAddRouteDeclaration(source, route) {
    if (!route)
        return false;
    const routerModuleExpression = (0, angular_1.getRouterModuleDeclaration)(source);
    if (!routerModuleExpression)
        return false;
    const routerModuleConfigArguments = routerModuleExpression.arguments;
    if (!routerModuleConfigArguments.length)
        return false;
    const routesExpression = routerModuleConfigArguments[0];
    const routesArrayLiteral = ts.isArrayLiteralExpression(routesExpression)
        ? routesExpression
        : resolveRoutesArrayLiteral(source, routesExpression);
    if (!routesArrayLiteral)
        return false;
    let hasRoute = false;
    loopElements: for (const element of routesArrayLiteral.elements) {
        if (!ts.isObjectLiteralExpression(element))
            continue loopElements;
        loopProps: for (const prop of element.properties) {
            if (!ts.isPropertyAssignment(prop))
                continue loopProps;
            if (!ts.isIdentifier(prop.name))
                continue loopProps;
            if (prop.name.text !== 'path')
                continue loopProps;
            if (!ts.isStringLiteral(prop.initializer))
                continue loopProps;
            if (prop.initializer.text === route) {
                hasRoute = true;
                break loopElements;
            }
        }
    }
    return !hasRoute;
}
exports.shouldAddRouteDeclaration = shouldAddRouteDeclaration;
function resolveRoutesArrayLiteral(source, routesExpression) {
    const routesVariableName = routesExpression.getText();
    let routesVariable;
    if (routesExpression.kind === ts.SyntaxKind.Identifier) {
        routesVariable = source.statements.filter(ts.isVariableStatement).find(v => {
            return v.declarationList.declarations[0].name.getText() === routesVariableName;
        });
    }
    if (!routesVariable)
        return undefined;
    return (0, angular_1.findNodes)(routesVariable, ts.SyntaxKind.ArrayLiteralExpression, 1)[0];
}
//# sourceMappingURL=ast.js.map