using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class CategoryNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.CategoryNotFound, id)
    { }

    public sealed class CategoryAlreadyExistsException(string name)
        : ConflictException(SharedResourcesKeys.CategoryAlreadyExists, name)
    { }

    public sealed class CategoryNameRequiredException()
        : ValidationException(SharedResourcesKeys.CategoryNameRequired)
    { }

    public sealed class CategoryHasMenuItemsException(int id)
        : ConflictException(SharedResourcesKeys.DeleteCategoryWithMenuItems, id)
    { }
}
























































































