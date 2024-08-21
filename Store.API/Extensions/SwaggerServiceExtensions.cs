using Microsoft.OpenApi.Models;

namespace Store.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation (this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title="StoreAPI", Version="v1" });
                var sercuritySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer schema. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }
                };
                options.AddSecurityDefinition("bearer", sercuritySchema);
            }); 
            return services;
        }
    }
}
