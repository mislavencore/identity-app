namespace Application.Authentication.Dto
{
    public class PasswordChangeResponseDto
    {
        public string? Email { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }

        public static PasswordChangeResponseDto BadRequest(string email)
        {
            return new PasswordChangeResponseDto()
            {
                Status = "error",
                Message = "Some issue appeared.",
                Email = email
            };
        }

        public static PasswordChangeResponseDto ForgottenPasswordSuccess(string email)
        {
            return new PasswordChangeResponseDto()
            {
                Status = "success",
                Message = "Email for password change has been sent.",
                Email = email
            };
        }

        public static PasswordChangeResponseDto PasswordResetSuccess(string email)
        {
            return new PasswordChangeResponseDto()
            {
                Status = "success",
                Message = "Password has been changed.",
                Email = email
            };
        }

        public static PasswordChangeResponseDto PasswordsDoNotMatch(string email)
        {
            return new PasswordChangeResponseDto()
            {
                Status = "error",
                Message = "Passwords do not match.",
                Email = email
            };
        }
        public static PasswordChangeResponseDto TokenError(string email)
        {
            return new PasswordChangeResponseDto()
            {
                Status = "error",
                Message = "Your token is invalid. Are you a hacker?",
                Email = email
            };
        }
        public static PasswordChangeResponseDto ExternalUser(string email)
        {
            return new PasswordChangeResponseDto()
            {
                Status = "error",
                Message = "You are trying to sign in with external user.",
                Email = email
            };
        }
    }
}