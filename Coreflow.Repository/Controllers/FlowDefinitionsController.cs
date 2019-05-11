using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Coreflow.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowDefinitionsController : ControllerBase
    {
        private string mPath = "Flows";

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            List<string> ret = new List<string>();

            foreach (string file in Directory.GetFiles(mPath))
            {
                ret.Add(System.IO.File.ReadAllText(file));
            }

            return Ok(ret);
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            return null;
        }

        [HttpPost("{id}")]
        public void Post([FromBody] string value, Guid id)
        {
            string filename = Path.Combine(mPath, id.ToString());
            System.IO.File.WriteAllText(filename, value);
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            string filename = Path.Combine(mPath, id.ToString());
            System.IO.File.Delete(filename);
        }
    }
}
