using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Persons
{
    public class GetAllPersonsUseCases
    {
        private readonly IRepository<PersonEntity, Guid> _personRepository;

        public GetAllPersonsUseCases(IRepository<PersonEntity, Guid> repository)
        {
            _personRepository = repository;
        }

        public async Task<IEnumerable<PersonEntity>> ExecuteAsync()
        {
            return await _personRepository.GetAllAsync();
        }
    }
}
