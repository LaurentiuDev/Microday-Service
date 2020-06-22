using Api.Features.BaseRepository;
using Api.Features.BaseRepository.Interfaces;
using Api.Features.SubTasks.Models;
using Api.Features.SubTasks.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Features.SubTasks
{
    [Route("api/sub-tasks")]
    public class SubTaskController : Controller
    {
        private readonly ISubTaskService _subTaskService;
        private readonly IMapper _mapper;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly UnitOfWork _unitOfWork;

        public SubTaskController(
            ISubTaskService subTaskService,
            IMapper mapper,
            ISieveProcessor sieveProcessor,
            IUnitOfWork unitOfWork
            )
        {
            _subTaskService = subTaskService;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        [Authorize]
        [HttpGet()]
        public IActionResult GetSubTasks(SieveModel sieveModel)
        {
            var subTasks = _subTaskService.GetAll().AsNoTracking();

            if (subTasks == null)
            {
                return NotFound();
            }
            Response.Headers.Add("X-Total-Count", subTasks.Count().ToString());

            subTasks = _sieveProcessor.Apply(sieveModel, subTasks);

            return Ok(subTasks);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSubTask([FromBody] SubTaskModel subTaskModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound(ModelState);
                }

                var subTask = _mapper.Map<Entities.SubTask>(subTaskModel);

                _subTaskService.Add(subTask);

                await _unitOfWork.Commit();

                return Ok(subTask);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubTask([FromBody] SubTaskModel subTaskModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var subTask = _mapper.Map<Entities.SubTask>(subTaskModel);

                _unitOfWork.SubTasks.Update(subTask);

                await _unitOfWork.Commit();

                return Ok(subTask);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubTask(Guid id)
        {
            try
            {
                var subTask = _subTaskService.Get(id);

                if (subTask == null)
                {
                    return NotFound();
                }

                _subTaskService.Delete(subTask);

                await _unitOfWork.Commit();

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
