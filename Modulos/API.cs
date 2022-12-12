using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;
using Discord.WebSocket;

namespace Ubibot.Modulos {
    internal class API : ModuleBase<SocketCommandContext> {

        Monitor monitor = new Monitor();

        public object ListarJogos() {
            string url = "https://game-status-api.ubisoft.com/v1/instances";
            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = client.GetAsync(url).Result) {
                using (HttpContent content = response.Content) {
                    var json = content.ReadAsStringAsync().Result;
                    List<Jogos> games = JsonConvert.DeserializeObject<List<Jogos>>(json);

                    List<Jogos> listaJogos = new List<Jogos>();

                    for (int i = 0; i < games.Count; i++) {

                        if (games[i].Platform == "PC") {

                            listaJogos.Add(games[i]);
                            //Console.WriteLine("add " + games[i].Name + " na lista");
                        }

                    }
                    return listaJogos;

                }

            }

        }

        public string GetJogos(int num) {
            return GetStatus(num);
        }

        static string GetStatus(int num) {
            API interno = new API();
            List<Jogos> listaJogos = (List<Jogos>)interno.ListarJogos();
            if (listaJogos[num].Maintenance == "true")
                return "MANUTENÇÃO! O servidor " + listaJogos[num].Name + " se encontra em manutenção.";
            else if (listaJogos[num].Status.ToString() == "Online") 
                return "TUDO OK! O servidor " + listaJogos[num].Name + " está " + listaJogos[num].Status;
            else {
                string problemas = null;
                foreach (var item in listaJogos[num].ImpactedFeatures) {
                    problemas += "- " + item + "\n";
                }
                return "PROBLEMA! O servidor " + listaJogos[num].Name + " está apresentando problemas, afetando:\n" + problemas;
            }
        }

        public string GetStatusGeral() {
            API interno = new API();
            List<Jogos> listaJogos = (List<Jogos>)interno.ListarJogos();
            string off = null;
            string manutencao = null;
            for (var i = 0; i < listaJogos.Count; i++) {
                if (listaJogos[i].Maintenance == "true")
                    manutencao += "[" + i + "] " + listaJogos[i].Name + " ";
                if (listaJogos[i].Status != "Online")
                    off += "["+ i +"] " + listaJogos[i].Name + " ";
            }
            if (off == null && manutencao == null)
                return "Nenhum servidor da Ubisoft se encontra offline.";
            else
            return "Os servidores de " + off + " se encontram Offline!";
            //WIP Refazer return pra manutenção
        }

        public async void MonitorarAPI(int id, SocketCommandContext contexto) {
            API interno = new API();
            List<Jogos> listaJogos = (List<Jogos>)interno.ListarJogos();
            contexto.Message.ReplyAsync("Agora monitorando " + listaJogos[id].Name);
            await monitor.Monitorar(id, interno, contexto);
            
        }


    }
}
