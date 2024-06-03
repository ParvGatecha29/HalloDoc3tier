using HalloDocDAL.Data;
using HalloDocDAL.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HelloDoc_BAL.Services
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task SendMessage(int senderId, string senderType, int receiverId, string receiverType, string message)
        {

            if (senderType == "Admin")
            {
                if (receiverType == "Patient")
                {
                    var data = _context.Chats.Any(r => r.Adminid == senderId && r.Patientid == receiverId);

                    if (!data)
                    {
                        var chat = new Chat
                        {
                            Adminid = senderId,
                            Patientid = receiverId,
                            Createddate = DateTime.Now,
                            Createdby = senderId
                        };
                        _context.Chats.Add(chat);
                        _context.SaveChanges();
                    }

                    var chats = _context.Chats.FirstOrDefault(r => r.Adminid == senderId && r.Patientid == receiverId);
                    var chatmessage = new Chatdatum
                    {
                        Chatid = chats.Id,
                        Message = message,
                        Messageby = "Admin",
                        Date = DateTime.Now,
                        Isdeleted = false
                    };

                    _context.Chatdata.Add(chatmessage);
                    _context.SaveChanges();


                    var messageData = new { Message = message, MessageBy = senderType, Time = DateTime.Now.ToShortTimeString(), ChatId = chats.Id };

                    await Clients.All.SendAsync("ReceiveMessage", messageData);
                }
                else if (receiverType == "Physician")
                {
                    var data = _context.Chats.Any(r => r.Adminid == senderId && r.Physicianid == receiverId && r.Patientid == null);

                    if (!data)
                    {
                        var chat = new Chat
                        {
                            Adminid = senderId,
                            Physicianid = receiverId,
                            Createddate = DateTime.Now,
                            Createdby = senderId
                        };
                        _context.Chats.Add(chat);
                        _context.SaveChanges();
                    }

                    var chats = _context.Chats.FirstOrDefault(r => r.Adminid == senderId && r.Physicianid == receiverId && r.Patientid == null);
                    var chatmessage = new Chatdatum
                    {
                        Chatid = chats.Id,
                        Message = message,
                        Messageby = "Admin",
                        Date = DateTime.Now,
                        Isdeleted = false
                    };

                    _context.Chatdata.Add(chatmessage);
                    _context.SaveChanges();

                    var messageData = new { Message = message, MessageBy = senderType, Time = DateTime.Now.ToShortTimeString(), ChatId = chats.Id };

                    await Clients.All.SendAsync("ReceiveMessage", messageData);
                }
            }
            else if (senderType == "Patient")
            {
                if (receiverType == "Admin")
                {
                    var data = _context.Chats.Any(r => r.Adminid == receiverId && r.Patientid == senderId);

                    if (!data)
                    {
                        var chat = new Chat
                        {
                            Adminid = receiverId,
                            Patientid = senderId,
                            Createddate = DateTime.Now,
                            Createdby = senderId
                        };
                        _context.Chats.Add(chat);
                        _context.SaveChanges();
                    }

                    var chats = _context.Chats.FirstOrDefault(r => r.Adminid == receiverId && r.Patientid == senderId);
                    var chatmessage = new Chatdatum
                    {
                        Chatid = chats.Id,
                        Message = message,
                        Messageby = "Patient",
                        Date = DateTime.Now,
                        Isdeleted = false
                    };

                    _context.Chatdata.Add(chatmessage);
                    _context.SaveChanges();

                    var messageData = new { Message = message, MessageBy = senderType, Time = DateTime.Now.ToShortTimeString(), ChatId = chats.Id };

                    await Clients.All.SendAsync("ReceiveMessage", messageData);
                }
                else if (receiverType == "Physician")
                {
                    var data = _context.Chats.Any(r => r.Patientid == senderId && r.Physicianid == receiverId);

                    if (!data)
                    {
                        var chat = new Chat
                        {
                            Patientid = senderId,
                            Physicianid = receiverId,
                            Createddate = DateTime.Now,
                            Createdby = senderId
                        };
                        _context.Chats.Add(chat);
                        _context.SaveChanges();
                    }

                    var chats = _context.Chats.FirstOrDefault(r => r.Patientid == senderId && r.Physicianid == receiverId);
                    var chatmessage = new Chatdatum
                    {
                        Chatid = chats.Id,
                        Message = message,
                        Messageby = "Patient",
                        Date = DateTime.Now,
                        Isdeleted = false
                    };

                    _context.Chatdata.Add(chatmessage);
                    _context.SaveChanges();

                    var messageData = new { Message = message, MessageBy = senderType, Time = DateTime.Now.ToShortTimeString(), ChatId = chats.Id };

                    await Clients.All.SendAsync("ReceiveMessage", messageData);
                }
            }
            else if (senderType == "Physician")
            {
                if (receiverType == "Admin")
                {
                    var data = _context.Chats.Any(r => r.Physicianid == senderId && r.Adminid == receiverId);

                    if (!data)
                    {
                        var chat = new Chat
                        {
                            Physicianid = senderId,
                            Adminid = receiverId,
                            Createddate = DateTime.Now,
                            Createdby = senderId
                        };
                        _context.Chats.Add(chat);
                        _context.SaveChanges();
                    }

                    var chats = _context.Chats.FirstOrDefault(r => r.Physicianid == senderId && r.Adminid == receiverId);
                    var chatmessage = new Chatdatum
                    {
                        Chatid = chats.Id,
                        Message = message,
                        Messageby = "Physician",
                        Date = DateTime.Now,
                        Isdeleted = false
                    };

                    _context.Chatdata.Add(chatmessage);
                    _context.SaveChanges();

                    var messageData = new { Message = message, MessageBy = senderType, Time = DateTime.Now.ToShortTimeString(), ChatId = chats.Id };

                    await Clients.All.SendAsync("ReceiveMessage", messageData);
                }
                else if (receiverType == "Patient")
                {
                    var data = _context.Chats.Any(r => r.Physicianid == senderId && r.Patientid == receiverId);

                    if (!data)
                    {
                        var chat = new Chat
                        {
                            Physicianid = senderId,
                            Patientid = receiverId,
                            Createddate = DateTime.Now,
                            Createdby = senderId
                        };
                        _context.Chats.Add(chat);
                        _context.SaveChanges();
                    }

                    var chats = _context.Chats.FirstOrDefault(r => r.Physicianid == senderId && r.Patientid == receiverId);
                    var chatmessage = new Chatdatum
                    {
                        Chatid = chats.Id,
                        Message = message,
                        Messageby = "Physician",
                        Date = DateTime.Now,
                        Isdeleted = false
                    };

                    _context.Chatdata.Add(chatmessage);
                    _context.SaveChanges();

                    var messageData = new { Message = message, MessageBy = senderType, Time = DateTime.Now.ToShortTimeString(), ChatId = chats.Id };

                    await Clients.All.SendAsync("ReceiveMessage", messageData);
                }
            }


        }

        public async Task GetMessages(int senderId, string senderType, int receiverId, string receiverType)
        {
            if (senderType == "Admin")
            {
                if (receiverType == "Patient")
                {
                    var chat = _context.Chats.FirstOrDefault(c => c.Adminid == senderId && c.Patientid == receiverId);

                    var messages = await _context.Chatdata
                        .Where(cd => cd.Chatid == chat.Id)
                        .OrderBy(cd => cd.Date)
                        .Select(cd => new { Message = cd.Message, MessageBy = cd.Messageby, Time = cd.Date.ToShortTimeString(), ChatId = chat.Id })
                        .ToListAsync();
                    var ChatId = chat.Id;
                    await Clients.Caller.SendAsync("ReceiveMessages", messages, ChatId);
                }
                else if (receiverType == "Physician")
                {
                    var chat = _context.Chats.FirstOrDefault(c => c.Adminid == senderId && c.Physicianid == receiverId && c.Patientid == null);

                    var messages = await _context.Chatdata
                        .Where(cd => cd.Chatid == chat.Id)
                        .OrderBy(cd => cd.Date)
                        .Select(cd => new { Message = cd.Message, MessageBy = cd.Messageby, Time = cd.Date.ToShortTimeString(), ChatId = chat.Id })
                        .ToListAsync();
                    var ChatId = chat.Id;
                    await Clients.Caller.SendAsync("ReceiveMessages", messages, ChatId);
                }
            }
            else if (senderType == "Patient")
            {
                if (receiverType == "Admin")
                {
                    var chat = _context.Chats.FirstOrDefault(c => c.Adminid == receiverId && c.Patientid == senderId);

                    var messages = await _context.Chatdata
                        .Where(cd => cd.Chatid == chat.Id)
                        .OrderBy(cd => cd.Date)
                        .Select(cd => new { Message = cd.Message, MessageBy = cd.Messageby, Time = cd.Date.ToShortTimeString(), ChatId = chat.Id })
                        .ToListAsync();
                    var ChatId = chat.Id;
                    await Clients.Caller.SendAsync("ReceiveMessages", messages, ChatId);
                }
                else if (receiverType == "Physician")
                {
                    var chat = _context.Chats.FirstOrDefault(c => c.Patientid == senderId && c.Physicianid == receiverId);

                    var messages = await _context.Chatdata
                        .Where(cd => cd.Chatid == chat.Id)
                        .OrderBy(cd => cd.Date)
                        .Select(cd => new { Message = cd.Message, MessageBy = cd.Messageby, Time = cd.Date.ToShortTimeString(), ChatId = chat.Id })
                        .ToListAsync();
                    var ChatId = chat.Id;
                    await Clients.Caller.SendAsync("ReceiveMessages", messages, ChatId);
                }
            }
            else if (senderType == "Physician")
            {
                if (receiverType == "Admin")
                {
                    var chat = _context.Chats.FirstOrDefault(c => c.Physicianid == senderId && c.Adminid == receiverId);

                    var messages = await _context.Chatdata
                        .Where(cd => cd.Chatid == chat.Id)
                        .OrderBy(cd => cd.Date)
                        .Select(cd => new { Message = cd.Message, MessageBy = cd.Messageby, Time = cd.Date.ToShortTimeString() })
                        .ToListAsync();
                    var ChatId = chat.Id;
                    await Clients.Caller.SendAsync("ReceiveMessages", messages, ChatId);
                }
                else if (receiverType == "Patient")
                {
                    var chat = _context.Chats.FirstOrDefault(c => c.Physicianid == senderId && c.Patientid == receiverId);

                    var messages = await _context.Chatdata
                        .Where(cd => cd.Chatid == chat.Id)
                        .OrderBy(cd => cd.Date)
                        .Select(cd => new { Message = cd.Message, MessageBy = cd.Messageby, Time = cd.Date.ToShortTimeString(), ChatId = chat.Id })
                        .ToListAsync();
                    var ChatId = chat.Id;
                    await Clients.Caller.SendAsync("ReceiveMessages", messages, ChatId);
                }
            }
        }

        public async Task SendMessageGroup(int adminId, int patientId, int physicianId, string senderType, string message)
        {
            var data = _context.Chats.Any(r => r.Adminid == adminId && r.Patientid == patientId && r.Physicianid == physicianId);

            if (!data)
            {
                var chat = new Chat
                {
                    Adminid = adminId,
                    Patientid = patientId,
                    Physicianid = physicianId,
                    Createddate = DateTime.Now
                };

                if (senderType == "Admin")
                {
                    chat.Createdby = adminId;
                }
                else if (senderType == "Patient")
                {
                    chat.Createdby = patientId;
                }
                else
                {
                    chat.Createdby = physicianId;
                }
                _context.Chats.Add(chat);
                _context.SaveChanges();
            }

            var chats = _context.Chats.FirstOrDefault(r => r.Adminid == adminId && r.Patientid == patientId && r.Physicianid == physicianId);
            var chatmessage = new Chatdatum
            {
                Chatid = chats.Id,
                Message = message,
                Date = DateTime.Now,
                Isdeleted = false
            };

            if (senderType == "Admin")
            {
                chatmessage.Messageby = "Admin";
                chatmessage.Sendername = _context.Admins.FirstOrDefault(a => a.Adminid == adminId).Firstname;
            }
            else if (senderType == "Patient")
            {
                chatmessage.Messageby = "Patient";
                chatmessage.Sendername = _context.Users.FirstOrDefault(a => a.Userid == patientId).Firstname;
            }
            else
            {
                chatmessage.Messageby = "Physician";
                chatmessage.Sendername = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId).Firstname;
            }

            _context.Chatdata.Add(chatmessage);
            _context.SaveChanges();

            var messageData = new { Message = message, MessageBy = senderType, SenderName = chatmessage.Sendername, Time = DateTime.Now.ToShortTimeString(), ChatId = chats.Id };

            await Clients.All.SendAsync("ReceiveMessage", messageData);
        }

        public async Task GetMessageGroup(int adminId, int patientId, int physicianId)
        {
            var chat = _context.Chats.FirstOrDefault(c => c.Adminid == adminId && c.Patientid == patientId && c.Physicianid == physicianId);

            var messages = await _context.Chatdata
                .Where(cd => cd.Chatid == chat.Id)
                .OrderBy(cd => cd.Date)
                .Select(cd => new { Message = cd.Message, MessageBy = cd.Messageby, SenderName = cd.Sendername, Time = cd.Date.ToShortTimeString(), ChatId = chat.Id })
                .ToListAsync();
            var ChatId = chat.Id;
            await Clients.Caller.SendAsync("ReceiveMessages", messages, ChatId);
        }
    }
}
