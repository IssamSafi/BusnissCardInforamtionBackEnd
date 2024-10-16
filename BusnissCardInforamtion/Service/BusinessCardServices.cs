using BusnissCardInforamtion.Model;
using BusnissCardInforamtion.Repository;
using CsvHelper;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

using System.IO;
using Microsoft.AspNetCore.Http;
using CsvHelper.Configuration;



namespace BusnissCardInforamtion.Service
{
    public class BusinessCardServices : IBusinessCardServices
    {
        private static List<BusinessCard> _businessCards = new List<BusinessCard>();

        private readonly BusinessCardRepository repository;
       
        public BusinessCardServices (BusinessCardRepository _repository)
        {
            this.repository = _repository;
        }
        


        public IEnumerable<BusinessCard> GetAll() => repository.GetAll();
        public BusinessCard GetById(int id) => repository.GetById(id);
        public BusinessCard Create(BusinessCard businessCard) => repository.Create(businessCard);
        public void Delete(int id) => repository.Delete(id);
        public void Update(int id, BusinessCard businessCard) => repository.Update(id, businessCard);
        public IEnumerable<BusinessCard> Filter(string name, DateTime? dateOfBirth, string phone, string gender, string email) => repository.Filter(name, dateOfBirth, phone, gender, email);



        
        public void ImportFromCsv(IFormFile file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, 
                MissingFieldFound = null 
            };

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<BusinessCard>().ToList();
                foreach (var card in records)
                {
                    card.Id = 0; 
                    Create(card);
                }
            }
        }


        public void ImportFromXml(IFormFile file)
        {
            var cards = new List<BusinessCard>();

            using (var stream = file.OpenReadStream())
            {
                var serializer = new XmlSerializer(typeof(List<BusinessCard>), new XmlRootAttribute("BusinessCards"));
                cards = (List<BusinessCard>)serializer.Deserialize(stream);
            }

            foreach (var card in cards)
            {
                card.Id = 0; 
                Create(card);
            }
        }







        public string ExportToXml(IEnumerable<BusinessCard> businessCards)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<BusinessCard>), new XmlRootAttribute("BusinessCards"));
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, businessCards.ToList());
                return stringWriter.ToString();
            }
        }

        public string ExportToCsv(IEnumerable<BusinessCard> businessCards)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Name,Gender,DateOfBirth,Email,Phone,Address,PhotoBase64");

            foreach (var card in businessCards)
            {
                csvBuilder.AppendLine($"{card.Id},{card.Name},{card.Gender},{card.DateOfBirth:yyyy-MM-dd},{card.Email},{card.Phone},{card.Address},{card.PhotoBase64}");
            }

            return csvBuilder.ToString();
        }
    }
   
}
