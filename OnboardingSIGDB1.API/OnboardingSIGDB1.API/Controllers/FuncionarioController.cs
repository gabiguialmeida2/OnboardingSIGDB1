using System;
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
    [Route("api/funcionarios")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly IVinculacaoFuncionarioEmpresaService _vinculacaoFuncionarioEmpresaService;
        private readonly IVinculacaoFuncionarioCargosService _vinculacaoFuncionarioCargosService;
        private readonly IMapper _mapper;

        public FuncionarioController(IFuncionarioService funcionarioService,
            IVinculacaoFuncionarioEmpresaService vinculacaoFuncionarioEmpresaService,
            IVinculacaoFuncionarioCargosService vinculacaoFuncionarioCargosService,
            IMapper mapper)
        {
            _funcionarioService = funcionarioService;
            _vinculacaoFuncionarioEmpresaService = vinculacaoFuncionarioEmpresaService;
            _vinculacaoFuncionarioCargosService = vinculacaoFuncionarioCargosService;
            _mapper = mapper;
        }
        /// <summary>
        /// GET api/funcionarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var funcionarios = await _funcionarioService.GetAll();
            return Content(JsonConvert.SerializeObject(_mapper.Map<IEnumerable<FuncionarioDto>>(funcionarios)),
                "application/json");
        }

        /// <summary>
        /// GET api/funcionarios/5
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var funcionario = await _funcionarioService.GetById(id);
            return Content(JsonConvert.SerializeObject(_mapper.Map<FuncionarioDto>(funcionario)),
                "application/json");
        }

        [HttpGet("pesquisar")]
        public async Task<IActionResult> Get([FromQuery] FuncionarioFiltroDto filtro)
        {
            var funcionarios = await _funcionarioService.GetFiltro(filtro);
            return Content(JsonConvert.SerializeObject(_mapper.Map<IEnumerable<FuncionarioDto>>(funcionarios)),
                "application/json");
        }


        /// <summary>
        /// POST api/funcionarios
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FuncionarioInsertDto funcionario)
        {
            var entity = _mapper.Map<Funcionario>(funcionario);
            await _funcionarioService.InsertFuncionario(entity);
            return Ok();
        }

        /// <summary>
        /// PUT api/funcionarios/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="funcionario"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] FuncionarioUpdateDto funcionario)
        {
            var entity = _mapper.Map<Funcionario>(funcionario);
            await _funcionarioService.UpdateFuncionario(id, entity);
            return Ok();
        }

        /// <summary>
        /// DELETE api/funcionarios/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _funcionarioService.Delete(id);
            return Ok();
        }

        /// <summary>
        /// POST api/funcionarios/vincular/1/empresa/2
        /// </summary>
        /// <param name="funcionarioId"></param>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        [HttpPost("vincular/{funcionarioId}/empresa/{empresaId}")]
        public async Task<IActionResult> VincularEmpresa(long funcionarioId, long empresaId)
        {
            await _vinculacaoFuncionarioEmpresaService.Vincular(funcionarioId, empresaId);
            return Ok();
        }

        /// <summary>
        /// POST api/funcionarios/vincular/1/cargo/2
        /// </summary>
        /// <param name="funcionarioId"></param>
        /// <param name="cargoId"></param>
        /// <param name="dataVinculacao"></param>
        /// <returns></returns>
        [HttpPost("vincular/{funcionarioId}/cargo/{cargoId}/data/{dataVinculacao}")]
        public async Task<IActionResult> VincularCargo(long funcionarioId, long cargoId, DateTime dataVinculacao)
        {
            await _vinculacaoFuncionarioCargosService.Vincular(funcionarioId, cargoId, dataVinculacao);
            return Ok();
        }
    }
}
