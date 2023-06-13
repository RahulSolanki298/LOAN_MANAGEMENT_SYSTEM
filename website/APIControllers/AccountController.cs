using System.Threading.Tasks;
using System.Web.Http;
using website.Dto;
using website.Interface;

namespace website.APIControllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountRepository _accountRepository=null;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [Route("api/Account/Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginDTO login)
        {
            var result = _accountRepository.LoginProcess(login);
            return Ok(result);
        }
    }
}
