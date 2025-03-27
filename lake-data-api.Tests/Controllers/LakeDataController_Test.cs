using lake_data_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace lake_data_api.Tests;

public class Tests
{
    private LakeDataController _controller;
    [SetUp]
    public void Setup()
    {
        var mockLogger = new Mock<ILogger<LakeDataController>>();
        _controller = new LakeDataController(mockLogger.Object);
    }

    [Test]
    public void ShouldReturnHello()
    {
        // arrange
        var rand = new Random();
        var id = rand.Next().ToString();

        // act
        var result = _controller.Get(id);

        // assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        Assert.That(((OkObjectResult)result).Value, Is.EqualTo("Hello"));
    }
}
