"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.markNavigationConnections = exports.getLookupName = void 0;
const text_1 = require("./text");
function getLookupName(prop, isNavigationConnection) {
    if (isNavigationConnection) {
        return (0, text_1.camel)(`${prop.entityRef.name}Id`);
    }
    else {
        return (0, text_1.camel)(`${prop.name}`);
    }
}
exports.getLookupName = getLookupName;
function markNavigationConnections(props) {
    return props.map(prop => ({ ...prop, isNavigationConnection: true }));
}
exports.markNavigationConnections = markNavigationConnections;
//# sourceMappingURL=template.js.map