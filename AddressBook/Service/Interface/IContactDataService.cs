using AddressBook.Data;

namespace AddressBook.Service.Interface
{
    public interface IContactDataService
    {
        Task<List<Contact>> GetAllContacts();
        Task<int> CreateContact(Contact contact);
        Task<Contact> FindContact( int contactId);

        Task<int> UpdateContact(Contact contact);
Task <int> DeleteContact(int contactId);  

    }
}
