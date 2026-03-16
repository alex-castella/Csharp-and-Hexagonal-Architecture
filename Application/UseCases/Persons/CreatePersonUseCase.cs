using Application.DTOs.Persons;
using Domain;
using Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Persons
{
    public class CreatePersonUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;
        private readonly ICodeRepository<PersonEntity> _codeRepository;

        public CreatePersonUseCase(IRepository<PersonEntity, Guid> repository, ICodeRepository<PersonEntity> codeRepository)
        {
            _repository = repository;
            _codeRepository = codeRepository;
        }

        public async Task<PersonEntity> ExecuteAsync(CreatePersonDto dto)
        {
            // Check if a person with the same code already exists
            if (await _codeRepository.ExistsWithCodeAsync(dto.Code))
            {
                throw new InvalidOperationException("A person with the same code already exists.");
            }

            //Turn dto into entity

            var person = new PersonEntity(
                dto.Code,
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.PhoneNumber);

            await _repository.AddAsync(person);
            await _repository.SaveChangesAsync();

            return person;

        }    

    }
}
