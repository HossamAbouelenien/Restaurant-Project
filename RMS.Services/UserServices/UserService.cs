using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.UserSpec;
using RMS.ServicesAbstraction.IUserServices;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.UserDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

       

        public async Task<IEnumerable<GetUserDTO>> GetAllUserWithBranchAsync(UserQueryParams queryParams)
        {
            var Repo = _unitOfWork.GetRepository<User>();
            var Spec = new UserWithBranchSpecifications(queryParams);
            var Users = await Repo.GetAllAsync(Spec);
            var DataToReturn = _mapper.Map<IEnumerable<GetUserDTO>>(Users);
            return DataToReturn;
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
            var user = _mapper.Map<User>(createUserDto);
            user.CreatedAt = DateTime.UtcNow;
            user.UserName = createUserDto.Email;
            var createdUser = await _userManager.CreateAsync(user, "Rms123@#");

            //SEND EMAIL TO USER WITH PASSWORD

            if (!createdUser.Succeeded)
            {
                throw new Exception(string.Join(", ", createdUser.Errors.Select(e => e.Description)));
            }
            var spec = new UserWithBranchSpecifications(user.Id);
            var addedUser = await repo.GetByIdAsync(spec);

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
            repo.Update(user);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<UserDetailsDTO>(user);
            return result;
        }


        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user); 
            return true;
        }
    }
}
