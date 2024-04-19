using HalloDocBAL.Interfaces;
using HalloDocDAL.Contacts;
using HalloDocDAL.Model;
using HalloDocDAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;

namespace HalloDocBAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepository userRepository, IUserRepo userRepo, IPhysicianRepository physicianRepository)
        {
            _userRepository = userRepository;
            _userRepo = userRepo;
            _physicianRepository = physicianRepository;
        }

        public async Task<bool> SignUp(Register register)
        {
            var u = await _userRepository.FindByEmail(register.Email);
            if (u != null)
            {
                return false;
            }
            register.password = Convert.ToBase64String(Encoding.UTF8.GetBytes(register.password));
            var aspuser = new Aspnetuser
            {
                Id = Guid.NewGuid().ToString(),
                Email = register.Email,
                Passwordhash = register.password,
                Username = register.Email,
                Createddate = DateTime.Now,
                Phonenumber = register.Phone
            };
            await _userRepository.AddUser(aspuser);

            Aspnetuserrole role = new Aspnetuserrole
            {
                UserId = aspuser.Id,
                RoleId = "2"
            };
            _userRepository.AddUserRole(role);
            var user = new User
            {
                Mobile = register.Phone,
                Street = register.Street,
                City = register.City,
                State = register.State,
                Zipcode = register.Zipcode,
                Intdate = register.Date,
                Strmonth = register.Month,
                Intyear = register.Year,
                Email = register.Email,
                Firstname = register.Email,
                Aspnetuserid = aspuser.Id,
                Createddate = DateTime.Now
            };
            return await _userRepo.AddUser(user);
        }

        public async Task<Aspnetuser> Login(Login model)
        {
            var user = await _userRepository.FindByEmail(model.Email);

            if (user != null)
            {
                if(user.Aspnetuserroles.Any(x=> x.RoleId == "3"))
                {
                    Physician p = _physicianRepository.GetPhysicianByAspId(user.Id);
                    if(p != null)
                    {
                        if(p.Isdeleted == true) {
                            return null;
                        }
                    }
                }
                var password = Encoding.UTF8.GetString(Convert.FromBase64String(user.Passwordhash));
                if (model.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<Aspnetuser> CheckUser(string email)
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
            model.Passwordhash = Convert.ToBase64String(Encoding.UTF8.GetBytes(model.Passwordhash));
            user.Passwordhash = model.Passwordhash;
            return await _userRepository.EditAspUser(user);
        }

        public bool IsUserBlocked(string email, string phone)
        {
            var user = _userRepository.IsUserBlocked(email, phone); ;
            return user;
        }
    }
}
