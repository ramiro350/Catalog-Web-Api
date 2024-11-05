using APICatalogo.Models;
﻿using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class CategoriaRepository :Repository<Categoria>, ICategoriaRepository
{
  public CategoriaRepository(AppDbContext context) : base(context)
  {
  }
}