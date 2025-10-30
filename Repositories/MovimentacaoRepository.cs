using Dapper;
using Domain;
using MySql.Data.MySqlClient;

namespace Infrastructure.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly string _connectionString;

        public MovimentacaoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void RegistrarMovimentacao(MovimentacaoEstoque movimentacao)
        {
            using var connection = new MySqlConnection(_connectionString);
            var query = @"INSERT INTO MovimentacaoEstoque 
                         (Tipo, Quantidade, DataMovimentacao, Lote, DataValidade, ProdutoId)
                         VALUES (@Tipo, @Quantidade, @DataMovimentacao, @Lote, @DataValidade, @ProdutoId)";
            connection.Execute(query, movimentacao);
        }

        public List<MovimentacaoEstoque> ListarPorProduto(int produtoId)
        {
            using var connection = new MySqlConnection(_connectionString);
            var query = "SELECT * FROM MovimentacaoEstoque WHERE ProdutoId = @ProdutoId";
            return connection.Query<MovimentacaoEstoque>(query, new { ProdutoId = produtoId }).ToList();
        }
    }
}
