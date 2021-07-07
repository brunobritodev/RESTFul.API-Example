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
    [Route("companies")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class CompanyController : ApiBaseController
    {
        private readonly IDummyUserService _dummyUserService;
        private readonly IMapper _mapper;

        public CompanyController(
            INotificationHandler<DomainNotification> notifications,
            IDomainNotificationMediatorService mediator,
            IDummyUserService dummyUserService,
            IMapper mapper) : base(notifications, mediator)
        {
            _dummyUserService = dummyUserService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all companies.
        /// </summary>
        /// <returns>List of <see cref="Applicant"/></returns>
        [HttpGet("")]
        public async Task<ActionResult<List<Company>>> Get()
        {
            var result = await _dummyUserService.GetCompanies();

            return ResponseGet(result);
        }

        /// <summary>
        /// Get details of a company
        /// </summary>
        [HttpGet("{company}")]
        public async Task<ActionResult<Company>> GetCompany(string company)
        {
            var result = await _dummyUserService.FindCompany(company);
            return ResponseGet(result);
        }

        /// <summary>
        /// Get all aplicants from a company
        /// </summary>
        [HttpGet("{company}/applicants")]
        public async Task<ActionResult<List<ApplicantViewModel>>> GetCompanyApplicants(string company)
        {
            var result = await _dummyUserService.GetCompanyUsers(company);
            return ResponseGet(_mapper.Map<List<ApplicantViewModel>>(result));
        }
    }
}
