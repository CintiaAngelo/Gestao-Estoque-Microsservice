using Dapper;
using Domain;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CadastrarProduto(Produto produto)
        {
            using var conn = new MySqlConnection(_connectionString);
            var query = @"INSERT INTO Produto 
                         (CodigoSKU, Nome, Categoria, PrecoUnitario, QuantidadeMinima, QuantidadeEstoque, DataCriacao)
                         VALUES (@CodigoSKU, @Nome, @Categoria, @PrecoUnitario, @QuantidadeMinima, @QuantidadeEstoque, @DataCriacao)";
            conn.Execute(query, produto);
        }

        public Produto? BuscarPorId(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.QueryFirstOrDefault<Produto>(
                "SELECT * FROM Produto WHERE Id = @Id",
                new { Id = id });
        }

        public Produto? BuscarPorSKU(string sku)
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.QueryFirstOrDefault<Produto>(
                "SELECT * FROM Produto WHERE CodigoSKU = @CodigoSKU",
                new { CodigoSKU = sku });
        }

        public List<Produto> ListarTodos()
        {
            using var conn = new MySqlConnection(_connectionString);
            return conn.Query<Produto>("SELECT * FROM Produto").ToList();
        }

        public void Atualizar(Produto produto)
        {
            using var conn = new MySqlConnection(_connectionString);
            var query = @"UPDATE Produto SET 
                         Nome = @Nome, 
                         Categoria = @Categoria, 
                         PrecoUnitario = @PrecoUnitario,
                         QuantidadeMinima = @QuantidadeMinima, 
                         QuantidadeEstoque = @QuantidadeEstoque
                         WHERE Id = @Id";
            conn.Execute(query, produto);
        }

        // Remove métodos duplicados não utilizados
    }
}