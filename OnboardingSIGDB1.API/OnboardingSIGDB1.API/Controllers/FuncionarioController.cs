using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnboardingSIGDB1.Domain._Base;
using OnboardingSIGDB1.Domain.Funcionarios;
using OnboardingSIGDB1.Domain.Funcionarios.Dtos;
using OnboardingSIGDB1.Domain.Funcionarios.Services;
using OnboardingSIGDB1.Domain.Funcionarios.Specifications;

namespace OnboardingSIGDB1.API.Controllers
{
    [Route("api/funcionarios")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly ArmazenadorDeFuncionario _armazenadorDeFuncionario;
        private readonly ExclusaoDeFuncionario _exclusaoDeFuncionario;
        private readonly VinculadorDeFuncionarioEmpresa _vinculadorDeFuncionarioEmpresa;
        private readonly VinculadorDeFuncionarioCargo _vinculadorDeFuncionarioCargo;
        private readonly IConsultaBase<Funcionario, FuncionarioDto> _consultaBase;

        public FuncionarioController(ArmazenadorDeFuncionario armazenadorDeFuncionario, 
            ExclusaoDeFuncionario exclusaoDeFuncionario, 
            VinculadorDeFuncionarioEmpresa vinculadorDeFuncionarioEmpresa, 
            VinculadorDeFuncionarioCargo vinculadorDeFuncionarioCargo, 
            IConsultaBase<Funcionario, FuncionarioDto> consultaBase)
        {
            _armazenadorDeFuncionario = armazenadorDeFuncionario;
            _exclusaoDeFuncionario = exclusaoDeFuncionario;
            _vinculadorDeFuncionarioEmpresa = vinculadorDeFuncionarioEmpresa;
            _vinculadorDeFuncionarioCargo = vinculadorDeFuncionarioCargo;
            _consultaBase = consultaBase;
        }



        /// <summary>
        /// GET api/funcionarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var funcionarios = _consultaBase.Consultar(ListarFuncionarioSpecificationBuilder.Novo()
                .Build());
            return Content(JsonConvert.SerializeObject(funcionarios),
                "application/json");
        }

        /// <summary>
        /// GET api/funcionarios/5
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var funcionario = _consultaBase.Consultar(ListarFuncionarioSpecificationBuilder.Novo()
                .ComId(id)
                .Build())
                .Lista
                .FirstOrDefault();

            return Content(JsonConvert.SerializeObject(funcionario),
                "application/json");
        }

        [HttpGet("pesquisar")]
        public async Task<IActionResult> Get([FromQuery] FuncionarioFiltroDto filtro)
        {
            var funcionarios = _consultaBase.Consultar(ListarFuncionarioSpecificationBuilder.Novo()
                .ComCpf(filtro.Cpf)
                .ComDataContratacaoInicio(filtro.DataContratacaoInicio)
                .ComDataContratacaoFim(filtro.DataContratacaoFim)
                .ComNome(filtro.Nome)
                .Build());

            return Content(JsonConvert.SerializeObject(funcionarios),
                "application/json");
        }


        /// <summary>
        /// POST api/funcionarios
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FuncionarioDto funcionario)
        {
            await _armazenadorDeFuncionario.Armazenar(funcionario);
            return Ok();
        }

        /// <summary>
        /// PUT api/funcionarios/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="funcionario"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] FuncionarioDto funcionario)
        {
            funcionario.Id = id;
            await _armazenadorDeFuncionario.Armazenar(funcionario);
            return Ok();
        }

        /// <summary>
        /// DELETE api/funcionarios/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exclusaoDeFuncionario.Excluir(id);
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
            await _vinculadorDeFuncionarioEmpresa.Vincular(funcionarioId, empresaId);
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
            await _vinculadorDeFuncionarioCargo.Vincular(funcionarioId, cargoId, dataVinculacao);
            return Ok();
        }
    }
}
