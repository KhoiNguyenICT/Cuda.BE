using System;
using System.Collections.Generic;
using System.Text;
using Cuda.Common.Interfaces;

namespace Cuda.Model.Entities
{
    public class BaseEntity: IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
