/*
* -- PLANO DE TRABALHO [Jogo da velha + Redes Neurais]:
*
* 1) Armazenar saída;
* 2) Armazenar "rotações" (x4);
* 3) Armazenar inversos (x2);
* 4) Inverter quando "bola" ganhar;
* 5) Integrar RNA;
* 6) Realizar jogada da RNA;
* 7) Armazenar jogadas em txt.
*
* */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

// Bibliotecas da rede neural
using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static int aux = 0; // Converte resp para inteiro na função Clicar()
        static int k = 0; // Número de jogadas da partida atual
        static int linhas = 0; // Número de linhas da partida atual
        static int inicio = 0; // Especifica onde o método começará a preencher o campo "vitória" com '1' ou '-1'
        static int fim = 0; // Especifica onde o método irá terminar de preencher o campo "vitória" com '1' ou '-1'
        static int total = 0; // Total de linhas de todas as partidas jogadas
        static int pos = 0; // Indica qual botão do jogo será clicado
        static string temp; // Armazena 'X' ou 'O'
        static string resp; // Armazena '1' ou '-1'

        // Inicializa com valores diferentes para que não seja interpretado como uma das possíveis posições de vitória
        static string a1 = "5", a2 = "6", a3 = "7", b1 = "8", b2 = "9", b3 = "10", c1 = "11", c2 = "12", c3 = "13";
        static string sa1 = "5", sa2 = "6", sa3 = "7", sb1 = "8", sb2 = "9", sb3 = "10", sc1 = "11", sc2 = "12", sc3 = "13";

        // Arrays principais recebendo a jogada atual e as posições de giro
        static string[] arr = new string[10];
        static string[] arr1 = new string[10];
        static string[] arr2 = new string[10];
        static string[] arr3 = new string[10];

        // Arrays secundários recebendo os inversos da jogada atual e das posições de giro *(-1)
        static string[] arr4 = new string[10];
        static string[] arr5 = new string[10];
        static string[] arr6 = new string[10];
        static string[] arr7 = new string[10];

        // Array que recebe somente a posição clicada na jogada (campo output do listview)
        static string[] arr8 = new string[10];

        // Array de resposta da RNA
        static double[] saida = new double[9];

        // Cria os dois listviews
        // itm: entradas + posições de giro
        // itm2: saídas
        static ListViewItem itm;
        static ListViewItem itm2;

        // Define local de armazenamento e nome do arquivo txt onde serão gravadas as jogadas
        static string enderecoTxt = ("treinamento.txt");

        /* 
         * Aqui é onde a RNA é criada
         * 
         * */

        static double sigmoidAlphaValue = 2.0;

        static ActivationNetwork network = new ActivationNetwork(
            (IActivationFunction)new BipolarSigmoidFunction(sigmoidAlphaValue),
            9, // Nine inputs in the network
            9, // Nine neurons in the first layer
            9); // Nine neuron in the second layer

        // create teacher
        BackPropagationLearning teacher = new BackPropagationLearning(network);

        ActivationLayer layer = network[0];

        // Estancia objeto para escrita no arquivo .txt
        System.IO.TextWriter arquivo;
        
        //Esvazia todos os arrays usados durante as jogadas
        public void esvaziarArray()
        {
            for (int cont = 0; cont < 10; cont++)
            {
                arr[cont] = "0";
                arr1[cont] = "0";
                arr2[cont] = "0";
                arr3[cont] = "0";
                arr4[cont] = "0";
                arr5[cont] = "0";
                arr6[cont] = "0";
                arr7[cont] = "0";
                arr8[cont] = "0";
            }
            
            //Printa a primeira linha com um vetor vazio
            itm = new ListViewItem(arr);
            listView1.Items.Add(itm);
        }

        public Form1()
        {
            InitializeComponent();
            esvaziarArray();
            // Lê o txt de forma a realizar o aprendizado
            leituraTxt();
        }

        public void vitoria()
        {
            linhas = (k * 8) + 1; // +1 por conta do vetor zerado que aparece no início de cada partida

            // Usado para definir até onde os valores do campo "vitória" serão alterados com '1' ou '-1'
            fim = inicio + linhas;

            // Se alguma possível posição de vitória foi alcançada (um usuário completou uma linha ou diagonal)
            if ((a1 == a2 && a2 == a3) || (b1 == b2 && b2 == b3) || (c1 == c2 && c2 == c3) || (a1 == b1 && b1 == c1) || (a2 == b2 && b2 == c2) || (a3 == b3 && b3 == c3) || (a1 == b2 && b2 == c3) || (a3 == b2 && b2 == c1))
            {
                // Desabilita todos os botões
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;

                // Mostra o vencedor ('X' ou 'O')
                MessageBox.Show("O vencedor foi o: " + temp);

                // Altera os valores do campo "vitória" (listview1, subitem9) com '1' ou '-1'
                if (temp == "X")
                {
                    for (int cont = (inicio); cont < fim; cont++)
                    {
                        // Vitória de 'X'
                        listView1.Items[cont].SubItems[9].Text = "1";
                    }
                }
                else if (temp == "O")
                {
                    for (int cont = (inicio); cont < fim; cont++)
                    {
                        // Vitória de 'O'
                        listView1.Items[cont].SubItems[9].Text = "-1";
                    }
                }

                arquivo.WriteLine(arr8[0] + " " + arr8[1] + " " + arr8[2] + " " + arr8[3] + " " + arr8[4] + " " + arr8[5] + " " + arr8[6] + " " + arr8[7] + " " + arr8[8]);

                // Desabilita botão de jogada da RNA
                btnNeural.Enabled = false;                
            }

            // Se nenhuma possível posição de vitória foi alcançada (nenhum usuário completou uma linha ou diagonal)
            else
            {
                if (k >= 9)
                {
                    MessageBox.Show("Deu velha! :(");

                    arquivo.WriteLine(arr8[0] + " " + arr8[1] + " " + arr8[2] + " " + arr8[3] + " " + arr8[4] + " " + arr8[5] + " " + arr8[6] + " " + arr8[7] + " " + arr8[8]);

                    // Desabilita botão de jogada da RNA
                    btnNeural.Enabled = false;    
                }
            }
            // Fecha o arquivo  
            arquivo.Close();
        }

        // Sempre que 1 dos 9 botões do jogo da velha for clicado, essa função será chamada
        public void clicar(int num)
        {
            // Preenche todo o arr8 com 0
            // arr8 é o array que será lançado no listview2
            for (int cont = 0; cont < 9; cont++)
            {
                arr8[cont] = "0";
            }

            if (k % 2 == 1)
            {
                temp = "O";
                resp = "-1";

                // Somente a posição que o usuário clicou será preenchida, as demais permanecerão zeradas
                arr8[num] = resp;
            }
            else
            {
                temp = "X";
                resp = "1";

                // Somente a posição que o usuário clicou será preenchida, as demais permanecerão zeradas
                arr8[num] = resp;
            }

            // Converte a resposta pra int, já que é recebida inicialmente como string.
            aux = (Convert.ToInt32(resp) * -1);

            // Armazena as posições de giro
            if (num == 0)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[2] = resp;
                arr2[8] = resp;
                arr3[6] = resp;

                // Para usuário 'O' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[2] = aux.ToString();
                arr6[8] = aux.ToString();
                arr7[6] = aux.ToString();
            }
            else if (num == 1)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[5] = resp;
                arr2[7] = resp;
                arr3[3] = resp;

                // Para usuário 'O' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[5] = aux.ToString();
                arr6[7] = aux.ToString();
                arr7[3] = aux.ToString();
            }
            else if (num == 2)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[8] = resp;
                arr2[6] = resp;
                arr3[0] = resp;

                // Para usuário '0' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[8] = aux.ToString();
                arr6[6] = aux.ToString();
                arr7[0] = aux.ToString();
            }
            else if (num == 3)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[1] = resp;
                arr2[5] = resp;
                arr3[7] = resp;

                // Para usuário 'O' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[1] = aux.ToString();
                arr6[5] = aux.ToString();
                arr7[7] = aux.ToString();
            }
            else if (num == 4)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[num] = resp;
                arr2[num] = resp;
                arr3[num] = resp;

                // Para usuário 'O' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[num] = aux.ToString();
                arr6[num] = aux.ToString();
                arr7[num] = aux.ToString();
            }
            else if (num == 5)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[7] = resp;
                arr2[3] = resp;
                arr3[1] = resp;

                // Para usuário 'O' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[7] = aux.ToString();
                arr6[3] = aux.ToString();
                arr7[1] = aux.ToString();
            }
            else if (num == 6)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[0] = resp;
                arr2[2] = resp;
                arr3[8] = resp;

                // Para usuário '0' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[0] = aux.ToString();
                arr6[2] = aux.ToString();
                arr7[8] = aux.ToString();
            }
            else if (num == 7)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[3] = resp;
                arr2[1] = resp;
                arr3[5] = resp;

                // Para usuário '0' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[3] = aux.ToString();
                arr6[1] = aux.ToString();
                arr7[5] = aux.ToString();
            }
            else if (num == 8)
            {
                // Para usuário 'X' posição recebe '1'
                arr[num] = resp;
                arr1[6] = resp;
                arr2[0] = resp;
                arr3[2] = resp;

                // Para usuário '0' posição recebe '-1'
                arr4[num] = aux.ToString();
                arr5[6] = aux.ToString();
                arr6[0] = aux.ToString();
                arr7[2] = aux.ToString();
            }

            // Exibe no listview os 4 vetores das quatro posições de giro
            itm = new ListViewItem(arr);
            listView1.Items.Add(itm);
            itm = new ListViewItem(arr1);
            listView1.Items.Add(itm);
            itm = new ListViewItem(arr2);
            listView1.Items.Add(itm);
            itm = new ListViewItem(arr3);
            listView1.Items.Add(itm);

            // Exibe no listview os 4 vetores das quatro posições de giro (-1)
            itm = new ListViewItem(arr4);
            listView1.Items.Add(itm);
            itm = new ListViewItem(arr5);
            listView1.Items.Add(itm);
            itm = new ListViewItem(arr6);
            listView1.Items.Add(itm);
            itm = new ListViewItem(arr7);
            listView1.Items.Add(itm);

            // Exibe no listview2 o vetor de saída
            itm2 = new ListViewItem(arr8);
            listView2.Items.Add(itm2);

            // Garante que o conteúdo do vetor output nunca seja negativo
            for (int a = 0; a < arr8.Length; a++)
            {
                if (arr8[a] != "0"){arr8[a] = "1";}
            }

            /*
             * Aqui é onde os dados são lançados no arquivo .txt
             *             
             * */

            arquivo = System.IO.File.AppendText(enderecoTxt);

            // Verificar se o arquivo já existe
            if (!System.IO.File.Exists(enderecoTxt))
            {
                // Se não existir, ele será criado
                System.IO.File.Create(enderecoTxt).Close();
            }

            // Caso contrário, exporta as jogadas para o arquivo txt
            if (k == 0)
            {
                arquivo.WriteLine(" ");
                arquivo.Write("0 0 0 0 0 0 0 0 0 :");
            }       

            arquivo.WriteLine(arr8[0] + " " + arr8[1] + " " + arr8[2] + " " + arr8[3] + " " + arr8[4] + " " + arr8[5] + " " + arr8[6] + " " + arr8[7] + " " + arr8[8]);

            arquivo.WriteLine(arr[0] + " " + arr[1] + " " + arr[2] + " " + arr[3] + " " + arr[4] + " " + arr[5] + " " + arr[6] + " " + arr[7] + " " + arr[8] + " :" +
                            arr8[6] + " " + arr8[3] + " " + arr8[0] + " " + arr8[7] + " " + arr8[4] + " " + arr8[1] + " " + arr8[8] + " " + arr8[5] + " " + arr8[2]);
            arquivo.WriteLine(arr1[0] + " " + arr1[1] + " " + arr1[2] + " " + arr1[3] + " " + arr1[4] + " " + arr1[5] + " " + arr1[6] + " " + arr1[7] + " " + arr1[8] + " :" +
                            arr8[8] + " " + arr8[7] + " " + arr8[6] + " " + arr8[5] + " " + arr8[4] + " " + arr8[3] + " " + arr8[2] + " " + arr8[1] + " " + arr8[0]);
            arquivo.WriteLine(arr2[0] + " " + arr2[1] + " " + arr2[2] + " " + arr2[3] + " " + arr2[4] + " " + arr2[5] + " " + arr2[6] + " " + arr2[7] + " " + arr2[8] + " :" +
                            arr8[2] + " " + arr8[5] + " " + arr8[8] + " " + arr8[1] + " " + arr8[4] + " " + arr8[7] + " " + arr8[0] + " " + arr8[3] + " " + arr8[6]);
            arquivo.Write(arr3[0] + " " + arr3[1] + " " + arr3[2] + " " + arr3[3] + " " + arr3[4] + " " + arr3[5] + " " + arr3[6] + " " + arr3[7] + " " + arr3[8] + " :");
        
            k++; //Número de jogadas da partida atual
            total++; //Número de jogadas no geral
        }

        /*
         * Aqui estão todas as funções relativas ao clique do usuário em cada um dos botões
         *      - Chama a função clicar que altera texto do botão e lança dados no listview
         *      - Chama a função que verifica se uma das posições de vitória foram alcançadas 
         * */ 

        private void button1_Click(object sender, EventArgs e)
        {
            clicar(0);
            a1 = temp;
            sa1 = resp;
            button1.Text = temp;
            button1.Enabled = false;
            vitoria();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clicar(1);
            a2 = temp;
            sa2 = resp;
            button2.Text = temp;
            button2.Enabled = false;
            vitoria();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            clicar(2);
            a3 = temp;
            sa3 = resp;
            button3.Text = temp;
            button3.Enabled = false;
            vitoria();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            clicar(3);
            b1 = temp;
            sb1 = resp;
            button4.Text = temp;
            button4.Enabled = false;
            vitoria();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            clicar(4);
            b2 = temp;
            sb2 = resp;
            button5.Text = temp;
            button5.Enabled = false;
            vitoria();
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            clicar(5);
            b3 = temp;
            sb3 = resp;
            button6.Text = temp;
            button6.Enabled = false;
            vitoria();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            clicar(6);
            c1 = temp;
            sc1 = resp;
            button7.Text = temp;
            button7.Enabled = false;
            vitoria();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            clicar(7);
            c2 = temp;
            sc2 = resp;
            button8.Text = temp;
            button8.Enabled = false;
            vitoria();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            clicar(8);
            c3 = temp;
            sc3 = resp;
            button9.Text = temp;
            button9.Enabled = false;
            vitoria();
        }

        // Botão que inicia nova partida
        private void button10_Click(object sender, EventArgs e)
        {
            //Limpa os valores para que não seja encontrada uma posição de vitória
            a1 = "5"; a2 = "6"; a3 = "7"; b1 = "8"; b2 = "9"; b3 = "10"; c1 = "11"; c2 = "12"; c3 = "13";
            sa1 = "5"; sa2 = "6"; sa3 = "7"; sb1 = "8"; sb2 = "9"; sb3 = "10"; sc1 = "11"; sc2 = "12"; sc3 = "13";

            //Zera o número de jogadas atuais
            k = 0;

            //Muda a linha inicial usada no método que muda os valores do campo "vitória"
            //Começará à partir da linha seguinte à última linha da partida anterior (~~mindfuck~~)
            inicio = linhas;

            //Zera a quantidade de linhas da jogada atual
            linhas = 0;

            //Limpa o conteúdo (texto) dos botões
            button1.Text = "";
            button2.Text = "";
            button3.Text = "";
            button4.Text = "";
            button5.Text = "";
            button6.Text = "";
            button7.Text = "";
            button8.Text = "";
            button9.Text = "";

            //Habilita novamente os botões
            //Lembre-se que eles sempre são desabilitados após vitória ou "velha"
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            btnNeural.Enabled = true;
            
            //Chama função que esvazia todos os arrays utilizados durante o jogo
            esvaziarArray();
        }

        // Função que realiza treinamento da RNA com dados recebidos do .txt
        public void treinarRedeNeural(double[][] input, double[][] output)
        {         
            teacher.LearningRate = 0.2;
            int iterations = 1500; // Critério de parada
            double error = 0.2; // Critério de parada

            int i = 0;
            System.Collections.ArrayList errorsList = new System.Collections.ArrayList();

            while (i < iterations && error > 0.1)
            {
                // Executa procedimento de aprendizado
                error = teacher.RunEpoch(input, output);
                errorsList.Add(error);
                i++;
            }
        }

        // Quando o usuário clicar pedindo jogada da RNA, analisar aprendizado e assim escolher a opção correta a ser clicada
        private void btnNeural_Click(object sender, EventArgs e)
        {
            double[] saida;
            double[] ordenado;
            int maior = 8;

            // Gerar valor
            saida = geraJogada();
            // Escolhe a melhor opção dada a saída da rede neural
            ordenado = BubbleSort(saida);

            //retornar posicao do maior valor escolhido
            for (int x = 0; x < 9; x++)
            {
                if (ordenado[maior] == saida[x])
                {
                    pos = x;
                }
            }

            Boolean botao = true; // Garante que um botão já clicado, não seja escolhido novamente

            while (botao == true)
            {
                // Gerar valor
                saida = geraJogada();

                // Escolhe a melhor opção dada a saída da rede neural
                ordenado = BubbleSort(saida);

                Console.Write("Saida: ");
                for (int z = 0; z < saida.Length; z++)
                {
                    Console.Write(saida[z].ToString("F2") + " ");

                }
                Console.Write("\n");

                Console.Write("Ordenado: ");
                for (int z = 0; z < ordenado.Length; z++)
                {
                    Console.Write(ordenado[z].ToString("F2") + " ");
                }
                Console.Write("\n");

                Console.Write("Network: ");
                for (int z = 0; z < network.Output.Length; z++)
                {
                    Console.Write(network.Output[z].ToString("F2") + " ");
                }
                Console.Write("\n");

                //retornar posicao do maior valor escolhido
                for (int x = 0; x < 9; x++)
                {
                    if (ordenado[maior] == network.Output[x])
                    {
                        pos = x;
                    }
                }

                // Verifica se o botão já está clicado
                botao = verificaBtn(pos);
                maior--;
            }                 
            
            // Chama a função de clique relativa ao botão escolhido pelo RNA
                if (pos == 0) { button1.PerformClick(); }
                if (pos == 1) { button2.PerformClick(); }
                if (pos == 2) { button3.PerformClick(); }
                if (pos == 3) { button4.PerformClick(); }
                if (pos == 4) { button5.PerformClick(); }
                if (pos == 5) { button6.PerformClick(); }
                if (pos == 6) { button7.PerformClick(); }
                if (pos == 7) { button8.PerformClick(); }
                if (pos == 8) { button9.PerformClick(); }

                // Lança MessageBox na tela com output da RNA + o número do botão escolhido
                MessageBox.Show("| " + network.Output[0].ToString("F2") + "   " + network.Output[1].ToString("F2") + "   " + network.Output[2].ToString("F2") + " |\n" +
                                "| " + network.Output[3].ToString("F2") + "   " + network.Output[4].ToString("F2") + "   " + network.Output[5].ToString("F2") + " |\n" +
                                "| " + network.Output[6].ToString("F2") + "   " + network.Output[7].ToString("F2") + "   " + network.Output[8].ToString("F2") + " |\n" +
                                "\nBotão escolhido: " + (pos + 1));

                // Escreve no console matriz output da RNA + o número do botão escolhido
                Console.WriteLine("| " + network.Output[0].ToString("F2") + "   " + network.Output[1].ToString("F2") + "   " + network.Output[2].ToString("F2") + " |\n" +
                                  "| " + network.Output[3].ToString("F2") + "   " + network.Output[4].ToString("F2") + "   " + network.Output[5].ToString("F2") + " |\n" +
                                  "| " + network.Output[6].ToString("F2") + "   " + network.Output[7].ToString("F2") + "   " + network.Output[8].ToString("F2") + " |\n" +
                                  "\nBotão escolhido: " + (pos + 1));            
        }

        // Lê arquivo de texto e realiza aprendizado
        public void leituraTxt()
        {
            try
            {
                int quantidade; // Inicializa variável que receberá número de linhas do arquivo .txt
                quantidade = txtNumRows(); // Recebe número de linhas chamando função que verifica txt
                string line;

                // Array que armazena todos os inputs contidos no arquivo
                double[][] input = new double[quantidade][];

                // Array que armazena todos os outputs contidos no arquivo
                double[][] output = new double[quantidade][];

                // Array que recebe conteúdo input de cada linha
                double[] arrInput = new double[10];

                // Array que recebe conteúdo output de cada linha
                double[] arrOutput = new double[10];

                // Verifica se arquivo existe
                if (File.Exists(enderecoTxt))
                {
                    using (StreamReader reader = new StreamReader(enderecoTxt))
                    {
                        // Conta total de linhas do arquivo
                        int conttotal = 0;

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] split1 = line.Split(new Char[] { ':' });
                            int y = 0;
                            int j = 0;

                            foreach (string s1 in split1)
                            {
                                if (s1.Trim() != " ")
                                {
                                    string[] split2 = s1.Split(new Char[] { ' ' });

                                    foreach (string s2 in split2)
                                    {
                                        if (s2.Trim() != "")
                                        {
                                            if (y < 9)
                                            {
                                                String valorStr = s2;
                                                double valorDb = Double.Parse(valorStr);
                                                arrInput[y] = valorDb;
                                                y++;
                                            }
                                            else
                                            {
                                                String valorStr = s2;
                                                double valorDb = Double.Parse(valorStr);
                                                arrOutput[j] = valorDb;
                                                j++;
                                            }
                                        }
                                    }
                                }
                            }

                            // Array que passa entradas para aprendizado da RNA
                            input[conttotal] = new double[] { arrInput[0], arrInput[1],arrInput[2], arrInput[3], 
                                                                           arrInput[4], arrInput[5], arrInput[6],
                                                                           arrInput[7], arrInput[8]
                                                                           };
                            // Array que passa saídas para aprendizado da RNA
                            output[conttotal] = new double[] { arrOutput[0], arrOutput[1], arrOutput[2], arrOutput[3], 
                                                                           arrOutput[4], arrOutput[5], arrOutput[6],
                                                                           arrOutput[7], arrOutput[8]
                                                                           };
                            conttotal++;
                        }
                        treinarRedeNeural(input, output);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Verifica a quantidade de linhas contidas no txt
        public int txtNumRows()
        {
            int quantidade = 0;
            string linha;

            // Verifica se o arquivo existe
            if (File.Exists(enderecoTxt))
            {
                using (StreamReader reader = new StreamReader(enderecoTxt))
                {
                    // Enquanto linha lida for diferente de 'nula', soma mais uma linha ao contador
                    while ((linha = reader.ReadLine()) != null)
                    {
                        quantidade++;
                    }
                }
            }
            // Retorna quantidade total de linhas
            return quantidade;
        }

        // Usado pra retornar a posição do maior valor do output da RNA
        private double[] BubbleSort(double[] saida)
        {
            double swap = 0;
            for (int i = 0; i < saida.Length; i++)
            {
                for (int j = 0; j < (saida.Length - 1); j++)
                {
                    if (saida[j] > saida[j + 1])
                    {
                        swap = saida[j];
                        saida[j] = saida[j + 1];
                        saida[j + 1] = swap;
                    }
                }
            }
            return saida;
        }

        public double[] geraJogada()
        {
            double[][] input = new double[1][] 
                {
                    new double[] { (Convert.ToDouble(arr[0])), (Convert.ToDouble(arr[1])), (Convert.ToDouble(arr[2])), 
                                   (Convert.ToDouble(arr[3])), (Convert.ToDouble(arr[4])), (Convert.ToDouble(arr[5])), 
                                   (Convert.ToDouble(arr[6])), (Convert.ToDouble(arr[7])), (Convert.ToDouble(arr[8]))
                                 } 
                };

            // Recebe a entrada da jogada atual e analisa
            network.Compute(input[0]);

            // Gera saída com base no aprendizado
            for (int a = 0; a < 9; a++)
            {
                saida[a] = (network.Output[a]);
            }
            return saida;
        }

        // Verifica se o botão escolhido pela RNA já foi clicado
        // Marcado = true
        // Não marcado = false
        public Boolean verificaBtn(int indice)
        {
            //Verifica se o valor do botão é igual ao valor dado a ele na inicialização, caso contrário, esse botão foi clicado
            switch (indice)
            {
                case 0: if (a1 == "5") return false;
                        else return true;
                case 1: if (a2 == "6") return false;
                        else return true;
                case 2: if (a3 == "7") return false;
                        else return true;
                case 3: if (b1 == "8") return false;
                        else return true;
                case 4: if (b2 == "9") return false;
                        else return true;
                case 5: if (b3 == "10") return false;
                        else return true;
                case 6: if (c1 == "11") return false;
                        else return true;
                case 7: if (c2 == "12") return false;
                        else return true;
                case 8: if (c3 == "13") return false;
                        else return true;
                default: if (c3 == "13") return false;
                        else return true;
            }
        }
    }
}