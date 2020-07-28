using System;
using Xunit;
using Moq;

using OccamsRazor.Common;
using OccamsRazor.Web.Repository;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Test.Services
{
    public class GameDataServiceTest
    {

        Mock<IGameDataService> service = new Mock<IGameDataService>();
        Mock<IGameDataRepository> repository = new Mock<IGameDataRepository>();

        [Fact]
        public void Test()
        {
        }
    }
}
