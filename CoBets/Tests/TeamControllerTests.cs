using System.Collections.Generic;
using System.Threading.Tasks;
using CoBetsDatabase.Models;
using CoBetsDatabase.Repositories.Interfaces;
using Controllers;
using Moq;
using Xunit;

namespace Tests
{
    public class TeamControllerTests
    {
        [Theory]
        [MemberData(nameof(TeamsData))]
        public async Task TestAddOk(Team team)
        {
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(r => r.Add(team)).ReturnsAsync(team);

            var teamController = new TeamController(teamRepository.Object);

            var result = await teamController.Add(team);
                
            Assert.Equal(team, result);
        }
        
        [Theory]
        [MemberData(nameof(TeamsListData))]
        public async Task TestGetAllOk(List<Team> expectedResult)
        {
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(r => r.GetAll()).ReturnsAsync(expectedResult);

            var teamController = new TeamController(teamRepository.Object);

            var result = await teamController.GetAll();

            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [MemberData(nameof(TeamsDataForDelete))]
        public async Task TestDeleteOk(int id, bool expectedResult)
        {
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(r => r.Delete(id)).ReturnsAsync(expectedResult);

            var teamController = new TeamController(teamRepository.Object);

            var result = await teamController.Delete(id);
                
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [MemberData(nameof(TeamsData))]
        public async Task TestFindElementByIdOk(Team expectedResult)
        {
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository
                .Setup(r => r.FindElementById(expectedResult.Id))
                .ReturnsAsync(expectedResult);

            var teamController = new TeamController(teamRepository.Object);

            var result = await teamController.FindElementById(expectedResult.Id);
                
            Assert.Equal(expectedResult, result);
        }
        
        public static IEnumerable<object[]> TeamsData => new List<object[]>()
        {
            new object[]
            {
                new Team(1, "team"),
            }
        };
        
        public static IEnumerable<object[]> TeamsListData => new List<object[]>()
        {
            new object[]
            {
                new List<Team>()
            },
            new object[]
            {
                new List<Team>()
                {
                    new Team(1, "team"),
                    new Team(2, "team"),
                    new Team(3, "team"),
                }
            }
        };
        
        public static IEnumerable<object[]> TeamsDataForDelete => new List<object[]>()
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