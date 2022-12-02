using BP.Samples.EFTests.Domain.Adaptables;
using BP.Samples.EFTests.Infrastructure.DataAccess.Context;
using BP.Samples.EFTests.Infrastructure.DataAccess.Entities;
using BP.Samples.EFTests.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BP.Samples.EFTests.UnitTests
{
    public class SelectTests
    {
        private readonly EFTestContext context;

        public SelectTests()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            IServiceProvider services = new ServiceCollection()
                .ConfigureIoC(config)
                .BuildServiceProvider();

            context = services.GetRequiredService<EFTestContext>();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateUser()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                SurName = "Test",
                Idcard = "45425235F",
                DateBirth = DateTime.Now,

            };
            context.Add(user);
            context.SaveChanges();
            Assert.Pass();
        }
    }
}