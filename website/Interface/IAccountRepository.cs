using website.Dto;

namespace website.Interface
{
    public interface IAccountRepository
    {
        ApplicationUserDTO LoginProcess(LoginDTO loginDT);
    }
}