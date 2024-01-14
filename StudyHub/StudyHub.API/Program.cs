using Amazon.S3;
using StudyHub.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContexts(builder.Configuration.GetConnectionString("StudyHubAPIContext"));
builder.Services.AddAppServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AWS - S3
// REF: https://www.youtube.com/watch?v=oY0-1mj4oCo&t=0s
// REF: https://www.youtube.com/watch?v=2q5jA813ZiI&t=667s
// TODO: Install and setup AWS CLI in machine to save aws keys
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
