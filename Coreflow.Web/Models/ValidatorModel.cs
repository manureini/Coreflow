using Coreflow.Validation;
using System;
using System.Collections.Generic;

namespace Coreflow.Web.Models
{
    public class ValidatorModel
    {
        public FlowDefinitionModel FlowDefinition { get; set; }

        public FlowValidationResult ValidationResult { get; set; }

        public List<(Guid, ICorrector)> Correctors { get; set; }
    }
}
