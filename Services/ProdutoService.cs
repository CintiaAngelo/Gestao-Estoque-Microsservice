using Domain;
using Infrastructure.Repositories;

namespace Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public void CadastrarProduto(Produto produto)
        {
            if (produto == null)
                throw new Exception("Produto não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(produto.CodigoSKU))
                throw new Exception("Código SKU é obrigatório.");

            if (produto.Categoria == Categoria.PERECIVEL && produto.DataCriacao == default)
                produto.DataCriacao = DateTime.Now;

            produto.QuantidadeEstoque = 0;

            _produtoRepository.CadastrarProduto(produto);
        }

        public List<Produto> ListarProdutosAbaixoDoMinimo()
        {
            var produtos = _produtoRepository.ListarTodos();
            return produtos.Where(p => p.QuantidadeEstoque < p.QuantidadeMinima).ToList();
        }

        public Produto? BuscarPorSKU(string sku)
        {
            return _produtoRepository.BuscarPorSKU(sku);
        }
    }
}
