using UtopiaAPI.Models;
using UtopiaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UtopiaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PagamentoController : ControllerBase{
    
    private UtopiaDbContext? _dbContext;

    public PagamentoController(UtopiaDbContext dbContext){
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("RegristarPagametno")]
    public IActionResult RegistrarPagamento(int clienteId, int pedidoId){
        var cliente = _dbContext.Cliente.FirstOrDefault(c => c.Id == clienteId);
        var pedido = _dbContext.Pedido.FirstOrDefault(p => p.Id == pedidoId);

        if (cliente == null || pedido == null)
        {
            return NotFound("Cliente ou pedido não encontrado.");
        }

        // Verifica se o pagamento foi um sucesso
        if (cliente.Carteira >= pedido.Total)
        {
            pedido.Status = "pago";
            cliente.Carteira -= pedido.Total;
        }
        else if (DateTime.Now - pedido.DataPedido > TimeSpan.FromDays(2))
        {
            pedido.Status = "não pago";
        }
        else
        {
            return BadRequest("Dinheiro insuficiente.");
        }

        // Registra o pagamento
        var novoPagamento = new Pagamento
        {
            Valor = pedido.Total,
            DataPagamento = DateTime.Now,
            ClienteId = clienteId,
            PedidoId = pedidoId
        };
        _dbContext.Pagamento.Add(novoPagamento);

        _dbContext.SaveChanges();

        return Ok(pedido);
    }

    [HttpGet]
    [Route("{id}VisualizarPagemento")]
    public IActionResult ObterPagamentoPorId(int id)
    {
    var pagamento = _dbContext.Pagamento.FirstOrDefault(p => p.Id == id);

    if (pagamento == null)
    {
        return NotFound("Pagamento não encontrado.");
    }

    return Ok(pagamento);
    }

}