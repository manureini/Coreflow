using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreflow.Helper;
using Coreflow.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coreflow.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowDefinitionsController : ControllerBase
    {
        private readonly IFlowDefinitionStorage mFlowDefinitionStorage;

        public FlowDefinitionsController(IFlowDefinitionStorage pFlowDefinitionStorage)
        {
            mFlowDefinitionStorage = pFlowDefinitionStorage;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(mFlowDefinitionStorage.GetDefinitions().Select(d => FlowDefinitionSerializer.Serialize(d)));
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            return Ok(FlowDefinitionSerializer.Serialize(mFlowDefinitionStorage.GetDefinitions().FirstOrDefault(d => d.Identifier == id)));
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            FlowDefinition flow = FlowDefinitionSerializer.Deserialize(value, null);
            mFlowDefinitionStorage.Add(flow);
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            mFlowDefinitionStorage.Remove(id);
        }
    }
}
