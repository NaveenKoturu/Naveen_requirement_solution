using PatientManagementAPI.Models;
using System.Text.RegularExpressions;

namespace PatientManagement.Validators
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; } = new();
    }

    public class PatientValidator
    {
        public ValidationResult Validate(Patient patient)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(patient.FirstName))
                result.Errors.Add("First name is required.");

            if (string.IsNullOrWhiteSpace(patient.LastName))
                result.Errors.Add("Last name is required.");

            if (patient.DateOfBirth > DateTime.Today)
                result.Errors.Add("Date of birth cannot be in the future.");

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                result.Errors.Add("Email is required.");
            }
            else if (!IsValidEmail(patient.Email))
            {
                result.Errors.Add("Invalid email format.");
            }

            if (!string.IsNullOrWhiteSpace(patient.PhoneNumber) && !IsValidPhoneNumber(patient.PhoneNumber))
            {
                result.Errors.Add("Invalid phone number format.");
            }

            return result;
        }

        private bool IsValidEmail(string email)
        {
            // Simple email regex. Consider using more robust validation.
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Simple phone regex. Adjust as needed.
            return Regex.IsMatch(phone, @"^\+?\d{10,15}$");
        }
    }
}