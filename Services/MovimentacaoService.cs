using Domain;
using Infrastructure.Repositories;

namespace Services
{
    public class MovimentacaoService
    {
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public MovimentacaoService(
            IMovimentacaoRepository movimentacaoRepository,
            IProdutoRepository produtoRepository)
        {
            _movimentacaoRepository = movimentacaoRepository;
            _produtoRepository = produtoRepository;
        }

        public void RegistrarMovimentacao(MovimentacaoEstoque movimentacao)
        {
            var produto = _produtoRepository.BuscarPorId(movimentacao.ProdutoId)
                ?? throw new Exception("Produto não encontrado.");

            if (movimentacao.Tipo == TipoMovimentacao.ENTRADA)
            {
                produto.QuantidadeEstoque += movimentacao.Quantidade;

                if (produto.Categoria == Categoria.PERECIVEL && movimentacao.DataValidade == null)
                    throw new Exception("Produtos perecíveis devem ter Data de Validade.");
            }
            else if (movimentacao.Tipo == TipoMovimentacao.SAIDA)
            {
                if (produto.QuantidadeEstoque < movimentacao.Quantidade)
                    throw new Exception("Estoque insuficiente para saída.");

                produto.QuantidadeEstoque -= movimentacao.Quantidade;
            }

            _movimentacaoRepository.RegistrarMovimentacao(movimentacao);
            _produtoRepository.Atualizar(produto);
        }

        public List<MovimentacaoEstoque> ListarPorProduto(int produtoId)
        {
            return _movimentacaoRepository.ListarPorProduto(produtoId);
        }
    }
}
