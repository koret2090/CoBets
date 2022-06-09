using System.Collections.Generic;
using System.Threading.Tasks;
using CoBetsDatabase.Models;
using CoBetsDatabase.Repositories.Interfaces;
using Controllers;
using Moq;
using Xunit;

namespace Tests
{
    public class PlayerControllerTests
    {
        [Theory]
        [MemberData(nameof(PlayersData))]
        public async Task TestAddOk(Player player)
        {
            var playerRepository = new Mock<IPlayerRepository>();
            playerRepository.Setup(r => r.Add(player)).ReturnsAsync(player);

            var playerController = new PlayerController(playerRepository.Object);

            var result = await playerController.Add(player);
                
            Assert.Equal(player, result);
        }
        
        [Theory]
        [MemberData(nameof(PlayersListData))]
        public async Task TestGetAllOk(List<Player> expectedResult)
        {
            var playerRepository = new Mock<IPlayerRepository>();
            playerRepository.Setup(r => r.GetAll()).ReturnsAsync(expectedResult);

            var playerController = new PlayerController(playerRepository.Object);

            var result = await playerController.GetAll();

            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [MemberData(nameof(PlayersDataForDelete))]
        public async Task TestDeleteOk(int id, bool expectedResult)
        {
            var playerRepository = new Mock<IPlayerRepository>();
            playerRepository.Setup(r => r.Delete(id)).ReturnsAsync(expectedResult);

            var playerController = new PlayerController(playerRepository.Object);

            var result = await playerController.Delete(id);
                
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [MemberData(nameof(PlayersData))]
        public async Task TestFindElementByIdOk(Player expectedResult)
        {
            var playerRepository = new Mock<IPlayerRepository>();
            playerRepository
                .Setup(r => r.FindElementById(expectedResult.Id))
                .ReturnsAsync(expectedResult);

            var playerController = new PlayerController(playerRepository.Object);

            var result = await playerController.FindElementById(expectedResult.Id);
                
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> PlayersData => new List<object[]>()
        {
            new object[]
            {
                new Player(1, 1, "player", 1, 1),
            }
        };
        
        public static IEnumerable<object[]> PlayersListData => new List<object[]>()
        {
            new object[]
            {
                new List<Player>()
            },
            new object[]
            {
                new List<Player>()
                {
                    new Player(1, 1, "player", 1, 1),
                    new Player(2, 2, "player", 2, 2),
                    new Player(3, 3, "player", 3, 3),
                }
            }
        };
        
        public static IEnumerable<object[]> PlayersDataForDelete => new List<object[]>()
        {
            new object[]
            {
                1,
                true
            },
            new object[]
            {
                1,
                false
            },
            new object[]
            {
                1,
                false
            },
        };
    }
}

