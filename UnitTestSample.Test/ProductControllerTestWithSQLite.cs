using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestSample.Web.Controllers;
using UnitTestSample.Web.Models;
using Xunit;


namespace UnitTestSample.Test
{
    public class ProductControllerTestWithSQLite : ProductControllerTest
    {
        public ProductControllerTestWithSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            SetContextOptions(new DbContextOptionsBuilder<UnitTestDbContext>().UseSqlite(connection).Options);
        }

        [Fact]
        public async Task Create_ModelValidProduct_ReturnsRedirectToActionWithSaveProduct()
        {
            var newProduct = new Product { Name = "Kalem 30", Price = 200, Stock = 100, Color = "Mavi" };
            using (var context = new UnitTestDbContext(_contextOptions))
            {
                var category = context.Category.First();
                newProduct.CategoryId = category.Id;

                var controller = new ProductsController(context);
                var result = await controller.Create(newProduct);

                var redirect = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirect.ActionName);
            }

            using (var context = new UnitTestDbContext(_contextOptions))
            {
                var product = context.Products.FirstOrDefault(q => q.Name == newProduct.Name);
                Assert.Equal(newProduct.Name, product.Name);
            }
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteCategory_ExistCategoryId_DeletedAllProducts(int categoryId)
        {
            using (var context = new UnitTestDbContext(_contextOptions))
            {
                var category = await context.Category.FindAsync(categoryId);
                Assert.NotNull(category);
                context.Category.Remove(category);
                context.SaveChanges();
            }

            using (var context = new UnitTestDbContext(_contextOptions))
            {
                var products = await context.Products.Where(q => q.CategoryId == categoryId).ToListAsync();
                Assert.Empty(products);
            }
        }
    }
}
