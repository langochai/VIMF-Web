using BMS.Models;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_API.Controllers
{
    [Route("")]
    [ApiController]
    public class AgvController : ControllerBase
    {
        IGenericRepository _repo;
        public AgvController(IGenericRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("rack-to-table/pick")]
        public async Task<IActionResult> RackToTablePick([FromBody] PostPickAgv postPickAgv)
        {
            //Đã lấy hàng thành công
            // Lấy ra phiếu xuất hàng update lại trạng thái phiếu xuất hàng
            try
            {
                AgvWork agvWork = new AgvWork();
                agvWork.AgvType = 1;// 1 là lấy hàng từ rack
                agvWork.Status = 1;
                agvWork.AgvWork1 = "Lấy hàng từ rack";
                agvWork.DateRun = DateTime.Now;
                await _repo.Insert(agvWork);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.ToString() });
            }

            return Ok("{\"message\": \"success\"}");
        }

        [HttpPost("rack-to-table/drop")]
        public async Task<IActionResult> RackToTableDrop([FromBody] PostDropAgv postDropAgv)
        {
            try
            {
                AgvWork agvWork = new AgvWork();
                agvWork.AgvType = 2;// 2 là drop từ rack xuống bàn
                agvWork.Status = 1;
                agvWork.AgvWork1 = "Drop xuống bàn";
                agvWork.DateRun = DateTime.Now;
                await _repo.Insert(agvWork);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.ToString() });
            }

            return Ok("{\"message\": \"success\"}");
        }

        [HttpPost("table-to-rack/pick")]
        public async Task<IActionResult> TableToRackPick([FromBody] PostPickAgv postPickAgv)
        {
            try
            {
                AgvWork agvWork = new AgvWork();
                agvWork.AgvType = 3;// 2 là drop từ rack xuống bàn
                agvWork.Status = 1;
                agvWork.AgvWork1 = "Lấy từ bàn";
                agvWork.DateRun = DateTime.Now;
                await _repo.Insert(agvWork);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.ToString() });
            }
            return Ok("{\"message\": \"success\"}");
        }

        [HttpPost("table-to-rack/drop")]
        public async Task<IActionResult> TableToRackDrop([FromBody] PostDropAgv postDropAgv)
        {
            try
            {
                AgvWork agvWork = new AgvWork();
                agvWork.AgvType = 3;// 2 là drop từ rack xuống bàn
                agvWork.Status = 1;
                agvWork.AgvWork1 = "Drop xuống rack";
                agvWork.DateRun = DateTime.Now;
                await _repo.Insert(agvWork);
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.ToString() });
            }

            return Ok("{\"message\": \"success\"}");
        }
    }
}
