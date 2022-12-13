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

        public async Task Monitorar(int id, API interno, SocketCommandContext contexto) {
            //Recebe o ID do jogo a ser monitorado, a instância da classe API para listar os jogos,
            //e o Context do Discord para poder enviar mensagens no chat.
            _Contexto = contexto;
            List<Jogos> listaJogos = (List<Jogos>)interno.ListarJogos();
            listaMonitora.Add(listaJogos[id]);

            /* foreach (Jogos jogos in listaMonitora) {
                Console.WriteLine(jogos.Name);
            }*/

            Console.WriteLine("Atualizador Iniciado");
            await Task.Run(Atualizador);
        }

        async Task Atualizador() {
            //Esta é uma task recursiva, que pega o AppID da Lista Monitora, e bate na API com o Status atual do app,
            //comparando os status dos games para verificar se houve alteração.
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
                                if (jogoMonitorado.Status != jogoAPI.Status) {
                                    if (jogoAPI.Status != "Online")
                                        await _Contexto.Channel.SendMessageAsync("QUEDA! " + jogoMonitorado.Name + " está passando por problemas de servidor:\n" +
                                        "Status foi alterado de [" + jogoMonitorado.Status + "] para [" + jogoAPI.Status + "]");
                                    else
                                        await _Contexto.Channel.SendMessageAsync("RESOLVIDO! " + jogoMonitorado.Name + " voltou ao ar:\n" +
                                        "Status foi alterado de [" + jogoMonitorado.Status + "] para [" + jogoAPI.Status + "]");
                                    jogoMonitorado.Status = jogoAPI.Status;
                                }
                            }
                        }
                    }

                }

            }
            await Task.Delay(TimeSpan.FromSeconds(300.0));
            Console.WriteLine("Verificando atualizador...");
            await Task.Run(Atualizador);
        }


    }
}
