using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnboardingSIGDB1.Domain._Base;
using OnboardingSIGDB1.Domain.Cargos;
using OnboardingSIGDB1.Domain.Cargos.Dtos;
using OnboardingSIGDB1.Domain.Cargos.Services;
using OnboardingSIGDB1.Domain.Cargos.Specifications;

namespace OnboardingSIGDB1.API.Controllers
{
    [Route("api/cargos")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly IConsultaBase<Cargo, CargoDto> _consultaBase;
        private readonly ArmazenadorDeCargo _armazenadorDeCargo;
        private readonly ExclusaoDeCargo _exclusaoDeCargo;

        public CargoController(IConsultaBase<Cargo, CargoDto> consultaBase, 
            ArmazenadorDeCargo armazenadorDeCargo, 
            ExclusaoDeCargo exclusaoDeCargo)
        {
            _consultaBase = consultaBase;
            _armazenadorDeCargo = armazenadorDeCargo;
            _exclusaoDeCargo = exclusaoDeCargo;
        }

        /// <summary>
        /// GET api/cargos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cargos = _consultaBase.Consultar(ListaCargoSpecificationBuilder.Novo()
                .Build());

            return Content(JsonConvert.SerializeObject(cargos),
                "application/json");
        }

        /// <summary>
        /// GET api/cargos/5
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var cargo =  _consultaBase.Consultar(ListaCargoSpecificationBuilder.Novo()
                .ComId(id)
                .Build())
                .Lista
                .FirstOrDefault();

            return Content(JsonConvert.SerializeObject(cargo),
                "application/json");
        }

        /// <summary>
        /// GET api/cargos/pesquisar
        /// </summary>
        /// <returns></returns>
        [HttpGet("pesquisar")]
        public async Task<IActionResult> Get([FromQuery] CargoFiltroDto filtro)
        {
            var cargos = _consultaBase.Consultar(ListaCargoSpecificationBuilder.Novo()
                .ComDescricao(filtro.Descricao)
                .Build());
            return Content(JsonConvert.SerializeObject(cargos),
                "application/json");
        }


        /// <summary>
        /// POST api/cargos
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CargoDto cargo)
        {
            await _armazenadorDeCargo.Armazenar(cargo);
            return Ok();
        }

        /// <summary>
        /// PUT api/cargos/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cargo"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] CargoDto cargo)
        {
            cargo.Id = id;
            await _armazenadorDeCargo.Armazenar(cargo);
            return Ok();
        }

        /// <summary>
        /// DELETE api/cargos/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exclusaoDeCargo.Excluir(id);
            return Ok();
        }
    }
}
