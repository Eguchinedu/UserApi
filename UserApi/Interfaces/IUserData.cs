using System.Globalization;
using UserApi.models;

namespace UserApi.Interfaces
{
    public interface IUserData
    {
        ICollection<UserModel> GetUsers();

        UserModel GetUser(int id);  


        bool AddUser(UserModel user);

        bool Save();


    }
}
