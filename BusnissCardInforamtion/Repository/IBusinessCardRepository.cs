using BusnissCardInforamtion.Model;

namespace BusnissCardInforamtion.Repository
{
    public interface IBusinessCardRepository
    {

        public IEnumerable<BusinessCard> GetAll();
        public BusinessCard GetById(int id);
        public BusinessCard Create(BusinessCard businessCard);
        public void Delete(int id);
        public void Update(int id, BusinessCard businessCard);
        public IEnumerable<BusinessCard> Filter(string name, DateTime? dateOfBirth, string phone, string gender, string email);
    }
}
