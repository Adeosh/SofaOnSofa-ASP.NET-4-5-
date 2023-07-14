using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using SofaOnSofa.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SofaOnSofa.Tests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products()
        {
            // Arrange - создание репозитория
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
            }.AsQueryable());
            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);

            // Action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Arrange - создание репозитория
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
            }.AsQueryable());
            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);

            // Act
            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, p1.ProductId);
            Assert.AreEqual(2, p2.ProductId);
            Assert.AreEqual(3, p3.ProductId);
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - создание репозитория
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);
            // Arrange - создание продукта
            Product product = new Product { Name = "Test" };
            // Arrange - добавление ошибки в состояние модели
            target.ModelState.AddModelError("error", "error");

            // Act - попытка сохранения
            ActionResult result = target.Edit(product, null);

            // Assert - убедитесь, что репозиторий не был вызван
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // Assert - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            // Arrange - создание продукта
            Product prod = new Product { ProductId = 2, Name = "Test" };
            // Arrange - создание репозитория
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                prod,
                new Product {ProductId = 3, Name = "P3"},
            }.AsQueryable());
            // Arrange - создание контроллера
            AdminController target = new AdminController(mock.Object);

            // Act - удаление
            target.Delete(prod.ProductId);

            // Assert - убедитесь, что метод удаления репозитория был вызван с правильным продуктом
            mock.Verify(m => m.DeleteProduct(prod.ProductId));
        }
    }
}
