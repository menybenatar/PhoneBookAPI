using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Data;
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public Task AddContact(ContactModel contact)
        {
            throw new NotImplementedException();
        }

        public Task DeleteContact(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ContactModel> GetContactById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ContactModel>> GetContacts(int pageNumber, int pageSize)
        {
            return await _context.Contacts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<IEnumerable<ContactModel>> SearchContacts(string query)
        {
            throw new NotImplementedException();
        }

        public Task UpdateContact(int id, ContactModel contact)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<ContactModel>> IContactService.GetContacts(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
