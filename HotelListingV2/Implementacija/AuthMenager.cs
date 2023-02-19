using AutoMapper;
using HotelListingV2.Data;
using HotelListingV2.Interfejsi;
using HotelListingV2.Models.Models;
using HotelListingV2.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingV2.Implementacija
{
    public class AuthMenager : IAuthMenager
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthMenager> logger;
        private ApiUser _user;
        private const string logInProvider = "HotelListingApi";
        private const string refreshToken = "RefreshToken";


        public AuthMenager(IMapper mapper, UserManager<ApiUser> userManager,IConfiguration configuration, ILogger<AuthMenager> logger)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<string> CreateRefreshToken()
        {
            await userManager.RemoveAuthenticationTokenAsync(_user, logInProvider, refreshToken);
            var newRefreshToken = await userManager.GenerateUserTokenAsync(_user, logInProvider, refreshToken);
            var resolt = await userManager.SetAuthenticationTokenAsync(_user, logInProvider, refreshToken, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDto> LogIn(LogInDto logInDto)
        {
            logger.LogInformation($"Looking for user with email:{logInDto.Equals}");
             _user = await userManager.FindByNameAsync(logInDto.Email);
            var validPassword = await userManager.CheckPasswordAsync(_user, logInDto.Password);
            if (validPassword == false || _user== null)
            {
                logger.LogInformation($"User with email{logInDto.Email} was not found!");
                return null;
            }
            var token= await GenerateToken();
            logger.LogInformation($"Generating token succefuly, {token} ");
            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };

        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto apiUserDto)
        {
             _user= mapper.Map<ApiUser>(apiUserDto);
            _user.UserName = apiUserDto.Email;
            
            var message=await userManager.CreateAsync(_user,apiUserDto.Password);
            if (message.Succeeded)
    {
                await userManager.AddToRoleAsync(_user,"User");
            }
            return message.Errors;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto)
        {
            var jwtSequrityTokenHandler= new JwtSecurityTokenHandler();
            var tokenContetn = jwtSequrityTokenHandler.ReadJwtToken(authResponseDto.Token);
            var username = tokenContetn.Claims.ToList().FirstOrDefault(q=>q.Type== System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            _user = await userManager.FindByNameAsync(username);
            if (_user==null || _user.Id!=authResponseDto.UserId)
            {
                return null;
            }
            var isValidRefreshToken = await userManager.VerifyUserTokenAsync(_user, logInProvider, refreshToken,authResponseDto.RefreshToken);
            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };

                
            }
            await userManager.UpdateSecurityStampAsync(_user);
            return null;
            
           
            
        }

        private async Task<string> GenerateToken()
        {
            //ovo mora ovako
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]));
            var credidentals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles= await userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await userManager.GetClaimsAsync(_user);
            var claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,_user.Email),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, _user.Email),
                  new Claim("uid", _user.Id),

            }.Union(userClaims).Union(roleClaims);
            var token = new JwtSecurityToken(
                issuer: configuration["JWTSettings:Issuer"],
                audience: configuration["JWTSettings:Audiance"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(configuration["JWTSettings:DurationInMinutes"])),
                signingCredentials: credidentals
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
