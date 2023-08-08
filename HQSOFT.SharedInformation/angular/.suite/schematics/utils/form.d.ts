import { NavigationProperty, Property } from '../models';
export declare function generateDefaultValueFromProperty(prop: Property | NavigationProperty): string;
export declare function generateValidatorsFromProperty(prop: Property | NavigationProperty): string[];
