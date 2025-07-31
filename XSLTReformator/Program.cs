using FluentValidation;
using XSLTReformator.Abstractions;
using XSLTReformator.Configurations;
using XSLTReformator.Contracts;
using XSLTReformator.Services;

namespace XSLTReformator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<XmlFileSettings>
                (builder.Configuration.GetSection(nameof(XmlFileSettings)));
            builder.Services.Configure<XsltFileSettings>
                (builder.Configuration.GetSection(nameof(XsltFileSettings)));

            builder.Services.AddScoped<IXsltProcessor, XsltProcessor>();
            builder.Services.AddScoped<IXmlFileService, XmlFileService>();
            builder.Services.AddScoped<IXmlModificationService, XmlModificationService>();
            builder.Services.AddScoped<IXmlTransformationService, XmlTransformationService>();

            builder.Services.AddScoped<IValidator<TransformRequest>, TransformRequestValidator>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Employees}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
