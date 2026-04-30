using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class IngredientNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.IngredientNotFound, id)
    { }

    public sealed class IngredientAlreadyExistsException(string name)
        : ConflictException(SharedResourcesKeys.IngredientAlreadyExists, name)
    { }

    public sealed class IngredientNameRequiredException()
        : ValidationException(SharedResourcesKeys.IngredientNameRequired)
    { }
}