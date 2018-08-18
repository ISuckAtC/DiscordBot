using System;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DiscordBot
{
    public class User
    {
        public string Name;
        public ulong ID;

        public User(string name, ulong id)
        {
            Name = name;
            ID = id;
        }
    }

    class Program
    {
        static public List<User> users = new List<User>();

        ulong guildID;
        ulong channelID;

        private DiscordSocketClient Client;
        private CommandService Commands;

        static void Main(string[] args)
        {

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task ConsoleLoop()
        {
            for (; ; )
            {
                string read = Console.ReadLine();
                switch (read)
                {
                    
                    case "Define":
                        Console.WriteLine("Define active");
                        guildID = ulong.Parse(Console.ReadLine());
                        Console.WriteLine("guild assigned");
                        channelID = ulong.Parse(Console.ReadLine());
                        Console.WriteLine("channel assigned");
                        break;

                    case "":
                        break;

                    default:
                        Console.WriteLine("Default active");
                        await Client.GetGuild(guildID).GetTextChannel(channelID).SendMessageAsync(read);
                        break;
                }
            }
        }

        private async Task MainAsync()
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            Client.MessageReceived += Client_MessageReceived;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly());

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;

            string token = File.ReadAllText(@"Data\token.txt");
            guildID = ulong.Parse(File.ReadAllText(@"Data\defaultGuildID.txt"));
            channelID = ulong.Parse(File.ReadAllText(@"Data\defaultChannelID.txt"));

            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await ConsoleLoop();

            await Task.Delay(-1);
        }

        private async Task Client_Log(LogMessage a)
        {
            Console.WriteLine($"[{DateTime.Now} at {a.Source}] {a.Message}");
        }

        private async Task Client_Ready()
        {
            await Client.SetGameAsync("Despair");

        }

        private async Task Client_MessageReceived(SocketMessage readMessage)
        {

            var Message = readMessage as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);

            Console.WriteLine("|| Server: " + Context.Guild + " | Channel: " + Context.Channel + " | ID: " + Context.User.Id);
            Console.WriteLine("|| [" + DateTime.Now.TimeOfDay + "] Sender: " + Context.User.Username + " | Content: " + Context.Message.Content);

            if (Context.Message == null || Context.Message.Content == null) return;
            if (Context.User.IsBot) return;

            int ArgPos = 0;

            if (Context.Message.Content == "Good bot" || Context.Message.Content == "Bad bot")
            {
                await Commands.ExecuteAsync(Context, ArgPos);
                return;
            }

            if (!(Message.HasCharPrefix('€', ref ArgPos))) return;


            var Result = await Commands.ExecuteAsync(Context, ArgPos); //Command fires


            if (!Result.IsSuccess) Console.WriteLine($"[{DateTime.Now} at Commands] Something went wrong | Text: {Context.Message.Content} | Error: {Result.ErrorReason}");
        }
    }
}
