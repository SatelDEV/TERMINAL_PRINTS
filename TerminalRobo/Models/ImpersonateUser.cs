using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TerminalRobo.Models
{
    internal class ImpersonateUser { 
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool LogonUser(
          String lpszUsername,
          String lpszDomain,
          String lpszPassword,
          int dwLogonType,
          int dwLogonProvider,
          ref IntPtr phToken);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public extern static bool CloseHandle(IntPtr handle);

    private static IntPtr tokenHandle = new IntPtr(0);
    private static WindowsImpersonationContext impersonatedUser;

    // If you incorporate this code into a DLL, be sure to demand that it
    // runs with FullTrust.
    [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
    public void Impersonate(string domainName, string userName, string password)
    {
        try
        {

            // Use the unmanaged LogonUser function to get the user token for
            // the specified user, domain, and password.
            // Xc: ---- For const descriptions see
            // ms-help://MS.MSDNQTR.v80.en/MS.MSDN.v80/MS.WIN32COM.v10.en/secauthn/security/logonuser.htm
            const int LOGON32_PROVIDER_DEFAULT = 0;
            // Passing this parameter causes LogonUser to create a primary token.
            //const int LOGON32_LOGON_INTERACTIVE = 2;
            // const int LOGON32_LOGON_NETWORK = 3; 
            tokenHandle = IntPtr.Zero;


            const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

            // Call  LogonUser to obtain a handle to an access token.
            // Xc: ---- LogonUser documentation
            // ms-help://MS.MSDNQTR.v80.en/MS.MSDN.v80/MS.WIN32COM.v10.en/secauthn/security/logonuser.htm
            bool returnValue = LogonUser(
                userName,
                domainName,
                password,
                LOGON32_LOGON_NEW_CREDENTIALS,
                //LOGON32_LOGON_INTERACTIVE, // Xc:-- will cache logon information for disconnected operations
                /* LOGON32_LOGON_NETWORK, // Xc:-- This type of access is faster */
                LOGON32_PROVIDER_DEFAULT,
                ref tokenHandle);         // tokenHandle - new security token


            if (false == returnValue)
            {
                int ret = Marshal.GetLastWin32Error();
                Console.WriteLine("LogonUser call failed with error code : " +
                    ret);
                throw new System.ComponentModel.Win32Exception(ret);
            }

            // Xc: ---- Assume new identity
            WindowsIdentity newId = new WindowsIdentity(tokenHandle);
            impersonatedUser = newId.Impersonate();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred. " + ex.Message);
        }
    }

    // Stops impersonation
    public void Undo()
    {
        impersonatedUser.Undo();
        // Free the tokens.
        if (tokenHandle != IntPtr.Zero)
            CloseHandle(tokenHandle);
    }

}
}

