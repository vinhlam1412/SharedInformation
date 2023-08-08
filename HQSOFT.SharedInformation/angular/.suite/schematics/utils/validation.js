"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ValidatorGenerator = void 0;
const core_1 = require("@angular-devkit/core");
const constants_1 = require("../constants");
const common_1 = require("./common");
exports.ValidatorGenerator = {
    Email: generateEmailValidators,
    MaxLength: generateMaxLengthValidators,
    MinLength: generateMinLengthValidators,
    Regex: generateRegexValidators,
    Required: generateIsRequiredValidators,
};
function generateIsRequiredValidators(prop) {
    return prop.IsRequired ? 'Validators.required' : null;
}
function generateEmailValidators(prop) {
    return prop.EmailValidation ? 'Validators.email' : null;
}
function generateRegexValidators(prop) {
    return prop.Regex ? `Validators.pattern('${prop.Regex}')` : null;
}
function generateMaxLengthValidators(prop) {
    return createMaxMinLengthValidatorsGenerator('max')(prop);
}
function generateMinLengthValidators(prop) {
    return createMaxMinLengthValidatorsGenerator('min')(prop);
}
function createMaxMinLengthValidatorsGenerator(type) {
    const attr = (core_1.strings.capitalize(type) + 'Length');
    return (prop) => {
        const value = prop[attr];
        if ((0, common_1.isNullOrUndefined)(value))
            return null;
        const validator = type + (constants_1.PROP_TYPES.get(prop.Type) === 'number' ? '' : 'Length');
        return `Validators.${validator}(${value})`;
    };
}
//# sourceMappingURL=validation.js.map