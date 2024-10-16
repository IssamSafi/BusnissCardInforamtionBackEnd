using BusnissCardInforamtion.Model;

namespace BusnissCardInforamtion.Service
{
    public interface IBusinessCardServices
    {
        public IEnumerable<BusinessCard> GetAll();
        public BusinessCard GetById(int id);
        public BusinessCard Create(BusinessCard businessCard);
        public void Delete(int id);
        public void Update(int id,BusinessCard businessCard);
        public IEnumerable<BusinessCard> Filter(string name, DateTime? dateOfBirth, string phone, string gender, string email);
        public void ImportFromCsv(IFormFile file);
        public void ImportFromXml(IFormFile file);

        public string ExportToXml(IEnumerable<BusinessCard> businessCards);
        public string ExportToCsv(IEnumerable<BusinessCard> businessCards);

    }
}
