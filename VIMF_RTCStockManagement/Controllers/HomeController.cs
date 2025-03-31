using BMS.Models;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_RTCStockManagement.Controllers
{
    public class HomeController : Controller
    {
        IGenericRepository _repo;
        public HomeController(IGenericRepository repo)
        {
            _repo = repo;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("[controller]/GetPosition")]
        public async Task<IActionResult> GetPositionWarehouse(int warehouseID)
        {
            try
            {
                List<spGetPositionByWarehouseIDResult> result = await _repo.ExecuteStoredProcedure(
                    p => p.spGetPositionByWarehouseIDAsync(warehouseID));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}