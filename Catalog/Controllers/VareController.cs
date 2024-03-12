using Catalog.Dtos;
using Catalog.Models;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("vare")]
    public class VareController : ControllerBase
    {
        private readonly IInMemVareRepository repository;

        // dependency injection her - denne klasse har nu ingen ide om hvad for en repo der bliver brugt (løs kobling)
        public VareController(IInMemVareRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<VareDto> GetVare()
        {
            // Kig i Extensions.cs hvorfor vi bruger DTO og vare.AsDto() kald
            var vare = repository.GetVare().Select(vare => vare.AsDto());
            return vare;
        }

        [HttpGet("{id}")]
        public ActionResult<VareDto> GetEnkeltVare(Guid id)
        //ActionResult gør det muligt at få flere returns så return NotFound eller ok
        {
            var vare = repository.GetEnkeltVare(id);
            if (vare is null) {
                return NotFound();
            }
            
             // Kig i Extensions.cs hvorfor vi bruger DTO og vare.AsDto() kald
            return Ok(vare.AsDto());
        }

        [HttpPost]
        public ActionResult<VareDto> CreateVare(CreateVareDto vareDto)
        {
            Vare vare = new Vare() {
                Id = Guid.NewGuid(),
                Name = vareDto.Name,
                Price = vareDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateVare(vare);

            return CreatedAtAction(nameof(GetVare), new { id = vare.Id}, vare.AsDto());
            // CreatedAtAction returnerer en HTTP 201 statuskode, og i responsens Location header vil der være en URL, der peger på den oprettede ressource (prøv post i swagger og se reponse headers). Responsens body vil indeholde den oprettede ressource repræsenteret som en VareDto og der vil være Guid på den nye postede vare under location.
        }


        [HttpPut("{id}")]
        public ActionResult UpdateVare(Guid id, UpdateVareDto vareDto)
        {
            // henter først en vare vha. GetVare metode udfra id bestemt i UpdateVare parametren.
            var existingVare = repository.GetEnkeltVare(id);

            if (existingVare == null)
            {
                return NotFound();
            }

            // records har "with" statement som modificer objekt selvom det er immuntable. Den laver bare en ny instans af record'en med de updated ændringer, super smart.
            Vare updatedVare = existingVare with {
                Name = vareDto.Name,
                Price = vareDto.Price
            };

            repository.UpdateVare(updatedVare);

            // returner statuskode 204
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteVare(Guid id)
        {
             // henter først en vare vha. GetVare metode udfra id bestemt i UpdateVare parametren.
            var existingVare = repository.GetEnkeltVare(id);

            if (existingVare == null)
            {
                return NotFound();
            }

            // ovenover matchede vi id med id fra paramtre og nu sletter vi den
            repository.DeleteVare(id);

            // statuskode 204
            return NoContent();
        }
    }
}