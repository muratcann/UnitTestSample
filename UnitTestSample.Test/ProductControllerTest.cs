﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestSample.Web.Models;

namespace UnitTestSample.Test
{
    public class ProductControllerTest
    {
        protected DbContextOptions<UnitTestDbContext> _contextOptions { get; private set; }
        public void SetContextOptions(DbContextOptions<UnitTestDbContext> contextOptions)
        {
            _contextOptions = contextOptions;
            Seed();
        }

        public void Seed()
        {
            using (UnitTestDbContext context = new UnitTestDbContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Category.Add(new Category { Name = "Kalemler" });
                context.Category.Add(new Category { Name = "Defterler" });
                context.SaveChanges();

                context.Products.Add(new Product { CategoryId = 1, Name = "kalem 10", Price = 100, Stock = 100, Color = "Kırmızı" });
                context.Products.Add(new Product { CategoryId = 1, Name = "kalem 20", Price = 100, Stock = 100, Color = "Mavi" });
                context.SaveChanges();
            }
        }
    }
}
