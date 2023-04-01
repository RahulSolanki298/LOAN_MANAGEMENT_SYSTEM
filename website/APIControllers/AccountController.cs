using System.Threading.Tasks;
using System.Web.Http;
using website.Dto;
using website.Interface;

namespace website.APIControllers
{
    public class AccountController : ApiController
    {
        public readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost]
        public ApplicationUserDTO Login(LoginDTO login)
        {
            var result = _accountRepository.LoginProcess(login);
            return result;
        }
    }
}
