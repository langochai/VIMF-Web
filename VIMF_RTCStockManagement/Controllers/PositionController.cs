using BMS.Models;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_RTCStockManagement.Controllers
{
    [Route("[controller]")]
    public class PositionController : Controller
    {
        private IGenericRepository _repo;
        public PositionController(IGenericRepository repo)
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
                List<Position> lstPositions = await _repo.GetAll<Position>();
                return Ok(lstPositions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Position position)
        {
            try
            {
                if (position.Id > 0)
                {
                    await _repo.Update(position);
                }
                else
                {
                    await _repo.Insert(position);
                }
                return Ok(position);
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
                Position position = await _repo.GetById<Position>(id);
                return Ok(position);
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
                Position position = await _repo.GetById<Position>(id);
                await _repo.Delete(position);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
