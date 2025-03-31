using BMS.Models;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_RTCStockManagement.Controllers
{
    [Route("[controller]")]
    public class MaterialController : Controller
    {
        private IGenericRepository _repo;
        public MaterialController(IGenericRepository repo)
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
                List<Material> materials = await _repo.GetAll<Material>();
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Material material)
        {
            try
            {
                if (material.Id > 0)
                {
                    await _repo.Update(material);
                }
                else
                {
                    await _repo.Insert(material);
                }
                return Ok(material);
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
                Material material = await _repo.GetById<Material>(id);
                return Ok(material);
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
                Material material = await _repo.GetById<Material>(id);
                await _repo.Delete(material);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
