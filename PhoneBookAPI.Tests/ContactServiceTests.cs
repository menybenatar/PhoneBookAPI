using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using PhoneBookAPI.Data;
using PhoneBookAPI.Helpers;
using PhoneBookAPI.Interfaces;
using PhoneBookAPI.Models;
using PhoneBookAPI.Services;
using Xunit;

namespace PhoneBookAPI.Tests
{
    public class ContactServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly AppDbContext _dbContext;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "PhoneBookDb")
                .Options;
            _dbContext = new AppDbContext(options);

            // Mock dependencies
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();

            _cacheMock = new Mock<IDistributedCache>();

            // Initialize the ContactService with in-memory DB and mocks
            _contactService = new ContactService(_dbContext, _mapper, _cacheMock.Object);
        }

        [Fact]
        public async Task AddContact_Should_Add_Contact_To_Db()
        {
            // Arrange
            var contactModel = new ContactModel { FirstName = "John", LastName = "Doe", PhoneNumber = "123456789" };
            var contactEntity = new ContactEntity { Id = 1, FirstName = "John", LastName = "Doe", PhoneNumber = "123456789" };


            // Act
            var resultId = await _contactService.AddContact(contactModel);

            // Assert
            var addedContact = await _dbContext.Contacts.FindAsync(resultId);
            Assert.NotNull(addedContact);
            Assert.Equal(contactEntity.FirstName, addedContact.FirstName);
        }

        [Fact]
        public async Task DeleteContact_Should_Remove_Contact_From_Db_And_Cache()
        {
            // Arrange
            var contactId = 1;
            var contactEntity = new ContactEntity { Id = contactId, FirstName = "John", LastName = "Doe", PhoneNumber = "123456789" };
            _dbContext.Contacts.Add(contactEntity);
            await _dbContext.SaveChangesAsync();

            // Act
            await _contactService.DeleteContact(contactId);

            // Assert
            var deletedContact = await _dbContext.Contacts.FindAsync(contactId);
            Assert.Null(deletedContact);
            _cacheMock.Verify(x => x.RemoveAsync(It.IsAny<string>(), default), Times.Once);
        }

        [Fact]
        public async Task SearchContacts_Should_Return_Matching_Contacts()
        {
            // Arrange
            var contactEntities = new List<ContactEntity>
            {
                new ContactEntity { FirstName = "John", LastName = "Doe", PhoneNumber = "123456789" },
                new ContactEntity { FirstName = "Jane", LastName = "Smith", PhoneNumber = "987654321" }
            };
            _dbContext.Contacts.AddRange(contactEntities);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _contactService.SearchContacts("John");
            var listContacts = result.ToList();
            // Assert
            Assert.NotNull(listContacts);
            Assert.True(listContacts.Count > 0);
        }

        [Fact]
        public async Task UpdateContact_Should_Update_Contact_And_Update_Cache()
        {
            // Arrange
            var contactId = 1;
            var contactModel = new ContactModel { Id = contactId, FirstName = "UpdatedJohn", LastName = "Doe", PhoneNumber = "34545" };
            var contactEntity = new ContactEntity { Id = contactId, FirstName = "John", LastName = "Doe", PhoneNumber = "456456456" };

            // Add contact to the in-memory database
            _dbContext.Contacts.Add(contactEntity);
            await _dbContext.SaveChangesAsync();

            // Mock cache behavior
            _cacheMock.Setup(c => c.GetAsync(It.IsAny<string>(), default))
                      .ReturnsAsync((byte[])null); // Simulate cache miss

            // Act
            await _contactService.UpdateContact(contactModel);

            // Assert
            var updatedContact = await _dbContext.Contacts.FindAsync(contactId);
            Assert.Equal("UpdatedJohn", updatedContact.FirstName);

            // If cache is expected to be updated twice, set the expected number of invocations to 2
            _cacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateContact_Should_Throw_If_Contact_Not_Found()
        {
            // Arrange
            var contactModel = new ContactModel { Id = 999, FirstName = "NonExistent", LastName = "Doe" };

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _contactService.UpdateContact(contactModel));
        }
    }

}
