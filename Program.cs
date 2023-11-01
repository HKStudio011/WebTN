using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebTN.Models;
//using WebTN.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddOptions();


builder.Services.AddDbContext<MyBlogContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyBlogContext"));
});

// Add send mail services
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
//builder.Services.AddSingleton<IEmailSender<AppUser>,SendMailService>();
builder.Services.AddSingleton<IEmailSender, SendMailService>();


// Add Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;         // Xác thực trước khi đăng nhập
});

// dang nhap bang dich vu ngoai
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var google = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = google["ClientId"];
        options.ClientSecret = google["ClientSecret"];
        options.CallbackPath = "/dang-nhap-tu-google";
    })
    .AddFacebook(options => 
    {
        var facebook = builder.Configuration.GetSection("Authentication:Facebook");
        options.AppId = facebook["AppId"];
        options.AppSecret = facebook["AppSecret"];
        options.CallbackPath = "/dang-nhap-tu-facebook";
    });

// Use UI custom
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MyBlogContext>()
                .AddDefaultTokenProviders();

// Use UI default
// builder.Services.AddDefaultIdentity<AppUser>()
//                 .AddEntityFrameworkStores<MyBlogContext>()
//                 .AddDefaultTokenProviders();

// Authorization services
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/khongthetrycap/";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapRazorPages();

app.Run();
