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
            // Validações básicas
            if (produto == null)
                throw new Exception("Produto não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(produto.CodigoSKU))
                throw new Exception("Código SKU é obrigatório.");

            if (string.IsNullOrWhiteSpace(produto.Nome))
                throw new Exception("Nome do produto é obrigatório.");

            // Validação: SKU único
            var existente = _produtoRepository.BuscarPorSKU(produto.CodigoSKU);
            if (existente != null)
                throw new Exception("Já existe um produto com este SKU.");

            // Validação: Preço positivo
            if (produto.PrecoUnitario <= 0)
                throw new Exception("Preço unitário deve ser positivo.");

            // Validação: Quantidade mínima não negativa
            if (produto.QuantidadeMinima < 0)
                throw new Exception("Quantidade mínima não pode ser negativa.");

            // Validação específica para produtos perecíveis
            if (produto.Categoria == Categoria.PERECIVEL)
            {
                // Valida se lote está presente (opcional, mas recomendado)
                // Produtos perecíveis devem ter data de criação definida
                if (produto.DataCriacao == default)
                {
                    produto.DataCriacao = DateTime.Now;
                }
            }

            // Configurar valores padrão
            if (produto.DataCriacao == default)
            {
                produto.DataCriacao = DateTime.Now;
            }

            produto.QuantidadeEstoque = 0; // Novo produto começa com estoque zero

            _produtoRepository.CadastrarProduto(produto);
        }

        public List<Produto> ListarProdutosAbaixoDoMinimo()
        {
            var produtos = _produtoRepository.ListarTodos();
            return produtos.Where(p => p.QuantidadeEstoque < p.QuantidadeMinima).ToList();
        }

        public Produto? BuscarPorSKU(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new Exception("SKU não pode ser vazio.");

            return _produtoRepository.BuscarPorSKU(sku);
        }

        public Produto? BuscarPorId(int id)
        {
            if (id <= 0)
                throw new Exception("ID do produto deve ser positivo.");

            return _produtoRepository.BuscarPorId(id);
        }

        public List<Produto> ListarTodos()
        {
            return _produtoRepository.ListarTodos();
        }

        public void AtualizarProduto(Produto produto)
        {
            // Validações similares ao cadastro
            if (produto == null)
                throw new Exception("Produto não pode ser nulo.");

            if (string.IsNullOrWhiteSpace(produto.CodigoSKU))
                throw new Exception("Código SKU é obrigatório.");

            if (produto.PrecoUnitario <= 0)
                throw new Exception("Preço unitário deve ser positivo.");

            if (produto.QuantidadeMinima < 0)
                throw new Exception("Quantidade mínima não pode ser negativa.");

            if (produto.QuantidadeEstoque < 0)
                throw new Exception("Quantidade em estoque não pode ser negativa.");

            // Verificar se produto existe
            var produtoExistente = _produtoRepository.BuscarPorId(produto.Id);
            if (produtoExistente == null)
                throw new Exception("Produto não encontrado para atualização.");

            _produtoRepository.Atualizar(produto);
        }

        public void AtualizarEstoque(int produtoId, int novaQuantidade)
        {
            if (novaQuantidade < 0)
                throw new Exception("Quantidade em estoque não pode ser negativa.");

            var produto = _produtoRepository.BuscarPorId(produtoId);
            if (produto == null)
                throw new Exception("Produto não encontrado.");

            produto.QuantidadeEstoque = novaQuantidade;
            _produtoRepository.Atualizar(produto);
        }

        public int ObterQuantidadeEstoque(int produtoId)
        {
            var produto = _produtoRepository.BuscarPorId(produtoId);
            if (produto == null)
                throw new Exception("Produto não encontrado.");

            return produto.QuantidadeEstoque;
        }

        public bool VerificarEstoqueSuficiente(int produtoId, int quantidadeRequerida)
        {
            if (quantidadeRequerida <= 0)
                throw new Exception("Quantidade requerida deve ser positiva.");

            var produto = _produtoRepository.BuscarPorId(produtoId);
            if (produto == null)
                throw new Exception("Produto não encontrado.");

            return produto.QuantidadeEstoque >= quantidadeRequerida;
        }

        public List<Produto> ListarProdutosPereciveis()
        {
            var produtos = _produtoRepository.ListarTodos();
            return produtos.Where(p => p.Categoria == Categoria.PERECIVEL).ToList();
        }

        public List<Produto> ListarProdutosNaoPereciveis()
        {
            var produtos = _produtoRepository.ListarTodos();
            return produtos.Where(p => p.Categoria == Categoria.NAO_PERECIVEL).ToList();
        }
    }
}