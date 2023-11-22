using UtopiaAPI.Models;
using UtopiaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace UtopiaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase{
    private UtopiaDbContext? _dbContext;

    public ProdutoController(UtopiaDbContext dbContext){
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("Cadastrar")]
    public async Task<ActionResult> Cadastrar(Produto produto)
    {
        if (_dbContext is null) return NotFound();
        if (_dbContext.Categoria is null) return NotFound();

        var categoriaTemp = await _dbContext.Categoria.FindAsync(produto.CategoriaId);
        if (categoriaTemp != null)
        {
            produto.Categoria = categoriaTemp;    
        } 
        else
        {
            return BadRequest("Não há essa categoria");
        }

        await _dbContext.AddAsync(produto);
        await _dbContext.SaveChangesAsync();

        return Created("", produto);
    }

    [HttpGet]
    [Route("listar")]
    public async Task<ActionResult<IEnumerable<Produto>>> Listar(){
        if(_dbContext is null) return NotFound();
        if(_dbContext.Produto is null) return NotFound();
        var categorias = await _dbContext.Produto
            .Include(p => p.Categoria)
            .ToListAsync();
        return await _dbContext.Produto.ToListAsync();
    }

    [HttpGet]
    [Route("Procurar")]
    public async Task<ActionResult<Produto>> Buscar(int id){
        if(_dbContext is null) return NotFound();
        if(_dbContext.Produto is null) return NotFound();
        var produtoTemp = await _dbContext.Produto.FindAsync(id);
        if(produtoTemp is null) return NotFound();
        return produtoTemp;
    }

    [HttpPut]
    [Route("Alterar")]
    public async Task<ActionResult> Alterar(Produto produto)
    {
        if(_dbContext is null) return NotFound();
        if(_dbContext.Produto is null) return NotFound();
        var produtoTemp = await _dbContext.Produto.FindAsync(produto.Id);
        if(produtoTemp is null) return NotFound();    
        _dbContext.Produto.Update(produto);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete()]
    [Route("excluir")]
    public async Task<ActionResult> Excluir(int id)
    {
        if(_dbContext is null) return NotFound();
        if(_dbContext.Produto is null) return NotFound();
        var produtoTemp = await _dbContext.Produto.FindAsync(id);
        if(produtoTemp is null) return NotFound();
        _dbContext.Remove(produtoTemp);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
}