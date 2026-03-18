using CarsImgApi.Models.Domain;
using CarsImgApi.Repository.Implementation;
using CarsImgApi.Repository.Interface;
using CarsImgApi.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;



/*Agregando middleWare para atrapar las excepciones globales y guardarlas con Serilog*/
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Solo guarda de "Info" para arriba (ignora los miles de logs de "Debug")
    .WriteTo.Console()
    .WriteTo.File("logs/errores-.txt",
        rollingInterval: RollingInterval.Day, // Crea un archivo nuevo cada día automáticamente
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error) // En el archivo SOLO guarda errores, no basura
    .CreateLogger();

try
{
    Log.Information(@"-------------------------------------------------------------
                      Iniciando la aplicación CarsImgApi...
                      -------------------------------------------------------------");


    var builder = WebApplication.CreateBuilder(args);

    // 3. Le decimos a .NET que reemplace su sistema de logs por defecto por Serilog
    builder.Host.UseSerilog();

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddScoped<IDataVehicle, VehicleRepository>();
    builder.Services.AddScoped<IImageVehicle, VehicleImageRepository>();
    builder.Services.AddScoped<ILoginUser, AuthService>();
    builder.Services.AddScoped<ICreateImage, CreateImageService>();


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "(\bBearer {token})",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();

    });

    //creando la autenticacion de la api
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                AuthenticationType = "Jwt", //aqui le decimos del tipoque es
                ValidateIssuer = true, //aqui le decimos que valide quien firma el token que somos nosotros
                ValidateAudience = true, // aqui validamos a la persona que va, que en este caso es el frontend
                ValidateLifetime = true, //aqui le ponemos que valide si el token expiro por el tiempo
                ValidateIssuerSigningKey = true, // aqui que valide la llave definida en el appsettings
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    //esto solo se usa para build de produccion, localmente hay que comentarlo
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://10.0.0.52:81")
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowCredentials();

        });
    });

    // Configure the HTTP request pipeline. descomentar esto cuando estemos probando en desarrollo.
    /*if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors(options => options.
                                AllowAnyMethod()
                                .AllowAnyHeader()
                                .SetIsOriginAllowed(origin => true)
                                .AllowCredentials());
    }

    app.UseCors(options => options.
                            AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());*/

    var app = builder.Build();

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context => {
            
            context.Response.StatusCode = 500; // Internal Server Error
            context.Response.ContentType = "application/json";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                var excepcion = contextFeature.Error;
                var ruta = context.Request.Path;

                // 4. AQUÍ USAMOS SERILOG.
                // Log.Error guardará la fecha, el mensaje, la ruta y el StackTrace completo.
                Log.Error(excepcion, "Fallo crítico no manejado al intentar acceder a {Ruta}", ruta);

                var ticketId = Guid.NewGuid(); // Generamos un ID de rastreo

                // 5. Devolvemos un mensaje seguro al usuario
                await context.Response.WriteAsJsonAsync(new
                {
                    Mensaje = "Ocurrió un error inesperado. Nuestro equipo técnico ya fue notificado.",
                    IdError = ticketId
                });
            }

        });
    });

    app.UseCors("AllowFrontend"); //descomentar para cuando se haga build pra produccion.

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

} catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al arrancar de forma catastrófica.");
}
finally
{
    // 6. Asegura que los últimos logs se escriban antes de que se apague el programa
    Log.CloseAndFlush();
}
