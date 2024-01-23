using Learning_platform.DTO;
using Learning_platform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Learning_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IWebHostEnvironment webHostEnvironment;

        private readonly UserManager<ApplicationUser> usermanager;
        private readonly RoleManager<IdentityRole> rolemanager;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext context;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };

        public UserController(UserManager<ApplicationUser> usermanager, IConfiguration config, RoleManager<IdentityRole> rolemanager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.usermanager = usermanager;
            this.rolemanager = rolemanager;
            this.config = config;
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("register")]//api/account/register
        public async Task<IActionResult> Registration([FromForm] RegisterUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                //save
                ApplicationUser user = new ApplicationUser();
                user.Email = userDto.Email;
                user.UserName = userDto.Email.Split("@")[0];

                if (userDto.Image != null)
                {
                    string[] allowedExtensions = { ".png", ".jpg" };
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

                    if (!allowedExtensions.Contains(Path.GetExtension(userDto.Image.FileName).ToLower()))
                    {
                        return BadRequest("Only .png and .jpg images are allowed!");
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + userDto.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    await using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await userDto.Image.CopyToAsync(fileStream);
                    }
                    user.Image = uniqueFileName;
                }

                IdentityResult result = await usermanager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    //var role= "Usre";
                    //role = userDto.RoleName;
                    //if (!await rolemanager.RoleExistsAsync(role))
                    //{
                    //    return BadRequest($"Invalid role name: {role}");
                    //}

                    await usermanager.AddToRoleAsync(user, "Usre");

                    return Ok("Account added successfully");
                }

                return BadRequest(result.Errors.FirstOrDefault());
            }

            return BadRequest(ModelState);
        }


        [HttpPost("login")]//api/account/login
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid == true)
            {
                //check - create token
                ApplicationUser user = await usermanager.FindByEmailAsync(userDto.Email);
                if (user != null)//email found
                {
                    bool found = await usermanager.CheckPasswordAsync(user, userDto.Password);
                    if (found)
                    {
                        //Claims Token
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Email, user.Email));
                        claims.Add(new Claim(ClaimTypes.Name, user.Image));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        //get role
                        var roles = await usermanager.GetRolesAsync(user);
                        foreach (var itemRole in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, itemRole/*.ToString()*/));
                        }
                        SecurityKey securityKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));

                        //Create token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],//url web api
                            audience: config["JWT:ValidAudiance"],//url consumer angular
                            expires: DateTime.Now.AddDays(double.Parse(config["JWT:DurationInDay"])),
                            claims: claims,
                            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo
                        });
                    }
                    return Ok("Email and password invalid");
                }
                return Unauthorized();

            }
            return Unauthorized();
        }
    }
}
