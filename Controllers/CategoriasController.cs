using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Services;
using APICatalogo.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APICatalogo.Controllers
{
  [Route("[controller]")]
  [ApiController]

  public class CategoriasController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly ILogger _logger;



    public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
    {
      _context = context;
      _logger = logger;
    }

    [HttpGet("usandoFromServices/{nome}")]
    public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuServico,
    string nome){
      return meuServico.Saudacao(nome);
    }

    [HttpGet("semUsarFromServices/{nome}")]
    public ActionResult<string> GetSaudacaoSemFromServices(IMeuServico meuServico,
    string nome){
      return meuServico.Saudacao(nome);
    }

    [HttpGet("Produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
      _logger.LogInformation("==============GET categorias/id ===============");
      return _context.Categorias.Include(p=> p.Produtos).ToList();
    }

    [HttpGet]
    // [ServiceFilter(typeof(APILoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
      var categorias = _context.Categorias.AsNoTracking().ToList();
      try 
      {
        //throw new DataMisalignedException();
        return categorias;
      }
      catch(Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
        "Ocorreu um problema ao tratar a sua solicitação.");
      }
    }

    [HttpGet("{id:int}", Name="ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
      throw new ArgumentException("Ocorreu um erro no tratamento do request.");
      // throw new Exception("Exceção ao retornar o produto pelo id.");
      // var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
      // if (categoria is null)
      // {
      //   return NotFound("Categoria não encontrado.");
      // }
      // return categoria;
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
      if(categoria is null)
      {
        return BadRequest();
      }

      _context.Categorias.Add(categoria);
      _context.SaveChanges();
      
      return new CreatedAtRouteResult("ObterCategoria",
        new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
      if(id != categoria.CategoriaId)
      {
        return BadRequest();
      }

      _context.Entry(categoria).State = EntityState.Modified;
      _context.SaveChanges();

      return Ok(categoria); 
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
      var categoria = _context.Categorias.FirstOrDefault(p=> p.CategoriaId == id);
      if(categoria is null)
      {
        return NotFound("Categoria não localizado.");
      }
      _context.Categorias.Remove(categoria);
      _context.SaveChanges();

      return Ok(categoria);
    }
  }
}