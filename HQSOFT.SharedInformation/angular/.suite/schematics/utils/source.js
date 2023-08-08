"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.readSourceJson = void 0;
const schematics_1 = require("@angular-devkit/schematics");
const common_1 = require("./common");
function readSourceJson(path) {
    let entity;
    try {
        entity = require(path);
    }
    catch (e) {
        throw new schematics_1.SchematicsException((0, common_1.interpolate)("[File Not Found] There is no file at \"{0}\" path." /* Exception.FileNotFound */, path));
    }
    return entity;
}
exports.readSourceJson = readSourceJson;
//# sourceMappingURL=source.js.map