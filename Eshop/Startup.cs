using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;

namespace Eshop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // phải có lệnh này khi muốn dùng session
            // cho phép ứng dụng sử dụng bộ nhớ đệm phân tán
            // Session sẽ được lưu vào bộ nhớ đệm này
            // Distributed memory cache
            services.AddDistributedMemoryCache();

            services.AddControllersWithViews();

            services.AddDbContext<EshopContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("EshopContext")));

            services.AddSession(options =>
            {
                // TimeSpan(7,0,0,0) : 7 ngày
                //options.IdleTimeout = new TimeSpan(0, 30, 0); tương đương với
                options.IdleTimeout = TimeSpan.FromMinutes(30);

                //options.Cookie.Name = "EshopSession";  // không bắt buộc

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();   // bắt buộc phải nằm sau useRouting & nằm trước useEndpoints

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
