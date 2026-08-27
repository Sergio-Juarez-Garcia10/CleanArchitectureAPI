using System.Text.RegularExpressions;

namespace Domain
{
    public class PersonEntity
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set;} = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public PersonEntity(string code, string firstName, string lastName, string email, string phoneNumber) 
        {

            ValidateCode(code);
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            ValidatePhoneNumber(phoneNumber);

            Id= Guid.NewGuid();
            Code = code.Trim().ToUpper();
            FirstName= firstName.Trim();
            LastName= lastName.Trim();
            Email= email.Trim();
            PhoneNumber= phoneNumber.Trim().ToLower();

        }

        public void UpdatePersonalInfo(string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            ValidatePhoneNumber(phoneNumber);
            
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email= email.Trim();
            PhoneNumber= phoneNumber.Trim();

        }

        private void ValidateCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("El codigo no puede estar Vacío.", nameof(code));

            if (code.Trim().Length < 3)
                throw new ArgumentException("El codigo debe tener al menos 3 caracteres", nameof(code));

            if (code.Trim().Length > 20)
                throw new ArgumentException("El codigo no puede tener mas de 20 caracteres", nameof(code));
        }

        private void ValidateFirstName(string firstName)
        {
            if(string.IsNullOrEmpty(firstName))
                throw new ArgumentException("El Nombre no puede estar vacio",nameof(firstName));

            if (firstName.Trim().Length < 2) 
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres",nameof(firstName));

            if (firstName.Trim().Length > 20) 
                throw new ArgumentException("El Nombre no puede exeder 20 carateres", nameof(firstName));
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("El Nombre no puede estar vacio", nameof(lastName));

            if (lastName.Trim().Length < 2)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres", nameof(lastName));

            if (lastName.Trim().Length > 20)
                throw new ArgumentException("El Nombre no puede exeder 20 carateres", nameof(lastName));
        }

        private void ValidateEmail(string email)
        {
            if(string.IsNullOrEmpty(email))
                throw new ArgumentException("El correo no puede estar vacio",nameof(email));

            if (email.Trim().Length > 50)
                throw new ArgumentException("El Nombre no puede exeder 50 carateres", nameof(email));

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; 

            if(!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("El Correo Electronico no es valido", nameof(email));
            }
        }

        private void ValidatePhoneNumber(string phoneNumber)
        {
            if(string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("El numero de telefono no puede estar vacio ",nameof(phoneNumber));

            if (phoneNumber.Trim().Length < 10)
                throw new ArgumentException("El numero de telefono debe tener al menos 10 ", nameof(phoneNumber));

            if (phoneNumber.Trim().Length > 10)
                throw new ArgumentException("El numero de telefono no puede tener mas de 10 ", nameof(phoneNumber));
        }
    }
}
