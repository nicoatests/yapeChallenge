using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TransferYape.Api.Controllers;
using TransferYape.Application.Transactions.Commands.CreateTransaction;
using TransferYape.Application.Transactions.Models;
using FluentAssertions;

namespace TransferYape.Api.UnitTests.Controllers;
public class TransactionsControllerTests
{
    private readonly IFixture _fixture;
    private readonly ISender _senderMock;
    private readonly IServiceProvider _serviceProviderMock;
    private readonly DefaultHttpContext _context;
    private readonly TransactionsController _controller;

    public TransactionsControllerTests()
    {
        _fixture = new Fixture();
        _senderMock = Substitute.For<ISender>();
        _serviceProviderMock = Substitute.For<IServiceProvider>();

        _context = new DefaultHttpContext
        {
            RequestServices = _serviceProviderMock
        };
        _controller = new TransactionsController()
        {
            ControllerContext =
            {
                HttpContext = _context
            }
        };
    }

    [Fact]
    public async Task CreateTranstaction_WhenCreates_ShouldReturnCreated()
    {
        // Arrange
        var cancellationToken = _fixture.Create<CancellationToken>();
        var passTypeId = _fixture.Create<Guid?>();
        var expectedResponse = new TransactionCreatedResponse(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Now));

        _senderMock.Send(Arg.Any<CreateTranstactionCommand>(), Arg.Any<CancellationToken>()).Returns(expectedResponse);
        _serviceProviderMock.GetService(typeof(ISender)).Returns(_senderMock);
        _controller.ControllerContext.HttpContext = _context;

        // Act
        var response = await _controller.CreateTranstaction(_fixture.Create<NewTransaction>(), cancellationToken);

        // Assert
        var okResult = response.Should().BeOfType<CreatedResult>().Which;
        okResult.Value.Should().BeEquivalentTo(expectedResponse);
    }
}
