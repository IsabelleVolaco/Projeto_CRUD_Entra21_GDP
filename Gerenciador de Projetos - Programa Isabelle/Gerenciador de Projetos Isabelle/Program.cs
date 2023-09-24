using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//---------------------------------------------------------------- G E R E N C I A D O R --- D E --- P R O J E T O S ----------------------------------------------------------------------//Isabelle Tschoeke Volaco - Entra21 C#

namespace Gerenciador_de_Projetos_Isabelle
{
    class Program
    {
        static void Main(string[] args)
        {
            string caminhoBancos = $"C:\\Users\\joaoh\\Documents\\Gerenciador de Projetos";//Mudar para o caminho do professor, só precisa mudar esse valor dessa variável.
            Console.WriteLine("Bem vindo(a) ao Gerenciador de Projetos!\nAqui você pode ver e editar quais os projetos disponíveis, os status deles e os participantes.");
            Console.WriteLine();

            do
            {
                try
                {
                    MostrarProjetos(caminhoBancos);

                    Console.WriteLine("Você deseja CRIAR um projeto novo (1), REMOVER projeto (2) ou EDITAR projeto (3)?");
                    string opcao = Console.ReadLine();


                    switch (opcao)
                    {
                        case "1":
                            CriarProjeto(caminhoBancos);
                            break;
                        case "2":
                            DeletarProjeto(caminhoBancos);
                            break;
                        case "3":
                            EditarProjeto(caminhoBancos);
                            break;
                        default: //caso a pessoa escolha algo errado.
                            Console.WriteLine("Não entendi o comando! Pode ser que esse comando não exista, volte ao início da aplicação.");
                            continue;
                    }
                }

                catch (Exception ex) //caso outros erros aconteçam.
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    continue;
                }

            } while (true);

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        static void MostrarProjetos(string caminhoBancos)
        {
            string jsonProjetos = File.ReadAllText($"{caminhoBancos}\\TOTAL_Projetos.txt");
            List<string> projetos = JsonSerializer.Deserialize<List<string>>(jsonProjetos);

            foreach (var projeto in projetos)
            {
                string jsonProjetoEscolhido = File.ReadAllText($"{caminhoBancos}\\{projeto.Trim().Replace(' ', '_')}.txt");
                List<string> projetoEscolhido = JsonSerializer.Deserialize<List<string>>(jsonProjetoEscolhido);
                Console.WriteLine("INFORMAÇÕES DO PROJETO");
                Console.WriteLine($"Nome: {projetoEscolhido[0]}");
                Console.WriteLine($"Estado: {projetoEscolhido[1]}");
                Console.WriteLine($"Descrição: {projetoEscolhido[2]}");
                Console.WriteLine("Integrantes:");
                if (projetoEscolhido.Count < 4)
                    Console.WriteLine("O projeto não possui integrantes");
                for (int i = 3; i < projetoEscolhido.Count; i++)
                {
                    Console.WriteLine(projetoEscolhido[i]);
                }

                Console.WriteLine("/-------------------------------------------------------------------------------------------------------/");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        static void CriarProjeto(string caminhoBancos)
        {
            //Criando um projeto;

            StreamWriter meuProjeto;
            Console.WriteLine("Digite um nome para o seu projeto, SEM ACENTOS:\nExemplo.: Reciclando Habitos");
            string nomePjt = Console.ReadLine();
            string caminhoPjt = $"{caminhoBancos}\\Projeto_{nomePjt.Trim().Replace(' ', '_')}.txt"; //trim tira espaços antes/depois - replace tranforma espaços em "_"
            meuProjeto = File.CreateText(caminhoPjt);
            Console.Clear();
            Console.WriteLine($"Projeto {nomePjt} criado com sucesso!");

            //Adicionando informações padrão dos projetos.
            string andamentoPjt = "- Nao iniciado -";
            Console.WriteLine("Digite uma descrição para o seu projeto, SEM ACENTOS:\nExemplo.: Esse e um projeto de educacao ambiental sobre praticas sustentaveis.");
            string descricaoPjt = Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"A descrição do Projeto {nomePjt} foi adicionada!");

            List<string> projetoPessoas = new List<string>();
            projetoPessoas.Add(nomePjt); //[0]
            projetoPessoas.Add(andamentoPjt); //[1]
            projetoPessoas.Add(descricaoPjt); //[2]

            //Salva e fecha o projeto recém criado.
            string json = JsonSerializer.Serialize(projetoPessoas); //Transforma lista em JSON.
            meuProjeto.WriteLine(json);
            meuProjeto.Close(); 

            //Adiciona o nome do novo projeto na lista de projetos.
            string jsonProjetos = File.ReadAllText($"{caminhoBancos}\\TOTAL_Projetos.txt");
            List<string> projetos = JsonSerializer.Deserialize<List<string>>(jsonProjetos);
            projetos.Add($"Projeto {nomePjt}");
            Salvar(projetos, $"{caminhoBancos}\\TOTAL_Projetos.txt");
        }


        static void DeletarProjeto(string caminhoBancos)
        {
            //Deletando um projeto;

            Console.WriteLine("Digite o nome do projeto que deseja DELETAR:");
            string nomePjt = Console.ReadLine();
            string caminhoPjt = $"{caminhoBancos}\\Projeto_{nomePjt.Trim().Replace(' ', '_')}.txt";
            File.Delete(caminhoPjt);
            Console.Clear();
            Console.WriteLine($"O Projeto {nomePjt} foi deletado com sucesso.");

            //Remove o nome do projeto da lista de projetos.
            string jsonProjetos = File.ReadAllText($"{caminhoBancos}\\TOTAL_Projetos.txt");
            List<string> projetos = JsonSerializer.Deserialize<List<string>>(jsonProjetos);
            for (int i= 0; i < projetos.Count; i++)
            {
                if (projetos[i] == $"Projeto {nomePjt}")
                    projetos.RemoveAt(i);
            }
            Salvar(projetos, $"{caminhoBancos}\\TOTAL_Projetos.txt");
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------
        static void EditarProjeto(string caminhoBancos)
        {
            //Editando projetos

            //Escolhe projeto.
            Console.WriteLine("Você selecionou EDITAR PROJETO.\nEscolha um projeto pelo seu número:\nEXEMPLO.: 1");
            string jsonProjetos = File.ReadAllText($"{caminhoBancos}\\TOTAL_Projetos.txt");
            List<string> projetos = JsonSerializer.Deserialize<List<string>>(jsonProjetos);

            for (int i = 0; i < projetos.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {projetos[i]}"); //Mostra o número de cada projeto.
            }

            int opcao = Convert.ToInt32(Console.ReadLine());
            string nomeProjetoEscolhido = projetos[opcao - 1];
            Console.WriteLine($"Você selecionou o Projeto {nomeProjetoEscolhido}:");

            //Lê e exibe as informações do projeto escolhido.
            string jsonProjetoEscolhido = File.ReadAllText($"{caminhoBancos}\\{nomeProjetoEscolhido.Trim().Replace(' ', '_')}.txt");
            List<string> projetoEscolhido = JsonSerializer.Deserialize<List<string>>(jsonProjetos);
            Console.WriteLine("INFORMAÇÕES DO PROJETO");
            Console.WriteLine($"Nome: {projetoEscolhido[0]}");
            Console.WriteLine($"Estado: {projetoEscolhido[1]}");
            Console.WriteLine($"Descrição: {projetoEscolhido[2]}");
            Console.WriteLine("Integrantes:");
            for (int i = 3; i < projetoEscolhido.Count; i++)
            {
                Console.WriteLine(projetoEscolhido[i]);
            }

            //Editando o projeto
            Console.WriteLine("Você deseja ADICIONAR integrantes (1), REMOVER (2) integrantes ou ALTERAR STATUS (3) do projeto? ");
            opcao = Convert.ToInt32(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    AdicionaPessoasPjt($"{caminhoBancos}\\{nomeProjetoEscolhido.Trim().Replace(' ', '_')}.txt");
                    break;
                case 2:
                    RemovePessoasPjt($"{caminhoBancos}\\{nomeProjetoEscolhido.Trim().Replace(' ', '_')}.txt");
                    break;
                case 3:
                    AlterarStatusPjt($"{caminhoBancos}\\{nomeProjetoEscolhido.Trim().Replace(' ', '_')}.txt");
                    break;
                default:
                    Console.WriteLine("Não entendi o comando! Pode ser que esse comando não exista, volte ao início da aplicação.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }

        }

        static void AdicionaPessoasPjt(string caminhoProjeto)
        {
            //Adicionando pessoas no projeto
            string jsonProjetoEscolhido = File.ReadAllText(caminhoProjeto);
            List<string> projetoEscolhido = JsonSerializer.Deserialize<List<string>>(jsonProjetoEscolhido);
            if (projetoEscolhido[1] == "- concluido -")
            {
                Console.WriteLine("Este projeto está concluído. Não é possível adicionar pessoas.");
                    return; //se entrar no if, a função para aqui.
            }


            Console.WriteLine($"Digite o nome do participante que deseja adicionar ao Projeto, SEM ACENTOS:\nEXEMPLO.: Maria S.");
            string nomePessoa = Console.ReadLine();
            projetoEscolhido.Add(nomePessoa);

            //Se adicionar pessoas no projeto vazio, ele inicia." 
            if (projetoEscolhido[1] == "- nao iniciado -")
            {
                projetoEscolhido[1] = "- em andamento -";
            }
            Salvar(projetoEscolhido, caminhoProjeto);

            Console.WriteLine($"O(a) integrante {nomePessoa} foi adicionado(a) ao projeto.");
        }

        static void RemovePessoasPjt(string caminhoProjeto)
        {
            //Removendo pessoas do projeto
            string deletandoPessoa = Console.ReadLine();
            string jsonMeuProjeto = File.ReadAllText(caminhoProjeto);
            List<string> pessoas = JsonSerializer.Deserialize<List<string>>(jsonMeuProjeto);
            if (pessoas[1] == "- concluido -")
            {
                Console.WriteLine("Este projeto está concluído. Não é possível remover pessoas.");
                return; //se entrar no if, a função para aqui.
            }

            for (int i = 3; i < pessoas.Count; i++) //começa a ler no index 3.
            {
                if (pessoas[i] == deletandoPessoa)
                {
                    pessoas.RemoveAt(i); //remove o elemento do index.    
                }

            }
            if (pessoas.Count < 4 && pessoas[1] == "- em andamento -")
            {
                pessoas[1] = "- nao iniciado -";
            }

            Salvar(pessoas, caminhoProjeto);
            Console.WriteLine($"O(a) integrante {deletandoPessoa} foi removido(a) do projeto.");
        }

        static void AlterarStatusPjt(string caminhoProjeto)
        {
            string jsonProjetoEscolhido = File.ReadAllText(caminhoProjeto);
            List<string> projetoEscolhido = JsonSerializer.Deserialize<List<string>>(jsonProjetoEscolhido);
            Console.WriteLine($"O status atual do Projeto {projetoEscolhido[0]} é: {projetoEscolhido[1]}.\nDigite a opção que deseja: EM ANDAMENTO (1) ou CONCLUÍDO (2) ");
            int opcao = Convert.ToInt32(Console.ReadLine());

            switch (opcao)
            {
                case 1:
                    projetoEscolhido[1] = "- em andamento -";
                    break;
                case 2:
                    projetoEscolhido[1] = "- concluido -";
                    break;    
                default:
                    Console.WriteLine("Não entendi o comando! Pode ser que esse comando não exista, volte ao início da aplicação.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }

        }


        //---------------------------------------------------------------------------------------------------------------------------------------------------------

        static void Salvar(List<string> dados, string caminhoBanco)
        {
            string json = JsonSerializer.Serialize(dados);
            File.WriteAllText(caminhoBanco, json);
        }
    }
}




