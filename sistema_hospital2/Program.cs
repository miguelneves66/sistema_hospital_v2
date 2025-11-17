using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;

namespace fila_hosp
{
    class Program
    {
        static string conexaoString = "server=localhost;uid=root;pwd=root;database=hospital_miguelv";

        static void Main(string[] args)
        {
            string opcao;

        menu:
            Console.Clear();
            Console.WriteLine("menu:");
            Console.WriteLine("1 - Cadastrar");
            Console.WriteLine("2 - Listar");
            Console.WriteLine("3 - Atender");
            Console.WriteLine("4 - Alterar");
            Console.WriteLine("q - Sair");
            Console.Write("opção: ");
            opcao = Console.ReadLine();
            if (opcao == "1")
            {
                Console.Clear();

                Console.Write("Nome: ");
                string nome = Console.ReadLine();

                Console.Write("Idade: ");
                int idade = int.Parse(Console.ReadLine());

                Console.Write("Preferencial (s/n): ");
                string letra = Console.ReadLine();
                bool pref = (letra == "s");

                using (MySqlConnection con = new MySqlConnection(conexaoString))
                {
                    con.Open();

                    string sql = "insert into pacientes (nome, idade, preferencial) values (@n, @i, @p)";
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    cmd.Parameters.AddWithValue("@n", nome);
                    cmd.Parameters.AddWithValue("@i", idade);
                    cmd.Parameters.AddWithValue("@p", pref);

                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("Paciente cadastrado!");
                Console.ReadLine();
                goto menu;
            }

            if (opcao == "2")
            {
                Console.Clear();
                Console.WriteLine("Fila:");

                using (MySqlConnection con = new MySqlConnection(conexaoString))
                {
                    con.Open();

                    string sql = "select * from pacientes order by preferencial desc, id asc";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader r = cmd.ExecuteReader();

                    int pos = 1;

                    if (!r.HasRows)
                    {
                        Console.WriteLine("nenhum paciente.");
                    }
                    else
                    {
                        while (r.Read())
                        {
                            string nome = r.GetString("nome");
                            int idade = r.GetInt32("idade");
                            bool pref = r.GetBoolean("preferencial");

                            if (pref)
                                Console.WriteLine(pos + ". " + nome + " - " + idade + " anos (P)");
                            else
                                Console.WriteLine(pos + ". " + nome + " - " + idade + " anos");

                            pos++;
                        }
                    }
                }

                Console.ReadLine();
                goto menu;
            }

            if (opcao == "3")
            {
                Console.Clear();

                using (MySqlConnection con = new MySqlConnection(conexaoString))
                {
                    con.Open();

                    string sql = "select * from pacientes order by preferencial desc, id asc limit 1";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader r = cmd.ExecuteReader();

                    if (!r.Read())
                    {
                        Console.WriteLine("ninguém na fila.");
                        r.Close();
                        Console.ReadLine();
                        goto menu;
                    }

                    int id = r.GetInt32("id");
                    string nome = r.GetString("nome");
                    int idade = r.GetInt32("idade");
                    r.Close();

                    Console.WriteLine("atendendo: " + nome + " - " + idade + " anos");

                    string sqlDel = "delete from pacientes where id=@id";
                    MySqlCommand cmdDel = new MySqlCommand(sqlDel, con);
                    cmdDel.Parameters.AddWithValue("@id", id);
                    cmdDel.ExecuteNonQuery();
                }

                Console.ReadLine();
                goto menu;
            }

            if (opcao == "4")
            {
                Console.Clear();

                using (MySqlConnection con = new MySqlConnection(conexaoString))
                {
                    con.Open();

                    string sql = "select id, nome, idade from pacientes order by id asc";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        Console.WriteLine(
                            r.GetInt32("id") + " - " +
                            r.GetString("nome") + " - " +
                            r.GetInt32("idade") + " anos");
                    }

                    r.Close();

                    Console.Write("digite o id para alterar: ");
                    int idEscolhido = int.Parse(Console.ReadLine());

                    Console.Write("novo nome: ");
                    string novoNome = Console.ReadLine();

                    Console.Write("nova idade: ");
                    int novaIdade = int.Parse(Console.ReadLine());

                    string sqlUp = "update pacientes set nome=@n, idade=@i where id=@id";
                    MySqlCommand cmdUp = new MySqlCommand(sqlUp, con);

                    cmdUp.Parameters.AddWithValue("@n", novoNome);
                    cmdUp.Parameters.AddWithValue("@i", novaIdade);
                    cmdUp.Parameters.AddWithValue("@id", idEscolhido);

                    cmdUp.ExecuteNonQuery();

                    Console.WriteLine("paciente alterado");
                }

                Console.ReadLine();
                goto menu;
            }

            if (opcao == "q")
            {
                Console.WriteLine("saindo...");
                return;
            }

            Console.WriteLine("opção errada.");
            Console.ReadLine();
            goto menu;
        }
    }
}
