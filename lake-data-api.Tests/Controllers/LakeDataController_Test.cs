using lake_data_api.Controllers;
using lake_data_api.Models;
using lake_data_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace lake_data_api.Tests;

public class Tests
{
    // private LakeDataController _controller;
    // private Mock<ILakeWaterLevelService> mockWaterLevelService;
    // private Mock<ILogger<LakeDataController>> mockLogger;

    // [SetUp]
    // public void Setup()
    // {
    //     mockLogger = new Mock<ILogger<LakeDataController>>();
    //     mockWaterLevelService = new Mock<ILakeWaterLevelService>();
    //     mockWaterLevelService
    //         .Setup(x => x.GetLakeWaterLevel(It.IsAny<Lake>()))
    //         .ReturnsAsync(new LakeWaterLevelModel
    //         {
    //             LakeWaterLevel = 2,
    //             TimeStamp = DateTime.Now
    //         });
        
    //     _controller = new LakeDataController(mockLogger.Object, mockWaterLevelService.Object);
    // }

    // [Test]
    // public async Task Get_ShouldCallLakeWaterLevelService_GetLakeWaterLevel()
    // {
    //     // arrange
    //     var rand = new Random();
    //     var id = rand.Next().ToString();

    //     // act
    //     var result = await _controller.Get(id);

    //     // assert
    //     mockWaterLevelService.Verify(mock => mock.GetLakeWaterLevel(It.IsAny<Lake>()), Times.Once);
    // }

    // [Test]
    // public async Task Get_ShouldReturnLakeWithLakeWaterLevelObject()
    // {
    //     // arrange
    //     var rand = new Random();
    //     var id = rand.Next().ToString();

    //     var mockWaterLevelValue = rand.Next(0, 2000);

    //     mockWaterLevelService
    //         .Setup(x => x.GetLakeWaterLevel(It.IsAny<Lake>()))
    //         .ReturnsAsync(new LakeWaterLevelModel
    //         {
    //             LakeWaterLevel = mockWaterLevelValue,
    //             TimeStamp = DateTime.Now
    //         });

    //     // act
    //     var result = await _controller.Get(id);

    //     // assert
    //     // we expect a 200
    //     Assert.That(result, Is.InstanceOf<OkObjectResult>());
        
    //     // we expect the result to be a Lake object
    //     var okResult = result as OkObjectResult;
    //     Assert.That(okResult, Is.Not.Null);
    //     Assert.That(okResult.Value, Is.InstanceOf<Lake>());

    //     // we expect the Lake object to have a LakeWaterLevel property
    //     var lake = okResult.Value as Lake;
    //     Assert.That(lake, Is.Not.Null);
    //     Assert.That(lake.LakeWaterLevel, Is.Not.Null);
    //     // we expect the LakeWaterLevel property to have a LakeWaterLevel value that was returned from the service
    //     Assert.That(lake.LakeWaterLevel.LakeWaterLevel, Is.EqualTo(mockWaterLevelValue));
    // }

    // [Test]
    // public async Task Get_ShouldThrow500OnError()
    // {
    //     // arrange
    //     var rand = new Random();
    //     var id = rand.Next().ToString();

    //     var mockWaterLevelValue = rand.Next(0, 2000);

    //     mockWaterLevelService
    //         .Setup(x => x.GetLakeWaterLevel(It.IsAny<Lake>()))
    //         .ThrowsAsync(new Exception("Test exception"));

    //     // act
    //     try
    //     {
    //         var result = await _controller.Get(id);
    //     }
    //     catch (Exception ex)
    //     {
    //         // assert
    //         Assert.That(ex.Message, Is.EqualTo("Test exception"));
    //     }
    // }
}
