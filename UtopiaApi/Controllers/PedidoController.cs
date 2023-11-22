using UtopiaAPI.Models;
using UtopiaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UtopiaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PedidoController : ControllerBase{
    private UtopiaDbContext? _dbContext;

    public PedidoController(UtopiaDbContext dbContext){
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("Criar Pedido")]
    public IActionResult CriarPedido(int clienteId, int carrinhoId)
    {
        var cliente = _dbContext.Cliente.FirstOrDefault(c => c.Id == clienteId);
        var carrinho = _dbContext.Carrinho.Include(c => c.Items).ThenInclude(item => item.Produto).FirstOrDefault(c => c.Id == carrinhoId);

        if (cliente == null || carrinho == null)
        {
            return NotFound("Cliente ou carrinho não encontrado.");
        }

        decimal totalPedido = (decimal)carrinho.Items.Sum(item => item.Subtotal);

        var novoPedido = new Pedido
        {
            DataPedido = DateTime.Now,
            Status = "esperando pagamento",
            ClienteId = clienteId,
            Cliente = cliente,
            CarrinhoId = carrinhoId,
            Carrinho = carrinho,
            Total = totalPedido
        };

        _dbContext.Pedido.Add(novoPedido);
        _dbContext.SaveChanges();

        return Ok(novoPedido); // ou você pode redirecionar para a ação que exibe os detalhes do pedido, por exemplo
    }

    [HttpGet]
    [Route("{id}Procurar Pedido")]
    public IActionResult ObterPedidoPorId(int id)
    {
    var pedido = _dbContext.Pedido.Include(p => p.Cliente).Include(p => p.Carrinho).FirstOrDefault(p => p.Id == id);

    if (pedido == null)
    {
        return NotFound("Pedido não encontrado.");
    }

    return Ok(pedido);
    }

    [HttpPatch]
    [Route ("{id}/AtualizarStatus")]
    public IActionResult AtualizarStatusDoPedido(int id, string novoStatus)
    {
    var pedido = _dbContext.Pedido.FirstOrDefault(p => p.Id == id);

    if (pedido == null)
    {
        return NotFound("Pedido não encontrado.");
    }

    pedido.Status = novoStatus;
    _dbContext.SaveChanges();

    return Ok(pedido);
    }



}