import { eUiPickType } from '../enums';
export interface Entity {
    Id: string;
    BaseClass: string;
    CreateTests: boolean;
    CheckConcurrency: boolean;
    DatabaseTableName: string;
    IsMultiTenant: boolean;
    Name: string;
    NamePlural: string;
    Namespace: string;
    NavigationProperties: NavigationProperty[];
    NavigationConnections: NavigationProperty[];
    OriginalName: string;
    PhysicalFileName: string;
    PrimaryKeyType: string;
    Properties: Property[];
    ShouldAddMigration: boolean;
    ShouldCreateUserInterface: boolean;
    ShouldUpdateDatabase: boolean;
    ShouldExportExcel: boolean;
}
export interface Property {
    Id: string;
    EmailValidation: boolean;
    EnumAngularImport: string;
    EnumNamespace: string;
    EnumType: string;
    EnumValues?: Record<string, number>;
    IsNullable: boolean;
    DefaultValue: any;
    IsRequired: boolean;
    IsTextArea: boolean;
    MaxLength?: number;
    MinLength?: number;
    Name: string;
    Regex: string;
    SortOrder: number;
    SortType: number;
    Type: ServerDataType;
    ShowOnList: boolean;
    ShowOnCreateModal: boolean;
    ShowOnEditModal: boolean;
    ReadonlyOnEditModal: boolean;
}
export interface NavigationProperty {
    DisplayProperty: string;
    DtoEntityName: string;
    DtoNamespace: string;
    EntityName: string;
    EntityNameWithDuplicationNumber: string;
    EntitySetName: string;
    EntitySetNameWithDuplicationNumber: string;
    IsRequired: boolean;
    Name: string;
    Namespace: string;
    ReferencePropertyName: string;
    Type: ServerDataType;
    UiPickType: keyof typeof eUiPickType;
}
export declare type ServerDataType = 'bool' | 'byte' | 'char' | 'Date' | 'DateTime' | 'decimal' | 'double' | 'enum' | 'float' | 'Guid' | 'int' | 'long' | 'sbyte' | 'short' | 'string' | 'uint' | 'ulong' | 'ushort';
