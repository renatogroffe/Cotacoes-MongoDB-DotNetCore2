using MongoDB.Driver;

namespace CarregarCotacoes
{
    public class CotacoesDAO
    {
        private MongoClient _client;
        private IMongoDatabase _db;

        public CotacoesDAO(string stringConnection)
        {
            _client = new MongoClient(stringConnection);
            _db = _client.GetDatabase("DBCotacoes");
        }

        public void CarregarDados(Historico historico)
        {
            var historicoCotacoes =
                _db.GetCollection<Historico>("Historico");
            historicoCotacoes.InsertOne(historico);
        }
    }
}