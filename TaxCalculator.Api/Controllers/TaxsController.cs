using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculator.DataAccess.Repositories.Interfaces;
using TaxCalculator.Domain;
using TaxCalculator.Domain.DTO;
using TaxCalculator.Service;

namespace TaxCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICalculatorService _calculatorService;

        public TaxsController(IUnitOfWork unitOfWork, ICalculatorService calculatorService)
        {
            _unitOfWork = unitOfWork;
            _calculatorService = calculatorService;
        }

        [HttpGet]
        public IEnumerable<Tax> Get()
        {
            return _unitOfWork.TaxRepository.GetAll().ToArray();
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult<Tax> GetTaxById(int id) 
        {
            var tax = _unitOfWork.TaxRepository.GetAll(w => w.TaxId == id).FirstOrDefault();
            if (tax == null)
            {
                return NoContent();
            }
            return tax;
        }
       
        [HttpPost]
        public async Task<ActionResult<Tax>> PostTax(TaxDTO tax)
        {
            var calculateTax = _calculatorService.CalculateTax(tax.PostalCode, tax.Salary);
            _unitOfWork.TaxRepository.Add(calculateTax);
            _unitOfWork.Save();
            return CreatedAtAction(nameof(Get), new { id = calculateTax.TaxId }, tax);
        }

        [HttpDelete]
        public async Task<ActionResult<Tax>> DeleteTax(int id)
        {
            var tax =  _unitOfWork.TaxRepository.GetAll(w => w.TaxId == id).FirstOrDefault();
            if (tax == null)
            {
                return NotFound();
            }

            _unitOfWork.TaxRepository.Delete(tax);
            _unitOfWork.Save();           

            return NoContent();
        }
    }
}
