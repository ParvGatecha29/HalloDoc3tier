using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace HalloDocBAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepository userRepository, IUserRepo userRepo)
        {
            _userRepository = userRepository;
            _userRepo = userRepo;
        }

        public async Task<bool> SignUp(Register register)
        {
            var u = await _userRepository.FindByEmail(register.Email);
            if (u != null)
            {
                return false;
            }
            var aspuser = new Aspnetuser
            {
                Id = Guid.NewGuid().ToString(),
                Email = register.Email,
                Passwordhash = register.password,
                Username = register.Email,
                Createddate = DateTime.Now,
            };
            await _userRepository.AddUser(aspuser);

            var user = new User
            {
                Email = register.Email,
                Firstname = register.Email,
                Aspnetuserid = aspuser.Id
            };
            return await _userRepo.AddUser(user);
        }

        public async Task<Aspnetuser> Login(Login model)
        {
            var user = await _userRepository.FindByEmail(model.Email);
            if (user != null)
            {
                if (model.Password == user.Aspnetuser.Passwordhash)
                {
                    return user.Aspnetuser;
                }
            }
            return null;
        }

        public async Task<User> CheckUser(string email)
        {
            var user = await _userRepository.FindByEmail(email);
            Debug.WriteLine(user);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<User> GetUser(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            Debug.WriteLine(user);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<bool> AddUser(User model)
        {
            var user = new User
            {
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Aspnetuserid = model.Aspnetuserid
            };
            return await _userRepo.AddUser(user);
        }

        public async Task<bool> EditUser(User model)
        {
            Debug.WriteLine(model.Email);
            var user = await _userRepository.GetByEmail(model.Email);
            user.Email = model.Email;
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Mobile = model.Mobile;
            user.Street = model.Street;
            user.City = model.City;
            user.State = model.State;
            user.Zipcode = model.Zipcode;
            user.Intdate = model.Intdate;
            user.Intyear = model.Intyear;
            user.Strmonth = model.Strmonth;
            return await _userRepo.EditUser(user);
        }

        public async Task<bool> EditAspNetUser(Aspnetuser model)
        {
            Debug.WriteLine(model.Email);
            var user = await _userRepository.FindByEmail(model.Email);
            user.Aspnetuser.Passwordhash = model.Passwordhash;
            return await _userRepository.EditAspUser(user.Aspnetuser);
        }

        public bool IsUserBlocked(string email, string phone)
        {
            var user = _userRepository.IsUserBlocked(email, phone); ;
            return user;
        }
    }
}
