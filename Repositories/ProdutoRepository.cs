using APICatalogo.Models;
﻿using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Pagination;
using X.PagedList;

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

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
    {
      var produtos = await GetAllAsync();

      var produtosOrdernados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

      var resultado = await produtosOrdernados.ToPagedListAsync(produtosParams.PageNumber, produtosParams._pageSize);
      // var produtosOrdernados =  PagedList<Produto>.ToPagedList(produtos.OrderBy(p => p.ProdutoId).AsQueryable(),
      // produtosParams.PageNumber, produtosParams._pageSize);
      return resultado;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
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

      var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParams.PageNumber,
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