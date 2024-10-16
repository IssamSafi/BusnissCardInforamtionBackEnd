using Azure.Messaging;
using BusnissCardInforamtion.Model;
using BusnissCardInforamtion.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BusnissCardInforamtion.Repository
{
    public class BusinessCardRepository : IBusinessCardRepository
    {
        private readonly BusnissCarddbContext context;
        public BusinessCardRepository(BusnissCarddbContext context)
        {
            this.context = context;
        }

        public BusinessCard Create(BusinessCard businessCard)
        {
           context.busnissCards.Add(businessCard);
            context.SaveChanges();
            return businessCard;
        }

        public void Delete(int id)
        {
             
            context.busnissCards.Remove(context.busnissCards.Find(id));
            context.SaveChanges();
            
        }

        public IEnumerable<BusinessCard> Filter(string name, DateTime? dateOfBirth, string phone, string gender, string email)
        {
            //to chaek if null or empty return empty 
            if (string.IsNullOrWhiteSpace(name) &&
                  !dateOfBirth.HasValue &&
                   string.IsNullOrWhiteSpace(phone) &&
                    string.IsNullOrWhiteSpace(gender) &&
                   string.IsNullOrWhiteSpace(email))
            {
                return Enumerable.Empty<BusinessCard>(); 
            }
            var query = context.busnissCards.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(b => b.Name.Contains(name));
            }
            if (dateOfBirth.HasValue)
            {
                query = query.Where(b => b.DateOfBirth == dateOfBirth.Value);
            }
            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(b => b.Phone.Contains(phone));
            }
            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(b => b.Gender == gender);
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(b => b.Email.Contains(email));
            }
            

            return  query.ToList();
        }

        public IEnumerable<BusinessCard> GetAll()
        {
            return context.busnissCards.ToList();
        }

        public BusinessCard GetById(int id)
        {
            return context.busnissCards.Find(id);
        }

        public void Update(int id, BusinessCard businessCard)
        {

            var up = context.busnissCards.Find(id);
            up.Name = businessCard.Name;
            up.Gender = businessCard.Gender;
            up.DateOfBirth = businessCard.DateOfBirth;
            up.Email = businessCard.Email;
            up.Phone = businessCard.Phone;
            up.Address = businessCard.Address;
            up.PhotoBase64 = businessCard.PhotoBase64;
            context.Entry(up).State = EntityState.Modified;
            context.SaveChanges();



            
        }
    }
}
