using Domain;

namespace Infrastructure.Repositories
{
    public interface IMovimentacaoRepository
    {
        void RegistrarMovimentacao(MovimentacaoEstoque movimentacao);
        List<MovimentacaoEstoque> ListarPorProduto(int produtoId);
    }
}
