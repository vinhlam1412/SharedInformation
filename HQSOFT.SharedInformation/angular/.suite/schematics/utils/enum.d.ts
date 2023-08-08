import { Entity, Enum, NavigationProperty, Property } from '../models';
export declare function getEnumsFromEntity(solution: string, entity: Entity): Enum[];
export declare function isEnum(property: Property | NavigationProperty): property is Property;
