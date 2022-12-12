using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubibot.Modulos {
    internal class Monitor {
        static List<Jogos> listaMonitora = new List<Jogos>();
        //static List<Jogos> listaEstados = new List<Jogos>();

        public string Monitorar(int id, API interno) {
            List<Jogos> listaJogos = (List<Jogos>)interno.ListarJogos();
            listaMonitora.Add(listaJogos[id]);
            /*
            foreach (Jogos jogos in listaMonitora) {
                Console.WriteLine(jogos.Name);
            }*/
            Atualizador(interno);

            return listaJogos[id].Name;
        }

        async void Atualizador(API interno) {
            //List<Jogos> listaJogos = (List<Jogos>)interno.ListarJogos();

            string url = "https://game-status-api.ubisoft.com/v1/instances";
            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = client.GetAsync(url).Result) {
                using (HttpContent content = response.Content) {
                    var json = content.ReadAsStringAsync().Result;
                    List<Jogos> games = JsonConvert.DeserializeObject<List<Jogos>>(json);

                    List<Jogos> listaJogos = new List<Jogos>();
                    //string[] listaJogos = null;

                    for (int i = 0; i < games.Count; i++) {

                        if (games[i].Platform == "PC") {

                            listaJogos.Add(games[i]);
                            //Console.WriteLine("add " + games[i].Name + " na lista");
                        }

                    }

                }

            }


            /*
            for (int i = 0; i < listaMonitora.Count; i++) {
                if (listaMonitora[i].AppID == listaJogos[i].AppID) {
                    if (listaMonitora[i].Status != listaJogos[i].Status) {
                        Console.WriteLine("Mudança detectada em " + listaMonitora[i].Name + ": Status foi de " + listaMonitora[i].Status + " para " + listaJogos[i].Status);
                        listaMonitora[i].Status = listaJogos[i].Status;
                    }
                }
            }*/
            Task.Delay(TimeSpan.FromSeconds(60.0));
            //Console.WriteLine(listaMonitora[59].Status);
            //Console.WriteLine(listaJogos[59].Status);
            Console.WriteLine("Rodando Atualizador()");
            Atualizador(interno);
        }


    }
}
