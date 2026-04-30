using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{


    public sealed class RecipeNotFoundException(int id)
         : NotFoundException(SharedResourcesKeys.RecipeNotFound, id)
    { }

    public sealed class RecipeAlreadyExistsException(int menuItemId, int ingredientId)
        : ConflictException(SharedResourcesKeys.RecipeAlreadyExists, menuItemId, ingredientId)
    { }

    public sealed class RecipeAddFailedException()
        : BadRequestException(SharedResourcesKeys.RecipeAddFailed)
    { }

    public sealed class RecipeUpdateFailedException(int id)
        : BadRequestException(SharedResourcesKeys.RecipeUpdateFailed, id)
    { }

    public sealed class RecipeDeleteFailedException(int id)
        : BadRequestException(SharedResourcesKeys.RecipeDeleteFailed, id)
    { }











}


















