"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.generateValidatorsFromProperty = exports.generateDefaultValueFromProperty = void 0;
const common_1 = require("./common");
const entity_1 = require("./entity");
const text_1 = require("./text");
const validation_1 = require("./validation");
function generateDefaultValueFromProperty(prop) {
    const propName = (0, text_1.camel)(prop.Name);
    let defaultValue;
    switch (prop.Type) {
        case 'Date':
        case 'DateTime':
            defaultValue = `${propName} ? new Date(${propName}) : null`;
            break;
        case 'bool':
            defaultValue = `${propName} ?? false`;
            break;
        default:
            defaultValue = `${propName} ?? null`;
            break;
    }
    return defaultValue;
}
exports.generateDefaultValueFromProperty = generateDefaultValueFromProperty;
function generateValidatorsFromProperty(prop) {
    const keys = (0, entity_1.isNavigationProperty)(prop)
        ? ['Required']
        : ['Required', 'MinLength', 'MaxLength', 'Email', 'Regex'];
    const validators = keys.map(key => validation_1.ValidatorGenerator[key](prop));
    return validators.filter(common_1.isString);
}
exports.generateValidatorsFromProperty = generateValidatorsFromProperty;
//# sourceMappingURL=form.js.map