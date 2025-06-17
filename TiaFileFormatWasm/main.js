// Set up the .NET WebAssembly runtime
import { dotnet } from './_framework/dotnet.js'

// Get exported methods from the .NET assembly
const { getAssemblyExports, getConfig } = await dotnet
    .withDiagnosticTracing(false)
    .create();

const config = getConfig();
// Access JSExport methods using exports.<Namespace>.<Type>.<Method>
const exports = await getAssemblyExports(config.mainAssemblyName);


export function GetFolders(project) {
    const result = exports.TiaWasmExports.GetFolders(project);
    return JSON.parse(result);
}