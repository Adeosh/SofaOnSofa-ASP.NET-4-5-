using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SofaOnSofa.Domain.Entities;
using SofaOnSofa.Domain.Interfaces;
using SofaOnSofa.Web.Controllers;
using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SofaOnSofa.Tests
{
    [TestClass]
    public class ShoppingCartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange - создание нескольких тестовых продуктов
            Product p1 = new Product { ProductId = 1, Name = "P1" };
            Product p2 = new Product { ProductId = 2, Name = "P2" };
            // Arrange - создание новой корзины
            ShoppingCart target = new ShoppingCart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            SelectedProduct[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Arrange - создание нескольких тестовых продуктов
            Product p1 = new Product { ProductId = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductId = 2, Name = "P2", Price = 50M };
            // Arrange - создание новой корзины
            ShoppingCart target = new ShoppingCart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450M);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - создание макета обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - создание пустой корзины
            ShoppingCart sCart = new ShoppingCart();
            // Arrange - создание сведений о доставке
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - создание экземпляров контроллера
            ShoppingCartController target = new ShoppingCartController(null, mock.Object);

            // Act
            ViewResult result = target.Checkout(sCart, shippingDetails);

            // Assert - убедитесь, что заказ не был передан обработчику
            mock.Verify(m =>
            m.ProcessOrder(It.IsAny<ShoppingCart>(), It.IsAny<ShippingDetails>()), Times.Never());
            // Assert - убедитесь, что метод возвращает представление по умолчанию
            Assert.AreEqual("", result.ViewName);
            // Assert - убедитесь, что мы передаем недопустимую модель в представление
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - создание макета обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - создание корзины с товаром
            ShoppingCart sCart = new ShoppingCart();
            sCart.AddItem(new Product(), 1);
            // Arrange - создание контроллера
            ShoppingCartController target = new ShoppingCartController(null, mock.Object);

            // Act - проба подтверждения заказа
            ViewResult result = target.Checkout(sCart, new ShippingDetails());

            // Assert - убедитесь, что заказ был передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<ShoppingCart>(), It.IsAny<ShippingDetails>()), Times.Once());
            // Assert - убедитесь, что метод возвращает завершенное представление
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - убедитесь, что мы передаем действительную модель в представление
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
