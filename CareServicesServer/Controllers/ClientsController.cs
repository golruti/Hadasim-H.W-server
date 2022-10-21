using CareServicesServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareServicesServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {

        private readonly ILogger<ClientsController> _logger;
        private CareServicesDbContext db;
        public ClientsController(ILogger<ClientsController> logger, CareServicesDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        [HttpGet(Name = "GetClients")]
        //[Route("GetClients")]
        public IActionResult GetClients()
        {
            var list = db.Clients.ToList();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        //[Route("GetClientById/{Id}")]
        public IActionResult GetClientById(int Id)
        {
            var data = db.Clients.FirstOrDefault(x => x.Id == Id);
            CoronaData CoronaData = null;
            List<CoronaVaccineData> CoronaVaccineData = new List<CoronaVaccineData>();
            if (data != null)
            {
                CoronaData = db.CoronaData.FirstOrDefault(x => x.ClientId == data.Id);
                if (CoronaData != null)

                    CoronaVaccineData = db.CoronaVaccineData.Where(x => x.CoronaDataId == CoronaData.Id).ToList();

            }
            return Ok(new { data = data, CoronaData = CoronaData, CoronaVaccineData = CoronaVaccineData });
        }

        //[Route("AddClient")]
        [HttpPost(Name = "AddClient")]
        public async Task<IActionResult> AddClient([FromBody] ClientDtoModel ClientDtoModel)
        {
            var res = await db.Clients.AddAsync(new Clients()
            {
                City = ClientDtoModel.City,
                DateOfBirth = ClientDtoModel.DateOfBirth,
                FirstName = ClientDtoModel.FirstName,
                HouseNumber = ClientDtoModel.HouseNumber,
                LastName = ClientDtoModel.LastName,
                MobilePhone = ClientDtoModel.MobilePhone,
                Phone = ClientDtoModel.Phone,
                Street = ClientDtoModel.Street,
                Tz = ClientDtoModel.Tz

            });
            await db.SaveChangesAsync();
            ClientDtoModel.CoronaData.ClientId = res.Entity.Id;
            var coronaData = await db.CoronaData.AddAsync(new CoronaData()
            {
                ClientId = ClientDtoModel.CoronaData.ClientId,
                RecoverDate = ClientDtoModel.CoronaData.RecoverDate,
                PositiveDate = ClientDtoModel.CoronaData.PositiveDate
            });
            await db.SaveChangesAsync();

            if (ClientDtoModel.CoronaData.CoronaVaccineData != null)
            {
                List<CoronaVaccineData> CoronaVaccineDataList = new List<CoronaVaccineData>();
                foreach (var item in ClientDtoModel.CoronaData.CoronaVaccineData)
                {
                    CoronaVaccineDataList.Add(new CoronaVaccineData()
                    {
                        CoronaDataId = coronaData.Entity.Id,
                        DateReceiptVaccination = item.DateReceiptVaccination,
                        VaccineManufacturer = item.VaccineManufacturer
                    });
                }
                await db.CoronaVaccineData.AddRangeAsync(CoronaVaccineDataList);
                await db.SaveChangesAsync();
            }
            return Ok();
        }

        //[Route("UpdateClient")]
        [HttpPut(Name = "UpdateClient")]
        public IActionResult UpdateClient([FromBody] ClientDtoModel client)
        {
            var cl = db.Clients.FirstOrDefault(x => x.Id == client.Id);
            if (cl != null)
            {
                cl.FirstName = client.FirstName;
                cl.LastName = client.LastName;
                cl.Phone = client.Phone;
                cl.MobilePhone = client.MobilePhone;
                cl.DateOfBirth = client.DateOfBirth;
                cl.City = client.City;
                cl.Street = client.Street;
                cl.HouseNumber = client.HouseNumber;
            }
            db.SaveChanges();

            var cd = db.CoronaData.FirstOrDefault(x => x.Id == client.CoronaData.Id);
            if (cd != null)
            {
                cd.PositiveDate = client.CoronaData.PositiveDate;
                cd.RecoverDate = client.CoronaData.RecoverDate;
            }
            else
            {
                var coronaData = db.CoronaData.Add(new CoronaData()
                {
                    ClientId = client.Id,
                    RecoverDate = client.CoronaData.RecoverDate,
                    PositiveDate = client.CoronaData.PositiveDate
                });
                db.SaveChanges();
                client.CoronaData.Id = coronaData.Entity.Id;
            }
            db.SaveChanges();

            if (client.CoronaData.CoronaVaccineData != null)
            {
                List<CoronaVaccineData> CoronaVaccineDataList = new List<CoronaVaccineData>();

                foreach (var item in client.CoronaData.CoronaVaccineData)
                {
                    if (item.Id == 0)
                    {
                        CoronaVaccineDataList.Add(new CoronaVaccineData()
                        {
                            CoronaDataId = client.CoronaData.Id,
                            DateReceiptVaccination = item.DateReceiptVaccination,
                            VaccineManufacturer = item.VaccineManufacturer
                        });

                    }
                    else
                    {
                        var cv = db.CoronaVaccineData.FirstOrDefault(x => x.Id == item.Id);
                        if (cv != null)
                        {
                            cv.VaccineManufacturer = item.VaccineManufacturer;
                            cv.DateReceiptVaccination = item.DateReceiptVaccination;
                        }
                    }
                }
                if (CoronaVaccineDataList.Count > 0)
                    db.CoronaVaccineData.AddRange(CoronaVaccineDataList);
                db.SaveChanges();
                if (client.ToremoveCoronaVaccineData != null && client.ToremoveCoronaVaccineData.Count > 0)
                {
                    var toRemove = db.CoronaVaccineData.Where(x => client.ToremoveCoronaVaccineData.Contains(x.Id)).ToList();
                    db.CoronaVaccineData.RemoveRange(toRemove);
                    db.SaveChanges();
                }
            }

            return Ok();
        }


        [HttpDelete("{Id}")]
        public IActionResult DeleteClient(int Id)
        {
            var client = db.Clients.FirstOrDefault(x => x.Id == Id);
            if (client != null)
            {
                var coronaData = db.CoronaData.FirstOrDefault(x => x.ClientId == Id);
                if (coronaData != null)
                {
                    var list = db.CoronaVaccineData.Where(x => x.CoronaDataId == coronaData.Id).ToList();
                    if (list.Count > 0)
                    {
                        db.CoronaVaccineData.RemoveRange(list);
                    }
                    db.CoronaData.Remove(coronaData);
                }
                db.Clients.Remove(client);
                db.SaveChanges();
            }
            return Ok();
        }
        //[Route("AddCoronaData")]
        //[HttpPost(Name = "AddCoronaData")]
        //public IActionResult AddCoronaData(CoronaData CoronaData)
        //{
        //    var res = db.CoronaData.Add(CoronaData);
        //    db.SaveChanges();
        //    return Ok(res);
        //}

        //[Route("UpdateCoronaData")]
        //[HttpPost(Name = "UpdateCoronaData")]
        //public IActionResult UpdateCoronaData(CoronaData CoronaData)
        //{
        //    var cd = db.CoronaData.FirstOrDefault(x => x.Id == CoronaData.Id);
        //    if (cd != null)
        //    {
        //        cd.PositiveDate = CoronaData.PositiveDate;
        //        cd.RecoverDate = CoronaData.RecoverDate;
        //    }
        //    db.SaveChanges();
        //    return Ok(cd);
        //}

        //[Route("AddCoronaVaccineData")]
        //[HttpPost(Name = "AddCoronaVaccineData")]
        //public IActionResult AddCoronaVaccineData(CoronaVaccineData CoronaVaccineData)
        //{
        //    var res = db.CoronaVaccineData.Add(CoronaVaccineData);
        //    db.SaveChanges();
        //    return Ok(res);
        //}

        //[Route("UpdateCoronaVaccineData")]
        //[HttpPost(Name = "UpdateCoronaVaccineData")]
        //public IActionResult UpdateCoronaVaccineDataa(CoronaVaccineData CoronaVaccineData)
        //{
        //    var cd = db.CoronaVaccineData.FirstOrDefault(x => x.Id == CoronaVaccineData.Id);
        //    if (cd != null)
        //    {
        //        cd.VaccineManufacturer = CoronaVaccineData.VaccineManufacturer;
        //        cd.DateReceiptVaccination = CoronaVaccineData.DateReceiptVaccination;
        //    }
        //    db.SaveChanges();
        //    return Ok(cd);
        //}
    }
}