using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aspnet_custom_identity.Model
{
    public class GoogleUser: User
    {
        public override string UserType  { get  { return "Google+";  } }
    }
   
}
