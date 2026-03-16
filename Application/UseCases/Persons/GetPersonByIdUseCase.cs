using Domain;
using Domain.Abstraction;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Application.UseCases.Persons
{
    public class GetPersonByIdUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;
       
        public GetPersonByIdUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PersonEntity> ExecuteAsync(Guid id)//No need to put ? to make it nullable, because if the code is null, it will throw an exception in the repository layer.
        {
            var person = await _repository.GetByIdAsync(id);
            if (person == null)
            {
                throw new Exception($"Person with ID {id} not found.");
            }
            return person;
        }

    }  
}
