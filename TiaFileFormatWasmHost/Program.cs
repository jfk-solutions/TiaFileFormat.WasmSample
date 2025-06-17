using Microsoft.Extensions.FileProviders;
//using TiaFileFormat;
//using TiaFileFormat.Database;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseDefaultFiles();
app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "..\\TiaFileFormatWasm\\bin\\browser-wasm\\AppBundle"))
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "..\\TiaFileFormatWasm\\bin\\browser-wasm\\AppBundle")
        ),
    RequestPath = "",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});


//var data = File.ReadAllBytes("d:\\Project4.zap20");
//using var ms = new MemoryStream(data);
//var tfp = TiaFileProvider.CreateFromSingleStream(ms);
//var database = TiaDatabaseFile.Load(tfp);
//database.ParseAllObjects();

app.Run();
