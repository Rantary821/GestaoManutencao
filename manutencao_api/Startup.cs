using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Driver;

// ... outras using statements

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // ... outras configurações
        services.AddScoped<gestaomanutencao.Services.UsuarioService>();

        // Configuração da autenticação JWT
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key"));

        // Obter a string de conexão do arquivo de configuração
        string connectionString = _configuration.GetConnectionString("MongoDBConnection");

        // Configurar o cliente MongoDB
        services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

        // Configurar autenticação JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                ValidAudience = jwtSettings.GetValue<string>("Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        // Configuração do CORS
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000") // Adicione a origem do seu aplicativo React
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });


        // Adicione essa linha para permitir a autorização nas suas APIs
        services.AddAuthorization();

        // Adicione essa linha para permitir o uso de controllers na sua aplicação
        services.AddControllers();

        // Adicione essa linha para configurar o Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Gestao Manutenção WEB API", Version = "v1" });
        });

        // ... outras configurações
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestao Manutenção WEB API");
            });
        }

        // Habilitar o CORS
        app.UseCors("CorsPolicy");

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
