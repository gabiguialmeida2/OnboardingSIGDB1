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
    [Route("api/empresas")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;
        private readonly IEmpresaConsultaService _empresaConsultaService;
        private readonly IEmpresaDeleteService _empresaDeleteService;
        
        private readonly IMapper _mapper;

        public EmpresaController(IEmpresaService empresaService, 
            IEmpresaConsultaService empresaConsultaService, 
            IEmpresaDeleteService empresaDeleteService, 
            IMapper mapper)
        {
            _empresaService = empresaService;
            _empresaConsultaService = empresaConsultaService;
            _empresaDeleteService = empresaDeleteService;
            _mapper = mapper;
        }


        /// <summary>
        /// GET api/empresas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var empresas = await _empresaConsultaService.GetAll();
            return Content(JsonConvert.SerializeObject(_mapper.Map<IEnumerable<EmpresaDto>>(empresas)),
                "application/json"); 
        }

        /// <summary>
        /// GET api/empresas/5
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var empresa = await _empresaConsultaService.GetById(id);
            return Content(JsonConvert.SerializeObject(_mapper.Map<EmpresaDto>(empresa)),
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
            var empresas = await _empresaConsultaService.GetFiltro(filtro);
            return Content(JsonConvert.SerializeObject(_mapper.Map<IEnumerable<EmpresaDto>>(empresas)),
                "application/json");
        }


        /// <summary>
        /// POST api/empresas
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmpresaInsertDto empresa)
        {
            var entity = _mapper.Map<Empresa>(empresa);
            await _empresaService.InsertEmpresa(entity);
            return Ok();
        }

        /// <summary>
        /// PUT api/empresas/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="empresa"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] EmpresaUpdateDto empresa)
        {
            var entity = _mapper.Map<Empresa>(empresa);
            await _empresaService.UpdateEmpresa(id, entity);
            return Ok();
        }

        /// <summary>
        /// DELETE api/empresas/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _empresaDeleteService.Delete(id);
            return Ok();
        }
    }
}
