using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class EntityHasRelatedRecordsException : Exception
    {
        public EntityHasRelatedRecordsException(
             string message = "No se puede eliminar la entidad porque existen registros relacionados.",
             Exception? innerException = null)
             : base(message, innerException)
        {
        }
    }
}