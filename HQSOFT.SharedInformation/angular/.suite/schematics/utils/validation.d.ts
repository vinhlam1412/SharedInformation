import { Property } from '../models';
export declare const ValidatorGenerator: {
    Email: typeof generateEmailValidators;
    MaxLength: typeof generateMaxLengthValidators;
    MinLength: typeof generateMinLengthValidators;
    Regex: typeof generateRegexValidators;
    Required: typeof generateIsRequiredValidators;
};
declare function generateIsRequiredValidators(prop: Property): "Validators.required" | null;
declare function generateEmailValidators(prop: Property): "Validators.email" | null;
declare function generateRegexValidators(prop: Property): string | null;
declare function generateMaxLengthValidators(prop: Property): string | null;
declare function generateMinLengthValidators(prop: Property): string | null;
export {};
