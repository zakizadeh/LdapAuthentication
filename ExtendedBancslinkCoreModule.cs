using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetZeroCore;
using Abp.AspNetZeroCore.Timing;
using Abp.AutoMapper;
using Abp.BackgroundJobs;


namespace Bancs
{
    [DependsOn(
        typeof(AbpZeroCoreModule),
        typeof(AbpZeroLdapModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAspNetZeroCoreModule),
        typeof(AbpAspNetZeroCoreModule),
        typeof(AbpMailKitModule))]
    public class BancsCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
             AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

    
            //Enable LDAP authentication 
       
            Configuration.Modules.ZeroLdap().Enable(typeof(MyLdapAuthenticationSource));

           
        }

        public override void Initialize()
        {
        }

        public override void PostInitialize()
        {
           
        }
    }
}
