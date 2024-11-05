﻿using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using APICatalogo.Repositories;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IRepository<Produto> _repository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IRepository<Produto> repository,
    IProdutoRepository produtoRepository,
    ILogger<ProdutosController> logger)
    {
        _produtoRepository = produtoRepository;
        _repository = repository;
        _logger = logger;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult <IEnumerable<Produto>> GetProdutosPorCategoria(int id)
    {
        var produtos =  _produtoRepository.GetProdutosPorCategoria(id);
        if(produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }

   [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetAll();
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);
        if(produto is null)
        {
            _logger.LogWarning($"Produto com id= {id} não encontrado...");
            return NotFound($"Produto com id= {id} não encontrado...");
        }
        return produto;
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var produtoCriado =  _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = produtoCriado.ProdutoId }, produtoCriado);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var produtoAtualizado = _repository.Update(produto);

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Produto com id={id} não encontrado...");
            return NotFound($"Produto com id={id} não encontrado...");
        }
        
        var produtoExcluido = _repository.Delete(produto);

        return Ok(produtoExcluido);
    }
}