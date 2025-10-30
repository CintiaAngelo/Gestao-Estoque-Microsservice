using Domain;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpPost]
        public IActionResult CadastrarProduto([FromBody] Produto produto)
        {
            try
            {
                _produtoService.CadastrarProduto(produto);
                return Ok("Produto cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet("abaixo-minimo")]
        public IActionResult ListarProdutosAbaixoDoMinimo()
        {
            var produtos = _produtoService.ListarProdutosAbaixoDoMinimo();
            return Ok(produtos);
        }
    }
}
