using Api.Features.Authentication.Entities;
using Api.Features.BaseRepository;
using Api.Features.BaseRepository.Interfaces;
using Api.Features.Users.Models;
using Api.Features.Users.Services;
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

namespace Api.Features.Users
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly UnitOfWork _unitOfWork;

        public UserController(
            IUserService userService,
            IMapper mapper,
            ISieveProcessor sieveProcessor,
            IUnitOfWork unitOfWork
            )
        {
            _userService = userService;
            _mapper = mapper;
            _sieveProcessor = sieveProcessor;
            _unitOfWork = unitOfWork as UnitOfWork;
        }

        [Authorize]
        [HttpGet()]
        public IActionResult GetUsers(SieveModel sieveModel)
        {
            var users = _userService.GetAll().AsNoTracking();

            if (users == null)
            {
                return NotFound();
            }
            Response.Headers.Add("X-Total-Count", users.Count().ToString());

            users = _sieveProcessor.Apply(sieveModel, users);

            var result = _mapper.Map<List<UserModel>>(users.ToList());

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound(ModelState);
                }

                var user = _mapper.Map<ApplicationUser>(userModel);

                _userService.Add(user);

                await _unitOfWork.Commit();

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = _mapper.Map<ApplicationUser>(userModel);

                _unitOfWork.Users.Update(user);

                await _unitOfWork.Commit();

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = _userService.Get(id);

                if (user == null)
                {
                    return NotFound();
                }

                _userService.Delete(user);

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
