using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Persons
{
    public class GetPersonByIdUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;

        public GetPersonByIdUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PersonEntity> EntityAsync(Guid id)
        {
            var person = await _repository.GetByIdAsync(id);

            if (person == null)
            {
                throw new InvalidOperationException($"No se encontro una perona con el id:{id}");
            }

            return person;
        }
    }
}
