using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Service
{
    public interface IActiveDirectoryService
    {
        bool Authenticate(string userName, string password);
    }

    public class ActiveDirectoryService : IActiveDirectoryService
    {
        public string Domain { get; set; }

        public bool Authenticate(string userName, string password)
        {
            bool isValid = false;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, Domain))
            {
                isValid = pc.ValidateCredentials(userName, password);
            }
            return isValid;
        }

    }

    public class ActiveDirectoryServiceFake : IActiveDirectoryService
    {
        public string Domain { get; set; }

        public bool Authenticate(string userName, string password)
        {
            bool isValid = true;
            return isValid;
        }

    }
}
