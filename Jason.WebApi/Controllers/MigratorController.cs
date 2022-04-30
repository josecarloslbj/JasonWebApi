using Jason.WebApi.Entities;
using Jason.WebApi.Infra.Interfaces;
using Jason.WebApi.Migrations.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Jason.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MigratorController : ControllerBase
    {
        private readonly IMigratorService ms;
        private readonly AppDbContext context;

        public MigratorController(IMigratorService ms, AppDbContext context)
        {
            this.ms = ms;
            this.context = context;
        }

        [HttpGet]
        public ActionResult Version()
        {
            return Ok(new { Version = "1.00" });
        }

        [HttpGet("VersionInfo")]
        public ActionResult VersionInfo()
        {
            var recs = context.VersionInfo.OrderByDescending(v => v.Version);

            return Ok(recs);
        }

        [HttpGet("MigrateUp")]
        public ActionResult MigrateUp()
        {
            var resp = ms.MigrateUp();

            return Ok(resp);
        }

        [HttpGet("MigrateDown/{version}")]
        public ActionResult MigrateDown(long version)
        {
            var resp = ms.MigrateDown(version);

            return Ok(resp);
        }
    }
}
