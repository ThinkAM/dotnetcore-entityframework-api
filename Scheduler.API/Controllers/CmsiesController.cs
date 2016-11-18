using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Abstract;
using Scheduler.Model;
using Scheduler.API.ViewModels;
using AutoMapper;
using Scheduler.API.Core;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.API.Controllers
{
    [Route("api/[controller]")]
    public class CmsiesController : Controller
    {
        ICmsRepository _cmsRepository;
        IFieldRepository _fieldRepository;

        int page = 1;
        int pageSize = 10;

        public CmsiesController(ICmsRepository cmsRepository, 
                                IFieldRepository fieldRepository)
        {
            _cmsRepository = cmsRepository;
            _fieldRepository = fieldRepository;
        }

        public IActionResult Get()
       {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out page);
                int.TryParse(vals[1], out pageSize);
            }

            int currentPage = page;
            int currentPageSize = pageSize;
            var totalCmsies = _cmsRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalCmsies / pageSize);

            IEnumerable<Cms> _cmss = _cmsRepository
                .AllIncluding(u => u.Fields, prop => prop.WorkItemType)
                .OrderBy(u => u.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            IEnumerable<CmsViewModel> _cmssVM = Mapper.Map<IEnumerable<Cms>, IEnumerable<CmsViewModel>>(_cmss);

            //TODO : Criar o View Model do CMS
            Response.AddPagination(page, pageSize, totalCmsies, totalPages);

            return new OkObjectResult(_cmssVM);
        }


        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet("{id}", Name = "GetCmsies")]
        public IActionResult Get(int id)
        {
            Cms _cms = _cmsRepository.GetSingle(u => u.Id == id, u => u.Fields);

            if (_cms != null)
            {
                CmsViewModel _cmsVM = Mapper.Map<Cms, CmsViewModel>(_cms);
                return new OkObjectResult(_cmsVM);
            }
            else
            {
                return NotFound();
            }
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpGet("{id}/fields", Name = "GetCmsFields")]
        public IActionResult GetFields(int id)
        {
            IEnumerable<Field> _cmsiesFields = _fieldRepository.FindBy(s => s.CmsId == id);

            if (_cmsiesFields != null)
            {
                //TODO : VIEW MODEL
                IEnumerable<FieldViewModel> _cmsiesFieldsVM = Mapper.Map<IEnumerable<FieldViewModel>>(_cmsiesFields);
                return new OkObjectResult(_cmsiesFieldsVM);
            }
            else
            {
                return NotFound();
            }
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        [HttpPost]
        //TODO : VIEW MODEL
        public IActionResult Create([FromBody]CmsViewModel cms)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cms _newCms = new Cms { Titulo = cms.Titulo, Avatar = cms.Avatar, QtdFields = cms.QtdFields, WorkItemTypeId = cms.WorkItemTypeId, FieldsCreated = cms.FieldsCreated};

            _cmsRepository.Add(_newCms);
            _cmsRepository.Commit();
            //TODO : VIEW MODEL
            //cms = Mapper.Map<Cms, CmsViewModel>(_newCms);

            CreatedAtRouteResult result = CreatedAtRoute("GetCmsies", new { controller = "Cmsies", id = _newCms.Id }, cms);
            return result;
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        [HttpPut("{id}")]
        //TODO : VIEW MODEL
        public IActionResult Put(int id, [FromBody]CmsViewModel cms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cms _cmsDb = _cmsRepository.GetSingle(id);

            if (_cmsDb == null)
            {
                return NotFound();
            }
            else
            {
                //TODO : AJUSTAR
                _cmsDb.Titulo = cms.Titulo;
                _cmsDb.Avatar = cms.Avatar;
                _cmsDb.QtdFields = cms.QtdFields;
                _cmsDb.FieldsCreated = cms.FieldsCreated;
                _cmsDb.WorkItemTypeId = cms.WorkItemTypeId;
                _cmsRepository.Commit();
            }

            //cms = Mapper.Map<Cms, CmsViewModel>(_cmsDb);

            CreatedAtRouteResult result = CreatedAtRoute("GetCmsies", new { controller = "Cmsies", _cmsDb.Id}, cms);
            return result;
           // return new NoContentResult();
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Cms _cmsDb = _cmsRepository.GetSingle(id);

            if (_cmsDb == null)
            {
                return new NotFoundResult();
            }
            else
            {
                IEnumerable<Field> _fields = _fieldRepository.FindBy(a => a.CmsId == id);

                foreach (var field in _fields)
                {
                    _fieldRepository.DeleteWhere(a => a.CmsId == field.Id);
                    _fieldRepository.Delete(field);
                }

                _cmsRepository.Delete(_cmsDb);

                _cmsRepository.Commit();

                return new NoContentResult();
            }
        }
    }
}
