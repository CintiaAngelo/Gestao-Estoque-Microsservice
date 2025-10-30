using System;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Produto
    {
        public int Id { get; set; }
        public string CodigoSKU { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Categoria Categoria { get; set; }

        public decimal PrecoUnitario { get; set; }
        public int QuantidadeMinima { get; set; }
        public int QuantidadeEstoque { get; set; }
        public DateTime DataCriacao { get; set; }
        public List<MovimentacaoEstoque>? Movimentacoes { get; set; }
    }

    public enum Categoria
    {
        PERECIVEL,
        NAO_PERECIVEL
    }
}