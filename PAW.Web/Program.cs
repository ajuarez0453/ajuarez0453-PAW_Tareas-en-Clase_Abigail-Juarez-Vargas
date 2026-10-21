using APW.Architecture;
using PAW.Web.Services;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Configuracion de MVC.
builder.Services.AddControllersWithViews();

// Servicios existentes de tu proyecto.
builder.Services.AddScoped<IRestProvider, RestProvider>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPawTaskService, PawTaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserActionService, UserActionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();

// NUEVO: Cache en memoria.
builder.Services.AddMemoryCache();

// Servicio de tipo de cambio.
builder.Services.AddHttpClient<
	IExchangeRateService,
	ExchangeRateService>(client =>
	{
		client.BaseAddress = new Uri(
			"https://api.hacienda.go.cr/");

		client.Timeout = TimeSpan.FromSeconds(10);
	});

var app = builder.Build();

// Configuracion HTTP.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// NUEVO: Endpoint para consultar el tipo de cambio.
app.MapGet("/api/exchange-rate", async (
	IExchangeRateService exchangeRateService,
	IMemoryCache cache,
	ILogger<Program> logger,
	CancellationToken cancellationToken) =>
{
	const string cacheKey = "ExchangeRate";

	// Primero buscamos los datos en cache.
	if (cache.TryGetValue(
		cacheKey,
		out TipoCambioResponse? cachedExchangeRate)
		&& cachedExchangeRate is not null)
	{
		return Results.Ok(cachedExchangeRate);
	}

	try
	{
		// Si no existen en cache, consultamos Hacienda.
		var exchangeRate =
			await exchangeRateService.GetExchangeRateAsync(
				cancellationToken);

		// Guardamos la respuesta durante 12 horas.
		cache.Set(
			cacheKey,
			exchangeRate,
			TimeSpan.FromHours(12));

		return Results.Ok(exchangeRate);
	}
	catch (HttpRequestException ex)
	{
		logger.LogError(
			ex,
			"Error HTTP al consultar el tipo de cambio.");

		return Results.Problem(
			title: "No se pudo consultar el tipo de cambio.",
			statusCode: 502);
	}
	catch (TaskCanceledException ex)
		when (!cancellationToken.IsCancellationRequested)
	{
		logger.LogError(
			ex,
			"Tiempo de espera agotado al consultar Hacienda.");

		return Results.Problem(
			title: "La consulta del tipo de cambio excedio el tiempo de espera.",
			statusCode: 504);
	}
});

// Rutas existentes de MVC.
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();