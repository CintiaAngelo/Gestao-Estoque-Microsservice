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
            // Validação: Movimentação não pode ser nula
            if (movimentacao == null)
                throw new Exception("Movimentação não pode ser nula.");

            // Validação: Quantidade positiva
            if (movimentacao.Quantidade <= 0)
                throw new Exception("Quantidade deve ser positiva.");

            // Validação: Produto deve existir
            var produto = _produtoRepository.BuscarPorId(movimentacao.ProdutoId)
                ?? throw new Exception("Produto não encontrado.");

            // Validações específicas para produtos perecíveis
            if (produto.Categoria == Categoria.PERECIVEL)
            {
                if (movimentacao.DataValidade == null)
                    throw new Exception("Produtos perecíveis devem ter data de validade.");

                // Validação: Não permitir movimentação após data de validade
                if (movimentacao.DataValidade.Value.Date < DateTime.Now.Date)
                    throw new Exception("Não é permitido movimentar produtos vencidos.");

                // Validação: Lote é obrigatório para perecíveis
                if (string.IsNullOrWhiteSpace(movimentacao.Lote))
                    throw new Exception("Produtos perecíveis devem ter lote informado.");
            }
            else
            {
                // Para não perecíveis, garantir que não tenha data de validade e lote
                movimentacao.DataValidade = null;
                movimentacao.Lote = null;
            }

            // Processar movimentação baseada no tipo
            if (movimentacao.Tipo == TipoMovimentacao.ENTRADA)
            {
                ProcessarEntrada(produto, movimentacao);
            }
            else if (movimentacao.Tipo == TipoMovimentacao.SAIDA)
            {
                ProcessarSaida(produto, movimentacao);
            }
            else
            {
                throw new Exception("Tipo de movimentação inválido.");
            }

            // Configurar data da movimentação
            movimentacao.DataMovimentacao = DateTime.Now;

            // Salvar no banco
            _movimentacaoRepository.RegistrarMovimentacao(movimentacao);
            _produtoRepository.Atualizar(produto);

            // Verificar alerta de estoque mínimo
            VerificarAlertaEstoqueMinimo(produto);
        }

        private void ProcessarEntrada(Produto produto, MovimentacaoEstoque movimentacao)
        {
            // Para entrada, simplesmente adiciona ao estoque
            produto.QuantidadeEstoque += movimentacao.Quantidade;
        }

        private void ProcessarSaida(Produto produto, MovimentacaoEstoque movimentacao)
        {
            // Validação: Estoque suficiente
            if (produto.QuantidadeEstoque < movimentacao.Quantidade)
            {
                throw new Exception($"Estoque insuficiente para saída. " +
                                  $"Disponível: {produto.QuantidadeEstoque}, " +
                                  $"Solicitado: {movimentacao.Quantidade}");
            }

            // Para saída, subtrai do estoque
            produto.QuantidadeEstoque -= movimentacao.Quantidade;
        }

        private void VerificarAlertaEstoqueMinimo(Produto produto)
        {
            if (produto.QuantidadeEstoque < produto.QuantidadeMinima)
            {
                // Aqui você poderia implementar um sistema de notificação mais robusto
                // Por enquanto, apenas log no console
                Console.WriteLine($"🚨 ALERTA: Produto '{produto.Nome}' (SKU: {produto.CodigoSKU}) " +
                                $"está abaixo do estoque mínimo! " +
                                $"Estoque: {produto.QuantidadeEstoque}, " +
                                $"Mínimo: {produto.QuantidadeMinima}");
            }
        }

        public List<MovimentacaoEstoque> ListarPorProduto(int produtoId)
        {
            if (produtoId <= 0)
                throw new Exception("ID do produto deve ser positivo.");

            return _movimentacaoRepository.ListarPorProduto(produtoId);
        }

        public List<MovimentacaoEstoque> ListarTodasMovimentacoes()
        {
            // Se necessário implementar um método no repositório para listar todas
            // Por enquanto, retorna lista vazia
            return new List<MovimentacaoEstoque>();
        }

        public List<MovimentacaoEstoque> ListarMovimentacoesPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var todasMovimentacoes = ListarTodasMovimentacoes();
            return todasMovimentacoes
                .Where(m => m.DataMovimentacao >= dataInicio && m.DataMovimentacao <= dataFim)
                .ToList();
        }

        public decimal CalcularValorTotalMovimentacoesPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var movimentacoes = ListarMovimentacoesPorPeriodo(dataInicio, dataFim);
            decimal total = 0;

            foreach (var mov in movimentacoes)
            {
                var produto = _produtoRepository.BuscarPorId(mov.ProdutoId);
                if (produto != null)
                {
                    if (mov.Tipo == TipoMovimentacao.ENTRADA)
                    {
                        total += mov.Quantidade * produto.PrecoUnitario;
                    }
                    else
                    {
                        total -= mov.Quantidade * produto.PrecoUnitario;
                    }
                }
            }

            return total;
        }

        public int ObterSaldoAtual(int produtoId)
        {
            var produto = _produtoRepository.BuscarPorId(produtoId);
            if (produto == null)
                throw new Exception("Produto não encontrado.");

            return produto.QuantidadeEstoque;
        }

        public Dictionary<int, int> ObterSaldosTodosProdutos()
        {
            var produtos = _produtoRepository.ListarTodos();
            return produtos.ToDictionary(p => p.Id, p => p.QuantidadeEstoque);
        }

        public void RegistrarEntradaRapida(int produtoId, int quantidade, string lote = null, DateTime? dataValidade = null)
        {
            var movimentacao = new MovimentacaoEstoque
            {
                ProdutoId = produtoId,
                Tipo = TipoMovimentacao.ENTRADA,
                Quantidade = quantidade,
                Lote = lote,
                DataValidade = dataValidade,
                DataMovimentacao = DateTime.Now
            };

            RegistrarMovimentacao(movimentacao);
        }

        public void RegistrarSaidaRapida(int produtoId, int quantidade)
        {
            var movimentacao = new MovimentacaoEstoque
            {
                ProdutoId = produtoId,
                Tipo = TipoMovimentacao.SAIDA,
                Quantidade = quantidade,
                DataMovimentacao = DateTime.Now
            };

            RegistrarMovimentacao(movimentacao);
        }
    }
}