using System;

namespace LegacyApp
{
    public interface IUserDataAccessAdapter
    {
        void AddUser(User user);
    }

    public interface IClientRepository
    {
        Client GetById(int clientId);
    }

    public interface IUserCreditService
    {
        int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }

    public class UserDataAccessAdapter : IUserDataAccessAdapter
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }

    public class UserService
    {
        private IClientRepository _clientRepository;
        private IUserCreditService _userCreditService;
        private IUserDataAccessAdapter _userDataAccessAdapter;

        public UserService()
        {
            _clientRepository = new ClientRepository();
            _userCreditService = new UserCreditService();
            _userDataAccessAdapter = new UserDataAccessAdapter();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateUserInput(firstName, lastName, email, dateOfBirth))
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

            var user = CreateUserObject(firstName, lastName, email, dateOfBirth, client);

            SetUserCreditLimit(user, client);

            if (!CheckCreditLimit(user))
            {
                return false;
            }

            _userDataAccessAdapter.AddUser(user);
            return true;
        }

        private bool ValidateUserInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            bool result = !string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) && email.Contains("@") &&
                          email.Contains(".") && CalculateAge(dateOfBirth) >= 21;
            return result;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            return age;
        }

        private User CreateUserObject(string firstName, string lastName, string email, DateTime dateOfBirth,
            Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        private void SetUserCreditLimit(User user, Client client)
        {
            var creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                user.HasCreditLimit = true;
                creditLimit *= 2;
                user.CreditLimit = creditLimit;
            }
            else
            {
                user.HasCreditLimit = true;
                user.CreditLimit = creditLimit;
            }
        }

        private bool CheckCreditLimit(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= 500;
        }
    }
}