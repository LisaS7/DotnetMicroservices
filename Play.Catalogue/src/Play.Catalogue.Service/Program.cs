using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalogue.Service.Entities;
using Play.Catalogue.Service.Repositories;
using Play.Catalogue.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

// config for appsettings.json
var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

// register one instance of mongodb
builder.Services.AddSingleton(serviceProvider =>
{
    var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
    return mongoClient.GetDatabase(serviceSettings.ServiceName);
});

builder.Services.AddSingleton<IRepository<Item>>(serviceProvider =>
{
    var database = serviceProvider.GetService<IMongoDatabase>();
    return new MongoRepository<Item>(database, "items");
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => { options.SuppressAsyncSuffixInActionNames = false; });
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

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
