var builder = WebApplication.CreateBuilder(args);

var startup = new palkkatietoapi.Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);


var app = builder.Build();
startup.Configure(app, builder.Environment);
