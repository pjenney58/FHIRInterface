using System;
using System.Security.Principal;

namespace Primary.Model
{
	public class ManagePrincipal
	{
		public ManagePrincipal()
		{
		}

        public void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            //if (HttpContext.Current != null)
            //{
             //   HttpContext.Current.User = principal;
            //}
        }
    }
}

