using Catalog.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// BsonSeralizer... fortæller at hver gang den ser en Guid i alle entiteter skal den serializeres til en string. 
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
// Samme som før bare så den ved hvad for en DateTime det er
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var connectionString = "mongodb://localhost:27017"; // Replace with your MongoDB connection string
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<IVareRepository, MongoDBVareRepository>();
// Blev nød til at sætte "SuppressAsyncSuffixInActionNames" til false fordi .NET ikke vil remove async suffix fra metoder i runtime
builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();
// tilføj flere .Add her til healthchecks på mongodb f.eks. senere
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
     // httpsreducrection er her når env. er sat til development for docker. ASP.NET tager production istedet for development når man kører i en docker fil (se launchsettings).
}


app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.Map("/health", appBuilder =>
{
    appBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/health");
    });
});

app.Run();
