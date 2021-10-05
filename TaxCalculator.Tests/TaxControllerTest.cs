using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Controllers;
using TaxCalculator.DataAccess;
using TaxCalculator.DataAccess.Repositories;
using TaxCalculator.DataAccess.Repositories.Interfaces;
using TaxCalculator.Domain;
using TaxCalculator.Domain.DTO;
using TaxCalculator.Service;
using TaxCalculator.Tests.FakeRepository;

namespace TaxCalculator.Tests
{
    public class TaxControllerTest
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICalculatorService> _calculatorService;
        private TaxsController _controller;
        private Mock<IBaseRepository<Tax>> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _calculatorService = new Mock<ICalculatorService>();
            
            var data = new List<Tax>();
            var item = new Tax { PostalCode = "A100", Salary = 99999, TaxId = 1 };
            data.Add(item);
            _mockRepository = new Mock<IBaseRepository<Tax>>();
            _mockRepository.SetupGet(u => u.GetAll(w => w.TaxId >= 1)).Returns(data);
            _controller = new TaxsController(_unitOfWork.Object, _calculatorService.Object);

        }

        [Test]
        public void CreatePost_ValidValuesSubmitted_ShouldCallComplete()
        {
            var dto = new TaxDTO()
            {
                PostalCode = "A100",
                Salary =  99999m
            };

            _controller.PostTax(dto);

            var result = _mockRepository.Object.GetAll().Count();
        }

        [Test]
        public void TestDetailsView()
        {
            var response = _controller.Get().ToList();
            Assert.AreEqual(1, response.Count());

        }
    }
}
