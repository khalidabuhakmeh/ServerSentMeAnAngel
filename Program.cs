using ServerSentMeAnAngel;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices();
var app = builder.Build();

app.UseDefaultFiles().UseStaticFiles();

app.MapGet("/orders", (FoodService foods, CancellationToken token) =>
    TypedResults.ServerSentEvents(
        foods.GetCurrent(token),
        eventType: "order")
);

app.Run();