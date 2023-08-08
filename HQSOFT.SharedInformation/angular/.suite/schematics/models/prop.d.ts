import { eFormControl, ePropType, eUiPickType } from '../enums';
import { NavigationProperty, Property } from './entity';
export interface Prop {
    ref: Property | NavigationProperty;
    name: string;
    entityRef: EntityRef;
    getInput: string[];
    type: ePropType;
    enumType?: string;
    formControl: eFormControl | eUiPickType;
    defaultValue: any;
    validators: string[];
    asterisk: string;
    question: string;
    showOnList?: boolean;
    readonlyOnEditModal?: boolean;
    showOnModal?: boolean;
    ngIf?: boolean;
}
export interface EntityRef {
    name: string;
    namespace: string;
    displayProperty?: string;
}
