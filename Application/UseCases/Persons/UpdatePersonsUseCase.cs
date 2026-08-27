using Application.DTOs.Persons;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Persons
{
    public class UpdatePersonsUseCase
    {

        private readonly IRepository<PersonEntity, Guid> _repository;
        
        public UpdatePersonsUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PersonEntity> ExecuteAsync(UpdatePersonDTO dTO)
        {
            var person = await _repository.GetByIdAsync(dTO.Id);

            if (person == null)
            {
                throw new InvalidOperationException($"No se encontro una perona");
            }

            person.UpdatePersonalInfo(dTO.FirtsName,dTO.LastName, dTO.Email, dTO.PhoneNumber);

            await _repository.UpdateAsync(person);
            await _repository.SaveChangesAsync();

            return person;
        }
    }
}
