"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ProxyJsonParams = exports.createProxyJsonGenerator = void 0;
const core_1 = require("@angular-devkit/core");
const constants_1 = require("../constants");
const enums_1 = require("../enums");
const angular_1 = require("./angular");
const enum_1 = require("./enum");
const { camelize, capitalize, dasherize } = core_1.strings;
function createProxyJsonGenerator(params) {
    const { apiName, moduleName, projectPath } = params;
    const apiDefinitionPath = `${projectPath}/proxy/generate-proxy.json`;
    const rootPath = dasherize(moduleName);
    const remoteServiceName = apiName === 'default' ? 'Default' : apiName;
    const moduleKey = camelize(moduleName);
    return (tree) => {
        let apiDefinition = {
            generated: [],
            modules: {
                [moduleKey]: {
                    rootPath,
                    remoteServiceName,
                    controllers: {},
                },
            },
            types: {},
        };
        const apiDefinitionExists = tree.exists(apiDefinitionPath);
        if (apiDefinitionExists) {
            const buffer = tree.read(apiDefinitionPath);
            apiDefinition = JSON.parse(buffer.toString());
        }
        apiDefinition.generated = [...new Set([...apiDefinition.generated, moduleKey])];
        apiDefinition.generated.sort();
        const types = createTypes(params);
        apiDefinition.types = {
            ...apiDefinition.types,
            ...params.enums,
            ...types,
        };
        let existsControllers = apiDefinition.modules[moduleKey]?.controllers || {};
        const isBoolean = (value, defaultValue) => typeof value === 'boolean' ? value : defaultValue;
        existsControllers = Object.keys(existsControllers).reduce((acc, key) => {
            const existController = existsControllers[key];
            return {
                ...acc,
                [key]: {
                    ...existController,
                    isRemoteService: isBoolean(existController.isRemoteService, true),
                    isIntegrationService: isBoolean(existController.isIntegrationService, false),
                },
            };
        }, {});
        apiDefinition.modules[moduleKey] = {
            rootPath,
            remoteServiceName,
            controllers: {
                ...existsControllers,
                ...createController(params),
            },
        };
        const text = JSON.stringify(apiDefinition, null, 2);
        tree[apiDefinitionExists ? 'overwrite' : 'create'](apiDefinitionPath, text);
    };
}
exports.createProxyJsonGenerator = createProxyJsonGenerator;
function createTypes(params) {
    const { checkConcurrency, solution, entityBaseClass, entityPrimaryKey, namespace, name, namePlural, props: properties, navProps: navigationProperties = [], navPropsSuffix, navConnectionProps: navigationConnectionProperties = [], navigationConnections, } = params;
    const inputNavigationConnectionProps = navigationConnections.reduce((acc, prop) => {
        const type = `${prop.Namespace}.${prop.EntityName}Dto`;
        if (acc.some(p => p.type === type))
            return acc;
        acc.push({
            name: prop.EntityName + 'Id',
            jsonName: null,
            type: 'Guid',
            typeSimple: 'string',
            isRequired: prop.IsRequired,
        });
        return acc;
    }, []);
    const inputProperties = createGetInputs(Object.assign(params, { props: [...params.props, ...inputNavigationConnectionProps] }), mapToGetInput);
    const exportToExcelTypes = createExportToExcelTypes(params);
    const types = {
        [`${solution}.Shared.LookupRequestDto`]: {
            baseType: 'Volo.Abp.Application.Dtos.PagedResultRequestDto',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'Filter',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
            ],
        },
        [`${solution}.Shared.LookupDto<T0>`]: {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: ['TKey'],
            properties: [
                { name: 'Id', jsonName: null, type: 'TKey', typeSimple: 'TKey', isRequired: false },
                {
                    name: 'DisplayName',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
            ],
        },
        [`${solution}.${namespace}.Get${namePlural}Input`]: {
            baseType: 'Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'FilterText',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
                ...inputProperties,
            ],
        },
        [`${solution}.${namespace}.${name}Dto`]: {
            baseType: `Volo.Abp.Application.Dtos.${entityBaseClass}<${entityPrimaryKey}>`,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [...properties, ...getConcurrencyStampProperty(checkConcurrency)],
        },
        [`${solution}.${namespace}.${name}CreateDto`]: {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [...properties, ...inputNavigationConnectionProps.map(p => mapPlural(p))],
        },
        [`${solution}.${namespace}.${name}UpdateDto`]: {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                ...properties,
                ...inputNavigationConnectionProps.map(p => mapPlural(p)),
                ...getConcurrencyStampProperty(checkConcurrency),
            ],
        },
        'Volo.Abp.Application.Dtos.PagedAndSortedResultRequestDto': {
            baseType: 'Volo.Abp.Application.Dtos.PagedResultRequestDto',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'Sorting',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.PagedResultRequestDto': {
            baseType: 'Volo.Abp.Application.Dtos.LimitedResultRequestDto',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'SkipCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.LimitedResultRequestDto': {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'DefaultMaxResultCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isRequired: false,
                },
                {
                    name: 'MaxMaxResultCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isRequired: false,
                },
                {
                    name: 'MaxResultCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.PagedResultDto<T0>': {
            baseType: 'Volo.Abp.Application.Dtos.ListResultDto<T>',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: ['T'],
            properties: [
                {
                    name: 'TotalCount',
                    jsonName: null,
                    type: 'System.Int64',
                    typeSimple: 'number',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.ListResultDto<T0>': {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: ['T'],
            properties: [
                {
                    name: 'Items',
                    jsonName: null,
                    type: '[T]',
                    typeSimple: '[T]',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.FullAuditedEntityDto<T0>': {
            baseType: 'Volo.Abp.Application.Dtos.AuditedEntityDto<TPrimaryKey>',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: ['TPrimaryKey'],
            properties: [
                {
                    name: 'IsDeleted',
                    jsonName: null,
                    type: 'System.Boolean',
                    typeSimple: 'boolean',
                    isRequired: false,
                },
                {
                    name: 'DeleterId',
                    jsonName: null,
                    type: 'System.Guid?',
                    typeSimple: 'string?',
                    isRequired: false,
                },
                {
                    name: 'DeletionTime',
                    jsonName: null,
                    type: 'System.DateTime?',
                    typeSimple: 'string?',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.AuditedEntityDto<T0>': {
            baseType: 'Volo.Abp.Application.Dtos.CreationAuditedEntityDto<TPrimaryKey>',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: ['TPrimaryKey'],
            properties: [
                {
                    name: 'LastModificationTime',
                    jsonName: null,
                    type: 'System.DateTime?',
                    typeSimple: 'string?',
                    isRequired: false,
                },
                {
                    name: 'LastModifierId',
                    jsonName: null,
                    type: 'System.Guid?',
                    typeSimple: 'string?',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.CreationAuditedEntityDto<T0>': {
            baseType: 'Volo.Abp.Application.Dtos.EntityDto<TPrimaryKey>',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: ['TPrimaryKey'],
            properties: [
                {
                    name: 'CreationTime',
                    jsonName: null,
                    type: 'System.DateTime',
                    typeSimple: 'string',
                    isRequired: false,
                },
                {
                    name: 'CreatorId',
                    jsonName: null,
                    type: 'System.Guid?',
                    typeSimple: 'string?',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.EntityDto<T0>': {
            baseType: 'Volo.Abp.Application.Dtos.EntityDto',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: ['TKey'],
            properties: [
                {
                    name: 'Id',
                    jsonName: null,
                    type: 'TKey',
                    typeSimple: 'TKey',
                    isRequired: false,
                },
            ],
        },
        'Volo.Abp.Application.Dtos.EntityDto': {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [],
        },
        [`${solution}.Users.AppUserDto`]: {
            baseType: 'Volo.Abp.Identity.IdentityUserDto',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [],
        },
        'Volo.Abp.Identity.IdentityUserDto': {
            baseType: 'Volo.Abp.Application.Dtos.ExtensibleEntityDto<System.Guid>',
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'TenantId',
                    jsonName: null,
                    type: 'System.Guid?',
                    typeSimple: 'string?',
                    isRequired: false,
                },
                {
                    name: 'UserName',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
                {
                    name: 'Email',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
                {
                    name: 'Name',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
                {
                    name: 'Surname',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
                {
                    name: 'EmailConfirmed',
                    jsonName: null,
                    type: 'System.Boolean',
                    typeSimple: 'boolean',
                    isRequired: false,
                },
                {
                    name: 'PhoneNumber',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
                {
                    name: 'PhoneNumberConfirmed',
                    jsonName: null,
                    type: 'System.Boolean',
                    typeSimple: 'boolean',
                    isRequired: false,
                },
                {
                    name: 'SupportTwoFactor',
                    jsonName: null,
                    type: 'System.Boolean',
                    typeSimple: 'boolean',
                    isRequired: false,
                },
                {
                    name: 'LockoutEnabled',
                    jsonName: null,
                    type: 'System.Boolean',
                    typeSimple: 'boolean',
                    isRequired: false,
                },
                {
                    name: 'IsLockedOut',
                    jsonName: null,
                    type: 'System.Boolean',
                    typeSimple: 'boolean',
                    isRequired: false,
                },
                {
                    name: 'ConcurrencyStamp',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                },
            ],
        },
        ...exportToExcelTypes,
    };
    if (navPropsSuffix) {
        types[`${solution}.${namespace}.${name + navPropsSuffix}Dto`] = {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                ...navigationProperties,
                ...navigationConnectionProperties.map(p => mapPlural(p, true)),
            ],
        };
    }
    return types;
}
function createExportToExcelTypes({ solution, name, namespace, shouldExportExcel, }) {
    if (!shouldExportExcel) {
        return {};
    }
    const result = {
        [`${solution}.Shared.DownloadTokenResultDto`]: {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'Token',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                    minLength: null,
                    maxLength: null,
                    minimum: null,
                    maximum: null,
                    regex: null,
                },
            ],
        },
        [`${solution}.${namespace}.${name}ExcelDownloadDto`]: {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'DownloadToken',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                    minLength: null,
                    maxLength: null,
                    minimum: null,
                    maximum: null,
                    regex: null,
                },
                {
                    name: 'FilterText',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                    minLength: null,
                    maxLength: null,
                    minimum: null,
                    maximum: null,
                    regex: null,
                },
                {
                    name: 'Name',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                    minLength: null,
                    maxLength: null,
                    minimum: null,
                    maximum: null,
                    regex: null,
                },
            ],
        },
        'Volo.Abp.Content.IRemoteStreamContent': {
            baseType: null,
            isEnum: false,
            enumNames: null,
            enumValues: null,
            genericArguments: null,
            properties: [
                {
                    name: 'FileName',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                    minLength: null,
                    maxLength: null,
                    minimum: null,
                    maximum: null,
                    regex: null,
                },
                {
                    name: 'ContentType',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isRequired: false,
                    minLength: null,
                    maxLength: null,
                    minimum: null,
                    maximum: null,
                    regex: null,
                },
                {
                    name: 'ContentLength',
                    jsonName: null,
                    type: 'System.Int64?',
                    typeSimple: 'number?',
                    isRequired: false,
                    minLength: null,
                    maxLength: null,
                    minimum: null,
                    maximum: null,
                    regex: null,
                },
            ],
        },
    };
    return result;
}
function createController(params) {
    const { solution, namespace, name } = params;
    const type = `${solution}.Controllers.${namespace}.${name}Controller`;
    return {
        [type]: {
            controllerName: name,
            type,
            isRemoteService: true,
            isIntegrationService: false,
            interfaces: [
                {
                    type: `${solution}.${namespace}.I${name}AppService`,
                },
            ],
            actions: createActions(params),
        },
    };
}
function createActions(params) {
    const { solution, namespace, name, namePlural, entityPrimaryKey, entityPrimaryKeySimple, entityPath, navPropsSuffix, } = params;
    const getInputs = createGetInputs(params, mapToGetInputExtended);
    const navPropActions = createNavPropActions(params);
    const lookupActions = createLookupActions(params);
    const exportToExcelActions = createExportToExcelActions(params);
    const actions = [
        {
            uniqueName: 'GetListAsyncByInput',
            name: 'GetListAsync',
            httpMethod: 'GET',
            url: entityPath,
            supportedVersions: [],
            parametersOnMethod: [
                {
                    name: 'input',
                    typeAsString: `${solution}.${namespace}.Get${namePlural}Input, ${solution}.Application.Contracts`,
                    type: `${solution}.${namespace}.Get${namePlural}Input`,
                    typeSimple: `${solution}.${namespace}.Get${namePlural}Input`,
                    isOptional: false,
                    defaultValue: null,
                },
            ],
            parameters: [
                {
                    nameOnMethod: 'input',
                    name: 'FilterText',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
                ...getInputs,
                {
                    nameOnMethod: 'input',
                    name: 'Sorting',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
                {
                    nameOnMethod: 'input',
                    name: 'SkipCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
                {
                    nameOnMethod: 'input',
                    name: 'MaxResultCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
            ],
            returnValue: {
                type: `Volo.Abp.Application.Dtos.PagedResultDto<${solution}.${namespace}.${name + navPropsSuffix}Dto>`,
                typeSimple: `Volo.Abp.Application.Dtos.PagedResultDto<${solution}.${namespace}.${name + navPropsSuffix}Dto>`,
            },
        },
        ...navPropActions,
        {
            uniqueName: 'GetAsyncById',
            name: 'GetAsync',
            httpMethod: 'GET',
            url: entityPath + '/{id}',
            supportedVersions: [],
            parametersOnMethod: [
                {
                    name: 'id',
                    typeAsString: `${entityPrimaryKey}, System.Private.CoreLib`,
                    type: entityPrimaryKey,
                    typeSimple: entityPrimaryKeySimple,
                    isOptional: false,
                    defaultValue: null,
                },
            ],
            parameters: [
                {
                    nameOnMethod: 'id',
                    name: 'id',
                    jsonName: null,
                    type: entityPrimaryKey,
                    typeSimple: entityPrimaryKeySimple,
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: [],
                    bindingSourceId: enums_1.eBindingSourceId.Path,
                    descriptorName: '',
                },
            ],
            returnValue: {
                type: `${solution}.${namespace}.${name}Dto`,
                typeSimple: `${solution}.${namespace}.${name}Dto`,
            },
        },
        ...lookupActions,
        ...exportToExcelActions,
        {
            uniqueName: 'CreateAsyncByInput',
            name: 'CreateAsync',
            httpMethod: 'POST',
            url: entityPath,
            supportedVersions: [],
            parametersOnMethod: [
                {
                    name: 'input',
                    typeAsString: `${solution}.${namespace}.${name}CreateDto, ${solution}.Application.Contracts`,
                    type: `${solution}.${namespace}.${name}CreateDto`,
                    typeSimple: `${solution}.${namespace}.${name}CreateDto`,
                    isOptional: false,
                    defaultValue: null,
                },
            ],
            parameters: [
                {
                    nameOnMethod: 'input',
                    name: 'input',
                    jsonName: null,
                    type: `${solution}.${namespace}.${name}CreateDto`,
                    typeSimple: `${solution}.${namespace}.${name}CreateDto`,
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Body,
                    descriptorName: '',
                },
            ],
            returnValue: {
                type: `${solution}.${namespace}.${name}Dto`,
                typeSimple: `${solution}.${namespace}.${name}Dto`,
            },
        },
        {
            uniqueName: 'UpdateAsyncByIdAndInput',
            name: 'UpdateAsync',
            httpMethod: 'PUT',
            url: entityPath + '/{id}',
            supportedVersions: [],
            parametersOnMethod: [
                {
                    name: 'id',
                    typeAsString: `${entityPrimaryKey}, System.Private.CoreLib`,
                    type: entityPrimaryKey,
                    typeSimple: entityPrimaryKeySimple,
                    isOptional: false,
                    defaultValue: null,
                },
                {
                    name: 'input',
                    typeAsString: `${solution}.${namespace}.${name}UpdateDto, ${solution}.Application.Contracts`,
                    type: `${solution}.${namespace}.${name}UpdateDto`,
                    typeSimple: `${solution}.${namespace}.${name}UpdateDto`,
                    isOptional: false,
                    defaultValue: null,
                },
            ],
            parameters: [
                {
                    nameOnMethod: 'id',
                    name: 'id',
                    jsonName: null,
                    type: entityPrimaryKey,
                    typeSimple: entityPrimaryKeySimple,
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: [],
                    bindingSourceId: enums_1.eBindingSourceId.Path,
                    descriptorName: '',
                },
                {
                    nameOnMethod: 'input',
                    name: 'input',
                    jsonName: null,
                    type: `${solution}.${namespace}.${name}UpdateDto`,
                    typeSimple: `${solution}.${namespace}.${name}UpdateDto`,
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Body,
                    descriptorName: '',
                },
            ],
            returnValue: {
                type: `${solution}.${namespace}.${name}Dto`,
                typeSimple: `${solution}.${namespace}.${name}Dto`,
            },
        },
        {
            uniqueName: 'DeleteAsyncById',
            name: 'DeleteAsync',
            httpMethod: 'DELETE',
            url: entityPath + '/{id}',
            supportedVersions: [],
            parametersOnMethod: [
                {
                    name: 'id',
                    typeAsString: `${entityPrimaryKey}, System.Private.CoreLib`,
                    type: entityPrimaryKey,
                    typeSimple: entityPrimaryKeySimple,
                    isOptional: false,
                    defaultValue: null,
                },
            ],
            parameters: [
                {
                    nameOnMethod: 'id',
                    name: 'id',
                    jsonName: null,
                    type: entityPrimaryKey,
                    typeSimple: entityPrimaryKeySimple,
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: [],
                    bindingSourceId: enums_1.eBindingSourceId.Path,
                    descriptorName: '',
                },
            ],
            returnValue: {
                type: 'System.Void',
                typeSimple: 'System.Void',
            },
        },
    ];
    return actions.reduce((acc, action) => {
        acc[action.uniqueName] = action;
        return acc;
    }, {});
}
function createGetInputs(params, mapperFn) {
    return params.props.reduce((acc, { isRef, name, type, typeSimple, isRequired }) => {
        const inputs = isRef
            ? [name]
            : type.startsWith('System.DateTime') || typeSimple.startsWith('number')
                ? [name + 'Min', name + 'Max']
                : [name];
        [type, typeSimple] = shouldMakeInputOptional(type, typeSimple);
        inputs.forEach(input => acc.push(mapperFn(input, type, typeSimple, isRequired)));
        return acc;
    }, []);
}
function shouldMakeInputOptional(type, typeSimple) {
    if (!type.endsWith('?') && !/[}]>$]/.test(type)) {
        type += '?';
        typeSimple += '?';
    }
    return [type, typeSimple];
}
function mapToGetInput(name, type, typeSimple, isRequired) {
    return {
        name,
        jsonName: null,
        type,
        typeSimple,
        isRequired,
    };
}
function mapPlural(prop, exceptName = false) {
    return {
        ...prop,
        ...(!exceptName && { name: prop.name + 's' }),
        type: `[${prop.type.replace('?', '')}]`,
        typeSimple: `[${prop.typeSimple.replace('?', '')}]`,
    };
}
function mapToGetInputExtended(name, type, typeSimple) {
    return {
        nameOnMethod: 'input',
        name,
        jsonName: null,
        type,
        typeSimple,
        isOptional: false,
        defaultValue: null,
        constraintTypes: null,
        bindingSourceId: enums_1.eBindingSourceId.Model,
        descriptorName: 'input',
    };
}
function getConcurrencyStampProperty(checkConcurency) {
    if (!checkConcurency) {
        return [];
    }
    return [
        {
            name: 'ConcurrencyStamp',
            jsonName: null,
            type: 'System.String',
            typeSimple: 'string',
            isRequired: false,
        },
    ];
}
function createNavPropActions(params) {
    const { solution, namespace, name, entityPrimaryKey, entityPrimaryKeySimple, entityPath, navPropsSuffix, } = params;
    return navPropsSuffix
        ? [
            {
                uniqueName: 'GetWithNavigationPropertiesAsyncById',
                name: 'GetWithNavigationPropertiesAsync',
                httpMethod: 'GET',
                url: entityPath + '/with-navigation-properties/{id}',
                supportedVersions: [],
                parametersOnMethod: [
                    {
                        name: 'id',
                        typeAsString: `${entityPrimaryKey}, System.Private.CoreLib`,
                        type: entityPrimaryKey,
                        typeSimple: entityPrimaryKeySimple,
                        isOptional: false,
                        defaultValue: null,
                    },
                ],
                parameters: [
                    {
                        nameOnMethod: 'id',
                        name: 'id',
                        jsonName: null,
                        type: entityPrimaryKey,
                        typeSimple: entityPrimaryKeySimple,
                        isOptional: false,
                        defaultValue: null,
                        constraintTypes: [],
                        bindingSourceId: enums_1.eBindingSourceId.Path,
                        descriptorName: '',
                    },
                ],
                returnValue: {
                    type: `${solution}.${namespace}.${name}WithNavigationPropertiesDto`,
                    typeSimple: `${solution}.${namespace}.${name}WithNavigationPropertiesDto`,
                },
            },
        ]
        : [];
}
function createLookupActions(params) {
    const { solution, name: entityName, entityPath, navProps, navConnectionProps = [], navigationConnections = [], } = params;
    return [
        ...navProps,
        ...navConnectionProps.map(e => ({
            ...e,
            name: navigationConnections.find(c => c.Name === e.name)?.EntityName || '',
        })),
    ].reduce((acc, prop) => {
        const keyType = prop.type;
        const navEntityPrimaryKey = constants_1.PRIMARY_KEY_TYPES.get(keyType) || constants_1.PRIMARY_KEY_TYPES.get('Guid');
        const navEntityPrimaryKeySimple = constants_1.PROP_TYPES.get(keyType) || constants_1.PROP_TYPES.get('Guid');
        const navEntityName = prop.name;
        if (entityName === navEntityName)
            return acc;
        const name = `Get${navEntityName}LookupAsync`;
        if (acc.find(action => action.name === name))
            return acc;
        acc.push({
            uniqueName: `${name}ByInput`,
            name,
            httpMethod: 'GET',
            url: `${entityPath}/${dasherize(navEntityName)}-lookup`,
            supportedVersions: [],
            parametersOnMethod: [
                {
                    name: 'input',
                    typeAsString: `${solution}.Shared.LookupRequestDto, ${solution}.Application.Contracts`,
                    type: `${solution}.Shared.LookupRequestDto`,
                    typeSimple: `${solution}.Shared.LookupRequestDto`,
                    isOptional: false,
                    defaultValue: null,
                },
            ],
            parameters: [
                {
                    nameOnMethod: 'input',
                    name: 'Filter',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
                {
                    nameOnMethod: 'input',
                    name: 'SkipCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
                {
                    nameOnMethod: 'input',
                    name: 'MaxResultCount',
                    jsonName: null,
                    type: 'System.Int32',
                    typeSimple: 'number',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
            ],
            returnValue: {
                type: `Volo.Abp.Application.Dtos.PagedResultDto<${solution}.Shared.LookupDto<${navEntityPrimaryKey}?>>`,
                typeSimple: `Volo.Abp.Application.Dtos.PagedResultDto<${solution}.Shared.LookupDto<${navEntityPrimaryKeySimple}?>>`,
            },
        });
        return acc;
    }, []);
}
function createExportToExcelActions({ entityPath, solution, namespace, name, namePlural, shouldExportExcel, }) {
    if (!shouldExportExcel) {
        return [];
    }
    return [
        {
            uniqueName: 'GetListAsExcelFileAsyncByInput',
            name: 'GetListAsExcelFileAsync',
            httpMethod: 'GET',
            url: entityPath + '/as-excel-file',
            supportedVersions: [],
            parametersOnMethod: [
                {
                    name: 'input',
                    typeAsString: `${solution}.${namespace}.${name}ExcelDownloadDto, ${solution}.Application.Contracts`,
                    type: `${solution}.${namespace}.${name}ExcelDownloadDto`,
                    typeSimple: `${solution}.${namespace}.${name}ExcelDownloadDto`,
                    isOptional: false,
                    defaultValue: null,
                },
            ],
            parameters: [
                {
                    nameOnMethod: 'input',
                    name: 'DownloadToken',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
                {
                    nameOnMethod: 'input',
                    name: 'FilterText',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
                {
                    nameOnMethod: 'input',
                    name: 'Name',
                    jsonName: null,
                    type: 'System.String',
                    typeSimple: 'string',
                    isOptional: false,
                    defaultValue: null,
                    constraintTypes: null,
                    bindingSourceId: enums_1.eBindingSourceId.Model,
                    descriptorName: 'input',
                },
            ],
            returnValue: {
                type: 'Volo.Abp.Content.IRemoteStreamContent',
                typeSimple: 'Volo.Abp.Content.IRemoteStreamContent',
            },
            allowAnonymous: null,
            implementFrom: `${solution}.${namespace}.I${namePlural}AppService`,
        },
        {
            uniqueName: 'GetDownloadTokenAsync',
            name: 'GetDownloadTokenAsync',
            httpMethod: 'GET',
            url: entityPath + '/download-token',
            supportedVersions: [],
            parametersOnMethod: [],
            parameters: [],
            returnValue: {
                type: `${solution}.Shared.DownloadTokenResultDto`,
                typeSimple: `${solution}.Shared.DownloadTokenResultDto`,
            },
            allowAnonymous: null,
            implementFrom: `${solution}.${namespace}.I${namePlural}AppService`,
        },
    ];
}
class ProxyJsonParams {
    constructor({ project, entity, ...options }) {
        this.shouldExportExcel = false;
        this.navConnectionProps = [];
        this.navigationConnections = [];
        Object.assign(this, options);
        this.projectPath = (0, angular_1.buildDefaultPath)(project);
        this.namespace = entity.Namespace;
        this.namePlural = entity.NamePlural;
        this.checkConcurrency = entity.CheckConcurrency;
        this.name = entity.Name;
        this.shouldExportExcel = entity.ShouldExportExcel;
        this.entityBaseClass =
            constants_1.BASE_CLASSES.get(entity.BaseClass) || constants_1.BASE_CLASSES.get('FullAuditedAggregateRoot');
        this.entityPrimaryKey =
            constants_1.PRIMARY_KEY_TYPES.get(entity.PrimaryKeyType) || constants_1.PRIMARY_KEY_TYPES.get('Guid');
        this.entityPrimaryKeySimple =
            constants_1.PROP_TYPES.get(entity.PrimaryKeyType) || constants_1.PROP_TYPES.get('Guid');
        this.entityPath = `api/${dasherize(this.moduleName)}/${dasherize(this.namePlural)}`;
        const naviationProperties = entity.NavigationProperties || [];
        this.enums = {};
        this.createProps(entity);
        this.createNavigationConnectionProps(entity);
        const dto = `${this.solution}.${this.namespace}.${this.name}Dto`;
        this.navProps = [...naviationProperties].reduce((acc, prop) => {
            const type = `${prop.Namespace}.${prop.EntityName}Dto`;
            if (acc.some(p => p.type === type))
                return acc;
            acc.push({
                name: prop.EntityName,
                jsonName: null,
                type,
                typeSimple: type,
                isRequired: prop.IsRequired,
            });
            return acc;
        }, [
            {
                name: this.name,
                jsonName: null,
                type: dto,
                typeSimple: dto,
                isRequired: false,
            },
        ]);
        this.navPropsSuffix =
            naviationProperties.length || entity.NavigationConnections?.length
                ? 'WithNavigationProperties'
                : '';
    }
    createProps(entity) {
        const properties = entity.Properties;
        const naviationProperties = entity.NavigationProperties || [];
        this.props = [...properties, ...naviationProperties].map((prop) => {
            const isRequired = prop.IsRequired;
            const optionalSuffix = isRequired ? '' : '?';
            const name = prop.Name;
            if ((0, enum_1.isEnum)(prop)) {
                const entries = Object.entries(prop.EnumValues);
                entries.sort((a, b) => a[1] - b[1]);
                const fullEnumName = `${prop.EnumNamespace}.${prop.EnumType}`;
                this.enums[fullEnumName] = {
                    baseType: 'System.Enum',
                    isEnum: true,
                    enumNames: entries.map(e => e[0]),
                    enumValues: entries.map(e => e[1]),
                    genericArguments: null,
                    properties: null,
                };
                const t = fullEnumName + optionalSuffix;
                return { name, jsonName: null, type: t, typeSimple: t, isRequired };
            }
            const type = `System.${capitalize(prop.Type)}`;
            const typeSimple = constants_1.PROP_TYPES.get(prop.Type);
            return {
                name,
                jsonName: null,
                type: type + optionalSuffix,
                typeSimple: typeSimple + optionalSuffix,
                isRef: Boolean(prop.ReferencePropertyName),
                isRequired,
            };
        });
    }
    createNavigationConnectionProps(entity) {
        const navigationConnections = entity.NavigationConnections || [];
        this.navigationConnections = navigationConnections;
        this.navConnectionProps = navigationConnections.reduce((acc, prop) => {
            const type = `${prop.Namespace}.${prop.EntityName}Dto`;
            if (acc.some(p => p.type === type))
                return acc;
            acc.push({
                name: prop.Name,
                jsonName: null,
                type,
                typeSimple: type,
                isRequired: prop.IsRequired,
            });
            return acc;
        }, []);
    }
}
exports.ProxyJsonParams = ProxyJsonParams;
//# sourceMappingURL=api-definition.js.map