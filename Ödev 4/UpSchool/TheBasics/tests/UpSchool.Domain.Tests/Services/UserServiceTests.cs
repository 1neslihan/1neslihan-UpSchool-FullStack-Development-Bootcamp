using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UpSchool.Domain.Data;
using UpSchool.Domain.Entities;
using UpSchool.Domain.Services;

namespace UpSchool.Domain.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUser_ShouldGetUserWithCorrectId()
        {
            var userRepositoryMock= A.Fake<IUserRepository>();
            Guid userId=new Guid("8f319b0a-2428-4e9f-b7c6-ecf78acf00f9");
            var cancellationSource = new CancellationTokenSource();
            var expectedUser = new User()
            {
                Id= userId
            };

            A.CallTo( ()=>  userRepositoryMock.GetByIdAsync(userId,cancellationSource.Token))
                .Returns(Task.FromResult(expectedUser));

            IUserService userService = new UserManager(userRepositoryMock);
            var user= await userService.GetByIdAsync(userId, cancellationSource.Token);
            Assert.Equal(expectedUser, user);
        }

        [Fact]
        public async Task AddAsync_ShouldThrowException_WhenEmailIsEmptyOrNull()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();
            var firstName = "Kara";
            var lastName = "Thrace";
            var age = 33;
            string email = null;
            var cancellationSource = new CancellationTokenSource();
            
            IUserService userService= new UserManager(userRepositoryMock);

            await Assert.ThrowsAsync<ArgumentException>(() => userService.AddAsync(firstName, lastName, age, email, cancellationSource.Token));

        }

        [Fact]
        public async Task AddAsync_ShouldReturn_CorrectUserId()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();
            Guid userId = new Guid("8f319b0a-2428-4e9f-b7c6-ecf78acf00f9");
            var cancellationSource = new CancellationTokenSource();
            var expectedUser = new User()
            {
                Id= userId
            };

            Assert.NotEqual(Guid.Empty, expectedUser.Id);
            Assert.NotNull(expectedUser.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenUserExists()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();
            Guid userId = new Guid("8f319b0a-2428-4e9f-b7c6-ecf78acf00f9");
            var cancellationSource = new CancellationTokenSource();
            
            IUserService userService = new UserManager(userRepositoryMock);
            var result = await userRepositoryMock.DeleteAsync(x => x.Id == userId, cancellationSource.Token);

            Assert.Equal(result,0);


        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesntExists()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();

            Guid userId = Guid.Empty;
            
            var cancellationSource = new CancellationTokenSource();

            IUserService userService = new UserManager(userRepositoryMock);
         
            await Assert.ThrowsAsync<ArgumentException>(() =>  userService.DeleteAsync(userId, cancellationSource.Token));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserIdIsEmpty()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();

            Guid userId = Guid.Empty;
            var expectedUser = new User()
            {
                Id= userId

            };

            var cancellationSource = new CancellationTokenSource();

            IUserService userService = new UserManager(userRepositoryMock);

            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(expectedUser, cancellationSource.Token));

        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserEmailEmptyOrNull()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();

            var email = string.Empty;

            var expectedUser = new User()
            {
                Email= email

            };

            var cancellationSource = new CancellationTokenSource();

            IUserService userService = new UserManager(userRepositoryMock);

            await Assert.ThrowsAsync<ArgumentException>(() => userService.UpdateAsync(expectedUser, cancellationSource.Token));

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturn_UserListWithAtLeastTwoRecords()
        {
            var userRepositoryMock = A.Fake<IUserRepository>();

            Guid userId1 = new Guid("8f319b0a-2428-4e9f-b7c6-ecf78acf00f9");
            var firstName1 = "Kara";
            var lastName1 = "Thrace";
            var age1 = 33;
            string email1 = "kara.thrace@battlestargalactica.com";

            var user1 = new User()
            {
                Id = userId1,
                FirstName= firstName1,
                LasName= lastName1,
                Age= age1,
                Email= email1,

            };

            Guid userId2 = new Guid("faa21d18-ae84-4bf7-a882-0152f34fe5be");
            var firstName2 = "Helena";
            var lastName2 = "Cain";
            var age2 = 50;
            string email2 = "helena.cain@battlestarpegasus.com";

            var user2 = new User()
            {
                Id = userId2,
                FirstName = firstName2,
                LasName= lastName2,
                Age= age2,
                Email= email2,
            };
            
            var cancellationSource = new CancellationTokenSource();

            A.CallTo(() => userRepositoryMock.AddAsync(user1, cancellationSource.Token));
            A.CallTo(() => userRepositoryMock.AddAsync(user2, cancellationSource.Token));
            
            IUserService userService = new UserManager(userRepositoryMock);
            var result =await userService.GetAllAsync(cancellationSource.Token);

            Assert.True(result.Count>=2);

            
            


        }
    }
}
