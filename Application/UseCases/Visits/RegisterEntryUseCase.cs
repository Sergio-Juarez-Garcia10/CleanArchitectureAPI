using Application.DTOs.Visits;
using Domain;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Visits
{
    public class RegisterEntryUseCase
    {

        private readonly IRepository<VisitEntity, Guid> _repository;
        private readonly IVisitRepository<VisitEntity> _visitRepository;

        private readonly ICodeRepository<PersonEntity> _codeRepository;
        public RegisterEntryUseCase(IRepository<VisitEntity, 
            Guid> repository,IVisitRepository<VisitEntity> visitRepository, 
            ICodeRepository<PersonEntity> codeRepository)
        {
            _repository = repository;
            _visitRepository = visitRepository;
            _codeRepository = codeRepository;
        }

        public async Task<VisitEntity> ExecuteAsync(RegisterEntryDto dto)
        {

            Guid personId;
            if (dto.PersonId.HasValue)
            {
                personId = dto.PersonId.Value;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                var person = await _codeRepository.GetByCodeAsync(dto.Code ?? 
                    throw new ArgumentNullException($"No se encontro una persona con el codigo:{dto.Code}."));
                personId = person.Id;
            }
            else
            {
                throw new ArgumentNullException("Debe proporcionar PersonId o Code para registrar entrada.");
            }

            if (await _visitRepository.HasActiveVisitAsync(personId))
            {
                throw new InvalidOperationException("Esta persona ya tiene una visita activa.");
            }

            var visit = new VisitEntity(personId, dto.EntryTime);

            await _repository.AddAsync(visit);
            await _repository.SaveChangesAsync();

            return await _repository.GetByIdAsync(visit.Id) 
                ?? throw new InvalidOperationException("Error al recupera la visita");    
        }
    }
}
