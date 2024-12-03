using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Runtime.InteropServices;

namespace ConsoleCosmodDB
{
    internal class Program
    {
        const string connectionString = "";
        static async Task Main(string[] args)
        {
            CosmosClient client = new CosmosClient(connectionString);
            Container container = client.GetContainer("Productos","Items");

            // Agregar un Item

            //Producto prod1 = new(id: "1",
            //                 Nombre: "Descripcion Producto 1",
            //                 categoria: "Electronica",
            //                 Precio: 500,
            //                 Descontinuado: false);

            //Producto product = await container.UpsertItemAsync(prod1, new PartitionKey("Electronica"));
            //Console.WriteLine("Producto Agregado " + product.ToString());

            // Agregar un lote como transaccion
            //Producto prod3 = new(id: "3",
            //                 Nombre: "Descripcion Producto 3",
            //                 categoria: "Electronica",
            //                 Precio: 100,
            //                 Descontinuado: false);
            //Producto prod4 = new(id: "4",
            //                     Nombre: "Descripcion Producto 4",
            //                     categoria: "Electronica",
            //                     Precio: 250,
            //                     Descontinuado: true);
            //Producto prod5 = new(id: "5",
            //                    Nombre: "Descripcion Producto 5",
            //                    categoria: "Electronica",
            //                    Precio: 50,
            //                    Descontinuado: true);

            //TransactionalBatch batch = container.CreateTransactionalBatch(new PartitionKey("Electronica"))
            //    .UpsertItem<Producto>(prod3)
            //    .UpsertItem<Producto>(prod4)
            //    .UpsertItem<Producto>(prod5);

            //using TransactionalBatchResponse batchResponse = await batch.ExecuteAsync();


            // Modificar Item
            //Producto prod1 = new(id: "100",
            //                 Nombre: "Descripcion modificada de Producto 1",
            //                 categoria: "Electronica",
            //                 Precio: 410,
            //                 Descontinuado: false);

            //ItemResponse<Producto> result = await container.ReplaceItemAsync<Producto>(prod1, "1");

            //Console.WriteLine("Item modificado");

            // ELiminar item
            //var response = await container.DeleteItemAsync<Producto>("4", new PartitionKey("Electronica"));
            //Console.WriteLine("Item Eliminado");

            //FeedIterator<Producto> queryResultSetIterator =
            //    container.GetItemQueryIterator<Producto>("Select * from Items p where p.Descontinuado=false");

            //while (queryResultSetIterator.HasMoreResults)
            //{
            //    FeedResponse<Producto> currentTesultSet = await queryResultSetIterator.ReadNextAsync();
            //    foreach (Producto item in currentTesultSet)
            //    {
            //        Console.WriteLine(item.Nombre);
            //    }
            //}

            // Consulta de un solo Item
            //Producto response = await container.ReadItemAsync<Producto>("3", new PartitionKey("Electronica"));
            //Console.WriteLine(response.ToString());

            IOrderedQueryable<Producto> queryable = container.GetItemLinqQueryable<Producto>();
            var matches = queryable
                .Where(p => p.categoria == "Electronica")
                .Where(p => p.Precio > 100);

            FeedIterator<Producto> linqFeed = matches.ToFeedIterator();

            while (linqFeed.HasMoreResults)
            {
                FeedResponse<Producto> currentTesultSet = await linqFeed.ReadNextAsync();
                foreach (Producto item in currentTesultSet)
                {
                    Console.WriteLine(item.Nombre);
                }
            }

        }
    }
}
