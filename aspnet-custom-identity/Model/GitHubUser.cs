using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Model
{
    public class GitHubUser : User
    {
        public override string UserType  { get  {  return "GitHub";  } }
    }
    
}
