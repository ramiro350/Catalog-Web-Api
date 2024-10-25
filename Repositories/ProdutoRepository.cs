using APICatalogo.Models;
﻿using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class ProdutoRepository : IProdutoRepository
{
  private readonly AppDbContext _context;

  public ProdutoRepository(AppDbContext context)
  {
    _context = context;
  }

  public IQueryable<Produto> GetProdutos()
  {
    var produtos = _context.Produtos;
    return produtos;
  }

  public Produto GetProduto(int id)
  {
    var produto =  _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
    return produto;
  }

  public Produto CreateProduto(Produto produto)
  {
    if(produto is null)
    {
      throw new ArgumentNullException(nameof(produto));
    }

    _context.Produtos.Add(produto);
    _context.SaveChanges();

    return produto;
  }

  public bool UpdateProduto(Produto produto)
  {
    if(produto is null)
    {
      throw new ArgumentNullException(nameof(produto));
    }

    if(_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
    {
      _context.Produtos.Update(produto);
      _context.SaveChanges();
      return true;
    }

    return false;
  }

  public bool DeleteProduto(int id)
  {
    var produto = _context.Produtos.Find(id);

    if(produto is not null)
    {
      _context.Produtos.Remove(produto);
      _context.SaveChanges();
      return true;
    }

    return false;
  }
}