using DependencyInjectionExercise.Data;
using DependencyInjectionExercise.Services;
using DependencyInjectionExercise.Services.NotificationSenders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseInMemoryDatabase("BookStoreDb"));

builder.Services.AddSingleton<DiscountService>();
builder.Services.AddSingleton<OrderTrackingService>();
builder.Services.AddSingleton<NotificationHub>();

builder.Services.AddSingleton<NotificationResolverService>();

builder.Services.AddSingleton<INotificationSender, EmailNotificationSender>();
builder.Services.AddSingleton<INotificationSender, SmsNotificationSender>();
builder.Services.AddSingleton<INotificationSender, PushNotificationSender>();
builder.Services.AddSingleton<INotificationSender, WebhookNotificationSender>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookStoreContext>();
    context.Database.EnsureCreated();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
