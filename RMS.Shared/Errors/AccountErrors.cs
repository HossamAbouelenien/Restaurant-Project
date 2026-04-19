using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.Errors
{
   

    public static class AccountErrors
    {
        public const string FirstNameRequired = "First name is required";

        public static string FirstNameMaxLength(int length)
            => $"First name must not exceed {length} characters.";

        public const string LastNameRequired = "Last name is required";

        public static string LastNameMaxLength(int length)
            => $"Last name must not exceed {length} characters.";

        public const string UsernameRequired = "Username is required";

        public static string UsernameMaxLength(int length)
            => $"Username must not exceed {length} characters.";

        public const string UsernameTaken = "Username is already taken";
        public const string EmailRequired = "Email is required";
        public const string EmailInvalidFormat = "Invalid email format";
        public const string EmailTaken = "Email is already taken";
        public const string PasswordRequired = "Password is required";

        public const string PasswordRegexMessage =
            "Password must be at least 8 characters long, include uppercase, lowercase, number, and special character.";

        public const string PhoneNumberRequired = "Phone number is required";
        public const string PhoneNumberRegexMessage = "Phone number must be exactly 11 digits";
        public const string PhoneNumberTaken = "Phone number is already taken";

        public const string UserNotFound = "User not found";
        public const string CustomerNotFound = "Customer not found";
        public const string WorkerNotFound = "Worker not found";
        public const string UserNotWorker = "User must be a worker to join a category";
        public const string UserIdInvalidFormat = "Invalid User ID format";
        public const string UserHasNotAcceptedTerms = "User has not accepted terms";
        public const string UserIdRequired = "User ID is required";

        public static bool IsValidId(string id)
            => Guid.TryParse(id, out _);

        public const string InvalidCredentials = "Invalid identifier or password.";
        public const string EmailNotVerified = "Email is not verified.";

        public const string LoginError = "An error occurred while logging in.";
        public const string TermsAlreadyAccepted = "Terms already accepted";
        public const string IdentifierRequired = "Identifier is required";
        public const string RefreshTokenError = "An error occurred while refreshing token.";
        public const string TokenIsInvalid = "Token is invalid";
        public const string TokenIsRequired = "Token is required";
        public const string InvalidPayload = "Invalid payload";
        public const string TokenRequired = "Token is required";
        public const string PasswordResetError = "An error occurred while resetting password.";
        public const string NewPasswordSameAsOld = "New password cannot be the same as the old password.";
        public const string InvalidOldPassword = "The old password is incorrect.";
        public const string OldPasswordRequired = "Old password is required";
        public const string PasswordsDoNotMatch = "Passwords do not match.";
        public const string CustomerIdRequired = "Customer ID is required";
        public const string DateOfBirthRequired = "Date of birth is required";
        public const string DateOfBirthInvalid = "Date of birth is invalid";
        public const string DateOfBirthMinimumAge = "Worker must be at least 18 years old";
        public const string DateOfBirthFutureError = "Date of birth must be in the past";
        public const string DeleteFailed = "Failed to delete user";
        public const string UserAlreadyDeleted = "User already deleted";
        public const string GetFailed = "Failed to get user";
        public const string EmailAlreadyInUse = "Email is already in use";
        public const string InvalidEmailChangeToken = "Invalid email change token";
        public const string NotAcceptedTerms = "User has not accepted terms";
        public const string WorkerNotActive = "Worker is not active";
        public const string UserIsDeleted = "User is deleted";
        public const string UserAlreadyVerified = "User is already verified";
        public const string UserVerificationPending = "User verification is pending";
    }
}
