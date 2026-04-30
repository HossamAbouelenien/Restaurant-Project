using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class EmailNotConfirmedException(string email)
         : BadRequestException(SharedResourcesKeys.EmailNotConfirmed, email)
    { }

    public sealed class EmailAlreadyExistsException(string email)
        : ConflictException(SharedResourcesKeys.EmailAlreadyExists, email)
    { }

    public sealed class RegistrationFailedException(string errors)
        : BadRequestException(SharedResourcesKeys.RegistrationFailed, errors)
    { }

    public sealed class AuthException()
        : BadRequestException(SharedResourcesKeys.AuthError)
    { }


    public sealed class UserNotFoundException(string email)
    : NotFoundException(SharedResourcesKeys.UserNotFound, email)
    { }






}
