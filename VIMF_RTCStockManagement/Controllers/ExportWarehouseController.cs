using BMS.Models;
using BMS.Models.DTO;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_RTCStockManagement.Controllers
{
    [Route("[controller]")]
    public class ExportWarehouseController : Controller
    {
        private IGenericRepository _repo;

        public ExportWarehouseController(IGenericRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateExportWarehouse(string itemCode, int warehouseID, int positionID,
            [FromBody] List<SerialNumberDTO> lstSerial)
        {
            try
            {
                Material material = await _repo.FindModel<Material>(x => x.MaterialCode == itemCode && x.WarehouseId == warehouseID);
                if (material == null) return BadRequest("Không tìm thấy vật tư!");

                ExportWarehouse exportWarehouse = new()
                {
                    ExportCode = GenerateExportCode(),
                    WarehouseId = warehouseID,
                    Status = 1,
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    EmployeeId = 1,
                    Stt = 1
                };
                await _repo.Insert(exportWarehouse);

                ExportWarehouseDetail exportWarehouseDetail = new()
                {
                    ExportWarehouseId = exportWarehouse.Id,
                    MaterialId = material.Id,
                    PositionId = positionID,
                    Status = 1,
                    StatusPriority = 1,
                    RequestDate = DateTime.Now,
                    Quantity = lstSerial.Count
                };
                await _repo.Insert(exportWarehouseDetail);

                foreach (var item in lstSerial)
                {
                    ExportSerialNumber exportSerialNumber = new()
                    {
                        ExportWarehouseDetailId = exportWarehouseDetail.Id,
                        SerialNumber = item.SerialNumber,
                        MaterialId = material.Id
                    };
                    await _repo.Insert(exportSerialNumber);
                }

                return Ok(exportWarehouse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public async Task<IActionResult> ExportWarehouse(string itemCode, int warehouseID, int quantity)
        {

            Material material = await _repo.FindModel<Material>(x => x.MaterialCode == itemCode && x.WarehouseId == warehouseID);
            if (material == null)
            {
                return BadRequest("KHông tồn tại Vật tư");
            }
            ExportWarehouse exportWarehouse = new()
            {
                ExportCode = GenerateExportCode(),
                WarehouseId = warehouseID,
                Status = 1,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                EmployeeId = 1,
                Stt = 1
            };

            await _repo.Insert(exportWarehouse);

            ExportWarehouseDetail exportWarehouseDetail = new()
            {
                ExportWarehouseId = exportWarehouse.Id,
                MaterialId = material.Id,
                Status = 1,
                StatusPriority = 1,
                RequestDate = DateTime.Now,
                Quantity = quantity
            };

            return Ok();
        }

        private string GenerateExportCode()
        {
            return "PXK" + DateTime.Now.ToString("yyMMddHHmmssfff");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTicket(int Id, int status)
        {
            try
            {
                var ticket = await _repo.GetById<ExportWarehouse>(Id);
                if (ticket is null) return BadRequest(new { Message = "Phiếu nhập không tồn tại" });
                ticket.Status = status;
                await _repo.Update(ticket);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetDetails")]
        public async Task<IActionResult> GetDetails(int Id)
        {
            try
            {
                var ticket = await _repo.GetById<ExportWarehouse>(Id);
                if (ticket is null) return BadRequest(new { Message = "Phiếu nhập không tồn tại" });
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ExportWarehouse> lstExportWarehouse = await _repo.GetAll<ExportWarehouse>();
                return Ok(lstExportWarehouse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(int exportId)
        {
            try
            {
                var ticket = await _repo.Insert<ExportWarehouse>(new ExportWarehouse
                {
                    Id = 0,
                    ExportCode = GenerateExportCode(),
                    WarehouseId = exportId
                });
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetTicket")]
        public async Task<IActionResult> GetTicket(int id)
        {
            try
            {
                List<ExportWarehouseDetail> lstDetails = await _repo.FindByExpression<ExportWarehouseDetail>
                    (d => d.ExportWarehouseId == id);
                return Ok(lstDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("SaveDetails")]
        public async Task<IActionResult> SaveDetails([FromBody] ExportWarehouseDetail detail)
        {
            try
            {
                if (detail.Id > 0)
                {
                    await _repo.Update(detail);
                }
                else
                {
                    await _repo.Insert(detail);
                }
                return Ok(detail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("DeleteDetails")]
        public async Task<IActionResult> DeleteDetails(int id)
        {
            try
            {
                ExportWarehouseDetail detail = await _repo.GetById<ExportWarehouseDetail>(id);
                await _repo.Delete(detail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}