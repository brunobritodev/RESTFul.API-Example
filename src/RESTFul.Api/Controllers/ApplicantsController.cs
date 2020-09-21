using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Filter;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTFul.Api.Commands;
using RESTFul.Api.Models;
using RESTFul.Api.Notification;
using RESTFul.Api.Service.Interfaces;
using RESTFul.Api.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTFul.Api.Controllers
{
    [Route("applicants")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ApplicantsController : ApiBaseController
    {
        private readonly IDummyUserService _dummyUserService;
        private readonly IMapper _mapper;

        public ApplicantsController(
            INotificationHandler<DomainNotification> notifications,
            IDomainNotificationMediatorService mediator,
            IDummyUserService dummyUserService,
            IMapper mapper) : base(notifications, mediator)
        {
            _dummyUserService = dummyUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all applicants, optionally filter them.
        /// </summary>
        /// <returns>List of <see cref="Applicant"/></returns>
        [HttpGet("")]
        public async Task<ActionResult<List<ApplicantViewModel>>> Get([FromQuery] ApplicantSearch search)
        {
            var result = _dummyUserService.Query().Apply(search);

            return ResponseGet(await _mapper.ProjectTo<ApplicantViewModel>(result).ToListAsync());
        }


        /// <summary>
        /// Get all olders applicants, which is actually declined.
        /// </summary>
        /// <returns>List of <see cref="Applicant"/></returns>
        [HttpGet("youngers-from-brazil")]
        public async Task<ActionResult<List<ApplicantViewModel>>> GetYoungersFromBrazil()
        {
            var search = new ApplicantSearch() { OlderThan = 25, Country = "Brazil"};
            var expression = _dummyUserService.Query().FilterExpression(search);
            var result = _dummyUserService.Query().Apply(search);

            return ResponseGet(await _mapper.ProjectTo<ApplicantViewModel>(result).ToListAsync());
        }

        /// <summary>
        /// Get the specified Applicant
        /// </summary>
        /// <param name="username">username of Applicant</param>
        /// <returns><see cref="Applicant"/></returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<Applicant>> GetByUsername(string username)
        {
            return ResponseGet(await _dummyUserService.Find(username).ConfigureAwait(false));
        }

        /// <summary>
        /// Create new Applicant
        /// </summary>
        /// <param name="command"><see cref="RegisterApplicantCommand"/></param>
        /// <returns><see cref="Applicant"/></returns>
        [HttpPost("")]
        public async Task<ActionResult<Applicant>> Post([FromBody] RegisterApplicantCommand command)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            await _dummyUserService.Save(command).ConfigureAwait(false);
            var newUser = await _dummyUserService.Find(command.Username).ConfigureAwait(false);
            return ResponsePost(nameof(GetByUsername), new { username = command.Username }, newUser);
        }

        /// <summary>
        /// Partially update an Applicant
        /// </summary>
        [HttpPatch("{username}")]
        public async Task<ActionResult> Patch(string username, [FromBody] JsonPatchDocument<Applicant> model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var actualUser = await _dummyUserService.Find(username).ConfigureAwait(false);
            model.ApplyTo(actualUser);
            await _dummyUserService.Update(actualUser).ConfigureAwait(false);
            return ResponsePutPatch();
        }

        /// <summary>
        /// Update an Applicant
        /// </summary>
        [HttpPut("{username}")]
        public async Task<ActionResult> Put(string username, [FromBody] UpdateApplicantCommand model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            var actual = await _dummyUserService.Find(username).ConfigureAwait(false);
            model.Update(actual);
            await _dummyUserService.Update(actual).ConfigureAwait(false);
            return ResponsePutPatch();
        }

        /// <summary>
        /// Remove an Applicant
        /// </summary>
        [HttpDelete("{username}")]
        public async Task<ActionResult<Applicant>> Delete(string username)
        {
            var actual = await _dummyUserService.Find(username);
            if (actual == null)
            {
                return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>() { { "username", new[] { "Applicant not found" } } }));
            }

            return ResponseDelete(await _dummyUserService.Remove(username));
        }

        [HttpPut("{username}/approve")]
        public async Task<ActionResult<Applicant>> Approve(string username)
        {
            await _dummyUserService.Approve(username);

            return ResponsePutPatch();
        }

        [HttpPut("{username}/decline")]
        public async Task<ActionResult<Applicant>> Decline(string username)
        {
            await _dummyUserService.Decline(username);

            return ResponsePutPatch();
        }

        [HttpPost("{username}/transfer/{company}")]
        public async Task<ActionResult<Applicant>> Transfer([FromBody] TransferApplicantCommand model)
        {
            var newApplicant = await _dummyUserService.Transfer(model);

            return ResponsePost(nameof(Get), new { id = newApplicant.Username }, newApplicant);
        }

    }


}
