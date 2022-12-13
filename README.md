# Ubibot Discord Bot
## Bot para atualizar status dos servidores dos jogos da Ubisoft
### Feito com base na API game-status da Ubisoft e desenvolvido em C# com a biblioteca n�o oficial [Discord.NET](https://github.com/discord-net/Discord.Net)

## Comandos:
- **!jogos** - Listar os servidores de jogos monitorados pela API, e exibir o ID de cada um
- **!status** *id* - Mostrar status de servidor do jogo selecionado
- **!monitorar** *id* - Bot vai monitorar e anunciar qualquer mudan�a no status do servidor do jogo selecionado.
- **!offline** - Listar servidores atualmente offline ou em manuten��o
- **!ping** - Testar resposta

## Features WIP:
- O usu�rio poder� escolher a plataforma para monitorar (PC, Xbox, Playstation, Switch). Atualmente consulta apenas servidores de PC.
- Implementar Discord slash commands.
- Implementar banco de dados.

## Nota:
Para baixar e rodar o bot, � necess�rio atribuir um token de aplicativo gerado no discord developer portal.
Isto deve ser feito em uma classe chamada "TokenDiscord" dentro da pasta Modulos, em um m�todo string obterSenha().