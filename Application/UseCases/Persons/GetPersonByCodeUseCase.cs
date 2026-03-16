using Domain;
using Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UseCases.Persons
{
    

    public class GetPersonByCodeUseCase
    {
        private readonly ICodeRepository<PersonEntity> _codeRepository;

        public GetPersonByCodeUseCase(ICodeRepository<PersonEntity> codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public async Task<PersonEntity> ExecuteAsync(string code) //No need to put ? to make it nullable, because if the code is null, it will throw an exception in the repository layer.
        {
            var person = await _codeRepository.GetByCodeAsync(code);
            if (person == null)
            {
                throw new InvalidOperationException($"Person with code {code} not found.");
            }
            return person;
        }
    }
}
