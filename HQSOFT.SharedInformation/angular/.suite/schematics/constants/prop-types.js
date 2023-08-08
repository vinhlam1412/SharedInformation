"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PRIMARY_KEY_TYPES = exports.FORM_CONTROLS = exports.PROP_TYPES = void 0;
const enums_1 = require("../enums");
exports.PROP_TYPES = new Map([
    ['bool', enums_1.ePropType.Boolean],
    ['byte', enums_1.ePropType.Number],
    ['char', enums_1.ePropType.String],
    ['Date', enums_1.ePropType.String],
    ['DateTime', enums_1.ePropType.String],
    ['decimal', enums_1.ePropType.Number],
    ['double', enums_1.ePropType.Number],
    ['enum', enums_1.ePropType.Number],
    ['float', enums_1.ePropType.Number],
    ['Guid', enums_1.ePropType.String],
    ['int', enums_1.ePropType.Number],
    ['long', enums_1.ePropType.Number],
    ['sbyte', enums_1.ePropType.Number],
    ['short', enums_1.ePropType.Number],
    ['string', enums_1.ePropType.String],
    ['uint', enums_1.ePropType.Number],
    ['ulong', enums_1.ePropType.Number],
    ['ushort', enums_1.ePropType.Number],
]);
exports.FORM_CONTROLS = new Map([
    ['bool', enums_1.eFormControl.Checkbox],
    ['byte', enums_1.eFormControl.Number],
    ['char', enums_1.eFormControl.Text],
    ['Date', enums_1.eFormControl.Date],
    ['DateTime', enums_1.eFormControl.Date],
    ['decimal', enums_1.eFormControl.Number],
    ['double', enums_1.eFormControl.Number],
    ['enum', enums_1.eFormControl.Select],
    ['float', enums_1.eFormControl.Number],
    ['Guid', enums_1.eFormControl.Text],
    ['int', enums_1.eFormControl.Number],
    ['long', enums_1.eFormControl.Number],
    ['sbyte', enums_1.eFormControl.Number],
    ['short', enums_1.eFormControl.Number],
    ['string', enums_1.eFormControl.Text],
    ['uint', enums_1.eFormControl.Number],
    ['ulong', enums_1.eFormControl.Number],
    ['ushort', enums_1.eFormControl.Number],
]);
exports.PRIMARY_KEY_TYPES = new Map([
    ['Guid', 'System.Guid'],
    ['int', 'System.Int32'],
    ['long', 'System.Int64'],
    ['string', 'System.String'],
]);
//# sourceMappingURL=prop-types.js.map