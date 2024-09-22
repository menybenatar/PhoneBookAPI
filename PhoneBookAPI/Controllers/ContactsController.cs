using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        // GET: api/contacts?pageNumber=1&pageSize=10
        /// <summary>
        /// Retrieves a paginated list of contacts.
        /// </summary>
        /// <param name="pageNumber">The current page number to fetch. Defaults to 1 (the first page).</param>
        /// <param name="pageSize">
        /// The number of contacts to return per page. 
        /// This value is limited to a maximum of 10 to prevent too many items from being fetched at once. 
        /// If the client requests more than 10, the page size is capped at 10.
        /// </param>
        /// <returns>A paginated list of contacts with a maximum of 10 per page.</returns>
        [HttpGet]
        public async Task<IActionResult> GetContacts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // Ensure the pageSize is capped at 10
                if (pageSize > 10)
                {
                    pageSize = 10;
                }
                var contacts = await _contactService.GetContacts(pageNumber, pageSize);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "An error occurred while retrieving contacts.", error = ex.Message });
            }
        }

        // GET: api/contacts/SearchContacts?query=John
        [HttpGet("SearchContacts")]
        public async Task<IActionResult> SearchContacts([FromQuery] string query)
        {
            try
            {
                var contacts = await _contactService.SearchContacts(query);
                if (contacts == null || !contacts.Any())
                {
                    _logger.LogWarning("No contacts found matching the query: {Query}", query);
                    return NotFound(new { message = "No contacts found matching the query." });
                }
                return Ok(contacts);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { message = $"No contacts found for the query: {query}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "An error occurred while searching contacts.", error = ex.Message });
            }
        }

        // POST: api/contacts/AddContact
        [HttpPost("AddContact")]
        public async Task<IActionResult> AddContact([FromBody] ContactModel contact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for contact creation.");
                    return BadRequest(ModelState);
                }

                contact.Id = await _contactService.AddContact(contact);
                _logger.LogInformation("Contact added successfully with ID: {Id}", contact.Id);
                return CreatedAtAction(nameof(AddContact), new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "An error occurred while adding the contact."});
            }
        }

        // PUT: api/contacts/EditContact
        [HttpPut("EditContact")]
        public async Task<IActionResult> EditContact([FromBody] ContactModel contact)
        {
            try
            {
                if (contact == null || contact.Id <= 0)
                {
                    _logger.LogWarning("Invalid contact data. Contact ID is missing or invalid.");
                    return BadRequest(new { message = "Invalid contact data. Contact ID is missing or invalid." });
                }

                await _contactService.UpdateContact(contact);
                _logger.LogInformation("Contact with ID {Id} updated successfully.", contact.Id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { message = $"Contact with ID {contact.Id} not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "An error occurred while updating the contact." });
            }
        }

        // DELETE: api/contacts/DeleteContact
        [HttpDelete("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                await _contactService.DeleteContact(id);
                _logger.LogInformation("Contact with ID {Id} deleted successfully.", id);
                return NoContent();
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { message = $"Contact with ID {id} not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new { message = "An error occurred while deleting the contact."});
            }
        }
    }
}
