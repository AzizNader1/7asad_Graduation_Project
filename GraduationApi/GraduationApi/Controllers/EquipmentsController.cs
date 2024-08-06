using GraduationApi.Data;
using GraduationApi.Interfaces;
using GraduationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GraduationApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly IEquipmentServices _EquipmentServices;
        private readonly IFarmerServices _farmerServices;
        private readonly ApplicationDbContext _context;

        public EquipmentsController(IEquipmentServices EquipmentServices, IFarmerServices farmerServices, ApplicationDbContext context)
        {
            _EquipmentServices = EquipmentServices;
            _farmerServices = farmerServices;
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEquipments()
        {
            var Equipments = await _EquipmentServices.GetAllEquipments();
            if (Equipments == null)
                return NotFound("there is no Equipments avaliable");

            return Ok(Equipments);
        }


        [HttpGet("{id}", Name = "GetEquipmentById")]
        public async Task<IActionResult> GetEquipmentById([FromRoute] int id)
        {
            var Equipment = await _EquipmentServices.GetEquipmentById(id);
            if (Equipment == null)
                return NotFound($"there is no avaliable Equipments for this id {id}");

            return Ok(Equipment);
        }


        [HttpDelete("{id}", Name = "DeleteEquipment")]
        public async Task<IActionResult> DeleteEquipment([FromRoute] int id)
        {
            var Equipment = await _EquipmentServices.GetEquipmentById(id);
            if (Equipment == null)
                return NotFound($"there is no avaliable Equipments for this {id}");
          var file =  _context.FileInformations.FirstOrDefault(a => a.EquipmentId == id);
            _context.FileInformations.Remove(file);
            _context.SaveChanges();
           var result = _EquipmentServices.DeleteEquipment(Equipment);
            return Ok(result);
        }


        [HttpPut("{id}", Name = "UpdateEquipment")]
        public async Task<IActionResult> UpdateEquipment([FromRoute] int id, [FromBody] EquipmentDto EquipmentDto)
        {
            var Equipment = await _EquipmentServices.GetEquipmentById(id);
            if (Equipment == null)
                return NotFound($"there is no Equipments for this id {id}");

            var isValidFarmer = await _farmerServices.IsValidFarmer(EquipmentDto.FarmerId);
            if (!isValidFarmer)
                return NotFound($"there is no valid farmers for this id {EquipmentDto.FarmerId}");
            
            Equipment.EquipmentName = EquipmentDto.EquipmentName;
            Equipment.EquipmentPrice = EquipmentDto.EquipmentPrice;
            Equipment.FarmerId = EquipmentDto.FarmerId;
            Equipment.EquipmentDescribtion = EquipmentDto.EquipmentDescribtion;

           var result = _EquipmentServices.UpdateEquipment(Equipment);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddEquipment([FromBody] EquipmentDto EquipmentDto)
        {

            var Equipment = new Equipment()
            {
                EquipmentName = EquipmentDto.EquipmentName,
                EquipmentPrice = EquipmentDto.EquipmentPrice,
                FarmerId = EquipmentDto.FarmerId,
                EquipmentDescribtion = EquipmentDto.EquipmentDescribtion
            };
           var result = _EquipmentServices.AddEquipment(Equipment);
            return Ok(result);
        }


        [HttpGet("{FarmerId}", Name = "GetEquipmentsByFarmerId")]
        public async Task<IActionResult> GetEquipmentsByFarmerId([FromRoute] int FarmerId)
        {
            var records = await _EquipmentServices.GetEquipmentsByFarmerId(FarmerId);
            if (records == null)
                return NotFound($"there was no Equipments for this farmer id {FarmerId}");

            var farmerEquipments = new List<EquipmentDetailsDto>();
            foreach (var record in records)
            {
                var order = new EquipmentDetailsDto()
                {
                    Id = record.EquipmentId,
                    FarmerId = record.FarmerId,
                    FarmerName = record.Farmer.FarmerName,
                    EquipmentName = record.EquipmentName,
                    EquipmentDescribtion = record.EquipmentDescribtion,
                    EquipmentPrice = record.EquipmentPrice
                };
                farmerEquipments.Add(order);
            }
            return Ok(farmerEquipments);
        }


        [HttpGet("{FarmerName}", Name = "GetEquipmentsByFarmerName")]
        public async Task<IActionResult> GetEquipmentsByFarmerName([FromRoute] string FarmerName)
        {
            var records = await _EquipmentServices.GetEquipmentsByFarmerName(FarmerName);
            if (records == null)
                return NotFound($"there was no Equipments for this farmer name {FarmerName}");

            var farmerEquipments = new List<EquipmentDetailsDto>();
            foreach (var record in records)
            {
                var order = new EquipmentDetailsDto()
                {
                    Id = record.EquipmentId,
                    FarmerId = record.FarmerId,
                    FarmerName = record.Farmer.FarmerName,
                    EquipmentName = record.EquipmentName,
                    EquipmentDescribtion = record.EquipmentDescribtion,
                    EquipmentPrice = record.EquipmentPrice
                };
                farmerEquipments.Add(order);
            }
            return Ok(farmerEquipments);
        }
    }
}
