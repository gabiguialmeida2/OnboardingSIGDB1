using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnboardingSIGDB1.Domain._Base;
using OnboardingSIGDB1.Domain.Empresas;
using OnboardingSIGDB1.Domain.Empresas.Dtos;
using OnboardingSIGDB1.Domain.Empresas.Services;
using OnboardingSIGDB1.Domain.Empresas.Specifications;

namespace OnboardingSIGDB1.API.Controllers
{
    [Route("api/empresas")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
      
        private readonly IConsultaBase<Empresa, EmpresaDto> _consultaBase;
        private readonly ArmazenadorDeEmpresa _armazenadorDeEmpresa;
        private readonly ExclusaoDeEmpresa _exclusaoDeEmpresa;

        private readonly IMapper _mapper;

        public EmpresaController(
            IMapper mapper,
            IConsultaBase<Empresa, EmpresaDto> consultaBase,
            ArmazenadorDeEmpresa armazenadorDeEmpresa,
            ExclusaoDeEmpresa exclusaoDeEmpresa)
        {
            _mapper = mapper;
            _consultaBase = consultaBase;
            _armazenadorDeEmpresa = armazenadorDeEmpresa;
            _exclusaoDeEmpresa = exclusaoDeEmpresa;
        }


        /// <summary>
        /// GET api/empresas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var empresa = _consultaBase
               .Consultar(ListarEmpresaSpecificationBuilder.Novo()
               .Build());

            return Content(JsonConvert.SerializeObject(empresa),
                "application/json");
        }

        /// <summary>
        /// GET api/empresas/5
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var empresa = _consultaBase
                .Consultar(ListarEmpresaSpecificationBuilder.Novo()
                .ComId(id)
                .Build())
                .Lista
                .FirstOrDefault();

            return Content(JsonConvert.SerializeObject(empresa),
                "application/json");
        }

        /// <summary>
        /// GET api/empresas/pesquisar
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpGet("pesquisar")]
        public async Task<IActionResult> Get([FromQuery] EmpresaFiltroDto filtro)
        {
            var empresas = _consultaBase
                 .Consultar(ListarEmpresaSpecificationBuilder.Novo()
                 .ComCnpj(filtro.Cnpj)
                 .ComDataFundacaoInicio(filtro.DataFundacaoInicio)
                 .ComDataFundacaoFim(filtro.DataFundacaoFim)
                 .ComNome(filtro.Nome)
                 .Build());

            return Content(JsonConvert.SerializeObject(empresas),
                "application/json");
        }


        /// <summary>
        /// POST api/empresas
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmpresaDto empresa)
        {
            await _armazenadorDeEmpresa.Armazenar(empresa);
            return Ok();
        }

        /// <summary>
        /// PUT api/empresas/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="empresa"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] EmpresaDto empresa)
        {
            empresa.Id = id;
            await _armazenadorDeEmpresa.Armazenar(empresa);
            return Ok();
        }

        /// <summary>
        /// DELETE api/empresas/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exclusaoDeEmpresa.Excluir(id);
            return Ok();
        }
    }
}
