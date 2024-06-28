using AddressBook.Data;
using AddressBook.Migrations;
using AddressBook.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Service.Class
{
    public class ContactDataService : IContactDataService
    {
        private readonly ApplicationDbContext _context;
        public ContactDataService(ApplicationDbContext context) 
        {
             _context=context;  
        }

        public async Task<int> CreateContact(Contact contact)
        {
            _context.Add(contact);
            var x=await _context.SaveChangesAsync();
            return x;
        }

        public async Task<int> DeleteContact(int contactId)
        {
            var existingContact = await _context.Contacts.FindAsync(contactId);
            if (existingContact == null)
            {
                return 0;
            }
            _context.Contacts.Remove(existingContact);
           return await _context.SaveChangesAsync();
        }

        public async Task<Contact> FindContact(int contactId)
        {
            return await _context.Contacts.FindAsync(contactId);
        }

        public async Task<List<Contact>> GetAllContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<int> UpdateContact(Contact updatedContact)
        {


            var existingContact = await _context.Contacts.FindAsync(updatedContact.Id);
            if (existingContact == null)
            {
                return 0;
            }
           
            existingContact.Name = updatedContact.Name;
            existingContact.PhoneNumber = updatedContact.PhoneNumber;
            existingContact.Address = updatedContact.Address;
            existingContact.Email = updatedContact.Email;
           return await _context.SaveChangesAsync();
        }


    }
}
