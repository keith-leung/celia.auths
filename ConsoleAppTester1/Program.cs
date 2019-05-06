using Celia.io.Core.Auths.Abstractions;
using Celia.io.Core.Auths.SDK;
using System;
using System.Threading.Tasks;

namespace ConsoleAppTester1
{
    class Program
    {
        protected static RoleManager roleManager = null;
        protected static UserManager userManager = null;
        protected static SignInManager signInManager = null;

        static void Main(string[] args)
        {
            roleManager = new RoleManager("br.com", "79faf82271944fe38***********",
                "http://localhost:57966"); 
            userManager = new UserManager("br.com", "79faf82271944fe38***********",
                "http://localhost:57966"); 
            signInManager = new SignInManager("br.com", "79faf82271944fe38***********",
                "http://localhost:57966"); 

            var t = UpdateUser();
            t.Wait(); 

            Console.WriteLine("Over!! ");
            Console.ReadLine();
        }

        public static async Task UpdateUser()
        {  
            var adminLogin = await signInManager.LoginByUserNameAsync("Administrator",
                "Adm2341541363"); 
            
            var t2 = userManager.UpdateUserAsync(adminLogin.AccessToken,
                new ApplicationUser()
                {
                    Email = "br@d5b981a7-ca3b-4789-a7ec-4b3a054b8017.com",
                    UserName = "fddfa",
                    PhoneNumber = "12346578901234",
                    Id = "d5b981a7-ca3b-4789-a7ec-4b3a054b8017",
                });
            t2.Wait();

            Console.WriteLine(t2.Result);
        }
    }
}
