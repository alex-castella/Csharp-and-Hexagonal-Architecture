using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain
{
    public class PersonEntity
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        //Metodos de utilidad
        public string FullName => $"{FirstName} {LastName}";

        public PersonEntity(string code, string FirstName, string LastName, string Email, string PhoneNumber)
        {
            ValidateCode(code);
            ValidateFirstName(FirstName);
            ValidateLastName(LastName);
            ValidateEmail(Email);
            ValidatePhoneNumber(PhoneNumber);

            Id = Guid.NewGuid();
            code = code.Trim().ToUpper();
            FirstName = FirstName.Trim();
            LastName = LastName.Trim();
            Email = Email.Trim().ToLower();
            PhoneNumber = PhoneNumber.Trim();
        }

        //Método para setear ya que los set son privados
        public void UpdatePersonalInfo(string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateFirstName(FirstName);
            ValidateLastName(LastName);
            ValidateEmail(Email);
            ValidatePhoneNumber(PhoneNumber);

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLower();
            PhoneNumber = phoneNumber.Trim();
        }

        private void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El codino no puede estar vacío.", nameof(code));
            if (code.Trim().Length < 3)
                throw new ArgumentException("El código debe tener al menos 3 caracteres", nameof(code));
            if (code.Trim().Length > 20)
                throw new ArgumentException("El código no puede exceder 20 caracteres", nameof(code));
        }

        private void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("El nombre no puede estar vacío", nameof(firstName));
            if (firstName.Trim().Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 2 caracteres", nameof(firstName));
            if (firstName.Trim().Length > 50)
                throw new ArgumentException("El nombre no puede exceder 50 caracteres.", nameof(firstName));
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("El apellido no puede estar vacío.", nameof(lastName));
            if (lastName.Trim().Length < 2)
                throw new ArgumentException("El apellido debe tener al menos 2 caracteres.", nameof(lastName));
            if (LastName.Trim().Length > 50)
                throw new ArgumentException("El apellido no puede exceder 50 caracteres.", nameof(LastName));
        }
        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo no puede estar vacío.", nameof(email));

            if (email.Trim().Length > 100)
                throw new ArgumentException("El correo no puede exceder 100 caracteres.", nameof(email));

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(emailPattern, email))
                throw new ArgumentException("El formato del correo es inválido", nameof(email));

        }
        private void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("El número de teléfono no puede estar vacío.", nameof(phoneNumber));
            if (phoneNumber.Trim().Length < 7)
                throw new ArgumentException("El número de teléfono debe tener al menos 7 caracteres.", nameof(phoneNumber));
            if (PhoneNumber.Trim().Length > 15)
                throw new ArgumentException("El número de teléfono no puede exceder 15 caracteres", nameof(PhoneNumber));
        }
    }
}