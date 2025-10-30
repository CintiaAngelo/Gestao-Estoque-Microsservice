using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly RelatorioService _relatorioService;

        public RelatorioController(RelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        [HttpGet("valor-total")]
        public IActionResult CalcularValorTotal()
        {
            try
            {
                var total = _relatorioService.CalcularValorTotalEstoque();
                return Ok(new { ValorTotalEstoque = total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet("vencendo")]
        public IActionResult ProdutosVencendo()
        {
            try
            {
                var lista = _relatorioService.ListarProdutosVencendoEm7Dias();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet("abaixo-minimo")]
        public IActionResult ProdutosAbaixoMinimo()
        {
            try
            {
                var lista = _relatorioService.ListarAbaixoDoMinimo();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
