﻿using BMS.Models;
using BMS.Models.DTO;
using DASSytemAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace VIMF_RTCStockManagement.Controllers
{
    [Route("[controller]")]
    public class ImportWarehouseController : Controller
    {
        private IGenericRepository _repo;

        public ImportWarehouseController(IGenericRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetPositionItem")]
        public async Task<IActionResult> GetPositionItem(string itemCode, int warehouseID)
        {
            try
            {
                List<spGetPositionItemResult> result = await _repo.ExecuteStoredProcedure<spGetPositionItemResult>(
                        p => p.spGetPositionItemAsync(itemCode, warehouseID));
                return Ok(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("CreateImportWarehouse")]
        public async Task<IActionResult> CreateImportWarehouse(string itemCode, int warehouseID, int positionID,
            [FromBody] List<SerialNumberDTO> lstSerial)
        {
            Material material = await _repo.FindModel<Material>(x => x.MaterialCode == itemCode && x.WarehouseId == warehouseID);
            if (material is null || material.Id <= 0)
            {
                material = new Material
                {
                    MaterialCode = itemCode,
                    MaterialName = itemCode,
                    UnitId = 1,
                    WarehouseId = warehouseID,
                    TonDau = 0,
                    Nhap = 0,
                    Xuat = 0,
                    TonCuoi = 0,
                    Note = "",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "Admin",
                    UpdatedDate = DateTime.Now,
                    Inventory = 0,
                    PositionId = positionID
                };
                await _repo.Insert(material);
            }

            ImportWarehouse importWarehouse = new()
            {
                ImportCode = GenerateImportCode(),
                ImportWarehouseDate = DateTime.Now,
                WarehouseId = warehouseID,
                Status = 1,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                EmployeeId = 1,
                ExportWarehouseId = 0,
                Stt = 1,
                ImportType = 1
            };
            await _repo.Insert(importWarehouse);

            ImportWarehouseDetail importWarehouseDetail = new()
            {
                ImportWarehouseId = importWarehouse.Id,
                MaterialId = material.Id,
                Quantity = lstSerial.Count,
                PositionId = positionID
            };
            await _repo.Insert(importWarehouseDetail);

            foreach (var item in lstSerial)
            {
                ImportSerialNumber importSerialNumber = new()
                {
                    ImportWarehouseDetailId = importWarehouseDetail.Id,
                    SerialNumber = item.SerialNumber,
                    MaterialId = material.Id
                };
                await _repo.Insert(importSerialNumber);
            }

            return Ok(importWarehouse);
        }

        private string GenerateImportCode()
        {
            return "PNK" + DateTime.Now.ToString("yyMMddHHmmssfff");
        }

        [HttpPost("ChangePosition")]
        public async Task<IActionResult> ChangePosition(string itemCode, int warehouseID, int positionID, int newPositionID,
            [FromBody] List<SerialNumberDTO> lstSerial)
        {
            try
            {
                Material material = await _repo.FindModel<Material>(x => x.MaterialCode == itemCode);
                if (material is null || material.Id <= 0)
                {
                    return BadRequest("Không tồn tại vật tư trong kho!");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetListPositionEmpty")]
        public async Task<IActionResult> GetListPositionEmpty()
        {
            try
            {
                List<spGetEmptyPositionsResult> result =
                    await _repo.ExecuteStoredProcedure<spGetEmptyPositionsResult>(
                        p => p.spGetEmptyPositionsAsync());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("ImportWarehouseModula")]
        public async Task<IActionResult> ImportWarehouseModula(string itemCode, int warehouseID, int quantity)
        {
            // Check số lượng tồn trong kho Gá để nhập vào kho Modula

            // Nếu số lượng tồn không đủ thì thông báo lỗi

            // Tạo phiếu xuất cho kho gá con để chuyển sang kho Modula

            // Tạo phiếu nhập cho kho Modula

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTicket(int Id, int status)
        {
            try
            {
                var ticket = await _repo.GetById<ImportWarehouse>(Id);
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
                var ticket = await _repo.GetById<ImportWarehouse>(Id);
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
                List<ImportWarehouse> lstImportWarehouse = await _repo.GetAll<ImportWarehouse>();
                return Ok(lstImportWarehouse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(int importId, int actionType, int positionId)
        {
            try
            {
                var ticket = await _repo.Insert<ImportWarehouse>(new ImportWarehouse
                {
                    Id = 0,
                    ImportCode = GenerateImportCode(),
                    WarehouseId = importId,
                    ImportType = actionType,
                    PositionId = positionId
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
                List<ImportWarehouseDetail> lstDetails = await _repo.FindByExpression<ImportWarehouseDetail>
                    (d => d.ImportWarehouseId == id);
                return Ok(lstDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("SaveDetails")]
        public async Task<IActionResult> SaveDetails([FromBody] ImportWarehouseDetail detail)
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
                ImportWarehouseDetail detail = await _repo.GetById<ImportWarehouseDetail>(id);
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