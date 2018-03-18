using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace APICotacoes.Controllers
{
    [Route("api/[controller]")]
    public class CotacoesController : Controller
    {
        [HttpGet]
        public List<Historico> GetCotacoes(
            [FromServices]IConfiguration configuration)
        {
            MongoClient client = new MongoClient(
                configuration.GetConnectionString("BaseCotacoes"));
            IMongoDatabase db = client.GetDatabase("DBCotacoes");

            return db.GetCollection<Historico>("Historico")
                .AsQueryable().ToList();
        }

        [HttpGet("carregar")]
        public object CarregarCotacoes(
            [FromServices]RabbitMQConfigurations rabbitMQConfigurations)
        {
            var factory = new ConnectionFactory()
            {
                HostName = rabbitMQConfigurations.HostName,
                Port = rabbitMQConfigurations.Port,
                UserName = rabbitMQConfigurations.UserName,
                Password = rabbitMQConfigurations.Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "CarregarCotacoes",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "Solicitação de Carga - " +
                    $"API Cotacoes - {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "CarregarCotacoes",
                                     basicProperties: null,
                                     body: body);
            }

            return new
            {
                Resultado = "Mensagem encaminhada com sucesso"
            };
        }
    }
}