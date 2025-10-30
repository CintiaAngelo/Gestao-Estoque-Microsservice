using System;
using System.Text.Json.Serialization;

namespace Domain
{
    public enum TipoMovimentacao
    {
        ENTRADA,
        SAIDA
    }

    public class MovimentacaoEstoque
    {
        public int Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoMovimentacao Tipo { get; set; }

        public int Quantidade { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string? Lote { get; set; }
        public DateTime? DataValidade { get; set; }
        public int ProdutoId { get; set; }
    }
}