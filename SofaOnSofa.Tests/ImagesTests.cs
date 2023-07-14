using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using SofaOnSofa.Web.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace SofaOnSofa.Tests
{
    [TestClass]
    public class ImagesTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Arrange - создание продукта с данными изображения
            Product prod = new Product
            {
                ProductId = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            // Arrange - создание репозитория
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                prod,
                new Product {ProductId = 3, Name = "P3"}
            }.AsQueryable());
            // Arrange - создание контроллера
            ProductController target = new ProductController(mock.Object);

            // Act - вызовите метод действия GetImage
            ActionResult result = target.GetImage(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Arrange - создание репозитория
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"}
            }.AsQueryable());
            // Arrange - создание контроллера
            ProductController target = new ProductController(mock.Object);

            // Act - вызовите метод действия GetImage
            ActionResult result = target.GetImage(100);

            // Assert
            Assert.IsNull(result);
        }
    }
}
