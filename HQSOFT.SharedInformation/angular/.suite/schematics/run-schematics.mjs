import { execa } from "execa";

const [ngCli, command, schematic, template, appName, jsonFile, angularPath] = process.argv.slice(2);
const localPath = '.suite/schematics/node_modules/' + ngCli.split('node_modules/')[1];

const { stdout } = await execa(
  localPath,
  [command, schematic, '--template', template, '--target', appName, '--source', jsonFile],
  {
    cwd: angularPath,
  },
);
console.log(stdout);
