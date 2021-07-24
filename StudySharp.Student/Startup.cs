using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudySharp.DomainServices;
using StudySharp.Shared.Constants;

namespace StudySharp.Student
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDomainServices(Configuration);
            services.AddAuthorization(opt => opt.AddPolicy(AuthorizationPolicies.StudentPolicy, policy => policy.RequireRole(nameof(DomainRoles.Student))));
            services.AddRazorPages().AddRazorPagesOptions(config =>
            {
                config.Conventions.AuthorizeFolder("/Student", AuthorizationPolicies.StudentPolicy);
            }).AddRazorRuntimeCompilation();
            services.ConfigureApplicationCookie(opt => opt.LoginPath = RedirectUrls.Unauthorized);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
