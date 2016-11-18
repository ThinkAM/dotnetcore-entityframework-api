using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Data.Abstract;
using System;
using Scheduler.Model;
using Scheduler.API.ViewModels;
using AutoMapper;
using Scheduler.API.Core;
using System.Linq;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Scheduler.API.Controllers
{
    [Route("api/[controller]")]
    public class FieldsController : Controller
    {
        private IFieldRepository _fieldRepository;
        private IWorkItemTypeRepository _workItemTypeRepository;
        private ICmsRepository _cmsRepository;
        int page = 1;
        int pageSize = 4;
        public FieldsController(IFieldRepository fieldRepository,
                                    IWorkItemTypeRepository workItemTypeRepository,
                                    ICmsRepository cmsRepository)
        {
            _fieldRepository = fieldRepository;
            _workItemTypeRepository = workItemTypeRepository;
            _cmsRepository = cmsRepository;
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
            var totalSchedules = _fieldRepository.Count();
            var totalPages = (int)Math.Ceiling((double)totalSchedules / pageSize);

            IEnumerable<Field> _fields = _fieldRepository
                .AllIncluding(s => s.Cms)
                .OrderBy(s => s.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            Response.AddPagination(page, pageSize, totalSchedules, totalPages);

            IEnumerable<FieldViewModel> _fieldsVM = Mapper.Map<IEnumerable<FieldViewModel>>(_fields);

            return new OkObjectResult(_fieldsVM);
        }
        //// GET: api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet("{id}", Name = "GetField")]
        public IActionResult Get(int id)
        {
            Field _field = _fieldRepository
                .GetSingle(s => s.Id == id, s => s.Cms);

            if (_field != null)
            {
                FieldViewModel _fieldVM = Mapper.Map<Field, FieldViewModel>(_field);
                return new OkObjectResult(_fieldVM);
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

        //TODO : TEM NESCESSIDADE DESSE DETALHES ? EU ACHO QUE NÃO, SERIA OS DETALHES DOS FIELDS, MAS AGENTE NAO CONVERSOU SOBRE ISSO
        //[HttpGet("{id}/details", Name = "GetScheduleDetails")]
        //public IActionResult GetScheduleDetails(int id)
        //{
        //    Schedule _schedule = _scheduleRepository
        //        .GetSingle(s => s.Id == id, s => s.Creator, s => s.Attendees);

        //    if (_schedule != null)
        //    {


        //        ScheduleDetailsViewModel _scheduleDetailsVM = Mapper.Map<Schedule, ScheduleDetailsViewModel>(_schedule);

        //        foreach (var attendee in _schedule.Attendees)
        //        {
        //            User _userDb = _userRepository.GetSingle(attendee.UserId);
        //            _scheduleDetailsVM.Attendees.Add(Mapper.Map<User, UserViewModel>(_userDb));
        //        }


        //        return new OkObjectResult(_scheduleDetailsVM);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        [HttpPost]
        public IActionResult Create([FromBody]FieldViewModel field)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Field _newField = Mapper.Map<FieldViewModel, Field>(field);
            _newField.DateCreated = DateTime.Now;

            _fieldRepository.Add(_newField);
            _fieldRepository.Commit();

            field = Mapper.Map<Field, FieldViewModel>(_newField);

            CreatedAtRouteResult result = CreatedAtRoute("GetField", new { controller = "Fields", id = field.Id }, field);
            return result;
        }

        // POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]FieldViewModel field)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Field _fieldDb = _fieldRepository.GetSingle(id);

            if (_fieldDb == null)
            {
                return NotFound();
            }
            else
            {
                _fieldDb.Name= field.Name;
                _fieldDb.Type = field.Type;
                _fieldDb.Description = field.Description;
                _fieldDb.DateCreated = field.DateCreated;
                _fieldDb.DateUpdated = field.DateUpdated;
                _fieldDb.CmsId = field.CmsId;
                _fieldRepository.Commit();
                //_fieldDb.Status = (ScheduleStatus)Enum.Parse(typeof(ScheduleStatus), schedule.Status);
                //_fieldDb.Type = (ScheduleType)Enum.Parse(typeof(ScheduleType), schedule.Type);
                //_fieldDb.TimeStart = schedule.TimeStart;
                //_fieldDb.TimeEnd = schedule.TimeEnd;

                // Remove current attendees

                //TODO : DUVIDA? NO SHEDULES, ELE DELETA OS ATENDIMENTOS, E NO NOSSO ? NESSA PARTE ? ELE DELETA OS WORK ITEM JUNTOS, OU OS CMS QUE TEM ESSE FIELD ? ACHO QUE NAO VAI TER ESSA PARTE, POR VOCE PODE EXCLUIR UM CAMPO SEM ESCLUIR UM TIPO DESSE CAMPO

                //_workItemTypeRepository.DeleteWhere(a => a.CmsId == id);

                //foreach (var cmsId in field.)
                //{
                //    _fieldDb.WorkItemType.Add(new Cms { CmsId = id, UserId = userId });
                //}

                //_scheduleRepository.Commit();
            }

            field = Mapper.Map<Field, FieldViewModel>(_fieldDb);

            return new NoContentResult();
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        [HttpDelete("{id}", Name = "RemoveField")]
        public IActionResult Delete(int id)
        {
            Field _fieldDb = _fieldRepository.GetSingle(id);

            if (_fieldDb == null)
            {
                return new NotFoundResult();
            }
            else
            {
                _fieldRepository.Delete(_fieldDb);

                _fieldRepository.Commit();

                return new NoContentResult();
            }
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
