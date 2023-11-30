using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToyStoreMVC.DataAccess.Data;

[assembly: HostingStartup(typeof(ToyStoreMVC.Areas.Identity.IdentityHostingStartup))]
namespace ToyStoreMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup 
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}