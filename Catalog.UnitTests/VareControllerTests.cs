using System;
using Catalog.Repositories;
using Xunit;
using Moq;
using Catalog.Models;
using Amazon.Runtime.Internal.Util;
using Catalog.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Catalog.Dtos;

namespace Catalog.UnitTests;

public class VareControllerTests
{
// Testnavngivningssystemet består af tre dele:
// 1. Kontekst (Context): Beskriver den situation eller det miljø, testen udføres i.
// 2. Handling (Action): Angiver den handling eller operation, der udføres i testen.
// 3. Forventet adfærd (Expected Behavior): Beskriver den forventede adfærd eller resultat af testen.
// aka "public void UnitOfWork_StateUnderTest_ExpectedBehavior()"

// Opretter en falsk repo og logger version af et vare repository da jeg bruger dem i flere tests nedenunder 
// istedet for at gøre det i hver metode
private readonly Mock<IVareRepository> repositoryStub = new();
private readonly Mock<ILogger<VareController>> loggerStub = new();
// laver en variabel rand med et random tal for at sætte Price på CreateRandomItem() længere nede
private readonly Random rand = new();

[Fact]
public async Task GetEnkeltVareAsync_WithUnexistingVare_ReturnsNotFound()
{
    // Arrange
    // Når controlleren kalder "GetEnkeltVareAsync" med enhver given Guid, forventes det, at den returnerer en null-værdi.
    repositoryStub.Setup(repo => repo.GetEnkeltVareAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Vare)null);

    // Opretter selve kontrolleren, som skal testes.
    var controller = new VareController(repositoryStub.Object, loggerStub.Object);

    // Act
    // giver en ny random guid som parameter på metoden
    var result = await controller.GetEnkeltVareAsync(Guid.NewGuid());

    // Assert
    // Tjekker at det returnerede resultat er af typen NotFoundResult, da det er det vi forventer i GetEnkeltVareAsync() metoden
    Assert.IsType<NotFoundResult>(result.Result);
}

// [Fact]
// public async Task GetEnkeltVareAsync_WithExistingVare_ReturnsExpectedVare()
// {
//     //Arrange
//     var expectedVare = CreateRandomItem();
//     repositoryStub.Setup(repo => repo.GetEnkeltVareAsync(It.IsAny<Guid>()))
//         .ReturnsAsync(expectedVare);

//      var controller = new VareController(repositoryStub.Object, loggerStub.Object);

//     // Act
//     // giver en ny random guid som parameter på metoden
//     var result = await controller.GetEnkeltVareAsync(Guid.NewGuid());

//     // Assert
//     Assert.IsType<VareDto>(result.Value);
//     //tjekker alle properties af returned DTO matcher expectedVare
//     var dto = (result as ActionResult<VareDto>).Value;
//     Assert.Equal(expectedVare.Id, dto.Id);
//     Assert.Equal(expectedVare.Name, dto.Name);
//     Assert.Equal(expectedVare.Price, dto.Price);
//     Assert.Equal(expectedVare.CreatedDate, dto.CreatedDate);
// }


// Opretter en tilfældig Vare-objekt til testene
private Vare CreateRandomItem()
{
    return new()
    {
        Id = Guid.NewGuid(),
        Name = Guid.NewGuid().ToString(),
        Price = rand.Next(1000),
        CreatedDate = DateTimeOffset.UtcNow
    };
}
}