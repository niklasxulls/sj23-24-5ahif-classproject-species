﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiveSpecies.Rest.Swagger;

public class CustomModelDocumentFilter<T> : IDocumentFilter where T : class
{
    public void Apply(OpenApiDocument openapiDoc, DocumentFilterContext context)
    {
        context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
    }
}
