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
    public class CreatePersonsUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;
        private readonly ICodeRepository<PersonEntity> _codeRepository;


        public CreatePersonsUseCase(IRepository<PersonEntity, Guid> repository, ICodeRepository<PersonEntity> codeRepository)
        {
            _repository = repository;
            _codeRepository = codeRepository;
        }

        public async Task<PersonEntity> ExecuteAsync(CreatePersonDTO dto)
        {
            if (await _codeRepository.ExistsWithCodeAsync(dto.Code))
            {
                throw new InvalidOperationException($"El codigo ya existe en el sistema");
            }

            var person = new PersonEntity(
                dto.Code, dto.FirtsName, dto.LastName, dto.Email, dto.PhoneNumber);

            await _repository.AddAsync(person);
            await _repository.SaveChangesAsync();

            return person;
        }
    }
}
