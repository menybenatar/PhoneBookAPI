using PhoneBookAPI.Models;

namespace PhoneBookAPI.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactModel>> GetContacts(int pageNumber, int pageSize);
        Task<IEnumerable<ContactModel>> SearchContacts(string query);
        Task AddContact(ContactModel contact);
        Task UpdateContact(int id, ContactModel contact);
        Task DeleteContact(int id);
    }
}
