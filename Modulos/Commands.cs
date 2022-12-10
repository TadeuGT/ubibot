using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubibot.Modulos {
    public class Commands : ModuleBase<SocketCommandContext> {
        [Command("ping")]
        public async Task Ping() {
            await ReplyAsync("Pong");
        }

        [Command("listar")]
        public async Task Listar() { 
            API a1 = new API();
            List<Jogos> listaJogos = (List<Jogos>)a1.listarJogos();
            for (var i = 0; i < listaJogos.Count; i++) {
                Console.WriteLine(listaJogos[i].Name);
                //await ReplyAsync(listaJogos[i].Name);
            }
        }

        [Command("jogos")]
        public async Task Jogos() {
            API a1 = new API();
            await ReplyAsync("Os servidores do R6 Siege estão " + a1.getJogos());
        }


    }
}
