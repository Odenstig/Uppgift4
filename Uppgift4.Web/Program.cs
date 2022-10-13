var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(builder =>
{
    builder.WithOrigins("https://localhost:7123")
    .AllowAnyHeader()
    .WithMethods("GET", "POST")
    .AllowCredentials();
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();
