using Data.Persistence;
using Domain.Abstraction;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories
{
    public class PersonRepository : IRepository<PersonEntity, Guid>,
        ICodeRepository<PersonEntity> //Implementa tanto la interfaz genérica de repositorio como la interfaz específica para el código
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context) //Inyectamos el contexto ya creado de la base de datos a través del constructor
        {
            _context = context;
        }

        public async Task<PersonEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == id); //Busca el primer registro que coincida con el id proporcionado, o devuelve null si no se encuentra
        }

        public async Task<IEnumerable<PersonEntity>> GetAllAsync()
        {
            return await _context.Persons
                .AsNoTracking() //Indica que no se realizará seguimiento de cambios en las entidades recuperadas, lo que mejora el rendimiento para consultas de solo lectura
                .OrderBy(p => p.FirstName) //Ordena los resultados por el nombre
                .ThenBy(p => p.LastName) // Ordena por el apellido
                .ToListAsync();
        }

        public async Task AddAsync(PersonEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity)); //Lanza una excepción si la entidad es nula

            await _context.Persons.AddAsync(entity); //Agrega la entidad al contexto

        }

        public Task UpdateAsync(PersonEntity entity) // El método de actualización no es asíncrono porque la actualización se realiza en memoria,
                                                     // y los cambios se guardarán en la base de datos cuando se llame a SaveChangesAsync en el contexto
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity)); //Lanza una excepción si la entidad es nula

            _context.Persons.Update(entity); //Marca la entidad como modificada en el contexto
                                             // Este método no es asíncrono porque la actualización se realiza en memoria,
                                             // y los cambios se guardarán en la base de datos cuando se llame a SaveChangesAsync en el contexto

            return Task.CompletedTask; //Devuelve una tarea completada para cumplir con la firma asíncrona del método

        }

        public Task DeleteAsync(PersonEntity entity) // El método de eliminación no es asíncrono porque la eliminación se realiza en memoria,
                                                     // y los cambios se guardarán en la base de datos cuando se llame a SaveChangesAsync en el contexto
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity)); //Lanza una excepción si la entidad es nula
            _context.Persons.Remove(entity); //Marca la entidad para eliminación en el contexto
            return Task.CompletedTask; //Devuelve una tarea completada para cumplir con la firma asíncrona del método
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync(); //Guarda los cambios realizados en el contexto en la base de datos y devuelve el número de registros afectados
        }


        //Implementación del método específico para obtener una persona por su código ICodeRepository
        public async Task<PersonEntity?> GetByCodeAsync(string code)// ? Puede ser null
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El código no puede ser nulo o vacío.", nameof(code)); //Lanza una excepción si el código es nulo o vacío

            var normalizedCode = code.Trim().ToUpperInvariant(); //Normaliza el código eliminando espacios y convirtiéndolo a mayúsculas para una comparación consistente

            return await _context.Persons
                .FirstOrDefaultAsync(p => p.Code == normalizedCode); //Busca el primer registro que coincida con el código proporcionado, o devuelve null si no se encuentra

        }

        public async Task<bool> ExistsWithCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El código no puede ser nulo o vacío.", nameof(code)); //Lanza una excepción si el código es nulo o vacío

            var normalizedCode = code.Trim().ToUpperInvariant(); //Normaliza el código eliminando espacios y convirtiéndolo a mayúsculas para una comparación consistente
            return await _context.Persons
                .AnyAsync(p => p.Code == normalizedCode); //Verifica si existe algún registro con el código proporcionado y devuelve true o false
        }
    }
}
