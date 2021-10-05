using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TaxCalculator.DataAccess;
using TaxCalculator.DataAccess.Repositories;
using TaxCalculator.DataAccess.Repositories.Interfaces;
using TaxCalculator.Domain;
using TaxCalculator.Service;

namespace TaxCalculator.Tests
{
    public class Tests
    {
        private ServiceCollection services;
        private ServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            services = new ServiceCollection();
            services.AddDbContext<EntitiesContext>(options =>
                    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaxCalculator;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICalculatorService, CalculatorService>();
            serviceProvider = services.BuildServiceProvider();
        }

        private Tax CalculateTax(decimal salary, string postalCode)
        {
            var customerService = serviceProvider.GetService<ICalculatorService>();
            return customerService.CalculateTax(postalCode, salary);
        }

        [Test]
        public void WhenTaxPostaCodeDontHaveConfigurationThenShouldBeThrowsException()
        {
            var salary = 0;
            var postalCode = "aaaa";

            Assert.Throws<ArgumentException>(() => CalculateTax(salary, postalCode));            
        }

        [Test]
        public void WhenTaxPostaCodeIsA100ThenTaxTypeIsFlatValueAndSalaryLess200000ShouldBeResultIs5Percent()
        {
            var salary = 199999;
            var postalCode = "A100";

            var result = CalculateTax(salary, postalCode);

            Assert.IsTrue(result.TaxValue == salary * 0.05m);
        }

        [Test]
        public void WhenTaxPostaCodeIsA100ThenTaxTypeIsFlatValueAndSalaryMost200000ShouldBeResultTaxFixedValue10000()
        {
            var salary = 300000;
            var postalCode = "A100";

            var result = CalculateTax(salary, postalCode);
            var taxValue = 10000m;

            Assert.IsTrue(result.TaxValue == taxValue);
        }

        [Test]
        public void WhenTaxPostaCodeIs7000ThenTypeIsFlatRateShouldBeResultTaxIs17_5Percent()
        {
            var salary = 2000;
            var postalCode = "7000";

            var result = CalculateTax(salary, postalCode);

            Assert.IsTrue(result.TaxValue == salary *  0.175m);
        }

        [Test]
        public void WhenPostalCodeIs7441AndSalaryIsLess8350ShouldBeTypeIsProgressiveAndTaxIs10Percent()
        {
            var salary = 8000;
            var postalCode = "7441";

            var result = CalculateTax(salary, postalCode);

            Assert.IsTrue(result.TaxValue == salary * 0.10m);
        }

        [Test]
        public void WhenPostalCodeIs7441AndSalaryIsLess33950ShouldBeTypeIsProgressiveAndSecondRate()
        {
            var salary = 10000m;
            var postalCode = "7441";
            var firstRate = 835m; 
            var secondRate = 247.35m; //
            var resultExpected = firstRate + secondRate;

            var result = CalculateTax(salary, postalCode);

            Assert.IsTrue(result.TaxValue == resultExpected);
        }

        //More tests next rates
    }
}