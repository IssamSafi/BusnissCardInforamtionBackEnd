using BusnissCardInforamtion.Model;
using BusnissCardInforamtion.Repository;
using BusnissCardInforamtion.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BusnissCardInforamtion.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BusinessCardController : ControllerBase
    {
        private readonly IBusinessCardServices _services;
        private readonly IBusinessCardRepository _businessCardRepository;
        public BusinessCardController(IBusinessCardRepository businessCardRepository,IBusinessCardServices businessCardServices)
        {
            _businessCardRepository = businessCardRepository;
            _services=businessCardServices;
        }



        [HttpGet]
        public IActionResult GetBusinessCards()
        {
            var businessCards = _businessCardRepository.GetAll();
            return Ok(businessCards);
        }



        [HttpGet("{id}")]
        public IActionResult GetBusinessCardsByid(int id)
        {
          var result=  _businessCardRepository.GetById(id);
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBusinessCard(int id)
        {
            _businessCardRepository.Delete(id);
            return NoContent();
        }


        [HttpPost]
        public IActionResult CreateBusinessCard([FromBody] BusinessCard businessCard)
        {
            if (businessCard == null)
            {
                return BadRequest("Invalid business card data.");
            }

            var createdCard = _services.Create(businessCard);
            return CreatedAtAction(nameof(GetBusinessCardsByid), new { id = createdCard.Id }, createdCard);
        }


        [HttpGet("filter")]
        public IActionResult FilterBusinessCards(
        string name=null,
        DateTime? dateOfBirth=null,
        string phone=null,
        string gender=null,
        string email=null)
       
        {
            var filteredCards = _businessCardRepository.Filter(name, dateOfBirth, phone, gender, email);
            return Ok(filteredCards);
        }


        [HttpPut]
        public IActionResult UpdateBusinessCard(int id, [FromBody] BusinessCard businessCard)
        {
            _businessCardRepository.Update(id, businessCard);
            return NoContent();
        }
        


        [HttpPost("import/csv")]
        public IActionResult ImportFromCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            _services.ImportFromCsv(file);
            return NoContent();
        }

        [HttpPost("import/xml")]
        public IActionResult ImportFromXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            _services.ImportFromXml(file);
            return NoContent();
        }


        [HttpGet("export/xml")]
        public IActionResult ExportToXml()
        {
            var businessCards = _businessCardRepository.GetAll();
            var xmlResult = _services.ExportToXml(businessCards);
            return File(Encoding.UTF8.GetBytes(xmlResult), "application/xml", "BusinessCards.xml");
        }


        [HttpGet("export/csv")]
        public IActionResult ExportToCsv()
        {
            var businessCards = _businessCardRepository.GetAll();
            var csvResult = _services.ExportToCsv(businessCards);
            return File(Encoding.UTF8.GetBytes(csvResult), "text/csv", "BusinessCards.csv");
        }





    }
}
