
using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Microsoft.AspNetCore.Mvc;
using Elsa.Webhooks;
using Parlot.Fluent;
using ElsaServer;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

var services = builder.Services;
var configuration = builder.Configuration;





services
    .AddElsa(elsa => elsa
        .UseIdentity(identity =>
        {
            identity.TokenOptions = options => options.SigningKey = "large-signing-key-for-signing-JWT-tokens";
            identity.UseAdminUserProvider();
        })
        .UseDefaultAuthentication()
     //   .UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef => ef.UseSqlite()))
     //   .UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(ef => ef.UseSqlite()))
     .UseWorkflowManagement(management => management.UseEntityFrameworkCore(options => options.UseSqlServer(configuration.GetConnectionString("Elsa"))))
        .UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(options => options.UseSqlServer(configuration.GetConnectionString("Elsa"))))


        .AddActivity<PrintMessage>() // custom print support vars
        .AddActivity<ExtractFirstStockActivity>()
        .AddActivity<CheckStockThresholdActivity>()

        //basic activity from elsa i changed its logic 
        .AddActivity<SwitchFlowTest>()

        //new activities
        .AddActivity<FilterStocksBelowThresholdActivity>()


        .UseScheduling()
        .UseJavaScript()
        .UseLiquid()
        .UseCSharp()
        .UseHttp(http => http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options))
        .UseWebhooks()
        .UseWorkflowsApi()
        .UseRealTimeWorkflows()
        .AddActivitiesFrom<Program>()
        .AddWorkflowsFrom<Program>()






 );





services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")));
services.AddRazorPages(options => options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
 
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseRouting();
app.UseCors();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseWorkflows();
app.UseWorkflowsSignalRHubs();
app.MapFallbackToPage("/_Host");
app.Run();
