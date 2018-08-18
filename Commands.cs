using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;
using System.IO;

using Discord;
using Discord.Commands;

namespace DiscordBot.Core.Commands
{

    
    
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("Create")]
        public async Task CreateCommand()
        {

            if (File.Exists(@"Data\" + Context.User.Id.ToString()))
            {
                await OutPut("You have already registered");
            }
            else
            {
                await OutPut("New user registered");

                string[] create = { Context.User.Username, Context.User.Id.ToString() };
                await File.WriteAllLinesAsync(@"data\" + Context.User.Id.ToString(), create);
            }
        }

        [Command("Dank"), Summary("Dank memes lol")]
        public async Task DankCommand()
        {
            await OutPut("memes");
        }

        [Command("Spam")]
        public async Task SpamCommand()
        {
            await OutPut("Spam");
        }

        public async Task OutPut(string output)
        {
            await Context.Channel.SendMessageAsync(output);
        }
    }

    public class RateBot : ModuleBase<SocketCommandContext>
    {

        

        [Command("Good bot")]
        public async Task GoodCommand()
        {

            string path = @"Data\PointInteger";

            string points = File.ReadAllText(path);

            Console.WriteLine("File found and read: " + points);

            if (!int.TryParse(points, out int pointsInt))
            {
                Console.WriteLine("Couldnt parse");
            }
            pointsInt++;

            Console.WriteLine("Saved to integer");

            
            

            await Context.Channel.SendMessageAsync("Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn!!");
            await Context.Channel.SendMessageAsync("I have [" + pointsInt + "] good bot points!");

            Console.WriteLine("Message sent");

            await File.WriteAllTextAsync(path, pointsInt.ToString());

            Console.WriteLine("File found and written to");
        }

        [Command("Bad bot")]
        public async Task BadCommand()
        {

            string path = @"Data\PointInteger";

            string points = File.ReadAllText(path);

            Console.WriteLine("File found and read: " + points);

            if (!int.TryParse(points, out int pointsInt))
            {
                Console.WriteLine("Couldnt parse");
            }
            pointsInt--;

            Console.WriteLine("Saved to integer");

            await Context.Channel.SendMessageAsync("Fuck you!");
            await Context.Channel.SendMessageAsync("I have [" + pointsInt + "] good bot points!");

            Console.WriteLine("Message sent");

            await File.WriteAllTextAsync(path, pointsInt.ToString());

            Console.WriteLine("File found and written to");
        }
    }
}
