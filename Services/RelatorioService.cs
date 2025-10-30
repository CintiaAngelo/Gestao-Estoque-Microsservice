using Domain;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class RelatorioService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoRepository _movimentacaoRepository;

        public RelatorioService(IProdutoRepository produtoRepository, IMovimentacaoRepository movimentacaoRepository)
        {
            _produtoRepository = produtoRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        // Valor total do estoque (quantidade * preço unitário)
        public decimal CalcularValorTotalEstoque()
        {
            var produtos = _produtoRepository.ListarTodos();
            return produtos.Sum(p => p.PrecoUnitario * p.QuantidadeEstoque);
        }

        // Produtos abaixo do estoque mínimo
        public List<Produto> ListarAbaixoDoMinimo()
        {
            var produtos = _produtoRepository.ListarTodos();
            return produtos.Where(p => p.QuantidadeEstoque < p.QuantidadeMinima).ToList();
        }

        // Produtos perecíveis com validade em até 7 dias (NÃO necessita de parâmetro)
        public List<Produto> ListarProdutosVencendoEm7Dias()
        {
            var produtosPereciveis = _produtoRepository.ListarTodos()
                .Where(p => p.Categoria == Categoria.PERECIVEL)
                .ToList();

            var resultado = new List<Produto>();
            var hoje = DateTime.Now;
            var limite = hoje.AddDays(7);

            foreach (var produto in produtosPereciveis)
            {
                var movs = _movimentacaoRepository.ListarPorProduto(produto.Id)
                    .Where(m => m.DataValidade.HasValue)
                    .ToList();

                // Se alguma movimentação do produto tiver validade dentro do período, inclui o produto
                if (movs.Any(m => m.DataValidade.Value.Date >= hoje.Date && m.DataValidade.Value.Date <= limite.Date))
                {
                    resultado.Add(produto);
                }
            }

            return resultado.Distinct().ToList();
        }
    }
}
