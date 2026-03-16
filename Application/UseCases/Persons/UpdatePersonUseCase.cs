using Application.DTOs.Persons;
using Domain;
using Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Persons
{
    public class UpdatePersonUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;

        public UpdatePersonUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<PersonEntity> ExecuteAsync(UpdatePersonDto dto)
        {
            var existingPerson = await _repository.GetByIdAsync(dto.Id);
            if (existingPerson == null)
            {
                throw new InvalidOperationException($"Person with ID {dto.Id} not found.");
            }
            // Update the properties of the existing person with mehod in the entity
            existingPerson.UpdatePersonalInfo(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);

            await _repository.UpdateAsync(existingPerson);
            await _repository.SaveChangesAsync();

            return existingPerson;

        }
    }
}
