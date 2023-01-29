using Abp;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bancs.Core.Security
{
    public class MyLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        readonly ILdapSettings _Settings;
        public MyLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
            _Settings = settings;
        }

        public override async Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, Tenant tenant)
        {
            var isEnabled = await  _Settings.GetIsEnabled(tenant.Id);
            if (isEnabled)
            {
                var domain = await _Settings.GetDomain(tenant.Id);
                var userName = await _Settings.GetUserName(tenant.Id);
                var password = await _Settings.GetPassword(tenant.Id);
                return ValidateUserByBind(userNameOrEmailAddress, plainPassword,domain);
            }
            else
            {
                return (false);
            }
        }

        private bool ValidateUserByBind(string username, string password, string domain)
        {
            bool result = true;
            var credentials = new NetworkCredential(username, password);
            var serverId = new LdapDirectoryIdentifier(domain);

            var conn = new LdapConnection(serverId, credentials);
            try
            {
                conn.Bind();
            }
            catch (Exception)
            {
                result = false;
            }

            conn.Dispose();

            return result;
        }
    }
}

