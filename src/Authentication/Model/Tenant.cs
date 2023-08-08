using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using DataShapes.Model;

namespace Authentication.Model
{
    public class Tenant : Entity
    {
        private string? Name { get; set; }
    }

    public static class TenantEndpoints
    {
        public static void MapTenantEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Tenant").WithTags(nameof(Tenant));

            group.MapGet("/", () =>
            {
                return new[] { new Tenant() };
            })
            .WithName("GetAllTenants")
            .WithOpenApi();

            group.MapGet("/{id}", (int id) =>
            {
                //return new Tenant { ID = id };
            })
            .WithName("GetTenantById")
            .WithOpenApi();

            group.MapPut("/{id}", (int id, Tenant input) =>
            {
                return TypedResults.NoContent();
            })
            .WithName("UpdateTenant")
            .WithOpenApi();

            group.MapPost("/", (Tenant model) =>
            {
                //return TypedResults.Created($"/Tenants/{model.ID}", model);
            })
            .WithName("CreateTenant")
            .WithOpenApi();

            group.MapDelete("/{id}", (int id) =>
            {
                //return TypedResults.Ok(new Tenant { ID = id });
            })
            .WithName("DeleteTenant")
            .WithOpenApi();
        }
    }
}