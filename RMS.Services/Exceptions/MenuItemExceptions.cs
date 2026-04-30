using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{


    public sealed class MenuItemNotFoundException(int id)
         : NotFoundException(SharedResourcesKeys.MenuItemNotFound, id)
    { }

    public sealed class MenuItemNoIngredientsException()
        : ValidationException(SharedResourcesKeys.MenuItemNoIngredients)
    { }

    public sealed class DuplicateIngredientException()
        : ConflictException(SharedResourcesKeys.DuplicateIngredient)
    { }

    public sealed class InvalidIngredientIdsException(string ids)
        : ValidationException(SharedResourcesKeys.InvalidIngredientIds, ids)
    { }

    public sealed class ImageUploadFailedException()
        : BadRequestException(SharedResourcesKeys.ImageUploadFailed)
    { }




}












































































