using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Exceptions.Base
{
    public abstract class AppException(string messageKey , params object[] args) : Exception(messageKey)
    {

        public object[] Args { get; set; } = args;


    }


    public abstract class NotFoundException(string messageKey, params object[] args)
        : AppException(messageKey, args) { }


    public abstract class ConflictException(string messageKey, params object[] args)
        : AppException(messageKey, args) { }



    public abstract class BadRequestException(string messageKey, params object[] args)
       : AppException(messageKey, args) { }



    public abstract class ValidationException(string messageKey, params object[] args)
       : AppException(messageKey, args) { }














}

























































