using APICatalogo.Models;
﻿using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{

  public ProdutoRepository(AppDbContext context) : base(context)
  {
  }

  public IEnumerable<Produto> GetProdutosPorCategoria(int id)
  {
      return GetAll().Where(c => c.CategoriaId == id);
  }
}