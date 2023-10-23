using UserApi.Data;
using UserApi.Interfaces;
using UserApi.models;

namespace UserApi.Repository
{
    public class UserData : IUserData
        
    {
        private readonly UserContext _context;

        public UserData(UserContext context)
        {
            _context = context;
            
        }
        public bool AddUser(UserModel user)
        {
            _context.Add(user);

            return Save();
        }

        public UserModel GetUser(int id)
        {
            return _context.userModels.Where(p => p.UserId == id).FirstOrDefault();
        }

        public ICollection<UserModel> GetUsers()
        {
            return _context.userModels.OrderBy(x => x.UserId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        
    }
}
