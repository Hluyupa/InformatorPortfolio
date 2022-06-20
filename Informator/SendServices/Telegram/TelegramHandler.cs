using Informator.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Informator.SendServices.Telegram
{
    public class TelegramHandler : IHandler
    {
        

        //Создание собственных команд для бота
        private readonly List<BotCommand> _myCommands = new List<BotCommand> {
            new BotCommand { Command = "/start", Description = "Инициализировать бота в чате" },
            new BotCommand { Command = "/stop", Description = "Остановить деятельность бота в чате"}
        };

        //Токен бота
        private static string token = "";
        
        //Определение бота
        private static TelegramBotClient client;

        //Найстройки получения обновлений
        private ReceiverOptions receiverOptions;

        private CancellationTokenSource cts;

        public TelegramHandler()
        {

            client = new TelegramBotClient(token);

            cts = new CancellationTokenSource();

            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            
            
            client.SetMyCommandsAsync(
                _myCommands,
                scope: BotCommandScopeAllChatAdministrators.AllChatAdministrators(),
                cancellationToken: cts.Token);

        }
        //Слушатель входящих сообщений
        public void Listener()
        {
            client.StartReceiving(
                HandlerUpdateAsync,
                HandlerErrorAsync,
                receiverOptions,
                cts.Token);
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }
            
            await using var scope = Program.ServiceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<InformatorContext>();

            if (update.Message.Type == MessageType.Text)
            {
                var chatId = update.Message.Chat.Id;
                var chatName = update.Message.Chat.Title;
                var chatLastName = update.Message.Chat.LastName;
                var messageText = update.Message.Text;

                switch (messageText)
                {
                    case "/start@HluyupaBot":
                    case "/start":
                        if (context.Contacts.Any(p=>p.Data.Equals(chatId.ToString()))==false)
                        {
                            var systemType = context.SystemTypes.FirstOrDefault(p => p.Name.Equals("Telegram"));
                            context.Contacts.Add(new Models.Contact
                            {
                                Data = chatId.ToString(),
                                SystemType = systemType,
                                User = new Models.DataUser
                                {
                                    FirstName = chatName,
                                    SecondName = "none",
                                    Email = "none"
                                }
                            });
                        }

                       
                       
                        break;
                    default:
                        context.Messages.Attach(new Models.Message
                        {
                            Date = update.Message.Date,
                            Sender = context.DataUsers.FirstOrDefault(p=>p.Contacts.Any(s=>s.Data == chatId.ToString())),
                            Text = messageText                            
                        }).State = EntityState.Added;
                       
                        break;

                }
                await context.SaveChangesAsync();
            }
        }

        private Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        //Метод отправки сообщений по чатам

        public async void Send(string message, List<string> mailingList)
        {
            var converter = new Html2Markdown.Converter(new TelegramSchemeParser());
            var mesMD = converter.Convert(message);
            foreach (var item in mailingList)
            {
                await client.SendTextMessageAsync(
                    chatId: Convert.ToInt32(item),
                    text: mesMD,
                    parseMode: ParseMode.Html);
            }
        }
    }
}
