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
            await Context.Message.ReplyAsync("Pong");
        }

        [Command("jogos")]
        public async Task Jogos() { 
            API a1 = new API();
            List<Jogos> listaJogos = (List<Jogos>)a1.ListarJogos();
            string resposta = null;
            
            for (var i = 0; i < listaJogos.Count; i++) {
                Console.WriteLine(listaJogos[i].Name);
                resposta += i + " - " + listaJogos[i].Name + "\n";
            }

            int chunkSize = 1900;
            int stringLength = resposta.Length;
            for (int i = 0; i < stringLength ; i += chunkSize) {
                if (i + chunkSize > stringLength) chunkSize = stringLength - i;
                await Context.Message.ReplyAsync(resposta.Substring(i, chunkSize));
            }
        }

        [Command("status")]
        public async Task Status(int num) {
            API a1 = new API();
            await Context.Message.ReplyAsync(a1.GetJogos(num));
        }

        [Command("offline")]
        public async Task Offline() {
            API a1 = new API();
            await Context.Message.ReplyAsync(a1.GetStatusGeral());
        }

        [Command("monitorar")]
        public Task Monitorar(int id) {
            SocketCommandContext contexto = Context;
            API a1 = new API();
            a1.MonitorarAPI(id, contexto);
            return Task.CompletedTask;
        }
    }
}
