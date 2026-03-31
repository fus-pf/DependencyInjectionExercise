using DependencyInjectionExercise.Data;
using DependencyInjectionExercise.Middlewares;
using DependencyInjectionExercise.Respositories.BookRepositories;
using DependencyInjectionExercise.Respositories.OrderRepositories;
using DependencyInjectionExercise.Services;
using DependencyInjectionExercise.Services.BookServices;
using DependencyInjectionExercise.Services.NotificationSenders;
using DependencyInjectionExercise.Services.OrderServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseInMemoryDatabase("BookStoreDb"));

builder.Services.AddScoped<ExceptionMiddleware>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddSingleton<DiscountService>();
builder.Services.AddScoped<OrderTrackingService>();
builder.Services.AddSingleton<NotificationHub>();

builder.Services.AddSingleton<NotificationResolverService>();

builder.Services.AddKeyedSingleton<INotificationSender, EmailNotificationSender>("email");
builder.Services.AddKeyedSingleton<INotificationSender, SmsNotificationSender>("sms");
builder.Services.AddKeyedSingleton<INotificationSender, PushNotificationSender>("push");
builder.Services.AddKeyedSingleton<INotificationSender, WebhookNotificationSender>("webhook");

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookStoreContext>();
    context.Database.EnsureCreated();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
