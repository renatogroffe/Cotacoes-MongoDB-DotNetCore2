using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace APICotacoes
{
    public class Historico
    {
        public ObjectId _id { get; set; }
        public string DataExtracao { get; set; }
        public List<Cotacao> Cotacoes { get; set; } = new List<Cotacao>();
    }

    public class Cotacao
    {
        public string NomeMoeda { get; set; }
        public DateTime DtUltimaCarga { get; set; }
        public double ValorCompra { get; set; }
        public double ValorVenda { get; set; }
        public string Variacao { get; set; }
    }
}