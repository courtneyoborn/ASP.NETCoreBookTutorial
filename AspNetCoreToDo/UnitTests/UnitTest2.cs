using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests
{
    public class UnitTest2
    {
        [Fact]
        public async Task AddNewItemAsComplete()
        {


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_MarkItemDone").Options;            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);
                var fakeUser = new ApplicationUser
                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                };

                await service.AddItemAsync(new ToDoItem
                {
                    Title = "Testing?"
                }, fakeUser);
            }            using (var context = new ApplicationDbContext(options))
            {
                var itemsInDatabase = await context
               .Items.CountAsync();
                Assert.Equal(1, itemsInDatabase);
                var item = await context.Items.FirstAsync();
                Assert.Equal("Testing?", item.Title);

               // item.IsDone = true;
                var saveResult = await context.SaveChangesAsync();
                Assert.True(item.IsDone);
            }
        }
    }
}