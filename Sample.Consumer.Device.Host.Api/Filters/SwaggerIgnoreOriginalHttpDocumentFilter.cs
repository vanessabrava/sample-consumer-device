using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Sample.Microservice.Device.Api.Filters
{
    internal class SwaggerIgnoreOriginalHttpDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var item in swaggerDoc.Paths)
                GetOriginalHttpParametersForMethod(item.Value?.Parameters);
        }

        private void GetOriginalHttpParametersForMethod(IList<OpenApiParameter> parametersToGet)
        {
            var parametersToRemove = new List<OpenApiParameter>();

            if (parametersToGet != null)
            {
                foreach (var itemGet in parametersToGet)
                    if (itemGet.Name.Contains("OriginalHttp"))
                        parametersToRemove.Add(itemGet);

                foreach (var param in parametersToRemove)
                    parametersToGet.Remove(param);
            }
        }
    }
}
