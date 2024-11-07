using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Repositories;
using Microsoft.EntityFrameworkCore;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParams)
    {
        var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParams);

        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.hasNext,
            categorias.hasPrevious
        };

        Response.Headers.Append("X-pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDTO = categorias.ToCategoriaDTOList();

        return Ok(categoriasDTO);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
    {
        var categoriasFiltradas = _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltroNome);

        return ObterCategorias(categoriasFiltradas);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.hasNext,
            categorias.hasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoriaDTOList();
        return Ok(categoriasDto);
    }

    [HttpGet]
    public  ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _uof.CategoriaRepository.GetAll();

        if(categorias is null)
        {
            return NotFound("Não existem categorias.");
        }

        var categoriasDTOs = categorias.ToCategoriaDTOList();

        return Ok(categoriasDTOs);  
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDTO = categoria.ToCategoriaDTO();

        return Ok(categoriaDTO);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDTO)
    {
        if (categoriaDTO is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDTO.ToCategoria();

        var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
        _uof.Commit();

        var novaCategoriaDTO = categoriaCriada.ToCategoriaDTO();

        return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDTO.CategoriaId }, novaCategoriaDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDTO)
    {
        if (id != categoriaDTO.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = categoriaDTO.ToCategoria();

        _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

        var CategoriaDTOAtualizada = categoria.ToCategoriaDTO();

        return Ok(CategoriaDTOAtualizada);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit();

        var CategoriaDTOExcluida = categoriaExcluida.ToCategoriaDTO();

        return Ok(CategoriaDTOExcluida);
    }
}