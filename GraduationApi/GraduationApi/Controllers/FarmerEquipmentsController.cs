using GraduationApi.Interfaces;
using GraduationApi.Models;
using GraduationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FarmerEquipmentsController : ControllerBase
    {

        private readonly IFarmerEquipmentServices _FarmerEquipment;
        private readonly IEquipmentServices _EquipmentServices;
        private readonly IFarmerServices _farmerServices;

        public FarmerEquipmentsController(IFarmerEquipmentServices FarmerEquipment, IEquipmentServices EquipmentServices, IFarmerServices farmerServices)
        {
            _FarmerEquipment = FarmerEquipment;
            _EquipmentServices = EquipmentServices;
            _farmerServices = farmerServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllFarmerEquipments()
        {
            var FarmerEquipments = await _FarmerEquipment.GetAllFarmerEquipments();
            if (FarmerEquipments == null)
                return NotFound("there is no equipment rent orders avaliable");

            return Ok(FarmerEquipments);
        }


        [HttpGet("{id}",Name = "GetFarmerEquipmentById")]
        public async Task<IActionResult> GetFarmerEquipmentById([FromRoute] int id)
        {
            var FarmerEquipments = await _FarmerEquipment.GetFarmerEquipmentById(id);
            if (FarmerEquipments == null)
                return NotFound($"there is no equipment rent orders avaliable for this id {id}");

            return Ok(FarmerEquipments);
        }


        [HttpPost]
        public async Task<IActionResult> AddFarmerEquipment(FarmerEquipmentDto dto)
        {
            var FarmerEquipment = new FarmerEquipment 
            {
                EquipmentId = dto.EquipmentId,
                FarmerId = dto.FarmerId,
                RentPrice = dto.RentPrice,
                RentStartDate = dto.RentStartDate,
                RentEndDate = dto.RentEndDate,
                EquipmentRentStatus = dto.EquipmentRentStatus,
                BuyerFarmerId = dto.BuyerFarmerId
            };

           var result = await _FarmerEquipment.AddFarmerEquipment(FarmerEquipment);

            return Ok(result);
        }


        [HttpPut("{id}",Name = "UpdateFarmerEquipment")]
        public async Task<IActionResult> UpdateFarmerEquipment([FromRoute]int id, [FromBody] FarmerEquipmentDto dto)
        {
           var FarmerEquipments = await _FarmerEquipment.GetFarmerEquipmentById(id);
            if (FarmerEquipments == null)
                return NotFound($"there is no avaliable product orders for this id {id}");

            var isValidEquipment = await _EquipmentServices.IsValidEquipment(dto.EquipmentId);
            if (!isValidEquipment)
                return BadRequest($"there is no valid Equipment for this id {dto.EquipmentId}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(dto.FarmerId);
            if (!isValidFarmer)
                return BadRequest($"there is no valid farmer for this id {dto.FarmerId}");

          FarmerEquipments.FarmerId = dto.FarmerId;
          FarmerEquipments.EquipmentId = dto.EquipmentId;
            FarmerEquipments.RentEndDate = dto.RentEndDate;
            FarmerEquipments.RentStartDate = dto.RentStartDate;
            FarmerEquipments.EquipmentRentStatus = dto.EquipmentRentStatus;
            FarmerEquipments.BuyerFarmerId = dto.BuyerFarmerId;

          var result =  _FarmerEquipment.UpdateFarmerEquipment(FarmerEquipments);
            return Ok(result);
        }


        [HttpDelete("{id}",Name = "DeleteFarmerEquipment")]
        public async Task<IActionResult> DeleteFarmerEquipment([FromRoute] int id)
        {
            var FarmerEquipments = await _FarmerEquipment.GetFarmerEquipmentById(id);
            if (FarmerEquipments == null)
                return NotFound($"there is no avaliable product orders for this id {id}");

           var result = _FarmerEquipment.DeleteFarmerEquipment(FarmerEquipments);
            return Ok(result);
        }


        [HttpGet("{id}", Name = "GetFarmerEquipmentsByFarmerId")]
        public async Task<IActionResult> GetFarmerEquipmentsByFarmerId([FromRoute] int id)
        {
            var records = await _FarmerEquipment.GetFarmerEquipmentsByFarmerId(id);
            if (records == null)
                return NotFound($"there was no records for this farmer id {id}");

            var farmerProducts = new List<FarmerEquipmentDetailsDto>();
            foreach (var record in records)
            {
                var equipments = await _EquipmentServices.GetEquipmentById(record.EquipmentId);
                var order = new FarmerEquipmentDetailsDto()
                {
                    Id = record.FarmerEquipmentId,
                    EquipmentId = record.EquipmentId,
                    FarmerId = record.FarmerId,
                    EquipmentName = equipments.EquipmentName,
                    FarmerName = record.Farmer.FarmerName,
                    RentPrice = record.RentPrice,
                    RentStartDate = record.RentStartDate,
                    RentEndDate = record.RentEndDate,
                    EquipmentRentStatus = record.EquipmentRentStatus,
                    OwnerPhone = record.Farmer.FarmerPhone,
                    BuyerFarmerId = record.BuyerFarmerId,                    
                };
                farmerProducts.Add(order);
            }
            return Ok(farmerProducts);
        }


        [HttpGet("{EquipmentId}", Name = "GetFarmerEquipmentsByEquipmentId")]
        public async Task<IActionResult> GetFarmerEquipmentsByEquipmentId([FromRoute] int EquipmentId)
        {
            var records = await _FarmerEquipment.GetFarmerEquipmentsByEquipmentId(EquipmentId);
            if (records == null)
                return NotFound($"there was no records for this Equipment id {EquipmentId}");

            var EquipmentProducts = new List<FarmerEquipmentDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerEquipmentDetailsDto()
                {
                    Id = record.FarmerEquipmentId,
                    EquipmentId = record.EquipmentId,
                    FarmerId = record.FarmerId,
                    EquipmentName = record.Equipment.EquipmentName,
                    FarmerName = farmers.FarmerName,
                    RentPrice  = record.RentPrice,
                    RentEndDate = record.RentEndDate,
                    RentStartDate = record.RentStartDate,
                    EquipmentRentStatus = record.EquipmentRentStatus,
                    OwnerPhone = farmers.FarmerPhone,
                    BuyerFarmerId = record.BuyerFarmerId,
                };
                EquipmentProducts.Add(order);
            }
            return Ok(EquipmentProducts);
        }


        [HttpGet("{EquipmentName}", Name = "GetFarmerEquipmentsByEquipmentName")]
        public async Task<IActionResult> GetFarmerEquipmentsByEquipmentName([FromRoute] string EquipmentName)
        {
            var records = await _FarmerEquipment.GetFarmerEquipmentsByEquipmentName(EquipmentName);
            if (records == null)
                return NotFound($"there was no records for this Equipment name {EquipmentName}");

            var EquipmentProducts = new List<FarmerEquipmentDetailsDto>();
            foreach (var record in records)
            {
                var farmers = await _farmerServices.GetFarmerById(record.FarmerId);
                var order = new FarmerEquipmentDetailsDto()
                {
                    Id = record.FarmerEquipmentId,
                    EquipmentId = record.EquipmentId,
                    FarmerId = record.FarmerId,
                    EquipmentName = record.Equipment.EquipmentName,
                    FarmerName = farmers.FarmerName,
                    RentPrice = record.RentPrice,
                    RentStartDate = record.RentStartDate,
                    RentEndDate = record.RentEndDate,
                    EquipmentRentStatus = record.EquipmentRentStatus,
                    OwnerPhone = farmers.FarmerPhone,
                    BuyerFarmerId = record.BuyerFarmerId,
                    
                };
                EquipmentProducts.Add(order);
            }
            return Ok(EquipmentProducts);
        }


        [HttpGet("{name}", Name = "GetFarmerEquipmentsByFarmerName")]
        public async Task<IActionResult> GetFarmerEquipmentsByFarmerName([FromRoute] string name)
        {
            var records = await _FarmerEquipment.GetFarmerEquipmentsByFarmerName(name);
            if (records == null)
                return NotFound($"there was no records for this farmer name {name}");

            var farmerProduct = new List<FarmerEquipmentDetailsDto>();
            foreach (var record in records)
            {
                var equipments = await _EquipmentServices.GetEquipmentById(record.EquipmentId);
                var order = new FarmerEquipmentDetailsDto()
                {
                    Id = record.FarmerEquipmentId,
                    EquipmentId = record.EquipmentId,
                    FarmerId = record.FarmerId,
                    EquipmentName = equipments.EquipmentName,
                    FarmerName = record.Farmer.FarmerName,
                    RentPrice = record.RentPrice,
                   RentStartDate = record.RentStartDate,
                   RentEndDate = record.RentEndDate,
                   EquipmentRentStatus = record.EquipmentRentStatus,
                   OwnerPhone = record.Farmer.FarmerPhone,
                   BuyerFarmerId = record.BuyerFarmerId,
                };
                farmerProduct.Add(order);
            }
            return Ok(farmerProduct);
        }
    }
}
