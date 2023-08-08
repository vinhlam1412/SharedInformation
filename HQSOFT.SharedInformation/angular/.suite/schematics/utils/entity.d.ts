import { Entity, EntityRef, NavigationProperty, Prop, Property } from '../models';
import { ABPModuleOptions } from './module';
export declare function buildModuleOptionsFromEntity(entity: Entity): ABPModuleOptions;
export declare function getEntityRefsFromProps(props: Prop[]): EntityRef[];
export declare function getPropsFromEntity(solution: string, entity: Entity): Prop[];
export declare function getNavPropsFromEntity(solution: string, entity: Entity): Prop[];
export declare function getNavConnectionsFromEntity(solution: string, entity: Entity): Prop[];
export declare function isNavigationProperty(prop: Property | NavigationProperty): prop is NavigationProperty;
