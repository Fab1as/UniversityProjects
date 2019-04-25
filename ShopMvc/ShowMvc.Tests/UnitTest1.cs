using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShopMvc.Controllers;
using ShopMvc.Data.Interfaces;
using ShopMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void GetAllCategories_Returns_RightCountOfCategories()
        {
            var mock = new Mock<ICategoryRepository>();
            mock.Setup(a => a.GetAll()).Returns(GetAll());

            var controller = new CategoriesController(mock.Object);

            // Act
            var result =  controller.Index() as ViewResult ;

            var model = result.Model as List<Category>;
            // Assert
            Assert.AreEqual(2, model.Count());
        }

        private List<Category> GetAll()
        {
            var categories = new List<Category>
            {
                new Category()
                {
                    Name = "category1",

                },
                new Category()
                {
                    Name = "category2",
                }
            };
            return categories;
        }
    }
}