using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace APICatalogo.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ProdutosController : ControllerBase
  {
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
      _context = context;
    }

    // [HttpGet]
    // public ActionResult<IEnumerable<Produto>> Get()
    // {
    //   var produtos = _context.Produtos.ToList();
    //   if(produtos is null)
    //   {
    //     return NotFound("Produtos não encontrados.");
    //   }
    //   return produtos;
    // }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
      return await _context.Produtos.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id:int}", Name="ObterProduto")]
    public async Task<ActionResult<Produto>> Get(int id,[BindRequired] string nome)
    {
      var nomeProduto = nome;

      var produto = await _context.Produtos.AsNoTracking().
      FirstOrDefaultAsync(p => p.ProdutoId == id);
      if (produto is null)
      {
        return NotFound("Produto não encontrado.");
      }
      return produto;
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
      if(produto is null)
      {
        return BadRequest();
      }

      _context.Produtos.Add(produto);
      _context.SaveChanges();
      
      return new CreatedAtRouteResult("ObterProduto",
        new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
      if(id != produto.ProdutoId)
      {
        return BadRequest();
      }

      _context.Entry(produto).State = EntityState.Modified;
      _context.SaveChanges();

      return Ok(produto); 
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
      var produto = _context.Produtos.FirstOrDefault(p=> p.ProdutoId == id);
      if(produto is null)
      {
        return NotFound("Produto não localizado.");
      }
      _context.Produtos.Remove(produto);
      _context.SaveChanges();

      return Ok(produto);
    }
  }
}