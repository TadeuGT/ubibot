using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubibot.Modulos {
    internal class Monitor : ModuleBase<SocketCommandContext> {
        static List<Jogos> listaMonitora = new List<Jogos>();
        static SocketCommandContext _Contexto;
        //static List<Jogos> listaEstados = new List<Jogos>();

        public async Task Monitorar(int id, API interno, SocketCommandContext contexto) {
            _Contexto = contexto;
            List<Jogos> listaJogos = (List<Jogos>)interno.ListarJogos();
            listaMonitora.Add(listaJogos[id]);
            
            foreach (Jogos jogos in listaMonitora) {
                Console.WriteLine(jogos.Name);
            }


            Console.WriteLine("Iniciando atualizador");
            await Task.Run(Atualizador);
        }

        async Task Atualizador() {
            //Pegar AppID da Lista Monitora, e bater na API os Ids, comparando os status dos games
            string extrairID = null;
            foreach (Jogos jogo in listaMonitora) {
                extrairID += jogo.AppID.ToString() + ",";
            }
            extrairID = extrairID.Remove(extrairID.Length - 1, 1);

            string url = "https://game-status-api.ubisoft.com/v1/instances?appIds=" + extrairID;
            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = client.GetAsync(url).Result) {
                using (HttpContent content = response.Content) {
                    var json = content.ReadAsStringAsync().Result;
                    List<Jogos> liveAPI = JsonConvert.DeserializeObject<List<Jogos>>(json);

                    foreach (Jogos jogoMonitorado in listaMonitora) {
                        foreach (Jogos jogoAPI in liveAPI) {
                            if (jogoMonitorado.AppID == jogoAPI.AppID) {
                                if (jogoMonitorado.Status == jogoAPI.Status) {
                                    Console.WriteLine("Nada mudou em " + jogoMonitorado.Name + " " + jogoAPI.Name);
                                    Console.WriteLine(_Contexto.Channel.Name);
                                    await _Contexto.Channel.SendMessageAsync("Teste");
                                    //WIP
                                }
                            }
                        }
                    }
                }

            }
            await Task.Delay(TimeSpan.FromSeconds(60.0));
            Console.WriteLine("Verificando atualizador...");
            await Task.Run(Atualizador);
        }


    }
}
