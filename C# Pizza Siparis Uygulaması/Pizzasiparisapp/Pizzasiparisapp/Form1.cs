using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;


namespace _171006Win_PizzaOtomasyonu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private SQLiteDataAdapter DB;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();

        //connection
        private void SetConnection()
        {
            sql_con = new SQLiteConnection("Data Source=dbapps.db;Version=3;New=False;Compress=True");
        }

        //set execuery
        private void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

        //load DB
        private void LoadData()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "select * from tbapps";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            dataGridView1.DataSource = DT;
            sql_con.Close();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            LoadData();

            Ebat kucuk = new Ebat { Adi = "Küçük", Carpan=1 };
            Ebat orta = new Ebat { Adi = "Orta", Carpan = 1.50 };
            Ebat buyuk = new Ebat { Adi = "Büyük", Carpan = 2 };
            Ebat xlarge = new Ebat { Adi = "Xlarge ", Carpan = 2.25 };
            cmbEbat.Items.Add(kucuk);
            cmbEbat.Items.Add(orta);
            cmbEbat.Items.Add(buyuk);
            cmbEbat.Items.Add(xlarge);

            Pizza klasik = new Pizza { Adi = "Klasik", Fiyat = 15 };
            Pizza italiano = new Pizza { Adi = "Italiano", Fiyat = 22 };
            Pizza turkish = new Pizza { Adi = "Turkish", Fiyat = 23 };
            Pizza tonbalik = new Pizza { Adi = "TonBalıklı", Fiyat = 20};
            Pizza akdeniz = new Pizza { Adi = "Akdeniz", Fiyat = 18 };
            Pizza tavuklu = new Pizza { Adi = "Tavuklu", Fiyat = 19 };
            Pizza bolmalzemeli = new Pizza { Adi = "Bol Malzemeli", Fiyat = 24 };
            Pizza mevsim = new Pizza { Adi = "Mevsim Yeşillikli", Fiyat = 17 };
            listPizzalar.Items.Add(klasik);
            listPizzalar.Items.Add(italiano);
            listPizzalar.Items.Add(turkish);
            listPizzalar.Items.Add(tonbalik);
            listPizzalar.Items.Add(akdeniz);
            listPizzalar.Items.Add(tavuklu);
            listPizzalar.Items.Add(bolmalzemeli);
            listPizzalar.Items.Add(mevsim);
            
            KenarTip ince = new KenarTip { Adi = "İnce Kenar", EkFiyat = 0 };
            rdbInceKenar.Tag = ince;
            KenarTip kalin = new KenarTip { Adi = "Kalın Kenar", EkFiyat = 2 };
            rdbKalinKenar.Tag = kalin;



        }
        Siparis s;
        private void btnHesapla_Click(object sender, EventArgs e)
        {
            Pizza p = (Pizza)listPizzalar.SelectedItem;
            p.Ebati =(Ebat) cmbEbat.SelectedItem;
            p.KenarTipi = rdbInceKenar.Checked ? (KenarTip)rdbInceKenar.Tag : (KenarTip)rdbKalinKenar.Tag;
            p.Malzemeler = new List<string>();
            foreach (CheckBox ctrl in groupBox1.Controls)
            {
                if (ctrl.Checked)
                {
                    p.Malzemeler.Add(ctrl.Text);
                }
            }
            decimal tutar = nudAdet.Value * p.Tutar;
            txtTutar.Text = tutar.ToString();
            s = new Siparis();
            s.Pizza = p;
            s.Adet =(int) nudAdet.Value;
           
        }

        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            if (s != null) {
                listSepet.Items.Add(s);
                }
        }

        private void btnSiparisOnay_Click(object sender, EventArgs e)
        {
            decimal toplamTutar = 0;
            int sayac = 0;
            foreach (Siparis spr in listSepet.Items)
            {
                toplamTutar += spr.Adet * spr.Pizza.Tutar;
                sayac++;
            }
            lblToplamTutar.Text = toplamTutar.ToString();
            MessageBox.Show(string.Format("Toplam Sipariş Adediniz :{0} Toplam Sipariş Tutarınız :{1}" ,sayac, toplamTutar));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txtQuery = "insert into tbapps (ID,Name)values('" + textBox1.Text + "','" + textBox2.Text + "')";
            ExecuteQuery(txtQuery);
            LoadData();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            string txtQuery = "update tbapps set Name= '" + textBox2.Text + "'where ID='" + textBox1.Text + "')";
            ExecuteQuery(txtQuery);
            LoadData();
        }
        /*
        private void dataGridWiew1_Cell(object sender, DataGridViewCellEventArgs e)
        {

        }
        */

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string txtQuery = "delete from tbapps where ID='" + textBox1.Text + "'";
            ExecuteQuery(txtQuery);
            LoadData();
        }
    }
}
