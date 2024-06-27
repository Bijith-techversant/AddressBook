using AddressBook.Service.Class;
using AddressBook.Service.Interface;

namespace AddressBook.Service.Configurations
{
    public static class ServiceExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IContactDataService, ContactDataService>();
        }
    }
}
