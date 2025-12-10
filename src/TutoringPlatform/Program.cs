using TutoringPlatform.Models;
using TutoringPlatform.Models.Enums;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TutoringDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            .MapEnum<UserType>("user_type")
            .MapEnum<BookingFormat>("booking_format")
            .MapEnum<BookingStatus>("booking_status")
    )
);

builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();