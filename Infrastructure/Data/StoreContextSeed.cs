using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
               if (!context.ProductBrands.Any())
               {
                   var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                   var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                   context.AddRange(brands);

                   await context.SaveChangesAsync();
               }

               if (!context.ProductTypes.Any())
               {
                   var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                   var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                   context.AddRange(types);

                   await context.SaveChangesAsync();
               }

               if (!context.Products.Any())
               {
                   var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                   var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                   context.AddRange(products);

                   await context.SaveChangesAsync();
               }

               if (!context.DeliveryMethods.Any())
               {
                   var dmData = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");

                   var delMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                   context.AddRange(delMethods);

                   await context.SaveChangesAsync();
               }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}