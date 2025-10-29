using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
        public enum Categoria
        {
            PERECIVEL,
            NAO_PERECIVEL
        }
        public class Produto
        {
            public string CodigoSKU { get; set; }
            public string Nome { get; set; }
            public Categoria Categoria { get; set; }
            public decimal PrecoUnitario { get; set; }
            public int QuantidadeMinima { get; set; }
            public string DataCriacao { get; set; }
        }

}

