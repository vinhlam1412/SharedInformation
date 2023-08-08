"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.isEnum = exports.getEnumsFromEntity = void 0;
function getEnumsFromEntity(solution, entity) {
    const solutionRegex = new RegExp('^' + solution);
    return entity.Properties.reduce((enums, prop) => {
        const { EnumType, EnumNamespace, EnumValues } = prop;
        if (!EnumValues)
            return enums;
        const name = EnumType;
        const namespace = EnumNamespace.replace(solutionRegex, '');
        if (enums.every(e => e.namespace + e.name !== namespace + name))
            enums.push({
                members: Object.entries(EnumValues),
                name,
                namespace,
            });
        return enums;
    }, []);
}
exports.getEnumsFromEntity = getEnumsFromEntity;
function isEnum(property) {
    return property.Type === 'enum';
}
exports.isEnum = isEnum;
//# sourceMappingURL=enum.js.map