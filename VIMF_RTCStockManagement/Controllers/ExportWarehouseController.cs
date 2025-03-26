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

        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateExportWarehouse(string itemCode, int warehouseID, int positionID,
            [FromBody] List<SerialNumberDTO> lstSerial)
        {
            try
            {
                Material material = await _repo.FindModel<Material>(x => x.MaterialCode == itemCode);
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

        public async Task<IActionResult> ExportWarehouseModula(string itemCode, int warehouseID, int quantity)
        {
            // Check số lượng trong kho modula

            //Nếu k đủ thì báo lỗi

            //Tạo phiếu xuất kho modula

            // Tạo phiếu nhập kho gá con

            return Ok();
        }

        public string GenerateExportCode()
        {
            return "PXK" + DateTime.Now.ToString("yyMMddHHmmssfff");
        }
    }
}