using APICatalogo.Models;
﻿using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{

  public ProdutoRepository(AppDbContext context) : base(context)
  {
  }

  // public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
  // {
  //     return GetAll()
  //     .OrderBy(p=> p.Nome)
  //     .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
  //     .Take(produtosParams.PageSize).ToList();
  // }

    public async Task<PagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
      var produtos = await GetAllAsync();
      var produtosOrdernados =  PagedList<Produto>.ToPagedList(produtos.OrderBy(p => p.ProdutoId).AsQueryable(),
      produtosParams.PageNumber, produtosParams._pageSize);
      return produtosOrdernados;
    }

    public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
    {
      var produtos =  await GetAllAsync();

      if(produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
      {
        if (produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
        {
          produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
        }
        else if (produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
        {
          produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
        }
        else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
        {
          produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
        }
      }

      var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroParams.PageNumber,
                                                                                              produtosFiltroParams.PageSize);
        return produtosFiltrados;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
  {
      var produtos = await GetAllAsync();
      var produtosPorCategoria = produtos.Where(c => c.CategoriaId == id);
      return produtosPorCategoria;
  }
}