using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Discord;
using Newtonsoft.Json;
using System.Net;

namespace Ubibot.Modulos {
    internal class API {

        //https://game-status-api.ubisoft.com/v1/instances
        //?appIds=e3d5ea9e-50bd-43b7-88bf-39794f4e3d40,
        //fb4cc4c9-2063-461d-a1e8-84a7d36525fc,
        //4008612d-3baf-49e4-957a-33066726a7bc

        public object listarJogos() {
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
                            Console.WriteLine("add " + games[i].Name + " na lista");
                        }

                    }
                    return listaJogos;

                }

            }

        }

        public string getJogos() {
            return getStatus();
        }

        static string getStatus() {

            string url = "https://game-status-api.ubisoft.com/v1/instances?appIds=e3d5ea9e-50bd-43b7-88bf-39794f4e3d40";
            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = client.GetAsync(url).Result) {
                using (HttpContent content = response.Content) {
                    var json = content.ReadAsStringAsync().Result;
                                                                 #pragma warning disable CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.
                    List<Jogos> games = JsonConvert.DeserializeObject<List<Jogos>>(json);
                                                                 #pragma warning restore CS8600 // Conversão de literal nula ou possível valor nulo em tipo não anulável.

                                                                 #pragma warning disable CS8602 // Desreferência de uma ref possivelmente nula.
                    foreach (var jogo in games) {
                        //Console.WriteLine("--- Status de " + jogo.Name);
                        Console.WriteLine("AppID: " + jogo.AppID);
                        Console.WriteLine("Name: " + jogo.Name);
                        Console.WriteLine("Plataform: " + jogo.Platform);
                        Console.WriteLine("Status: " + jogo.Status);
                        Console.WriteLine("Maintenance: " + jogo.Maintenance);
                        //Console.WriteLine("Impacted Features: " + jogo.ImpactedFeatures[0]);
                        Console.WriteLine("\n");
                    }
                                                                 #pragma warning restore CS8602 // Desreferência de uma ref possivelmente nula.

                    return games[0].Status.ToString();

                }

            }

        }


    }
}
