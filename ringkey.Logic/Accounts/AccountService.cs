using ringkey.Data;

namespace ringkey.Logic.Accounts
{
    public class AccountService
    {
        private UnitOfWork _unitOfWork;
        
        public AccountService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        
    }
}