using Microsoft.EntityFrameworkCore;
using RESTFul.Api.Commands;
using RESTFul.Api.Contexts;
using RESTFul.Api.Models;
using RESTFul.Api.Notification;
using RESTFul.Api.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTFul.Api.Service
{
    public class DummyUserService : IDummyUserService
    {
        private readonly IDomainNotificationMediatorService _domainNotification;
        private readonly RestfulContext _context;

        public DummyUserService(IDomainNotificationMediatorService domainNotification,
            RestfulContext context)
        {
            _domainNotification = domainNotification;
            _context = context;
        }
        private async Task CheckApplicants()
        {
            if (_context.Applicants.Any())
                return;

            var companies = Company.Get(250).Generate(2);

            await _context.Companies.AddRangeAsync(companies);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Applicant> Query()
        {
            CheckApplicants().Wait();
            return _context.Applicants.AsQueryable();
        }

        public async Task<IEnumerable<Applicant>> All()
        {
            await CheckApplicants();
            return await _context.Applicants.ToListAsync();
        }

        public async Task Save(RegisterApplicantCommand command)
        {
            var user = command.ToEntity();
            if ((await CheckIfUserIsValid(user)))
                return;

            await _context.Applicants.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Applicant applicant)
        {
            if ((await CheckIfUserIsValid(applicant)))
                return;

            var actua = await Find(applicant.Username);
            _context.Applicants.Update(actua);

            await _context.SaveChangesAsync();
        }

        public async Task<Applicant> Remove(string username)
        {
            var actual = await Find(username);
            if (actual != null)
            {
                actual.Delete();
                _context.Applicants.Update(actual);
            }
            else
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
            }
            await _context.SaveChangesAsync();
            return actual;
        }

        public async Task Approve(string username)
        {
            var applicant = await Find(username);
            if (applicant == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return;
            }

            applicant.Approve();
            await _context.SaveChangesAsync();
        }

        public async Task Decline(string username)
        {
            var applicant = await Find(username);
            if (applicant == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return;
            }

            applicant.Decline();
            await _context.SaveChangesAsync();
        }

        public async Task<Applicant> Transfer(TransferApplicantCommand command)
        {
            var applicant = await Find(command.Username);
            if (applicant == null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Not found"));
                return null;
            }
            if (command.Company == applicant.CompanyId)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Can't transfer for same company"));
                return null;
            }
            if (command.Company <= 0)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid company"));
                return null;
            }

            applicant.Delete();
            var newApplicant = new Applicant(applicant, command.Company);
            await _context.AddAsync(newApplicant);
            await _context.SaveChangesAsync();
            return newApplicant;
        }


        private async Task<bool> CheckIfUserIsValid(Applicant command)
        {
            var valid = true;
            if (string.IsNullOrEmpty(command.FirstName))
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid firstname"));
                valid = false;
            }

            if (string.IsNullOrEmpty(command.LastName))
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid firstname"));
                valid = false;
            }

            if (command.CompanyId <= 0)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Invalid company"));
                valid = false;
            }
            if ((await FindCompany(command.CompanyId)) != null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Username already exists"));
                valid = false;
            }

            if ((await Find(command.Username)) != null)
            {
                _domainNotification.Notify(new DomainNotification("Applicant", "Username already exists"));
                valid = false;
            }

            return valid;
        }

        private Task<Company> FindCompany(int companyId)
        {
            return _context.Companies.FirstOrDefaultAsync(f => f.Id == companyId);
        }

        public Task<Applicant> Find(string username)
        {
            return _context.Applicants.Include(i => i.Company).FirstOrDefaultAsync(f => f.Username == username);

        }
    }
}
