using CleanArchitecture.Application.Constants;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity;
using CleanArchitecture.Identity.Constants;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Identity.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new Exception($"Failed Login");
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new Exception($"Failed Login");
        }

        var token = await GenerateToken(user);
        var authResponse = new AuthResponse
        {
            Id = user.Id,
            Email = request.Email,
            UserName = user.UserName,
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };

        return authResponse;
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.UserName);
        if (existingUser is not null)
        {
            throw new Exception($"{request.UserName} is existing");
        }

        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail is not null)
        {
            throw new Exception($"{request.Email} is exisiting");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            Name = request.Name,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception($"{result.Errors}");
        }

        await _userManager.AddToRoleAsync(user, ApplicationUserRoles.Operator);
        var token = await GenerateToken(user);
        return new RegistrationResponse
        {
            Email = user.Email,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            UserId = user.Id,
            UserName = user.UserName,
        };

    }

    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));

        var claims = new[]
        {
            new Claim(ApplicationClaimTypes.Sub, user.UserName),
            new Claim(ApplicationClaimTypes.Email, user.Email),
            new Claim(ApplicationClaimTypes.Uid, user.Id),
            new Claim(ApplicationClaimTypes.UserName, user.UserName),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

        return jwtSecurityToken;
    }
}
