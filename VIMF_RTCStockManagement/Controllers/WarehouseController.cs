using BMS.Models;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_RTCStockManagement.Controllers
{
    [Route("[controller]")]
    public class WarehouseController : Controller
    {
        private IGenericRepository _repo;

        public WarehouseController(IGenericRepository repo)
        {
            _repo = repo;
        }

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
                List<Warehouse> lstWarehouse = await _repo.GetAll<Warehouse>();
                return Ok(lstWarehouse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] Warehouse warehouse)
        {
            try
            {
                if (warehouse.Id > 0)
                {
                    await _repo.Update(warehouse);
                }
                else
                {
                    await _repo.Insert(warehouse);
                }
                return Ok(warehouse);
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
                Warehouse warehouse = await _repo.GetById<Warehouse>(id);
                return Ok(warehouse);
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
                Warehouse warehouse = await _repo.GetById<Warehouse>(id);
                await _repo.Delete(warehouse);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}