using UtopiaAPI.Models;
using UtopiaAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UtopiaAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CarrinhoController : ControllerBase{
    private UtopiaDbContext? _dbContext;

    public CarrinhoController(UtopiaDbContext dbContext){
        _dbContext = dbContext;
    }

    [HttpPost]
    [Route ("{carrinhoId}/Adicionar Item/{produtoId}")]
    public async Task<ActionResult<Carrinho>> AdicionarItem(int produtoId, int carrinhoId){
        
        if(_dbContext is null) return NotFound();
        if(_dbContext.Produto is null) return NotFound();
        if(_dbContext.Carrinho is null) return NotFound();
        
        var carrinho = await _dbContext.Carrinho.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == carrinhoId);
        if(carrinho is null) return NotFound("Carrinho não encontrado");
        
        var produto = await _dbContext.Produto.FindAsync(produtoId);
        if (produto == null) return NotFound("Produto não encontrado");

        if (carrinho.Items is null)
        {
            carrinho.Items = new List<ItemCarrinho>();
        }

        var itemExistente = carrinho.Items.FirstOrDefault(item => item.Produto != null && item.Produto.Id == produtoId);
        if (itemExistente is null){
            var itemCarrinho = new ItemCarrinho {Produto = produto, Quantidade = 1};
            carrinho.Items.Add(itemCarrinho);
        } else {
            itemExistente.Quantidade++;
        }

        await _dbContext.SaveChangesAsync();
        return Ok("Produto Adicionado ao carrinho com sucesso");
    }

    [HttpGet]
    [Route("{carrinhoId}/Produtos")]
    public async Task<ActionResult<IEnumerable<ItemCarrinho>>> ListarProdutosCarrinho(int carrinhoId)
    {
       if (_dbContext == null) return NotFound("DbContext não inicializado.");
    
    var carrinho = await _dbContext.Carrinho.Include(c => c.Items).ThenInclude(i => i.Produto).FirstOrDefaultAsync(c => c.Id == carrinhoId);
    if (carrinho == null) return NotFound("Carrinho não encontrado.");

    return Ok(carrinho.Items);
    }

    [HttpDelete]
    [Route("{carrinhoId}/RemoverItem/{produtoId}")]
    public async Task<ActionResult<Carrinho>> RemoverItem(int carrinhoId, int produtoId)
    {
      if (_dbContext == null) return NotFound("DbContext não inicializado.");

        var carrinho = await _dbContext.Carrinho
            .Include(c => c.Items)
            .ThenInclude(item => item.Produto)
            .FirstOrDefaultAsync(c => c.Id == carrinhoId);
        if (carrinho == null) return NotFound("Carrinho não encontrado.");

        var item = carrinho.Items.FirstOrDefault(i => i.Produto.Id == produtoId);
        if (item == null) return NotFound("Item não encontrado no carrinho.");

        if (item.Quantidade > 1)
        {
            item.Quantidade--;
        }
        else
        {
            carrinho.Items.Remove(item);
        }

        await _dbContext.SaveChangesAsync();

        return Ok(carrinho);
    }


}