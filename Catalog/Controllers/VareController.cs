using System.Runtime.CompilerServices;
using Catalog.Dtos;
using Catalog.Models;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("api/vare")]
    public class VareController : ControllerBase
    {
        private readonly IVareRepository repository;

        // dependency injection her - denne klasse har nu ingen ide om hvad for en repo der bliver brugt (løs kobling)
        public VareController(IVareRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<VareDto>> GetVareAsync()
        {
            // Kig i Extensions.cs hvorfor vi bruger DTO og vare.AsDto() kald
            // await er "wrapped" i () fordi await er seperede fra metoden vi vil gøre async. Nu ved compiler at først skal den udføre (await...) og så når det er completed udfør select.
            var vare = (await repository.GetVareAsync())
                        .Select(vare => vare.AsDto());
            return vare;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VareDto>> GetEnkeltVareAsync(Guid id)
        //ActionResult gør det muligt at få flere returns så return NotFound eller ok
        {
            var vare = await repository.GetEnkeltVareAsync(id);
            if (vare is null)
            {
                return NotFound();
            }

            // Kig i Extensions.cs hvorfor vi bruger DTO og vare.AsDto() kald
            return Ok(vare.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult<VareDto>> CreateVareAsync(CreateVareDto vareDto)
        {
            Vare vare = new Vare()
            {
                Id = Guid.NewGuid(),
                Name = vareDto.Name,
                Price = vareDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateVareAsync(vare);

            return CreatedAtAction(nameof(GetVareAsync), new { id = vare.Id }, vare.AsDto());
            // CreatedAtAction returnerer en HTTP 201 statuskode, og i responsens Location header vil der være en URL, der peger på den oprettede ressource (prøv post i swagger og se reponse headers). Responsens body vil indeholde den oprettede ressource repræsenteret som en VareDto og der vil være Guid på den nye postede vare under location.
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVareAsync(Guid id, UpdateVareDto vareDto)
        {
            // henter først en vare vha. GetVare metode udfra id bestemt i UpdateVare parametren.
            var existingVare = await repository.GetEnkeltVareAsync(id);

            if (existingVare == null)
            {
                return NotFound();
            }

            // records har "with" statement som modificer objekt selvom det er immuntable. Den laver bare en ny instans af record'en med de updated ændringer, super smart.
            Vare updatedVare = existingVare with
            {
                Name = vareDto.Name,
                Price = vareDto.Price
            };

            await repository.UpdateVareAsync(updatedVare);

            // returner statuskode 204
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVareAsync(Guid id)
        {
            // henter først en vare vha. GetVare metode udfra id bestemt i UpdateVare parametren.
            var existingVare = await repository.GetEnkeltVareAsync(id);

            if (existingVare == null)
            {
                return NotFound();
            }

            // ovenover matchede vi id med id fra paramtre og nu sletter vi den
            await repository.DeleteVareAsync(id);

            // statuskode 204
            return NoContent();
        }
    }
}