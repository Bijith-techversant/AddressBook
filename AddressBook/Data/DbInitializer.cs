using AddressBook.Data;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate(); 

            if (!context.Contacts.Any())
            {
                // Seed data here
                context.Contacts.AddRange(
                    new Contact { Name = "Ashok", PhoneNumber = "1234567890", Address = "123 Main St", Email = "Ashok@example.com" },
                    new Contact { Name = "Anjali", PhoneNumber = "9876543210", Address = "456 Other St", Email = "Anjali@example.com" }
                );

                context.SaveChanges();
            }
        }

    }
}

