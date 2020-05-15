using Api.Features.BaseRepository;
using Api.Features.BaseRepository.Interfaces;
using Api.Features.Tasks.Entities;
using Api.Features.Tasks.Models;
using Api.Features.Tasks.Services;
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

namespace Api.Features.Tasks
{
    [Route("api/tasks")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly UnitOfWork _unitOfWork;

        public TaskController(
            ITaskService taskService,
            IMapper mapper,
            ISieveProcessor sieveProcessor,
            IUnitOfWork unitOfWork
            )
        {
            _taskService = taskService;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        [Authorize]
        [HttpGet()]
        public IActionResult GetTasks(SieveModel sieveModel)
        {
            var tasks = _taskService.GetAll().AsNoTracking();

            if (tasks == null)
            {
                return NotFound();
            }
            Response.Headers.Add("X-Total-Count", tasks.Count().ToString());

            tasks = _sieveProcessor.Apply(sieveModel, tasks);

            return Ok(tasks);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskModel taskModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound(ModelState);
                }

                var task = _mapper.Map<Entities.Task>(taskModel);

                _taskService.Add(task);

                await _unitOfWork.Commit();

                return Ok(task);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModel taskModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var task = _mapper.Map<Entities.Task>(taskModel);

                _unitOfWork.Tasks.Update(task);

                await _unitOfWork.Commit();

                return Ok(task);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            try
            {
                var task = _taskService.Get(id);

                if (task == null)
                {
                    return NotFound();
                }

                _taskService.Delete(task);

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
