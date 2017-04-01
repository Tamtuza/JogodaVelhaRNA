/*
* -- PLANO DE TRABALHO [Jogo da velha + Redes Neurais]:
*
* X 1) Armazenar saída;
* X 2) Armazenar "rotações" (x4);
* X 3) Armazenar inversos (x2);
* X 4) Inverter quando "bola" ganhar;
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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        static int k = 0;
        static int linhas = 0;
        static int inicio = 0;
        static int fim = 0;
        static int total = 0;
        static int aux = 0;
        static string temp;
        static string resp;
        static string a1="5", a2="6", a3="7", b1="8", b2="9", b3="10", c1="11", c2="12", c3="13";
        static string sa1 = "5", sa2 = "6", sa3 = "7", sb1 = "8", sb2 = "9", sb3 = "10", sc1 = "11", sc2 = "12", sc3 = "13";

        //Arrays normais
        static string[] arr = new string[10];
        static string[] arr1 = new string[10];
        static string[] arr2 = new string[10];
        static string[] arr3 = new string[10];

        //Arrays inversos
        static string[] arr4 = new string[10];
        static string[] arr5 = new string[10];
        static string[] arr6 = new string[10];
        static string[] arr7 = new string[10];

        //Array de saída
        static string[] arr8 = new string[10];

        static ListViewItem itm;
        static ListViewItem itm2;

        public void esvaziarArray() {
            for (int cont = 0; cont < 10; cont++) {
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
        }
        public void vitoria()
        {
            linhas = (k * 8) + 1; // +1 por conta do vetor zerado no início de cada partida
            fim = inicio + linhas;
            if ((a1 == a2 && a2 == a3) || (b1 == b2 && b2 == b3) || (c1 == c2 && c2 == c3) || (a1 == b1 && b1 == c1) || (a2 == b2 && b2 == c2) || (a3 == b3 && b3 == c3) || (a1 == b2 && b2 == c3) || (a3 == b2 && b2 == c1))
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                MessageBox.Show("O vencedor foi o: " + temp);

                if (temp == "X")
                {
                    for (int cont = (inicio); cont < fim; cont++)
                    {
                        listView1.Items[cont].SubItems[9].Text = "1";
                    }
                }
                else if (temp == "O"){
                    for (int cont = (inicio); cont < fim; cont++)
                    {
                        listView1.Items[cont].SubItems[9].Text = "-1";
                    }
                }
            }
            else {
                if (k >= 9) {
                    MessageBox.Show("DEU VELHA!!!");                    
                }            
            }
            //Simulando fonte de dados
            DataTable dt = new DataTable();

            //Criando colunas
            dt.Columns.Add("a1", typeof(int));
            dt.Columns.Add("a2", typeof(string));
            dt.Columns.Add("a3", typeof(int));
            dt.Columns.Add("b1", typeof(int));
            dt.Columns.Add("b2", typeof(int));
            dt.Columns.Add("b3", typeof(int));
            dt.Columns.Add("c1", typeof(int));
            dt.Columns.Add("c2", typeof(int));
            dt.Columns.Add("c3", typeof(int));
            dt.Columns.Add("vitoria", typeof(int));
            dt.Columns.Add("", typeof(string));
            dt.Columns.Add("sa1", typeof(int));
            dt.Columns.Add("sa2", typeof(int));
            dt.Columns.Add("sa3", typeof(int));
            dt.Columns.Add("sb1", typeof(int));
            dt.Columns.Add("sb2", typeof(int));
            dt.Columns.Add("sb3", typeof(int));
            dt.Columns.Add("sc1", typeof(int));
            dt.Columns.Add("sc2", typeof(int));
            dt.Columns.Add("sc3", typeof(int));

            //Adicionando valores
            dt.Rows.Add(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[0],
                        arr8[0], arr8[1], arr8[2], arr8[3], arr8[4], arr8[5], arr8[6], arr8[7], arr8[8]);
            dt.Rows.Add(arr1[0], arr1[1], arr1[2], arr1[3], arr1[4], arr1[5], arr1[6], arr1[7], arr1[8], arr1[0]);
            dt.Rows.Add(arr2[0], arr2[1], arr2[2], arr2[3], arr2[4], arr2[5], arr2[6], arr2[7], arr2[8], arr2[0]);
            dt.Rows.Add(arr3[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[0]);
            dt.Rows.Add(arr4[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[0]);
            dt.Rows.Add(arr5[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[0]);
            dt.Rows.Add(arr6[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[0]);
            dt.Rows.Add(arr7[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8], arr[0]);

            //Exportando para Datatable, pode criar um método da rotina abaixo
            Write(dt, @"C:\arquivo.txt");
        }
        public void clicar(int indice)
        {
            for (int cont = 0; cont < 9; cont++)
            {
                arr8[cont] = "0";
            }
            if (k % 2 == 1)
            {
                temp = "O";
                resp = "-1";
                arr8[indice] = resp;
            }
            else
            {
                temp = "X";
                resp = "1";
                arr8[indice] = resp;
            }
            
            aux = (Convert.ToInt32(resp) * -1);

            // Armazena as posições de giro
            if (indice == 0)
            {
                arr[indice] = resp;
                arr1[2] = resp;
                arr2[8] = resp;
                arr3[6] = resp;
                arr4[indice] = aux.ToString();
                arr5[2] = aux.ToString();
                arr6[8] = aux.ToString();
                arr7[6] = aux.ToString();
            }
            else if (indice == 1)
            {
                arr[indice] = resp;
                arr1[5] = resp;
                arr2[7] = resp;
                arr3[3] = resp;
                arr4[indice] = aux.ToString();
                arr5[5] = aux.ToString();
                arr6[7] = aux.ToString();
                arr7[3] = aux.ToString();
            }
            else if (indice == 2)
            {
                arr[indice] = resp;
                arr1[8] = resp;
                arr2[6] = resp;
                arr3[0] = resp;
                arr4[indice] = aux.ToString();
                arr5[8] = aux.ToString();
                arr6[6] = aux.ToString();
                arr7[0] = aux.ToString();
            }
            else if (indice == 3)
            {
                arr[indice] = resp;
                arr1[1] = resp;
                arr2[5] = resp;
                arr3[7] = resp;
                arr4[indice] = aux.ToString();
                arr5[1] = aux.ToString();
                arr6[5] = aux.ToString();
                arr7[7] = aux.ToString();
            }
            else if (indice == 4)
            {
                arr[indice] = resp;
                arr1[indice] = resp;
                arr2[indice] = resp;
                arr3[indice] = resp;
                arr4[indice] = aux.ToString();
                arr5[indice] = aux.ToString();
                arr6[indice] = aux.ToString();
                arr7[indice] = aux.ToString();
            }
            else if (indice == 5)
            {
                arr[indice] = resp;
                arr1[7] = resp;
                arr2[3] = resp;
                arr3[1] = resp;
                arr4[indice] = aux.ToString();
                arr5[7] = aux.ToString();
                arr6[3] = aux.ToString();
                arr7[1] = aux.ToString();
            }
            else if (indice == 6)
            {
                arr[indice] = resp;
                arr1[0] = resp;
                arr2[2] = resp;
                arr3[8] = resp;
                arr4[indice] = aux.ToString();
                arr5[0] = aux.ToString();
                arr6[2] = aux.ToString();
                arr7[8] = aux.ToString();
            }
            else if (indice == 7)
            {
                arr[indice] = resp;
                arr1[3] = resp;
                arr2[1] = resp;
                arr3[5] = resp;
                arr4[indice] = aux.ToString();
                arr5[3] = aux.ToString();
                arr6[1] = aux.ToString();
                arr7[5] = aux.ToString();
            }
            else if (indice == 8)
            {
                arr[indice] = resp;
                arr1[6] = resp;
                arr2[0] = resp;
                arr3[2] = resp;
                arr4[indice] = aux.ToString();
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

            itm2 = new ListViewItem(arr8);
            listView2.Items.Add(itm2);

            k++;
            total++;
        }

        /// <summary>
        /// Método responsavel por exportar para TXT
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="outputFilePath"></param>
        public static void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;

                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;

                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i].ColumnName.PadRight(maxLengths[i] + 2));
                }

                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().PadRight(maxLengths[i] + 2));
                        }
                        else
                        {
                            sw.Write(new string(' ', maxLengths[i] + 2));
                        }
                    }

                    sw.WriteLine();
                }

                sw.Close();
            }
        }

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

        private void button10_Click(object sender, EventArgs e)
        {
            a1 = "5"; a2 = "6"; a3 = "7"; b1 = "8"; b2 = "9"; b3 = "10"; c1 = "11"; c2 = "12"; c3 = "13";
            sa1 = "5"; sa2 = "6"; sa3 = "7"; sb1 = "8"; sb2 = "9"; sb3 = "10"; sc1 = "11"; sc2 = "12"; sc3 = "13";
            k = 0;
            inicio = linhas;
            linhas = 0;

            button1.Text = "";
            button2.Text = "";
            button3.Text = "";
            button4.Text = "";
            button5.Text = "";
            button6.Text = "";
            button7.Text = "";
            button8.Text = "";
            button9.Text = "";

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;

            esvaziarArray();
        }
    }
}
