using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnboardingSIGDB1.Domain.Dto;
using OnboardingSIGDB1.Domain.Dto.Filtros;
using OnboardingSIGDB1.Domain.Entitys;
using OnboardingSIGDB1.Domain.Interfaces.Services;

namespace OnboardingSIGDB1.API.Controllers
{
    [Route("api/cargos")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly ICargoService _cargoService;
        private readonly ICargoConsultaService _cargoConsultaService;
        private readonly ICargoDeleteService _cargoDeleteService;
        private readonly IMapper _mapper;

        public CargoController(ICargoService cargoService, 
            ICargoConsultaService cargoConsultaService, 
            ICargoDeleteService cargoDeleteService, 
            IMapper mapper)
        {
            _cargoService = cargoService;
            _cargoConsultaService = cargoConsultaService;
            _cargoDeleteService = cargoDeleteService;
            _mapper = mapper;
        }

        /// <summary>
        /// GET api/cargos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cargos = await _cargoConsultaService.GetAll();
            return Content(JsonConvert.SerializeObject(_mapper.Map<IEnumerable<CargoDto>>(cargos)),
                "application/json");
        }

        /// <summary>
        /// GET api/cargos/5
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var cargo = await _cargoConsultaService.GetById(id);
            return Content(JsonConvert.SerializeObject(_mapper.Map<CargoDto>(cargo)),
                "application/json");
        }

        /// <summary>
        /// GET api/cargos/pesquisar
        /// </summary>
        /// <returns></returns>
        [HttpGet("pesquisar")]
        public async Task<IActionResult> Get([FromQuery] CargoFiltroDto filtro)
        {
            var cargos = await _cargoConsultaService.GetFiltro(filtro);
            return Content(JsonConvert.SerializeObject(_mapper.Map<IEnumerable<CargoDto>>(cargos)),
                "application/json");
        }


        /// <summary>
        /// POST api/cargos
        /// </summary>
        /// <param name="cargo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CargoInsertUpdateDto cargo)
        {
            var entity = _mapper.Map<Cargo>(cargo);
            await _cargoService.InsertCargo(entity);
            return Ok();
        }

        /// <summary>
        /// PUT api/cargos/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cargo"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] CargoInsertUpdateDto cargo)
        {
            var entity = _mapper.Map<Cargo>(cargo);
            await _cargoService.UpdateCargo(id, entity);
            return Ok();
        }

        /// <summary>
        /// DELETE api/cargos/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cargoDeleteService.Delete(id);
            return Ok();
        }
    }
}
