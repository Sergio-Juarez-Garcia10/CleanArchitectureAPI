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
    public class RegisterExitUseCase
    {
        private readonly IRepository<VisitEntity, Guid> _repository;
        private readonly IVisitRepository<VisitEntity> _visitRepository;

        public RegisterExitUseCase(
            IRepository<VisitEntity, Guid> repository,
            IVisitRepository<VisitEntity> visitRepository)
        {
            _repository = repository;
            _visitRepository = visitRepository;
        }

        public async Task<VisitEntity> ExecuteAsync(RegisterExitDTO dto)
        {
            VisitEntity? visit;

            if (dto.VisitId.HasValue)
            {
                visit = await _repository.GetByIdAsync(dto.VisitId.Value);

                if (visit == null)
                {
                    throw new InvalidOperationException($"No se encontró una visita con el ID: {dto.VisitId}");
                }
            }
            else if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                visit = await _visitRepository.GetActiveVisitByPersonCodeAsync(dto.Code);

                if (visit == null)
                {
                    throw new InvalidOperationException($"No se encontró una visita activa para la persona con código: {dto.Code}");
                }
            }
            else
            {
                throw new InvalidOperationException("Debe proporcionar VisitId o Código para registrar la salida");
            }

            visit.RegisterExit(dto.ExitTime);

            await _repository.UpdateAsync(visit);
            await _repository.SaveChangesAsync();

            return visit;

        }
    }
}
