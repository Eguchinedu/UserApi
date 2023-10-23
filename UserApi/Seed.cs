using UserApi.Data;
using UserApi.models;
using static System.Reflection.Metadata.BlobBuilder;

namespace UserApi
{
    public class Seed
    {
        private readonly UserContext _context;

        public Seed(UserContext context)
        {
            _context = context;
        }

        public void SeedUserContext()
        {
            if(!_context.userModels.Any())
            {
                var users = new List<UserModel>()
                {
                    new UserModel()
                    {
                        FirstName = "Egu",
                        LastName = "Chinedu",
                        UserName = "EguChinedu",
                        Password = "password",
                       
                    },
                    new UserModel()
                    {
                        FirstName = "Zeus",
                        LastName = "egu",
                        UserName = "zeus_egu",
                        Password = "password"

                    },
                    new UserModel()
                    {
                        FirstName = "Hades",
                        LastName = "Orion",
                        UserName = "hades_orion",
                        Password = "password"

                    },
                };
                _context.userModels.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}
