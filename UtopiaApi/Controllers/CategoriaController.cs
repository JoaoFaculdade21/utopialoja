using UtopiaAPI.Models;
using UtopiaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace UtopiaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriaController : ControllerBase{
    private UtopiaDbContext? _dbContext;

    public CategoriaController(UtopiaDbContext dbContext){
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route("cadastrar")]
    public async Task<ActionResult> Cadastrar(Categoria categoria){
        if(_dbContext is null) return NotFound();
        if(_dbContext.Categoria is null) return NotFound();
        await _dbContext.AddAsync(categoria);
        await _dbContext.SaveChangesAsync();
        return Created ("", categoria);
    }
    
    [HttpGet]
    [Route("listar")]
    public async Task<ActionResult<IEnumerable<Categoria>>> Listar(){
        if(_dbContext is null) return NotFound();
        if(_dbContext.Categoria is null) return NotFound();
        return await _dbContext.Categoria.ToListAsync();
    }
    
    [HttpGet]
    [Route("procurar")]
    public async Task<ActionResult<Categoria>> Buscar(int id){
          if(_dbContext is null) return NotFound();
        if(_dbContext.Categoria is null) return NotFound();
        var categoriaTemp = await _dbContext.Categoria.FindAsync(id);
        if(categoriaTemp is null) return NotFound();
        return categoriaTemp;
    }

    [HttpPut]
    [Route("alterar")]
    public async Task<ActionResult> Alterar(Categoria categoria)
    {
        if(_dbContext is null) return NotFound();
        if(_dbContext.Categoria is null) return NotFound();
        var categoriaTemp = await _dbContext.Categoria.FindAsync(categoria.Id);
        if(categoriaTemp is null) return NotFound();      
        _dbContext.Entry(categoriaTemp).State = EntityState.Detached; 
        _dbContext.Categoria.Update(categoria);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete()]
    [Route("excluir")]
    public async Task<ActionResult> Excluir(int id)
    {
        if(_dbContext is null) return NotFound();
        if(_dbContext.Categoria is null) return NotFound();
        var categoriaTemp = await _dbContext.Categoria.FindAsync(id);
        if(categoriaTemp is null) return NotFound();
        _dbContext.Remove(categoriaTemp);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    

}