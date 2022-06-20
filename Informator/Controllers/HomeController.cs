using Informator.Database;
using Informator.Models;
using Informator.SendServices;
using Informator.SendServices.Telegram;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Informator.Controllers
{
    
    public class HomeController : Controller
    {
        private InformatorContext _context;

        private readonly HandlerManager _handlerManager;
        public HomeController(HandlerManager handlerManager, InformatorContext context)
        {
            _handlerManager = handlerManager;
            _context = context;
        }
        public IActionResult SendMessPage()
        {
            return View();
        }

        public IActionResult MyProfilePage()
        {
            return View();
        }

        public IActionResult MailingListsPage()
        {
            return View();
        }

        public IActionResult AddresseesPage()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetContacts()
        {
            //var contactTask = Task.Run(() => _context.Contacts.Include(p=>p.User).Include(p=>p.SystemType).ToList());
            var contactTask = Task.Run(() => _context.DataUsers.Include(p => p.Contacts).ToList());
            var contactList = await contactTask;
            return Json(JsonConvert.SerializeObject(contactList, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        


        [HttpGet]
        public async Task<JsonResult> GetGroups()
        {

            var groupTask = Task.Run(() => _context.Groups.Select(p => new Group { Id = p.Id, Name = p.Name, Members = p.Members.ToList() }).ToList());
            var groupList = await groupTask;
            return Json(JsonConvert.SerializeObject(groupList, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        public JsonResult GetUpdatedGroup(Group group)
        {
            var users = _context.DataUsers.Select(p=> new DataUser
            {
                Id = p.Id,
                Contacts=p.Contacts,
                Groups=p.Groups,
                FirstName=p.FirstName,
                Email=p.Email,
                MiddleName=p.MiddleName,
                PhoneNumber=p.PhoneNumber,
                PlaceOfWork=p.PlaceOfWork,
                SecondName=p.SecondName,
                UserIdentity=p.UserIdentity
                
            }).AsNoTracking().ToList();
            UpdatedGroup updatedGroup = new UpdatedGroup();
            updatedGroup.Id = group.Id;
            updatedGroup.Name = group.Name;
            updatedGroup.UsersIntoGroup = new List<DataUser>();
            updatedGroup.UsersOutGroup = new List<DataUser>();
            foreach (var item in users)
            {
                if (item.Groups.Any(p=>p.Id==group.Id))
                {
                    updatedGroup.UsersIntoGroup.Add(item);
                }
                else
                {
                    updatedGroup.UsersOutGroup.Add(item);
                }
            }
           
            return Json(JsonConvert.SerializeObject(updatedGroup, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        public JsonResult UpdateGroup(UpdatedGroup updatedGroup)
        {
            var group = _context.Groups.Include(p=>p.Members).FirstOrDefault(p => p.Id == updatedGroup.Id);
            group.Members.Clear();
            group.Name = updatedGroup.Name;
            foreach (var item in updatedGroup.UsersIntoGroup)
            {
                group.Members.Add(_context.DataUsers.Find(item.Id));
            }
            _context.SaveChanges();
            var groupList = _context.Groups.Select(p => new Group { Id = p.Id, Name = p.Name, Members = p.Members.ToList() }).ToList();
            return Json(JsonConvert.SerializeObject(
                groupList,
                Formatting.None,
                new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        }

        [HttpPost]
        public void DeleteGroup(Group group)
        {
            _context.Groups.Remove(_context.Groups.FirstOrDefault(p => p.Id.Equals(group.Id)));
            _context.SaveChanges();
            //return Json(JsonConvert.SerializeObject(_context.Groups.ToList(), Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        public void CreateNewMailingList(Group group)
        {
            
            _context.Groups.Attach(group).State = EntityState.Added;
            _context.SaveChanges();
        }

        [HttpPost]
        public void SendSelectedContacts(SelectContact result)
        {
           

            if (result.Contacts == null && result.Groups == null)
            {
                return;
            }           
            var users = _context.DataUsers.Include(p => p.Contacts).Include(p=>p.Groups).ToList();
            var contacts = new List<Contact>();
            if (result.Contacts != null)
            {
                result.Contacts.ForEach(p => contacts.AddRange(p.Contacts));
                
            }
            if (result.Groups != null)
            {
                foreach (var item in result.Groups)
                {
                    foreach (var member in users.Where(p => p.Groups.Any(e=>e.Id == item.Id)))
                    {
                        contacts.AddRange(member.Contacts.ToList());
                    }
                }
            }
            _handlerManager.SendMessageChoicePlatform<TelegramHandler>(result.TextMessage, contacts.Select(p => p.Data).Distinct().ToList());
        }
    }
}