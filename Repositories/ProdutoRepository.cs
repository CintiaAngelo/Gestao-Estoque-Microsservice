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

        public void Atualizar(Produto produto)
        {
            throw new NotImplementedException();
        }

        public void AtualizarProduto(Produto produto)
        {
            throw new NotImplementedException();
        }

        public Produto? BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Produto BuscarPorSKU(string codigoSKU)
        {
            throw new NotImplementedException();
        }

        public void CadastrarProduto(Produto produto)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string sql = @"INSERT INTO Produto 
                           (CodigoSKU, Nome, Categoria, PrecoUnitario, QuantidadeMinima, QuantidadeEstoque, DataCriacao)
                           VALUES (@CodigoSKU, @Nome, @Categoria, @PrecoUnitario, @QuantidadeMinima, @QuantidadeEstoque, @DataCriacao)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CodigoSKU", produto.CodigoSKU);
            cmd.Parameters.AddWithValue("@Nome", produto.Nome);
            cmd.Parameters.AddWithValue("@Categoria", produto.Categoria.ToString());
            cmd.Parameters.AddWithValue("@PrecoUnitario", produto.PrecoUnitario);
            cmd.Parameters.AddWithValue("@QuantidadeMinima", produto.QuantidadeMinima);
            cmd.Parameters.AddWithValue("@QuantidadeEstoque", produto.QuantidadeEstoque);
            cmd.Parameters.AddWithValue("@DataCriacao", produto.DataCriacao);
            cmd.ExecuteNonQuery();
        }

        public List<Produto> ListarTodos()
        {
            var produtos = new List<Produto>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Produto";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                produtos.Add(new Produto
                {
                    Id = reader.GetInt32("Id"),
                    CodigoSKU = reader.GetString("CodigoSKU"),
                    Nome = reader.GetString("Nome"),
                    Categoria = Enum.Parse<Categoria>(reader.GetString("Categoria")),
                    PrecoUnitario = reader.GetDecimal("PrecoUnitario"),
                    QuantidadeMinima = reader.GetInt32("QuantidadeMinima"),
                    QuantidadeEstoque = reader.GetInt32("QuantidadeEstoque"),
                    DataCriacao = reader.GetDateTime("DataCriacao")
                });
            }

            return produtos;
        }

        public Produto? ObterPorSKU(string codigoSKU)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT * FROM Produto WHERE CodigoSKU = @CodigoSKU";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@CodigoSKU", codigoSKU);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Produto
                {
                    Id = reader.GetInt32("Id"),
                    CodigoSKU = reader.GetString("CodigoSKU"),
                    Nome = reader.GetString("Nome"),
                    Categoria = Enum.Parse<Categoria>(reader.GetString("Categoria")),
                    PrecoUnitario = reader.GetDecimal("PrecoUnitario"),
                    QuantidadeMinima = reader.GetInt32("QuantidadeMinima"),
                    QuantidadeEstoque = reader.GetInt32("QuantidadeEstoque"),
                    DataCriacao = reader.GetDateTime("DataCriacao")
                };
            }

            return null;
        }
    }
}
