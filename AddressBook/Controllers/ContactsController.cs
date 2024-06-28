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

        public ContactsController(IContactDataService contactDataService)
        {
           
            _contactDataService = contactDataService;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await _contactDataService.GetAllContacts();
        }

        [HttpGet]
        public async Task<ActionResult<Contact>>GetContactById(int id)
        {
            return await _contactDataService.FindContact(id);

        }
       
      

      
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                // Log the exception if needed
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Contact updatedContact)
        {
            if (updatedContact == null || !ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid data" });
            }        
           var updated =await  _contactDataService.UpdateContact(updatedContact);           
            return Ok(new { success = true });
        }

      

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
           await _contactDataService.DeleteContact(id);
            return NoContent();
        }


    }
}
