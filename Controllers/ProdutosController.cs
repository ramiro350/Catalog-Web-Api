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
    private readonly IUnitOfWork _uof;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IUnitOfWork uof,
    ILogger<ProdutosController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult <IEnumerable<Produto>> GetProdutosPorCategoria(int id)
    {
        var produtos =  _uof.ProdutoRepository.GetProdutosPorCategoria(id);
        if(produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }

   [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _uof.ProdutoRepository.GetAll();
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);
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

        var produtoCriado =  _uof.ProdutoRepository.Create(produto);
        _uof.Commit();

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

        var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            _logger.LogWarning($"Produto com id={id} não encontrado...");
            return NotFound($"Produto com id={id} não encontrado...");
        }
        
        var produtoExcluido = _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();

        return Ok(produtoExcluido);
    }
}