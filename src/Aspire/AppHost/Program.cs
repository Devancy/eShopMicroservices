using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var catalogDb = builder.AddPostgres("CatalogPostgres")
	.AddDatabase("CatalogDb");
var catalogApi = builder.AddProject<Catalog_API>("catalog-api")
	.WithReference(catalogDb)
	.WaitFor(catalogDb);

var discountGrpc = builder.AddProject<Discount_Grpc>("discount-grpc");
var distributedCache = builder.AddValkey("DistributedCache");

var rabbitmq = builder.AddRabbitMQ("MessageBroker");
var basketDb = builder.AddPostgres("BasketPostgres")
	.AddDatabase("BasketDb");
var basketApi = builder.AddProject<Basket_API>("basket-api")
	.WithReference(basketDb).WaitFor(basketDb)
	.WithReference(distributedCache).WaitFor(distributedCache)
	.WithReference(rabbitmq).WaitFor(rabbitmq)
	.WithReference(discountGrpc).WaitFor(discountGrpc);

var orderDb = builder.AddSqlServer("OrderSqlServer")
	.AddDatabase("OrderDb");
var orderApi = builder.AddProject<Ordering_API>("ordering-api")
	.WithReference(orderDb).WaitFor(orderDb)
	.WithReference(rabbitmq).WaitFor(rabbitmq);

builder.AddProject<YarpApiGateway>("gateway")
	.WithReference(catalogApi).WaitFor(catalogApi)
	.WithReference(discountGrpc).WaitFor(orderApi)
	.WithReference(basketApi).WaitFor(basketApi)
	.WithReference(orderApi).WaitFor(orderApi);

builder.Build().Run();