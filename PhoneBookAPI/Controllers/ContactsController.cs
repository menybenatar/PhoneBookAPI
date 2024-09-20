using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Models;
using PhoneBookAPI.Services;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: api/contacts?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetContacts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var contacts = await _contactService.GetContacts(pageNumber, pageSize);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and return an appropriate error response
                return StatusCode(500, new { message = "An error occurred while retrieving contacts.", error = ex.Message });
            }
        }

        // GET: api/contacts/search?query=John
        [HttpGet("search")]
        public async Task<IActionResult> SearchContacts([FromQuery] string query)
        {
            try
            {
                var contacts = await _contactService.SearchContacts(query);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while searching contacts.", error = ex.Message });
            }
        }

        // POST: api/contacts
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactModel contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _contactService.AddContact(contact);
                return CreatedAtAction(nameof(AddContact), new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the contact.", error = ex.Message });
            }
        }

        // PUT: api/contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditContact(int id, [FromBody] ContactModel contact)
        {
            try
            {
                if (id != contact.Id)
                {
                    return BadRequest(new { message = "Contact ID mismatch" });
                }

                await _contactService.UpdateContact(id, contact);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the contact.", error = ex.Message });
            }
        }

        // DELETE: api/contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                await _contactService.DeleteContact(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the contact.", error = ex.Message });
            }
        }
    }
}
