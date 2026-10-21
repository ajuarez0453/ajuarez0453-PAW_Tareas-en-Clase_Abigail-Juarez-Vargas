using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 1. CONFIGURACION DE SERVICIOS
// ========================================

// Permite descubrir los endpoints de la Minimal API.
builder.Services.AddEndpointsApiExplorer();

// Configuracion de Swagger.
builder.Services.AddSwaggerGen();

// Inyeccion de dependencias.
// Utilizamos el repositorio que ya existe
// en nuestro proyecto.
builder.Services.AddScoped<
	IPawTaskRepository,
	PawTaskRepository>();

// Configuracion de CORS.
// Permite que PAW.Web consuma nuestra Minimal API.
builder.Services.AddCors(options =>
{
	options.AddPolicy("PAWWeb", policy =>
	{
		policy.WithOrigins("https://localhost:7076")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

var app = builder.Build();

// ========================================
// 2. CONFIGURACION HTTP Y SWAGGER
// ========================================

// Habilitamos Swagger en desarrollo.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PAWWeb");

// ========================================
// 3. GET - OBTENER TODAS LAS TAREAS
// ========================================

app.MapGet("/tasks", async (
	IPawTaskRepository repository) =>
{
	// Consultamos las tareas desde el repositorio.
	var tasks = await repository.ReadAsync();

	// Convertimos las entidades a DTO.
	var result = tasks
		.Select(PawTaskDTO.ConvertFrom)
		.ToList();

	// HTTP 200 - OK.
	return Results.Ok(result);
})
.WithName("GetAllTasks")
.WithTags("Tasks")
.WithSummary("Obtener todas las tareas");

// ========================================
// 4. GET - OBTENER UNA TAREA POR ID
// ========================================

app.MapGet("/tasks/{id:int}", async (
	int id,
	IPawTaskRepository repository) =>
{
	// Buscamos la tarea solicitada.
	var task = await repository.FindAsync(id);

	// HTTP 404 si no existe.
	if (task == null)
	{
		return Results.NotFound(
			new { message = "Tarea no encontrada." });
	}

	// HTTP 200 si existe.
	return Results.Ok(
		PawTaskDTO.ConvertFrom(task));
})
.WithName("GetTaskById")
.WithTags("Tasks")
.WithSummary("Obtener una tarea por ID");

// ========================================
// 5. POST - CREAR UNA TAREA
// ========================================

app.MapPost("/tasks", async (
	PawTaskDTO dto,
	IPawTaskRepository repository) =>
{
	// Validamos que tenga nombre.
	if (string.IsNullOrWhiteSpace(dto.Name))
	{
		return Results.BadRequest(
			new { message = "El nombre es obligatorio." });
	}

	// Convertimos el DTO a entidad.
	var task = PawTaskDTO.ConvertTo(dto);

	// El ID sera generado por la base de datos.
	task.Id = 0;

	// Registramos las fechas.
	task.CreatedAt = DateTime.Now;
	task.LastModified = DateTime.Now;

	// Guardamos mediante el repositorio.
	var created = await repository.CreateAsync(task);

	if (!created)
	{
		return Results.BadRequest(
			new { message = "No se pudo crear la tarea." });
	}

	// HTTP 201 - Created.
	return Results.Created(
		$"/tasks/{task.Id}",
		PawTaskDTO.ConvertFrom(task));
})
.WithName("CreateTask")
.WithTags("Tasks")
.WithSummary("Crear una nueva tarea");

// ========================================
// 6. PUT - ACTUALIZAR UNA TAREA
// ========================================

app.MapPut("/tasks/{id:int}", async (
	int id,
	PawTaskDTO dto,
	IPawTaskRepository repository) =>
{
	// Validamos el nombre.
	if (string.IsNullOrWhiteSpace(dto.Name))
	{
		return Results.BadRequest(
			new { message = "El nombre es obligatorio." });
	}

	// Buscamos la tarea.
	var task = await repository.FindAsync(id);

	if (task == null)
	{
		return Results.NotFound(
			new { message = "Tarea no encontrada." });
	}

	// Actualizamos las propiedades.
	task.Name = dto.Name;
	task.Description = dto.Description;
	task.Status = dto.Status;
	task.DueDate = dto.DueDate;
	task.ModifiedBy = dto.ModifiedBy;

	// Actualizamos la fecha de modificacion.
	task.LastModified = DateTime.Now;

	// Guardamos los cambios.
	var updated = await repository.UpdateAsync(task);

	if (!updated)
	{
		return Results.BadRequest(
			new { message = "No se pudo actualizar la tarea." });
	}

	// HTTP 200 - OK.
	return Results.Ok(
		PawTaskDTO.ConvertFrom(task));
})
.WithName("UpdateTask")
.WithTags("Tasks")
.WithSummary("Actualizar una tarea existente");

// ========================================
// 7. DELETE - ELIMINAR UNA TAREA
// ========================================

app.MapDelete("/tasks/{id:int}", async (
	int id,
	IPawTaskRepository repository) =>
{
	// Buscamos la tarea.
	var task = await repository.FindAsync(id);

	if (task == null)
	{
		return Results.NotFound(
			new { message = "Tarea no encontrada." });
	}

	// Eliminamos mediante el repositorio.
	var deleted = await repository.DeleteAsync(task);

	if (!deleted)
	{
		return Results.BadRequest(
			new { message = "No se pudo eliminar la tarea." });
	}

	// HTTP 204 - No Content.
	return Results.NoContent();
})
.WithName("DeleteTask")
.WithTags("Tasks")
.WithSummary("Eliminar una tarea");

// ========================================
// 8. EJECUTAR LA APLICACION
// ========================================

app.Run();