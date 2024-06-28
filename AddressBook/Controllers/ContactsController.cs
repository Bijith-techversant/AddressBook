using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AddressBook.Data;
using AddressBook.Service.Class;
using AddressBook.Service.Interface;

namespace AddressBook.Controllers
{
    public class ContactsController : Controller
    {
        
        private readonly IContactDataService _contactDataService;

        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactDataService contactDataService, ILogger<ContactsController> logger)
        {

            _contactDataService = contactDataService;
            _logger = logger;
        }

       /// <summary>
       /// View for displaying Contacts
       /// </summary>
       /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View();
        }

        /// <summary>
        /// Retrieves all contacts.
        /// </summary>
        /// <returns>An ActionResult containing a collection of Contact objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            try
            {
                var contacts = await _contactDataService.GetAllContacts();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving contacts.");
                return StatusCode(500, "Internal server error");
            }
        }
        /// <summary>
        /// Retrieves a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact to retrieve.</param>
        /// <returns>An ActionResult containing the Contact object, or a not found response if the contact does not exist.</returns>
        [HttpGet]
        public async Task<ActionResult<Contact>> GetContactById(int id)
        {
            try
            {
                var contact = await _contactDataService.FindContact(id);

                if (contact == null)
                {
                    return NotFound(); // Return 404 Not Found if contact with specified ID is not found
                }

                return Ok(contact); // Return 200 OK with the retrieved contact
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving contact with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Displays the view for creating contacts.
        /// </summary>
        /// <returns>An IActionResult representing the Create view.</returns>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="contact">The Contact object containing information to create.</param>
        /// <returns>A JSON response indicating success or failure with an optional message.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _contactDataService.CreateContact(contact);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Validation failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a contact.");
                return Json(new { success = false, message = "An error occurred while creating a contact." });
            }
        }

        /// <summary>
        /// Updates a contact.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="updatedContact">The updated Contact object.</param>
        /// <returns>A  response indicating success or failure with an optional message.</returns>

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromBody] Contact updatedContact)
        {
           

            try
            {
                if (updatedContact == null || updatedContact.Id != id || !ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "Invalid data" });
                }
                var updated = await _contactDataService.UpdateContact(updatedContact);
                return Ok(new { success = true, message = "Contact updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error updating contact: {ex.Message}" });
            }
        }

        /// <summary>
        /// Deletes a contact by its ID.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <returns>An IActionResult indicating success or failure.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _contactDataService.DeleteContact(id);
                return NoContent(); // 204 No Content response indicates successful deletion
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting contact with ID {id}.");
                return StatusCode(500, new { success = false, message = $"Error deleting contact: {ex.Message}" });
            }
        }


    }
}
