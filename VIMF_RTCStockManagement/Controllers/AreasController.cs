using BMS.Models;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_RTCStockManagement.Controllers
{
    [Route("[controller]")]
    public class AreasController : Controller
    {
        private IGenericRepository _repo;
        public AreasController(IGenericRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Area> lstAreas = await _repo.GetAll<Area>();
                return Ok(lstAreas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Area area)
        {
            try
            {
                if (area.Id > 0)
                {
                    await _repo.Update(area);
                }
                else
                {
                    await _repo.Insert(area);
                }
                return Ok(area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Area area = await _repo.GetById<Area>(id);
                return Ok(area);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Area area = await _repo.GetById<Area>(id);
                await _repo.Delete(area);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
