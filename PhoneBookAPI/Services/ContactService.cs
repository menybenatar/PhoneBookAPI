using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PhoneBookAPI.Data;
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public ContactService(AppDbContext context, IMapper mapper, IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<int> AddContact(ContactModel contactModel)
        {
            try
            {
                var contactEntity = _mapper.Map<ContactEntity>(contactModel);
                _context.Contacts.Add(contactEntity);
                await _context.SaveChangesAsync();
                return contactEntity.Id;
            }
            catch (Exception ex)
            {
                string errorStr = $"An error occurred while adding a contact.";
                throw new Exception(errorStr, ex);
            }
        }

        public async Task DeleteContact(int id)
        {
            try
            {
                var contactEntity = await _context.Contacts.FindAsync(id); ;
                if (contactEntity == null)
                {
                    throw new KeyNotFoundException($"Contact with ID {id} not found.");
                }

                _context.Contacts.Remove(contactEntity);
                await _context.SaveChangesAsync();

                // Remove from the cache after deletion
                await RemoveContactFromCache(id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string errorStr = $"An error occurred while deleting contact with ID {id}.";
                throw new Exception(errorStr, ex);
            }
        }

        public async Task<IEnumerable<ContactModel>> GetContacts(int pageNumber, int pageSize)
        {
            try
            {
                var contacts = await _context.Contacts
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ContactModel>>(contacts);
            }
            catch (Exception ex)
            {
                string errorStr = "An error occurred while retrieving contacts.";
                throw new Exception(errorStr, ex);
            }
        }

        public async Task<IEnumerable<ContactModel>> SearchContacts(string query)
        {
            try
            {
                var contacts = await _context.Contacts
                    .Where(c => c.FirstName.Contains(query) ||
                                c.LastName.Contains(query) ||
                                c.PhoneNumber.Contains(query) ||
                                c.Address.Contains(query))
                    .ToListAsync();

                if (!contacts.Any())
                {
                    throw new KeyNotFoundException($"No contacts found for the query: {query}");
                }

                return _mapper.Map<IEnumerable<ContactModel>>(contacts);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string errorStr = $"An error occurred while searching for contacts with query: {query}";
                throw new Exception(errorStr, ex);
            }
        }

        public async Task UpdateContact(ContactModel contact)
        {
            try
            {
                var existingContact = await _context.Contacts.FindAsync(contact.Id);
                if (existingContact == null)
                {
                    throw new KeyNotFoundException($"Contact with ID {contact.Id} not found.");
                }

                _mapper.Map(contact, existingContact);
                await _context.SaveChangesAsync();  

                // After the update is successful, update the cache
                await SetContactToCache(existingContact);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string errorStr = $"An error occurred while updating contact with ID {contact.Id}.";
                throw new Exception(errorStr, ex);
            }
        }

        public async Task<ContactModel> GetContactById(int id)
        {
            try
            {
                // Try to get the contact from the cache or DB
                var contactEntity = await GetContactFromCacheOrDb(id);
                if (contactEntity == null)
                {
                    throw new KeyNotFoundException($"Contact with ID {id} not found.");
                }

                // Map the entity to the model
                var contactModel = _mapper.Map<ContactModel>(contactEntity);
                return contactModel;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                string errorStr = $"An error occurred while retrieving the contact with ID {id}.";
                throw new Exception(errorStr, ex);
            }
        }
        private async Task<ContactEntity?> GetContactFromCacheOrDb(int contactId)
        {
            var cacheKey = GetCacheKey(contactId);

            // Try to get contact from cache
            var cachedContact = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedContact))
            {
                return JsonConvert.DeserializeObject<ContactEntity>(cachedContact);
            }

            // If not in cache, get from the database
            var contactFromDb = await _context.Contacts.FindAsync(contactId);
            if (contactFromDb != null)
            {
                // Store in cache for future requests
                await SetContactToCache(contactFromDb);
            }

            return contactFromDb;
        }
        private async Task SetContactToCache(ContactEntity contact)
        {
            var cacheKey = GetCacheKey(contact.Id);
            var contactJson = JsonConvert.SerializeObject(contact);
            await _cache.SetStringAsync(cacheKey, contactJson);
        }

        private async Task RemoveContactFromCache(int contactId)
        {
            var cacheKey = GetCacheKey(contactId);
            await _cache.RemoveAsync(cacheKey);
        }

        private string GetCacheKey(int contactId)
        {
            return $"Contact_{contactId}";
        }
    }
}
