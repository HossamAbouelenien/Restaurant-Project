using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.EmailServices;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.UserSpec;
using RMS.ServicesAbstraction.IEmailServices;
using RMS.ServicesAbstraction.IUserServices;
using RMS.Shared;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.UserDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using Microsoft.EntityFrameworkCore;

namespace RMS.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }



        public async Task<PaginatedResult<GetUserDTO>> GetAllUserWithBranchAsync(UserQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<User>();
            var spec = new UserWithBranchSpecifications(queryParams);
            var users = await repo.GetAllAsync(spec);
            var dataToReturn = _mapper.Map<IEnumerable<GetUserDTO>>(users);
            var countOfReturnedUsers = dataToReturn.Count();
            var countSpec = new UsersCountSpecification(queryParams);
            var countOfAllUsers = await repo.CountAsync(countSpec);
            var paginatedResult = new PaginatedResult<GetUserDTO>(
                queryParams.PageIndex,
                countOfReturnedUsers,
                countOfAllUsers,
                dataToReturn
            );

            return paginatedResult;
        }

        public async Task<UserDetailsDTO> GetUserDetailsAsync(string id)
        {
            var Repo = _unitOfWork.GetRepository<User>();
            var Spec = new UserWithBranchSpecifications(id);
            var user = await Repo.GetByIdAsync(Spec);
            var DataToReturn = _mapper.Map<UserDetailsDTO>(user);
            return DataToReturn;
        }

        public async Task<UserDetailsDTO> AddUserAsync(CreateUserDto createUserDto)
        {

            var repo = _unitOfWork.GetRepository<User>();

            User user = new()
            {
                Email = createUserDto.Email,
                Name = createUserDto.Name,
                UserName = createUserDto.Email,
                NormalizedEmail = createUserDto.Email.ToUpper(),
                EmailConfirmed = true,
                RoleId = string.IsNullOrEmpty(createUserDto.RoleId) ? SD.Role_Customer : createUserDto.RoleId,
                CreatedAt = DateTime.UtcNow
            };

           
            var createdUser = await _userManager.CreateAsync(user, SD.DefaultPassword);

            if (!createdUser.Succeeded)
            {
                throw new Exception(string.Join(", ", createdUser.Errors.Select(e => e.Description)));
            }

            var role = string.IsNullOrEmpty(createUserDto.RoleId)
                ? SD.Role_Customer
                : createUserDto.RoleId;

            var roleResult = await _userManager.AddToRoleAsync(user, role);

            var spec = new UserWithBranchSpecifications(user.Id);
            var addedUser = await repo.GetByIdAsync(spec);

            var subject = $"Welcome to {SD.RestaurantName}";

            var body =
                $@"<div style='font-family: Arial, sans-serif; line-height:1.8; max-width:600px; margin:auto;'>

                <h2 style='color:#2c3e50;'>Welcome to {SD.RestaurantName}</h2>

                <p>Hello {createUserDto.Name},</p>

                <p>
                    Your account has been created successfully. Below are your login details:
                </p>

                <p>
                    <b>Email:</b> {createUserDto.Email}<br>
                    <b>Temporary Password:</b> {SD.DefaultPassword}<br>
                    <b>Role:</b> {role}
                </p>

                <p style='color:#d35400; font-weight:bold;'>
                    ⚠️ For your security, this is a temporary password.<br>
                    You must change your password immediately after logging in.
                </p>

                <hr>

                <h3 style='color:#2c3e50;'>مرحبًا {createUserDto.Name}</h3>

                <p>
                    تم إنشاء حسابك بنجاح. بيانات تسجيل الدخول الخاصة بك:
                </p>

                <p>
                    <b>البريد الإلكتروني:</b> {createUserDto.Email}<br>
                    <b>كلمة المرور المؤقتة:</b> {SD.DefaultPassword}<br>
                    <b>الدور:</b> {role}
                </p>

                <p style='color:#d35400; font-weight:bold;'>
                    ⚠️ هذه كلمة مرور مؤقتة لأسباب أمنية.<br>
                    يجب عليك تغيير كلمة المرور فور تسجيل الدخول.
                </p>

                </div>";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return _mapper.Map<UserDetailsDTO>(addedUser);

        }

        public async Task<UserDetailsDTO> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            var repo = _unitOfWork.GetRepository<User>();
            var spec = new UserWithBranchSpecifications(id);
            var user = await repo.GetByIdAsync(spec);
            if (user == null)
                throw new Exception("User not found");
            _mapper.Map(updateUserDto, user);
            user.UserName = updateUserDto.Email;
            repo.Update(user);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<UserDetailsDTO>(user);
            return result;
        }


        public async Task<bool> ToggleUserStatusAsync(string id)
        {
            var repo = _unitOfWork.GetRepository<User>();

            var spec = new UserByIdIgnoreFilterSpecification(id);

            var user = await repo.GetByIdAsync(spec);

            if (user == null)
                throw new Exception("User not found");

            if (!user.IsDeleted)
            {
                user.IsDeleted = true;
                user.DeletedAt = DateTime.UtcNow;
            }
            else
            {
                user.IsDeleted = false;
                user.DeletedAt = null;
            }

            user.UpdatedAt = DateTime.UtcNow;

            repo.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<PaginatedResult<GetUserDTO>> GetInactiveUsersAsync(UserQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<User>();

            var spec = new InactiveUsersWithBranchSpecification(queryParams);

            var users = await repo.GetAllAsync(spec);

            var dataToReturn = _mapper.Map<IEnumerable<GetUserDTO>>(users);

            var countSpec = new InactiveUsersCountSpecification(queryParams);

            var countOfAllUsers = await repo.CountAsync(countSpec);

            return new PaginatedResult<GetUserDTO>(
                queryParams.PageIndex,
                dataToReturn.Count(),
                countOfAllUsers,
                dataToReturn
            );
        }

        public async Task<List<string>> GetRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles
                              .Where(r => r.Name != null)
                              .Select(r => r.Name!)
                              .Distinct()
                              .OrderBy(r => r)
                              .ToList()  
            );
        }


        public async Task<GetCustomerDTO> AddCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {

            var repo = _unitOfWork.GetRepository<User>();

            User user = new()
            {
              
                Name = createCustomerDTO.Name,
                UserName = $"{createCustomerDTO.Name}.{ createCustomerDTO.PhoneNumber}",
                PhoneNumber = createCustomerDTO.PhoneNumber,
                RoleId = SD.Role_Customer,
                CreatedAt = DateTime.Now,
                BranchId = createCustomerDTO.BranchId
            };


            var createdUser = await _userManager.CreateAsync(user, SD.DefaultPassword);

            if (!createdUser.Succeeded)
            {
                throw new Exception(string.Join(", ", createdUser.Errors.Select(e => e.Description)));
            }

            var role = SD.Role_Customer;

            var roleResult = await _userManager.AddToRoleAsync(user, role);

            var spec = new UserWithBranchSpecifications(user.Id);
            var addedUser = await repo.GetByIdAsync(spec);

            return _mapper.Map<GetCustomerDTO>(addedUser);

        }

        public async Task<PaginatedResult<GetCustomerDTO>> GetAllCustomerUserAysnc(CustomerQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<User>();
            var spec = new CustomerByPhoneSpecification(queryParams);
            var users = await repo.GetAllAsync(spec);
            var dataToReturn = _mapper.Map<IEnumerable<GetCustomerDTO>>(users);
            var countOfReturnedUsers = dataToReturn.Count();
            var countSpec = new CustomersByPhoneCountSpecification(queryParams);
            var countOfAllUsers = await repo.CountAsync(countSpec);
            var paginatedResult = new PaginatedResult<GetCustomerDTO>(
                queryParams.PageIndex,
                countOfReturnedUsers,
                countOfAllUsers,
                dataToReturn
            );

            return paginatedResult;
        }


        public async Task<GetCustomerDTO> UpdateCustomerAddress(string id, UpdateCustomerAddressDTO updateCustomerAddressDTO)
        {
            var repo = _unitOfWork.GetRepository<User>();
            var spec = new UserWithBranchSpecifications(id);
            var user = await repo.GetByIdAsync(spec);

            if (user == null)
                throw new Exception("User not found");

            // ✅ Map الـ DTO الأول لـ Address Entity
            var newAddress = _mapper.Map<Address>(updateCustomerAddressDTO.addressDTO);

            // ✅ تأكد إن الـ collection مش null
            user.Addresses ??= new List<Address>();

            user.Addresses.Add(newAddress);

            repo.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<GetCustomerDTO>(user);
        }
    }

}
