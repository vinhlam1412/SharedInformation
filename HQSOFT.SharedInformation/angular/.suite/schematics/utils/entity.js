"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.isNavigationProperty = exports.getNavConnectionsFromEntity = exports.getNavPropsFromEntity = exports.getPropsFromEntity = exports.getEntityRefsFromProps = exports.buildModuleOptionsFromEntity = void 0;
const core_1 = require("@angular-devkit/core");
const constants_1 = require("../constants");
const enums_1 = require("../enums");
const form_1 = require("./form");
const text_1 = require("./text");
function buildModuleOptionsFromEntity(entity) {
    return {
        name: entity.Name,
        route: core_1.strings.dasherize(entity.NamePlural),
    };
}
exports.buildModuleOptionsFromEntity = buildModuleOptionsFromEntity;
function getEntityRefsFromProps(props) {
    return props.reduce((refs, prop) => {
        const { name, namespace } = prop.entityRef;
        if (refs.every(ref => ref.namespace + ref.name !== namespace + name))
            refs.push({ name, namespace });
        return refs;
    }, []);
}
exports.getEntityRefsFromProps = getEntityRefsFromProps;
function getPropsFromEntity(solution, entity) {
    return entity.Properties.map(createPropBuilder(solution, entity));
}
exports.getPropsFromEntity = getPropsFromEntity;
function getNavPropsFromEntity(solution, entity) {
    return entity.NavigationProperties.map(createPropBuilder(solution, entity));
}
exports.getNavPropsFromEntity = getNavPropsFromEntity;
function getNavConnectionsFromEntity(solution, entity) {
    //TODO: remove static UıPickType when backend returns this value
    const navigationConnections = entity.NavigationConnections.map(prop => ({
        ...prop,
        UiPickType: 'Typeahead',
    }));
    return navigationConnections.map(createPropBuilder(solution, entity, true));
}
exports.getNavConnectionsFromEntity = getNavConnectionsFromEntity;
function createPropBuilder(solution, entity, isNavConnection = false) {
    const solutionRegex = new RegExp('^' + solution);
    return (property) => {
        let propVisibility = {};
        const entityRef = { name: entity.Name, namespace: entity.Namespace };
        if (isNavigationProperty(property)) {
            entityRef.name = property.EntityName;
            entityRef.namespace = property.Namespace.replace(solutionRegex, '');
            entityRef.displayProperty = property.DisplayProperty;
        }
        else {
            const showOnCreateModal = property.ShowOnCreateModal;
            const showOnEditModal = property.ShowOnEditModal;
            let ngIf = '';
            if (showOnCreateModal && showOnEditModal) {
                ngIf = '';
            }
            else if (showOnCreateModal) {
                ngIf = '*ngIf="!selected"';
            }
            else {
                ngIf = '*ngIf="!!selected"';
            }
            propVisibility = {
                showOnList: property.ShowOnList,
                showOnModal: showOnCreateModal || showOnEditModal,
                ngIf,
                readonlyOnEditModal: property.ReadonlyOnEditModal,
            };
        }
        const defaultValue = getDefaultValue(isNavConnection, property);
        return {
            ref: property,
            entityRef,
            name: property.Name,
            getInput: buildGetInput(property),
            type: constants_1.PROP_TYPES.get(property.Type),
            enumType: 'EnumType' in property ? property.EnumType : undefined,
            formControl: 'UiPickType' in property
                ? enums_1.eUiPickType[property.UiPickType]
                : constants_1.FORM_CONTROLS.get(property.Type),
            defaultValue,
            validators: (0, form_1.generateValidatorsFromProperty)(property),
            get asterisk() {
                return this.validators.includes('Validators.required') ? '*' : '';
            },
            get question() {
                return this.validators.includes('Validators.required') ? '' : '?';
            },
            ...propVisibility,
        };
    };
}
function getDefaultValue(isNavConnection, property) {
    if (isNavConnection) {
        return `${(0, text_1.camel)(property.Name)}.map(({id}) => id)`;
    }
    if ('DefaultValue' in property) {
        const defaultValue = property.DefaultValue;
        const propName = (0, text_1.camel)(property.Name);
        if (defaultValue !== undefined && defaultValue !== null) {
            const parsed = typeof defaultValue === 'string' ? `'${defaultValue}'` : defaultValue;
            return `${propName} ?? ${parsed}`;
        }
    }
    return (0, form_1.generateDefaultValueFromProperty)(property);
}
function buildGetInput(property) {
    const name = (0, text_1.camel)(property.Name);
    const maxMin = [name + 'Max', name + 'Min'];
    const control = constants_1.FORM_CONTROLS.get(property.Type);
    return control === enums_1.eFormControl.Date || control === enums_1.eFormControl.Number ? maxMin : [name];
}
function isNavigationProperty(prop) {
    return 'EntityName' in prop;
}
exports.isNavigationProperty = isNavigationProperty;
//# sourceMappingURL=entity.js.map