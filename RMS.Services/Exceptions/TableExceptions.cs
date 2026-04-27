using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
   

    public sealed class TableOrderNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.TableOrderNotFound, id)
    { }

  
    public sealed class TableNumberAlreadyExistsException(string tableNumber)
        : ConflictException(SharedResourcesKeys.TableNumberAlreadyExists, tableNumber)
    { }

    public sealed class OccupiedTableException(int id)
        : ConflictException(SharedResourcesKeys.OccupiedTable, id)
    { }

    public sealed class FreeTableException(int id)
        : ConflictException(SharedResourcesKeys.FreeTable, id)
    { }

    public sealed class CompletedTableOrderException(int id)
        : ConflictException(SharedResourcesKeys.CompletedTableOrder, id)
    { }

    public sealed class TableNumberDuplicateInBranchException(string tableNumber)
        : ConflictException(SharedResourcesKeys.TableBranch, tableNumber)
    { }

   
    public sealed class TableCapacityInvalidException()
        : ValidationException(SharedResourcesKeys.TableCapacityInvalid)
    { }

    public sealed class TableNumberRequiredException()
        : ValidationException(SharedResourcesKeys.TableNumberRequired)
    { }












}




















