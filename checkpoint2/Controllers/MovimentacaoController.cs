using Domain;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly MovimentacaoService _movimentacaoService;

        public MovimentacaoController(MovimentacaoService movimentacaoService)
        {
            _movimentacaoService = movimentacaoService;
        }

        [HttpPost]
        public IActionResult RegistrarMovimentacao([FromBody] MovimentacaoEstoque mov)
        {
            try
            {
                _movimentacaoService.RegistrarMovimentacao(mov);
                return Ok("Movimentação registrada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
