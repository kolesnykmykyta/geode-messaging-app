using DataAccess.DbContext;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Geode.DataAccess.Tests
{
    public class UnitOfWorkTests
    {
        [Theory]
        [InlineData(typeof(Chat))]
        [InlineData(typeof(Message))]
        [InlineData(typeof(User))]
        [InlineData(typeof(ChatMember))]
        public void GenericRepository_ValidTypes_ReturnsExpectedRepository(Type repositoryType)
        {
            UnitOfWork sut = new UnitOfWork(null);
            var genericMethod = typeof(UnitOfWork)
                .GetMethod(nameof(UnitOfWork.GenericRepository))!
                .MakeGenericMethod(repositoryType);
            Type expectedType = typeof(GenericRepository<>).MakeGenericType(repositoryType);

            var actual = genericMethod.Invoke(sut, null);

            Assert.IsType(expectedType, actual);
        }

        [Fact]
        public void GenericRepository_MultipleRepositoryRequest_CachesCreatedRepositories()
        {
            UnitOfWork sut = new UnitOfWork(null);

            IGenericRepository<Chat> firstRepository = sut.GenericRepository<Chat>();
            IGenericRepository<Chat> secondRepository = sut.GenericRepository<Chat>();

            Assert.Equal(firstRepository, secondRepository);
        }

        [Fact]
        public void SaveChanges_Invoke_InvokesContextMethod()
        {
            Mock<DatabaseContext> mockDbContext = new Mock<DatabaseContext>();
            UnitOfWork sut = new UnitOfWork(mockDbContext.Object);

            sut.SaveChanges();

            mockDbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Dispose_Invoke_InvokesContextMethod()
        {
            Mock<DatabaseContext> mockDbContext = new Mock<DatabaseContext>();
            UnitOfWork sut = new UnitOfWork(mockDbContext.Object);

            sut.Dispose();

            mockDbContext.Verify(x => x.Dispose(), Times.Once);
        }
    }
}
