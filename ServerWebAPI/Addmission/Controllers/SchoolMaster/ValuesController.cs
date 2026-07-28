using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerWebAPI.Addmission.Controllers.SchoolMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public string valy = "hello i'm a good girl";
    }
}
