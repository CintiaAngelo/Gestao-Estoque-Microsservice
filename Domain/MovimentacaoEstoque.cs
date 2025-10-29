using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public TipoMovimentacao Tipo { get; set; }
        public int Quantidade { get; set; }
        public string DataMovimentacao { get; set; }
        public string? Lote { get; set; }
        public string? DataValidade { get; set; }
        public int ProdutoId { get; set; }



    }
}
