using UtopiaAPI.Models;
using UtopiaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UtopiaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase{
    private UtopiaDbContext? _dbContext;

    public ClienteController(UtopiaDbContext dbContext){
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("Cadastar")]
    public async Task<ActionResult> Cadastrar(Cliente cliente){
        if(_dbContext is null) return NotFound();
        if(_dbContext.Cliente is null) return NotFound();
        await _dbContext.AddAsync(cliente);

        var novoCarrinho = new Carrinho { Items = new List<ItemCarrinho>() };
        await _dbContext.AddAsync(novoCarrinho);
        await _dbContext.SaveChangesAsync();

        if (novoCarrinho != null)
        {
            cliente.Carrinho = novoCarrinho;    
        } 
        else
        {
            return BadRequest("Não tem carrinho");
        }

        cliente.CarrinhoId = novoCarrinho.Id;

        await _dbContext.SaveChangesAsync();
        return Created ("", cliente);
    }

    [HttpGet]
    [Route("listar")]
    public async Task<ActionResult<IEnumerable<Cliente>>> Listar(){
        if(_dbContext is null) return NotFound();
        if(_dbContext.Cliente is null) return NotFound();
        return await _dbContext.Cliente.ToListAsync();
    }

    [HttpGet]
    [Route("Procurar")]
    public async Task<ActionResult<Cliente>> Buscar(int id){
        if(_dbContext is null) return NotFound();
        if(_dbContext.Cliente is null) return NotFound();
        var clienteTemp = await _dbContext.Cliente.FindAsync(id);
        if(clienteTemp is null) return NotFound();
        return clienteTemp;
    }

    [HttpPut]
    [Route("Alterar")]
    public async Task<ActionResult> Alterar(Cliente cliente)
    {
        if(_dbContext is null) return NotFound();
        if(_dbContext.Cliente is null) return NotFound();
        var clienteTemp = await _dbContext.Cliente.FindAsync(cliente.Id);
        if(clienteTemp is null) return NotFound();      
        _dbContext.Entry(clienteTemp).State = EntityState.Detached; 
        _dbContext.Cliente.Update(cliente);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete()]
    [Route("excluir")]
    public async Task<ActionResult> Excluir(int id)
    {
        if(_dbContext is null) return NotFound();
        if(_dbContext.Cliente is null) return NotFound();
        var clienteTemp = await _dbContext.Cliente.FindAsync(id);
        if(clienteTemp is null) return NotFound();
        _dbContext.Remove(clienteTemp);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}