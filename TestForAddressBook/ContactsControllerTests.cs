using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using AddressBook.Controllers; 
using AddressBook.Data;
using AddressBook.Service.Interface;
using Microsoft.Extensions.Logging;
public class ContactsControllerTests
{
    private readonly Mock<IContactDataService> _mockService;
    private readonly ContactsController _controller;

    public ContactsControllerTests()
    {
        _mockService = new Mock<IContactDataService>();
        var mockLogger = new Mock<ILogger<ContactsController>>();
        _controller = new ContactsController(_mockService.Object, mockLogger.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult()
    {
        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task GetContacts_ReturnsAllContacts()
    {
        // Arrange
        var contacts = new List<Contact> { new Contact { Id = 1, Name = "John Doe", PhoneNumber = "1234567890", Address = "123 Main St", Email = "john@example.com" } };
        _mockService.Setup(service => service.GetAllContacts()).ReturnsAsync(contacts);

        // Act
        var result = await _controller.GetContacts();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Contact>>>(result);
        var returnValue = Assert.IsType<List<Contact>>(actionResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetContactById_ReturnsContact()
    {
        // Arrange
        var contact = new Contact { Id = 1, Name = "John Doe", PhoneNumber = "1234567890", Address = "123 Main St", Email = "john@example.com" };
        _mockService.Setup(service => service.FindContact(1)).ReturnsAsync(contact);

        // Act
        var result = await _controller.GetContactById(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Contact>>(result);
        var returnValue = Assert.IsType<Contact>(actionResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public void Create_ReturnsViewResult()
    {
        // Act
        var result = _controller.Create();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Edit_ValidContact_ReturnsSuccess()
    {
        // Arrange
        int contactId = 1;
        var updatedContact = new Contact { Id = 1, Name = "Updatedashok ", PhoneNumber = "1234567890", Address = "123 Main St", Email = "john@example.com" };
        _mockService.Setup(service => service.UpdateContact(updatedContact)).ReturnsAsync(1);

        // Act
        var result = await _controller.Edit(contactId, updatedContact);

        // Assert
        Assert.IsType<OkObjectResult>(result); // Assuming you return OkObjectResult in Edit method upon success
        var okResult = (OkObjectResult)result;
        Assert.Equal(200, okResult.StatusCode);


    }



    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
