using Domain;

namespace Infrastructure.Repositories
{
    public interface IProdutoRepository
    {
        void CadastrarProduto(Produto produto);
        Produto? BuscarPorId(int id);
        Produto? BuscarPorSKU(string sku);
        List<Produto> ListarTodos();
        void Atualizar(Produto produto);
    }
}
