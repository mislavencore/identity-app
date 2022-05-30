namespace Application.Authentication.Dto
{
    public class GeneralResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public static GeneralResponseDto EmailValidationSuccess()
        {
            return new GeneralResponseDto()
            {
                Status = "success",
                Message = "Email has been validated successfully."
            };
        }

        public static GeneralResponseDto InvalidRequest()
        {
            return new GeneralResponseDto()
            {
                Status = "error",
                Message = "Invalid Email Confirmation Request."
            };
        }
        public static GeneralResponseDto UserAlreadyExists()
        {
            return new GeneralResponseDto()
            {
                Status = "error",
                Message = "User already exists!"
            };
        }
        public static GeneralResponseDto UserCreatedSuccessfully()
        {
            return new GeneralResponseDto()
            {
                Status = "success",
                Message = "User created successfully!"
            };
        }
        public static GeneralResponseDto UserCreationFailure()
        {
            return new GeneralResponseDto()
            {
                Status = "error",
                Message = "User creation failed! Please check user details and try again."
            };
        }
    }
}