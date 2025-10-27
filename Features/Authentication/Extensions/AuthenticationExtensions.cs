using Features.Authentication.Models;
using Features.Authentication.Services;
using Microsoft.AspNetCore.Authorization;

namespace Features.Authentication.Extensions;

public static class AuthenticationExtensions
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var auth = app.MapGroup("/api/auth").WithTags("Authentication");

        // Register endpoint
        auth.MapPost("/register", async (UserRegistrationRequest request, AuthenticationService authService) =>
        {
            try
            {
                var response = await authService.RegisterAsync(request);
                return Results.Created($"/api/users/{response.Id}", response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .WithName("Register")
        .WithSummary("Register a new user account")
        .Produces<UserRegistrationResponse>(201)
        .Produces(400)
        .Produces(409);

        // Login endpoint
        auth.MapPost("/login", async (UserLoginRequest request, AuthenticationService authService, HttpContext context) =>
        {
            try
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString();
                var userAgent = context.Request.Headers.UserAgent.ToString();
                
                var response = await authService.LoginAsync(request, ipAddress, userAgent);
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Unauthorized();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .WithName("Login")
        .WithSummary("Authenticate user and return tokens")
        .Produces<AuthenticationResponse>(200)
        .Produces(400)
        .Produces(401);

        // Refresh token endpoint
        auth.MapPost("/refresh", async (RefreshTokenRequest request, AuthenticationService authService) =>
        {
            try
            {
                var response = await authService.RefreshTokenAsync(request.RefreshToken);
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Results.Unauthorized();
            }
        })
        .WithName("RefreshToken")
        .WithSummary("Refresh access token using refresh token")
        .Produces<AuthenticationResponse>(200)
        .Produces(401);

        // Logout endpoint
        auth.MapPost("/logout", async (RefreshTokenRequest request, AuthenticationService authService) =>
        {
            try
            {
                var response = await authService.LogoutAsync(request.RefreshToken);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .RequireAuthorization()
        .WithName("Logout")
        .WithSummary("Logout user and revoke refresh token")
        .Produces<LogoutResponse>(200)
        .Produces(400)
        .Produces(401);

        // Verify email endpoint
        auth.MapPost("/verify-email", async (EmailVerificationRequest request, AuthenticationService authService) =>
        {
            try
            {
                var response = await authService.VerifyEmailAsync(request.Token);
                return Results.Ok(response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .WithName("VerifyEmail")
        .WithSummary("Verify user email address")
        .Produces<EmailVerificationResponse>(200)
        .Produces(400);

        // Get current user endpoint
        auth.MapGet("/me", async (HttpContext context, AuthenticationService authService) =>
        {
            var userIdClaim = context.User.FindFirst("sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Results.Unauthorized();
            }

            // This would require a GetUserById method in AuthenticationService
            // For now, return basic info from claims
            var userInfo = new
            {
                Id = userId,
                Email = context.User.FindFirst("email")?.Value,
                FirstName = context.User.FindFirst("given_name")?.Value,
                LastName = context.User.FindFirst("family_name")?.Value,
                EmailVerified = bool.TryParse(context.User.FindFirst("email_verified")?.Value, out var verified) && verified
            };

            return Results.Ok(userInfo);
        })
        .RequireAuthorization()
        .WithName("GetCurrentUser")
        .WithSummary("Get current authenticated user information")
        .Produces(200)
        .Produces(401);

        return app;
    }
}