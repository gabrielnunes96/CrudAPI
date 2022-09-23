using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.User;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private IRepository<UserEntity> _userRepository;
        public UserService(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEntity> Get(Guid id)
        {
            return await _userRepository.SelectASync(id);
        }
        public async Task<IEnumerable<UserEntity>> GetAll()
        {
            return await _userRepository.SelectASync();
        }
        public async Task<UserEntity> Post(UserEntity user)
        {
            return await _userRepository.InsertASync(user);
        }
        public async Task<UserEntity> Put(UserEntity user)
        {
            return await _userRepository.UpdateASync(user);
        }
        public async Task<bool> Delete(Guid id)
        {
            return await _userRepository.DeleteASync(id);
        }
    }
}
