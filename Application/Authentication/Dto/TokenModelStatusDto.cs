namespace Application.Authentication.Dto
{
    public class TokenModelStatusDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public bool RequiresTwoFactor { get; set; }

        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        public static TokenModelStatusDto Success(string? token, string? refreshToken, bool requiresTwoFactor = false)
        {
            return new TokenModelStatusDto()
            {
                Token = token,
                RefreshToken = refreshToken,
                RequiresTwoFactor = requiresTwoFactor,
                Status = "success",
                Message = "Token is successfully generated."
            };
        }

        public static TokenModelStatusDto Success2FA(bool requiresTwoFactor)
        {
            return new TokenModelStatusDto()
            {
                Status = "success",
                Message = "Verify the code sent on your email.",
                RequiresTwoFactor = requiresTwoFactor
            };
        }

        public static TokenModelStatusDto InvalidClient()
        {
            return new TokenModelStatusDto()
            {
                Status = "error",
                Message = "Invalid client request."
            };
        }
        public static TokenModelStatusDto InvalidToken()
        {
            return new TokenModelStatusDto()
            {
                Status = "error",
                Message = "Invalid access token or refresh token."
            };
        }

        public static TokenModelStatusDto Unauthorized()
        {
            return new TokenModelStatusDto()
            {
                Status = "error",
                Message = "User is unauthorized."
            };
        }

        public static TokenModelStatusDto Locked()
        {
            return new TokenModelStatusDto()
            {
                Status = "error",
                Message = "User is locked."
            };
        }
        public static TokenModelStatusDto NotAllowed()
        {
            return new TokenModelStatusDto()
            {
                Status = "error",
                Message = "Please validate your email address"
            };
        }

        public static TokenModelStatusDto Invalid2FATokenValidation()
        {
            return new TokenModelStatusDto()
            {
                Status = "error",
                Message = "Invalid Token Verification.",
                RequiresTwoFactor = true
            };
        }
    }
}