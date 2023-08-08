"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.formatFiles = void 0;
const path = require("path");
async function formatFiles(tree) {
    let prettier;
    try {
        prettier = await Promise.resolve().then(() => require('prettier'));
    }
    catch (e) {
        return;
    }
    if (!prettier)
        return;
    const files = tree.actions.filter(f => ['o', 'c', 'r'].includes(f.kind));
    await Promise.all(Array.from(files).map(async (action) => {
        const content = tree.read(action.path);
        if (!content)
            return;
        const fullPath = path.join(path.join(path.resolve(), tree.root.path), action.path);
        let options = {
            filepath: fullPath,
        };
        const resolvedOptions = await prettier.resolveConfig(fullPath, {
            editorconfig: true,
        });
        options = {
            ...options,
            ...(resolvedOptions || {}),
        };
        const isFileSupported = await prettier.getFileInfo(fullPath);
        if (isFileSupported.ignored || !isFileSupported.inferredParser)
            return;
        try {
            tree.overwrite(action.path, prettier.format(content.toString('utf-8'), options));
        }
        catch (e) {
            console.warn(`${action.path} file could not formatted. Reason: "${e.message}"`);
        }
    }));
}
exports.formatFiles = formatFiles;
//# sourceMappingURL=format-files.js.map