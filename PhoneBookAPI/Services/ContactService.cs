using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Data;
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddContact(ContactModel contactModel)
        {
            var contactEntity = _mapper.Map<ContactEntity>(contactModel);
            _context.Contacts.Add(contactEntity);
            await _context.SaveChangesAsync();
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
            throw new NotImplementedException();
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
