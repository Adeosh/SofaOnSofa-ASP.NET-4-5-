using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using SofaOnSofa.Web.Controllers;
using SofaOnSofa.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SofaOnSofa.Web.HtmlHelpers;
using System.Web.Mvc;

namespace SofaOnSofa.Tests
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            //Act
            ProductList result = (ProductList)controller.List(null, 2).Model;

            //Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange - определите HTML-помощника
            HtmlHelper myHelper = null;

            //Arrange - создание PagingInfo базы
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>" +
                @"<a class=""selected"" href=""Page2"">2</a>" +
                @"<a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange - проверка о предоставление правильной информации
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Act
            ProductList result = (ProductList)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1", Style = "Sty1"},
                new Product {ProductId = 2, Name = "P2", Style = "Sty2"},
                new Product {ProductId = 3, Name = "P3", Style = "Sty1"},
                new Product {ProductId = 4, Name = "P4", Style = "Sty2"},
                new Product {ProductId = 5, Name = "P5", Style = "Sty3"}
            }.AsQueryable());

            // Arrange - создание контроллера и измените размер страницы на 3 элемента
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Action
            Product[] result = ((ProductList)controller.List("Sty2", 1).Model)
            .Products.ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Style == "Sty2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Style == "Sty2");
        }

        [TestMethod]
        public void Can_Create_Styles()
        {
            // Arrange
            // - создание репозитория
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1", Style = "Loft"},
                new Product {ProductId = 2, Name = "P2", Style = "Loft"},
                new Product {ProductId = 3, Name = "P3", Style = "Classic"},
                new Product {ProductId = 4, Name = "P4", Style = "Modern"},
            }.AsQueryable());
            // Arrange - создание контроллера
            NavController target = new NavController(mock.Object);

            // Act = получение набора стилей
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Classic");
            Assert.AreEqual(results[1], "Loft");
            Assert.AreEqual(results[2], "Modern");
        }

        [TestMethod]
        public void Generate_Style_Specific_Product_Count()
        {
            //Arrange 
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductId = 1, Name = "P1", Style = "Sty1"},
                new Product {ProductId = 2, Name = "P2", Style = "Sty2"},
                new Product {ProductId = 3, Name = "P3", Style = "Sty1"},
                new Product {ProductId = 4, Name = "P4", Style = "Sty2"},
                new Product {ProductId = 5, Name = "P5", Style = "Sty3"}
            }.AsQueryable());

            //Arrange - создание контроллера и разбитие размера страницы на 3 элемента
            ProductController target = new ProductController(mock.Object);
            target.pageSize = 3;

            //Action - поиск кол-ва продуктов, специфичных для конекретного стиля
            int res1 = ((ProductList)target.List("Sty1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductList)target.List("Sty2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductList)target.List("Sty3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductList)target.List(null).Model).PagingInfo.TotalItems;

            //Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
