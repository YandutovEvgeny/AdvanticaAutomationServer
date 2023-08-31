using AdvanticaAutomationTestServer.Infractructure;
using AdvanticaAutomationTestServer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = "Server=DESKTOP-L473M1B\\SQLEXPRESS;Database=grpcdb;Trusted_Connection=True;TrustServerCertificate=Yes;";
builder.Services.AddDbContext<WorkerContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<WorkerStreamService>();

app.Run();
