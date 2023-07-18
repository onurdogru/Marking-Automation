//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO; //ini file
using System.Linq;
using System.Runtime.InteropServices; //ini kaydet Dll
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;



namespace TamOtomatikBlisterMakinesi2
{
    public partial class Form1 : Form
    {
        

        //SQL ve ini alanı başlangıç
        #region SQL ve INI ALANI


        /*
		SqlConnection sqlConnection;
		SqlCommand sqlCommand;
		SqlDataReader sqlDataReader;
		SqlDataAdapter sqlDataAdapter;
		*/

        static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string modelFolderPath = desktopPath + @"\Debug";
        string modelFilePath = desktopPath + @"\Debug\Modeller.ini";
        INIKaydet ini;


        //SQL ve ini alanı bitiş
        #endregion

        //Thread tanıtımı baslangıc
        #region THREAD TANITIMLARI



        bool boolReadStatus, boolIsEmri;
        int readLoopCounter = 0, countModel = 0, selectedModel = 0, dropDownHeight = 0;
        Thread threadRead, threadWriteBool, threadWriteString, threadReadString;
        //Thread tanıtımı bitiş  Tread threadReadString, bool boolIsEmri readLoopCp


        #endregion//




        public Form1()
        {
            InitializeComponent();
        }

        void Form3() { }



        private void Form1_Load(object sender, EventArgs e)
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            cmbBxWorkingMod.SelectedIndex = 0;
            ini = new INIKaydet(modelFilePath);
            //loopMainRead();

            threadWriteBool = new Thread(() => nxCompoletBoolWrite("connectionStart", true));
            threadWriteBool.Start();
          //  threadRead = new Thread(readPlcData);
            //threadRead.Start();
            

            threadWriteString = new Thread(() => nxCompoletStringWrite("jogHiz", "20"));
            threadWriteString.Start();


            //******************************************************GİZLENEN BUTONLAR***************************************************/
            //btnProductionEnd.Visible = false;
            btnStop.Visible = false;
            btnRawMaterial.Visible = false;
            btnExecutiveEngineMalfunction.Visible = false;
            btnResistorMotorFault.Visible = false;
            textBox17.Visible = false;
            textBox23.Visible = false;
            textBox21.Visible = false;
            textBox20.Visible = false;
            textBox19.Visible = false;
            textBox18.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            btnSave.Visible = false;
            btnKaydet.Visible = false;



            textBox1.Text = "20";
            string modelAdi = txtMODELADI.Text;
            ShowData1($"Select * from {modelAdi}Temizle");
            ShowData2($"Select * from {modelAdi}Temizle");
            cmbGet();

            //textBox8.Text = (gdvFikstur1.Rows.Count + 1).ToString();
            textBox17.Text = (gdvFikstur2.Rows.Count).ToString();


            comboBox1.SelectedIndex = 0;
            cmbBaski1.SelectedIndex = 0;
            cmbBaski2.SelectedIndex = 0;

            //if (dataAccessProccess())
            //    //btnDBStatus.BackColor = Color.Green;
            if (nxCompoletBoolRead("connectionOk") && boolReadStatus == false)
            {
                btnStatus.BackColor = Color.ForestGreen;
                uretilenAdet.Text = nxCompoletStringRead("uretilenAdet");
                threadRead = new Thread(readPlcData);
                threadRead.Start();
            }
            else
            {
                btnStatus.BackColor = Color.Red;
            }
            //dropDownHeight = cmbBxModelSelect.DropDownHeight;

        }

        //FORM CLOSİNG KISMI EKLENEBİLİR
        #region FORM CLOSİNG ALANI


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {


            //Setting.Default.heatingTime = txtBxHeatingTime.Text;
            //Setting.Default.vacuumTime = txtBxVacuumTime.Text;
            //Setting.Default.vacuumAfter = txtBxVacuumAfter.Text;
            //Setting.Default.coolingTime = txtCoolingTime.Text;
            Setting.Default.txtPrdOrder = txtPrdOrder.Text;
            Setting.Default.txtUProductionModel = txtUProductionModel.Text;
            //Setting.Default.txtUProductionUnit = txtUProductionUnit.Text;
            Setting.Default.Save();

            try
            {
                //	sqlConnection.Close();
                threadWriteBool = new Thread(() => nxCompoletBoolWrite("connectionStart", false));
                threadWriteBool.Start();
                threadRead.Abort();
                Thread.Sleep(100);
            }
            catch (Exception)
            {
            }
            Environment.Exit(1);
        }
        #endregion

        #region SQL BAGLANTI KISMI


        //SQL KISMI

        /*
        private bool dataAccessProccess()
		{

			string server = @"192.168.10.22";
			string database = "ALP802";
			string user = "otomasyon";
			string pass = "123KUM*";
			String connection = @"Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + user + ";Password=" + pass;
			//sqlConnection = new SqlConnection(connection);
			try
			{
				//sqlConnection.Open();
				return true;
			}
			catch (Exception)
			{
				MessageBox.Show( "Veritabanı bağlantısı kurulamadı !");
				return false;
			}

		}
		*/
        #endregion

        #region GET BÖLÜMLERİ (GETMODELNAME, GETMODELDATA, GETMODELSETTİNG, CONTROLMODELDATA INI vb.)

        /*
        private void getModelName()
		{
			if(Directory.Exists(modelFolderPath) && File.Exists(modelFilePath))
			{
				//cmbBxModelSelect.Items.Clear();
				int modelCount = Int16.Parse(ini.Oku("Model", "Model Count").Trim());
				for (int i = 1; i <= modelCount; i++)
				{
					//cmbBxModelSelect.Items.Add(ini.Oku("Model" + i.ToString(), "Model Name"));
				}
				//controlModelFileData(modelCount);
			}
		}
		*/
        private void getModelData(int index)
        {
            //cmbBxModelSelect.Text = ini.Oku("Model"+(index+1).ToString(), "Model Name").Trim();
            //txtBxRunningDistance.Text= ini.Oku("Model" + (index + 1).ToString(), "Running Distance").Trim();
            //txtBxMoldNumber.Text = ini.Oku("Model" + (index + 1).ToString(), "Mold Number").Trim();
        }

        private void getModelSetting()
        {
            //txtBxHeatingTime.Text = Setting.Default.heatingTime;
            //txtBxVacuumTime.Text = Setting.Default.vacuumTime;
            //txtBxVacuumAfter.Text = Setting.Default.vacuumAfter;
            //txtCoolingTime.Text = Setting.Default.coolingTime;
            //txtPrdOrder.Text = Setting.Default.txtPrdOrder;
            //txtUProductionModel.Text = Setting.Default.txtUProductionModel;
            //txtUProductionUnit.Text = Setting.Default.txtUProductionUnit;
        }
        private void getModelSelectedData()
        {
            //cmbBxModelSelect.SelectedIndex = Setting.Default.modelCmbBxIndex;
        }
        /*
		private void controlModelFileData(int modelCount)
		{
			try
			{
				 modelCount = Int16.Parse(ini.Oku("Model", "Model Count").Trim());
				for (int i = 1; i <= modelCount; i++)
				{
					ini.Oku("Model"+i.ToString(), "Model Name").Trim();
			}
			}
			catch (Exception)
			{

				MessageBox.Show("Modeller dosyasınındaki model sayılarını düzenleyiniz !");
			}
		}
		*/
        #endregion


        //readPLCData Kısmı / buton renk dönüşümleri ile ilgili
        #region readPLCdata






        //SONRA BAKILACAK
        private void readPlcData()
        {

            while (true)
            {
                Thread.Sleep(100);
                if (readLoopCounter == 10)
                {
                    readLoopCounter = 0;


                    /************************************************HARDWARE*****************************************************************************/
                    /********************************DONANIM BUTONLARI İLE İLGİLİ AKSİYONLAR**************************************************************/
                    if (nxCompoletBoolRead("Donanim[" + 0 + "]") && !boolReadStatus)
                    {
                        btnTopPressUp.BackColor = Color.Green;
                    }
                    else
                    {
                        btnTopPressUp.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 1 + "]") && !boolReadStatus)
                    {
                        btnTopPressDown.BackColor = Color.Green;
                    }
                    else
                    {
                        btnTopPressDown.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 2 + "]") && !boolReadStatus)
                    {
                        btnBottomPressUp.BackColor = Color.Green;
                    }
                    else
                    {
                        btnBottomPressUp.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 3 + "]") && !boolReadStatus)
                    {
                        btnBottomPressDown.BackColor = Color.Green;
                    }
                    else
                    {
                        btnBottomPressDown.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 4 + "]") && !boolReadStatus)
                    {
                        btnElavotorUp.BackColor = Color.Green;
                    }
                    else
                    {
                        btnElavotorUp.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 5 + "]") && !boolReadStatus)
                    {
                        btnElavotorDown.BackColor = Color.Green;
                    }
                    else
                    {
                        btnElavotorDown.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 6 + "]") && !boolReadStatus)
                    {
                        btnPressingUp.BackColor = Color.Green;
                    }
                    else
                    {
                        btnPressingUp.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 7 + "]") && !boolReadStatus)
                    {
                        btnPressingDown.BackColor = Color.Green;
                    }
                    else
                    {
                        btnPressingDown.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 8 + "]") && !boolReadStatus)
                    {
                        btnDoorSensor.BackColor = Color.Green;
                    }
                    else
                    {
                        btnDoorSensor.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 9 + "]") && !boolReadStatus)
                    {
                        btnElavotorDoorSensor.BackColor = Color.Green;
                    }
                    else
                    {
                        btnElavotorDoorSensor.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 12 + "]") && !boolReadStatus)
                    {
                        btnHomeOk.BackColor = Color.Green;
                    }
                    else
                    {
                        btnHomeOk.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 11 + "]") && !boolReadStatus)
                    {
                        btnResistanceTooHot.BackColor = Color.Green;
                    }
                    else
                    {
                        btnResistanceTooHot.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 10 + "]") && !boolReadStatus)
                    {
                        btnStop.BackColor = Color.Green;
                    }
                    else
                    {
                        btnStop.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 13 + "]") && !boolReadStatus)
                    {
                        btnRawMaterial.BackColor = Color.Green;
                    }
                    else
                    {
                        btnRawMaterial.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 14 + "]") && !boolReadStatus)
                    {
                        btnExecutiveEngineMalfunction.BackColor = Color.Green;
                    }
                    else
                    {
                        btnExecutiveEngineMalfunction.BackColor = Color.FromArgb(41, 53, 65); ;
                    }
                    if (nxCompoletBoolRead("Donanim[" + 15 + "]") && !boolReadStatus)
                    {
                        btnResistorMotorFault.BackColor = Color.Green;
                    }
                    else
                    {
                        btnResistorMotorFault.BackColor = Color.FromArgb(41, 53, 65); ;
                    }


                    /**************************************HARDWARE**********************************/
                    uretilenAdet.Text = nxCompoletStringRead("uretilenAdet");
                    txtBxCycleTime.Text = nxCompoletStringRead("cevrimSure");


                    if (nxCompoletStringRead("uretimDurum") == "0")
                    {
                        textBox9.Text = "Üretim Yok";
                    }
                    else if (nxCompoletStringRead("uretimDurum") == "1")
                    {
                        textBox9.Text = "Üretim Yapılıyor...";
                    }
                    else if (nxCompoletStringRead("uretimDurum") == "2")
                    {
                        textBox9.Text = "Üretim Durduruldu !";
                    }
                }
                readLoopCounter++;
            }
        }

        #endregion


        #region AccessBaglantı

        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= C:\\Veriler\\Fiksturler.mdb");
        OleDbDataAdapter da;
        DataSet ds;


        #endregion SQLBaglantı

        private void ShowData1(string veri)
        {
            da = new OleDbDataAdapter(veri, conn);
            ds = new DataSet();
            conn.Open();
            da.Fill(ds, $"{veri}");
            var result = ds.Tables[$"{veri}"];

            result.Columns.Add("PozNo", typeof(int));
            result.Columns["PozNo"].SetOrdinal(0);
            for (int i = 0; i < result.Rows.Count; i++)
            {
                result.Rows[i]["PozNo"] = i + 1;
            }
            gdvFikstur1.DataSource = result;
            gdvFikstur1.Columns["PozisyonNo"].Visible = false;

            conn.Close();

            for (int i = gdvFikstur1.Rows.Count - 1; i > 0; i--)
            {
                string pozisyonNo = gdvFikstur1.Rows[i].Cells[1].Value.ToString();
                textBox8.Text = (int.Parse(pozisyonNo) + 1).ToString();
                break;
            }

            string gecici = "";

            for (int i = 0; i <= gdvFikstur1.RowCount - 1; i++)
            {
                for (int j = i + 1; j < gdvFikstur1.RowCount; j++)
                {

                    if (Convert.ToInt32(gdvFikstur1.Rows[i].Cells[0].Value) > Convert.ToInt32(gdvFikstur1.Rows[j].Cells[0].Value))
                    {
                        gecici = gdvFikstur1.Rows[i].Cells[0].Value.ToString();
                        gdvFikstur1.Rows[i].Cells[0].Value = gdvFikstur1.Rows[j].Cells[0].Value;
                        gdvFikstur1.Rows[j].Cells[0].Value = gecici.ToString();
                    }

                }
            }
        }

        private void ShowData2(string veri)
        {

            da = new OleDbDataAdapter(veri, conn);
            ds = new DataSet();
            conn.Open();
            da.Fill(ds, $"{veri}");

            var result = ds.Tables[$"{veri}"];

            result.Columns.Add("PozNo", typeof(int));
            result.Columns["PozNo"].SetOrdinal(0);
            for (int i = 0; i < result.Rows.Count; i++)
            {
                result.Rows[i]["PozNo"] = i + 1;
            }
            gdvFikstur2.DataSource = result;
            gdvFikstur2.Columns["PozisyonNo"].Visible = false;

            conn.Close();
            textBox8.Text = (gdvFikstur1.Rows.Count + 1).ToString();
            textBox17.Text = (gdvFikstur2.Rows.Count).ToString();

            string gecici = "";

            for (int i = 0; i <= gdvFikstur2.RowCount - 1; i++)
            {
                for (int j = i + 1; j < gdvFikstur2.RowCount; j++)
                {

                    if (Convert.ToInt32(gdvFikstur2.Rows[i].Cells[0].Value) > Convert.ToInt32(gdvFikstur2.Rows[j].Cells[0].Value))
                    {
                        gecici = gdvFikstur2.Rows[i].Cells[0].Value.ToString();
                        gdvFikstur2.Rows[i].Cells[0].Value = gdvFikstur2.Rows[j].Cells[0].Value;
                        gdvFikstur2.Rows[j].Cells[0].Value = gecici.ToString();
                    }

                }
            }
        }

        private void lblMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void lblExit_Click(object sender, EventArgs e)
        {

            Application.Exit();
        }

        private void lblMin_MouseHover(object sender, EventArgs e)
        {
            lblMin.ForeColor = Color.Red;
        }

        private void lblMin_MouseLeave(object sender, EventArgs e)
        {
            lblMin.ForeColor = Color.White;
        }

        private void lblExit_MouseHover(object sender, EventArgs e)
        {
            lblExit.ForeColor = Color.Red;
        }

        private void lblExit_MouseLeave(object sender, EventArgs e)
        {
            lblExit.ForeColor = Color.White;

        }
        private void btnHome_MouseDown(object sender, MouseEventArgs e)
        {
            btnHome.ForeColor = Color.FromArgb(255, 180, 0);
        }

        private void btnHome_MouseUp(object sender, MouseEventArgs e)
        {
            btnHome.ForeColor = Color.White;
        }







        ///*******************************************************ÇALIŞMA MOD - HOME BUTON ÇALIŞMA PRENSİBİ***********************///////
        private void btnHome_Click(object sender, EventArgs e)
        {

            threadWriteBool = new Thread(() => nxCompoletBoolWrite("homeButon", true));
            threadWriteBool.Start();
        }

        private void btnModelSelectSave_MouseDown(object sender, MouseEventArgs e)
        {
            btnModelSelectSave.BackColor = Color.Green;
        }

        private void btnModelSelectSave_MouseUp(object sender, MouseEventArgs e)
        {
            btnModelSelectSave.BackColor = Color.ForestGreen;
        }

        private void btnSelectModeNewRecord_MouseDown(object sender, MouseEventArgs e)
        {
            btnSelectModeNewRecord.ForeColor = Color.FromArgb(255, 180, 0);
        }

        private void btnSelectModeNewRecord_MouseUp(object sender, MouseEventArgs e)
        {
            btnSelectModeNewRecord.ForeColor = Color.White;
        }

        private void btnModelSettingsSend_MouseDown(object sender, MouseEventArgs e)
        {
            //btnModelSettingsSend.BackColor = Color.Navy;
        }

        private void btnModelSettingsSend_MouseUp(object sender, MouseEventArgs e)
        {
            //btnModelSettingsSend.BackColor = Color.MidnightBlue;
        }

        private void textClear(ComboBox comboBox, params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Text = "";
            }
            comboBox.Text = "";
        }
        private bool textEmptyControl(ComboBox comboBox, params TextBox[] textBoxes)
        {
            bool flag = false;
            foreach (var textBox in textBoxes)
            {
                if (textBox.Text == "")
                    flag = true;
            }
            if (comboBox.Text == "")
                flag = true;
            return flag;
        }

        private void btnSelectModeNewRecord_Click(object sender, EventArgs e)
        {

            btnModelSelectSave.Enabled = true;

            string modelAdi = txtMODELADI.Text;

            conn.Open();
            OleDbCommand com = new OleDbCommand($"CREATE TABLE {modelAdi} (PozisyonNO VARCHAR(50) PRIMARY KEY , XPozisyon VARCHAR(50), ZPozisyon VARCHAR(50), WBaşlangıçPozisyon VARCHAR(50), WBitişPozisyon VARCHAR(50), WDönmeSüre VARCHAR(50)) ", conn);
            com.ExecuteNonQuery();
            conn.Close();
            DataAdd2();
            cmbMODEL.Items.Clear();
            cmbGet();

        }
        private void btnModelSelectSave_Click(object sender, EventArgs e)
        {
            for (int i = gdvFikstur1.Rows.Count - 1; i > 0; i--)
            {
                string pozisyonNo = gdvFikstur1.Rows[i].Cells[1].Value.ToString();
                textBox8.Text = (int.Parse(pozisyonNo) + 1).ToString();
                break;
            }
            if (textBox8.Text != "" && textBox15.Text != "" && textBox13.Text != "" && textBox12.Text != "" && textBox11.Text != "" && textBox10.Text != "")
            {

                string modelAdi = txtMODELADI.Text;
                DataAdd();
                ShowData1($"Select * from {modelAdi}");
                ShowData2($"Select * from {modelAdi}");
                textBox15.Clear();
                textBox13.Clear();
                textBox12.Clear();
                textBox11.Clear();
                textBox10.Clear();


            }
            else
            {
                MessageBox.Show("Pozisyon bilgilerinden birinde kesik var, kontrol ediniz !!", "Hata");
            }
        }

        private void btnModelSelectSend_Click(object sender, EventArgs e)
        {

            conn.Open();


            var tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

            foreach (System.Data.DataRow row in tables.Rows)
            {
                string tableName = (string)row["TABLE_NAME"];
                cmbMODEL.Items.Add(tableName);
            }

            conn.Close();

        }
        private void modelSave(string message)
        {
        }

        private void cmbGet()
        {
            conn.Open();


            var tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

            foreach (System.Data.DataRow row in tables.Rows)
            {
                string tableName = (string)row["TABLE_NAME"];
                cmbMODEL.Items.Add(tableName);
            }

            conn.Close();
        }




        //ÇALIŞMA MOD - COMBOBOX
        private void cmbBxWorkingMod_SelectedIndexChanged(object sender, EventArgs e)
        {


            ///*********************************************Çalışma Mod Combobox Çalışma Prensibi***************************************************************//
            threadWriteString = new Thread(() => nxCompoletStringWrite("calismaMod", cmbBxWorkingMod.SelectedIndex.ToString()));
            threadWriteString.Start();


        }








        //****************************************X EKSENİ SOL BUTON******************************************//

        private void btnElavotorMotorUp_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[0]", true));
            threadWriteBool.Start();

        }
        private void btnElavotorMotorUp_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[0]", false));
            threadWriteBool.Start();
        }









        //****************************************X EKSENİ SAĞ BUTON******************************************//

        private void btnElavotorMotorDown_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[1]", true));
            threadWriteBool.Start();
        }

        private void btnElavotorMotorDown_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[1]", false));
            threadWriteBool.Start();
        }









        /**************************************************Z EKSENİ AŞAĞI BUTON***************************************************/

        private void btnBakeryDown_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[2]", true));
            threadWriteBool.Start();
        }

        private void btnBakeryDown_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[2]", false));
            threadWriteBool.Start();
        }



















        /**************************************************Z EKSENİ YUKARI BUTON***************************************************/

        private void btnBakeryUp_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[3]", true));
            threadWriteBool.Start();

        }

        private void btnBakeryUp_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[3]", false));
            threadWriteBool.Start();
        }










        /**************************************FİKSTÜR 1 SOL BUTON**********************************************/
        private void bntExecutiveMotorLeft_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[4]", true));
            threadWriteBool.Start();
        }

        private void bntExecutiveMotorLeft_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[4]", false));
            threadWriteBool.Start();
        }

















        /*****************************************FİKSTÜR 1 SAĞ BUTON*****************************************/

        private void bntExecutiveMotorRight_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[5]", true));
            threadWriteBool.Start();
        }

        private void bntExecutiveMotorRight_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[5]", false));
            threadWriteBool.Start();
        }









        /***********************************FİKSTÜR 2 SAĞ/SOL BUTONLAR*************************************************/

        private void btnTopPressingPistonDown_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[6]", true));
            threadWriteBool.Start();
        }

        private void btnTopPressingPistonDown_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[6]", false));
            threadWriteBool.Start();
        }

        private void btnTopPressingPistonUp_MouseDown(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[7]", true));
            threadWriteBool.Start();
        }

        private void btnTopPressingPistonUp_MouseUp(object sender, MouseEventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("jogServo[7]", false));
            threadWriteBool.Start();
        }














        /**********************************DÜĞME PİSTON İLERİ/GERİ**************************************************/

        private void btnBottomPressingPistonDown_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("dugmePiston", true));
            threadWriteBool.Start();
        }

        private void btnBottomPressingPistonUp_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("dugmePiston", false));
            threadWriteBool.Start();
        }











        /********************************************************TABLA START BUTONU**********************************************************************/

        private void btnPressingPistonDown_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("tablaStart", true));
            threadWriteBool.Start();
        }

        private void btnPressingPistonUp_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("adetSifirla", true));
            threadWriteBool.Start();
            uretilenAdet.Text = "0";
            //NOT : BURAYA ÜRETİLEN ADET KISMI "0" OLARAK EKLENECEK.
        }




        /************************************************************************************/
















        /*
        private void btnVacuumStop_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Vakum", false));
            threadWriteBool.Start();
        }

        private void btnVacuumStart_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Vakum", true));
            threadWriteBool.Start();
        }

        private void btnMoldCoolingStop_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Sisirme", false));
            threadWriteBool.Start();
        }

        private void btnMoldCoolingStart_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("k2Sisirme", true));
            threadWriteBool.Start();
        }

        */

        /*
        private void txtBxRunningDistance_KeyPress(object sender, KeyPressEventArgs e)
        {
            //numberControl(txtBxRunningDistance, e);
        }

        private void txtBxRunningDistance_Leave(object sender, EventArgs e)
        {
            //minMaxControl(txtBxRunningDistance, 0, 800);
        }

        private void txtBxMoldNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //numberControl(txtBxMoldNumber, e);
        }

        private void txtBxMoldNumber_Leave(object sender, EventArgs e)
        {
            //minMaxControl(txtBxMoldNumber, 1, 5);
        }
        */

        /************************************************************************************/

        private void txtBxHeatingTime_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtBxVacuumTime_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtBxVacuumAfter_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtCoolingTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            //dotControl(txtCoolingTime, e);
        }

        private void txtCoolingTime_Leave(object sender, EventArgs e)
        {
            //txtCoolingTime.Text = txtCoolingTime.Text.Replace(",", ".");
            //minMaxControl(txtCoolingTime, 0, 99);
        }


        /************************************************************************************/

        /*
        private void minMaxControl(TextBox textBox, double min, double max)
        {
            if (textBox.Text != "")
            {
                bool isNumeric = float.TryParse(textBox.Text, out _);
                if (isNumeric && double.Parse(textBox.Text.Replace(".", ",")) <= max && double.Parse(textBox.Text.Replace(".", ",")) >= min)
                {

                }
                else
                {
                    MessageBox.Show(min.ToString() + " - " + max.ToString() + " değer aralığında giriniz");
                    textBox.Text = "";
                }
            }
        }
        */

        private void dotControl(TextBox textBox, KeyPressEventArgs e)
        {
            int countDot = 0;
            for (int i = 0; i < textBox.Text.Length; i++)
            {
                if (textBox.Text.Contains(".") || textBox.Text.Contains(","))
                {
                    countDot++;
                }
            }

            if ((e.KeyChar == '.' || e.KeyChar == ',') && countDot <= 0)
            {
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

        }
        /*
        private void numberControl(TextBox textBox, KeyPressEventArgs e)
        {

             if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

        }
        */

        private void btnModelSettingsSend_Click(object sender, EventArgs e)
        {


        }

        /*
        private void btnResetProductionUnit_Click(object sender, EventArgs e)
        {

        }
        */

        private void txtUProductionModel_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnElavotorMotorDown_Click(object sender, EventArgs e)
        {

        }

        private void txtBxModelName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBxUnit_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBxCycleTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUProductionUnit_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTopPressUp_Click(object sender, EventArgs e)
        {

        }

        private void btnElavotorDoorSensor_Click(object sender, EventArgs e)
        {

        }





        //************************************************JOG HIZ TEXTBOX İLE İLGİLİ KISIM*******************************//
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }





        //********************************************JOG ARALIK COMBO BOX ÇALIŞMA PRENSİBİ******************************************//

        //Kontrol edilecek!!

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            threadWriteString = new Thread(() => nxCompoletStringWrite("jogSecim", cmbBxWorkingMod.SelectedIndex.ToString()));
            threadWriteString.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPrdOrder.Text != "" && txtUProductionModel.Text != "")
            {
                if (nxCompoletBoolWrite("f1PosGit", true))
                {
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1GidilecekPos", txtPrdOrder.Text));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1GidilecekGrup", txtUProductionModel.Text));
                    threadWriteString.Start();

                    Thread.Sleep(1000);

                    threadWriteBool = new Thread(() => nxCompoletBoolWrite("f1PosGit", false));
                    threadWriteBool.Start();
                    MessageBox.Show("Veriler gönderildi !", "Başarılı");
                }
            }
            else
            {
                MessageBox.Show("Lütfen boşlukları doldurunuz !", "Hata");
            }
        }

        private void txtPrdOrder_TextChanged(object sender, EventArgs e)
        {

        }


        //OKUNACAK KISIMLAR
        private void txtPrdOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            threadWriteString = new Thread(() => nxCompoletStringWrite("f1GidilecekPos",
                txtPrdOrder.Text.ToString()));
        }

        private void txtUProductionModel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            threadWriteString = new Thread(() => nxCompoletStringWrite("f1GidilecekGrup",
                txtUProductionModel.Text.ToString()));
        }

        private void tableLayoutPanelMidCenterMid_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanelMidBottomIn_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFik_Click(object sender, EventArgs e)
        {

        }

        private void txtMODELADI_TextChanged(object sender, EventArgs e)
        {
            string modelAdi = txtMODELADI.Text;
            ShowData1($"Select * from {modelAdi}");
            ShowData2($"Select * from {modelAdi}");

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        //OKUNACAK KISIMLAR
        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            threadWriteString = new Thread(() => nxCompoletStringWrite("f2GidilecekPos", textBox6.Text.ToString()));
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            threadWriteString = new Thread(() => nxCompoletStringWrite("f2GidilecekGrup", textBox7.Text.ToString()));
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "")
            {
                if (nxCompoletBoolWrite("f2PosGit", true))
                {
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2GidilecekPos", textBox6.Text));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2GidilecekGrup", textBox7.Text));
                    threadWriteString.Start();

                    Thread.Sleep(1000);

                    threadWriteBool = new Thread(() => nxCompoletBoolWrite("f2PosGit", false));
                    threadWriteBool.Start();
                    MessageBox.Show("Veriler gönderildi !", "Başarılı");
                }
            }
            else
            {
                MessageBox.Show("Lütfen boşlukları doldurunuz !", "Hata");
            }
        }

        private void btnCut_Click(object sender, EventArgs e)
        {
        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanelMidMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnGetPrdOrder_Click(object sender, EventArgs e)
        {

        }


        //***********************************************ÜRETİM BİTİR - AKTİF OLMAYAN BUTON***************************//
        private void btnProductionEnd_Click(object sender, EventArgs e)
        {
        }




        public bool nxCompoletBoolRead(string variable)  //NX READ
        {
            try
            {
                boolReadStatus = false;
                bool staticValue = Convert.ToBoolean(nxCompolet1.ReadVariable(variable));
                return staticValue;
            }
            catch
            {
                boolReadStatus = true;
                return false;
            }
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Black, Color.Black);
        }
        private void groupBox2_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Black, Color.Black);
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                               box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                               box.ClientRectangle.Width - 1,
                                               box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);
                // Clear text and border

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);
                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        private void DataAdd()
        {
            string modelAdi = txtMODELADI.Text;
            conn.Open();
            OleDbCommand com = new OleDbCommand($"insert into {modelAdi} (PozisyonNO, XPozisyon, ZPozisyon, WBaşlangıçPozisyon, WBitişPozisyon, WDönmeSüre)values (@PozisyonNO, @XPozisyon, @ZPozisyon, @WBaşlangıçPozisyon, @WBitişPozisyon, @WDönmeSüre)", conn);

            string gelenDeger8 = textBox8.Text.Replace('.', ',');
            string gelenDeger15 = textBox15.Text.Replace('.', ',');
            string gelenDeger13 = textBox13.Text.Replace('.', ',');
            string gelenDeger12 = textBox12.Text.Replace('.', ',');
            string gelenDeger11 = textBox11.Text.Replace('.', ',');
            string gelenDeger10 = textBox10.Text.Replace('.', ',');

            
            double a8 = Convert.ToDouble(gelenDeger8);
            double a15 = Convert.ToDouble(gelenDeger15);
            double a13 = Convert.ToDouble(gelenDeger13);
            double a12 = Convert.ToDouble(gelenDeger12);
            double a11 = Convert.ToDouble(gelenDeger11);
            double a10 = Convert.ToDouble(gelenDeger10);

            
          
            com.Parameters.AddWithValue("@PozisyonNO", a8);
            com.Parameters.AddWithValue("@XPozisyon", a15);
            com.Parameters.AddWithValue("@ZPozisyon", a13);
            com.Parameters.AddWithValue("@WBaşlangıçPozisyon", a12);
            com.Parameters.AddWithValue("@WBitişPozisyon", a11);
            com.Parameters.AddWithValue("@WDönmeSüre", a10);

            com.ExecuteNonQuery();
            conn.Close();

        }
        private void DataAdd2()
        {
            string modelAdi = txtMODELADI.Text;
            conn.Open();

            foreach (DataGridViewRow row in gdvFikstur1.Rows)
            {
                OleDbCommand com = new OleDbCommand($"insert into {modelAdi} (PozisyonNO, XPozisyon, ZPozisyon, WBaşlangıçPozisyon, WBitişPozisyon, WDönmeSüre)values (@PozisyonNO, @XPozisyon, @ZPozisyon, @WBaşlangıçPozisyon, @WBitişPozisyon, @WDönmeSüre)", conn);

                string a8 = row.Cells[0].Value.ToString();
                string a15 = row.Cells[1].Value.ToString();
                string a13 = row.Cells[2].Value.ToString();
                string a12 = row.Cells[3].Value.ToString();
                string a11 = row.Cells[4].Value.ToString();
                string a10 = row.Cells[5].Value.ToString();

                string a1 = a8.Replace('.', ',');
                string a2 = a15.Replace('.', ',');
                string a3 = a13.Replace('.', ',');
                string a4 = a12.Replace('.', ',');
                string a5 = a11.Replace('.', ',');
                string a6 = a10.Replace('.', ',');

                double b1 = Convert.ToDouble(a1);
                double b2 = Convert.ToDouble(a2);
                double b3 = Convert.ToDouble(a3);
                double b4 = Convert.ToDouble(a4);
                double b5 = Convert.ToDouble(a5);
                double b6 = Convert.ToDouble(a6);

                com.Parameters.AddWithValue("@PozisyonNO", b1);
                com.Parameters.AddWithValue("@XPozisyon", b2);
                com.Parameters.AddWithValue("@ZPozisyon", b3);
                com.Parameters.AddWithValue("@WBaşlangıçPozisyon", b4);
                com.Parameters.AddWithValue("@WBitişPozisyon", b5);
                com.Parameters.AddWithValue("@WDönmeSüre", b6);
                com.ExecuteNonQuery();
            }


            conn.Close();
            MessageBox.Show("Model oluşturuldu!", "Başarılı");
            cmbMODEL.Text = modelAdi;
        }

        private void txtCheckReplace()
        {

            string b2 = textBox15.Text.Replace(',', '.');
            string b3 = textBox13.Text.Replace(',', '.');
            string b4 = textBox12.Text.Replace(',', '.');
            string b5 = textBox11.Text.Replace(',', '.');
            string b6 = textBox10.Text.Replace(',', '.');
            textBox15.Text = b2;
            textBox13.Text = b3;
            textBox12.Text = b4;
            textBox11.Text = b5;
            textBox10.Text = b6;

        }


        //NX COMPOLET METOTOLARI
        #region nxCompoletMetotoları


        public bool nxCompoletBoolWrite(string variable, bool value)  //NX WRITE
        {
            try
            {
                nxCompolet1.WriteVariable(variable, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string modelAdi = txtMODELADI.Text;
            DataAdd();
            ShowData1($"Select * from {modelAdi}");
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox8.Text = gdvFikstur1.SelectedRows[0].Cells[1].Value.ToString();
            textBox15.Text = gdvFikstur1.SelectedRows[0].Cells[2].Value.ToString();
            textBox13.Text = gdvFikstur1.SelectedRows[0].Cells[3].Value.ToString();
            textBox12.Text = gdvFikstur1.SelectedRows[0].Cells[4].Value.ToString();
            textBox11.Text = gdvFikstur1.SelectedRows[0].Cells[5].Value.ToString();
            textBox10.Text = gdvFikstur1.SelectedRows[0].Cells[6].Value.ToString();
            txtCheckReplace();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string modelAdi = txtMODELADI.Text;

            string gelenDeger8 = textBox8.Text.Replace('.', ',');
            string gelenDeger15 = textBox15.Text.Replace('.', ',');
            string gelenDeger13 = textBox13.Text.Replace('.', ',');
            string gelenDeger12 = textBox12.Text.Replace('.', ',');
            string gelenDeger11 = textBox11.Text.Replace('.', ',');
            string gelenDeger10 = textBox10.Text.Replace('.', ',');

            double a8 = Convert.ToDouble(gelenDeger8);
            double a15 = Convert.ToDouble(gelenDeger15);
            double a13 = Convert.ToDouble(gelenDeger13);
            double a12 = Convert.ToDouble(gelenDeger12);
            double a11 = Convert.ToDouble(gelenDeger11);
            double a10 = Convert.ToDouble(gelenDeger10);

            conn.Open();
            OleDbCommand com = new OleDbCommand($"update {modelAdi} set XPozisyon = '" + a15 + "', ZPozisyon = '" + a13 + "', WBaşlangıçPozisyon = '" + a12 + "', WBitişPozisyon = '" + a11 + "', WDönmeSüre = '" + a10 + "'where PozisyonNo = '" + Convert.ToInt32(textBox8.Text) + "' ", conn);
            com.ExecuteNonQuery();
            textBox8.Text = (gdvFikstur1.Rows.Count).ToString();
            textBox17.Text = (gdvFikstur1.Rows.Count).ToString();
            conn.Close();
            ShowData1($"Select * from {modelAdi}");
            ShowData2($"Select * from {modelAdi}");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectedIndex = gdvFikstur1.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                gdvFikstur1.Rows.RemoveAt(selectedIndex);
                gdvFikstur1.Refresh();
                gdvFikstur2.Rows.RemoveAt(selectedIndex);
                gdvFikstur2.Refresh();
            }
            string modelAdi = txtMODELADI.Text;
            conn.Open();
            OleDbCommand com = new OleDbCommand($"delete from {modelAdi} where PozisyonNo=@PozisyonNo", conn);
            com.Parameters.AddWithValue("@PozisyonNo", textBox8.Text);
            com.ExecuteNonQuery();
            conn.Close();
            ShowData1($"Select * from {modelAdi}");
            ShowData2($"Select * from {modelAdi}");

            textBox8.Text = "";
            textBox15.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox11.Text = "";
            textBox10.Text = "";

            ShowData1($"Select * from {modelAdi}");
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMODELADI_Enter(object sender, EventArgs e)
        {

        }

        private void cmbMODEL_SelectedIndexChanged(object sender, EventArgs e)
        { }

        private void cmbMODEL_TextChanged(object sender, EventArgs e)
        {

            string modelAdi = cmbMODEL.Text;

            //bool Fikstur1VarMı = Arama(radioButton1.Text, kelime);
            //bool Fikstur2VarMı = Arama(radioButton2.Text, kelime2);

            ShowData1($"Select * from {modelAdi}");
            txtMODELADI.Text = cmbMODEL.Text;

            ShowData2($"Select * from {modelAdi}");
            txtMODELADI.Text = cmbMODEL.Text;


        }

        static bool Arama(string metin, string kelime)
        {
            if (metin.Contains(kelime))
            {
                return true;
            }
            else
                return false;


        }

        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox17.Text = gdvFikstur2.SelectedRows[0].Cells[0].Value.ToString();
            textBox23.Text = gdvFikstur2.SelectedRows[0].Cells[1].Value.ToString();
            textBox21.Text = gdvFikstur2.SelectedRows[0].Cells[2].Value.ToString();
            textBox20.Text = gdvFikstur2.SelectedRows[0].Cells[3].Value.ToString();
            textBox19.Text = gdvFikstur2.SelectedRows[0].Cells[4].Value.ToString();
            textBox18.Text = gdvFikstur2.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            string modelAdi = txtMODELADI.Text;
            conn.Open();
            OleDbCommand com = new OleDbCommand($"update {modelAdi} set XPozisyon = '" + textBox23.Text + "', ZPozisyon = '" + textBox21.Text + "', WBaşlangıçPozisyon = '" + textBox20.Text + "', WBitişPozisyon = '" + textBox19.Text + "', WDönmeSüre = '" + textBox18.Text + "'where PozisyonNO = '" + Convert.ToInt32(textBox17.Text) + "' ", conn);
            com.ExecuteNonQuery();
            conn.Close();
            ShowData2($"Select * from {modelAdi}");


        }

        private void button9_Click(object sender, EventArgs e)
        {
            int selectedIndex = gdvFikstur2.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                gdvFikstur2.Rows.RemoveAt(selectedIndex);
                gdvFikstur2.Refresh();
            }
            string modelAdi = txtMODELADI.Text;
            conn.Open();
            OleDbCommand com = new OleDbCommand($"delete from {modelAdi} where PozisyonNO=@PozisyonNO", conn);
            com.Parameters.AddWithValue("@PozisyonNO", textBox17.Text);
            com.ExecuteNonQuery();
            conn.Close();


            textBox17.Text = "";
            textBox23.Text = "";
            textBox21.Text = "";
            textBox20.Text = "";
            textBox19.Text = "";
            textBox18.Text = "";

            ShowData2($"Select * from {modelAdi}");



        }

        private void txtGrupPozisyon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                threadWriteString = new Thread(() => nxCompoletStringWrite("f2SonrakiGrupPos", txtGrupPozisyon.Text.ToString()));
                threadWriteString.Start();
            }
        }

        private void txtGrupSayi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                threadWriteString = new Thread(() => nxCompoletStringWrite("f2GrupSayi", txtGrupSayi.Text.ToString()));
                threadWriteString.Start();
            }
        }

        private void txtGrupPozisyon1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                threadWriteString = new Thread(() => nxCompoletStringWrite("f1SonrakiGrupPos", txtGrupPozisyon1.Text.ToString()));
                threadWriteString.Start();
            }
        }

        private void txtGrupSayi1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                threadWriteString = new Thread(() => nxCompoletStringWrite("f1GrupSayi", txtGrupSayi1.Text.ToString()));
                threadWriteString.Start();
            }
        }

        private void cbmBaski2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbBaski1_SelectedIndexChanged(object sender, EventArgs e)
        {
            threadWriteString = new Thread(() => nxCompoletStringWrite("f1UrunSecim", cmbBaski1.SelectedIndex.ToString()));
            threadWriteString.Start();
        }

        public string nxCompoletStringRead(string variable)  //NX STRING
        {
            try
            {
                string staticStringValue = Convert.ToString(nxCompolet1.ReadVariable(variable));
                return staticStringValue;
            }
            catch (Exception e)
            {
                return "error";
            }

        }
        public bool nxCompoletStringWrite(string variable, string value)  //NX STRING
        {
            try
            {
                nxCompolet1.WriteVariable(variable, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void btnVeriGonder_Click(object sender, EventArgs e)
        {
            if (txtGrupSayi.Text != "" && txtGrupSayi1.Text != "" && txtGrupPozisyon.Text != "" && txtGrupPozisyon1.Text != "")
            {
                //if (gdvFikstur1.Rows[0].Cells[0].Value.ToString() != "" || gdvFikstur1.Rows[0].Cells[1].Value.ToString() != "" || gdvFikstur1.Rows[0].Cells[2].Value.ToString() != "" || gdvFikstur1.Rows[0].Cells[3].Value.ToString() != "" || gdvFikstur1.Rows[0].Cells[4].Value.ToString() != "" || gdvFikstur1.Rows[0].Cells[5].Value.ToString() != "" )
                //{

                //txtBxModelName = txtMODELADI;
                threadWriteBool = new Thread(() => nxCompoletBoolWrite("recipeSend", true));
                threadWriteBool.Start();

                threadWriteString = new Thread(() => nxCompoletStringWrite("f1SonrakiGrupPos", txtGrupPozisyon.Text));
                threadWriteString.Start();
                threadWriteString = new Thread(() => nxCompoletStringWrite("f1GrupSayi", txtGrupSayi.Text));
                threadWriteString.Start();
                threadWriteString = new Thread(() => nxCompoletStringWrite("f2SonrakiGrupPos", txtGrupPozisyon1.Text));
                threadWriteString.Start();
                threadWriteString = new Thread(() => nxCompoletStringWrite("f2GrupSayi", txtGrupSayi1.Text));
                threadWriteString.Start();

                int i = 0;
                int j = 0;

                //----------------------Fikstür 1-----------------------
                foreach (DataGridViewRow row in gdvFikstur1.Rows)
                {
                    string a8 = row.Cells[0].Value.ToString();
                    string a15 = row.Cells[2].Value.ToString();
                    string a13 = row.Cells[3].Value.ToString();
                    string a12 = row.Cells[4].Value.ToString();
                    string a11 = row.Cells[5].Value.ToString();
                    string a10 = row.Cells[6].Value.ToString();

                    string a1 = a8.Replace('.', ',');
                    string a2 = a15.Replace('.', ',');
                    string a3 = a13.Replace('.', ',');
                    string a4 = a12.Replace('.', ',');
                    string a5 = a11.Replace('.', ',');
                    string a6 = a10.Replace('.', ',');


                    //-----------------------0----------------------
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1PosNo[" + i + "]", a1));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1XPos[" + i + "]", a2));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1ZPos[" + i + "]", a3));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1WBasPos[" + i + "]", a4));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1WBitisPos[" + i + "]", a5));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f1WDonmeSure[" + i + "]", a6));
                    threadWriteString.Start();
                    i++;
                }

                i = 0;
               

                //----------------------Fikstür 2-----------------------
                foreach (DataGridViewRow row in gdvFikstur2.Rows)
                {
                    string a8 = row.Cells[0].Value.ToString();
                    string a15 = row.Cells[1].Value.ToString();
                    string a13 = row.Cells[2].Value.ToString();
                    string a12 = row.Cells[3].Value.ToString();
                    string a11 = row.Cells[4].Value.ToString();
                    string a10 = row.Cells[5].Value.ToString();

                    string a1 = a8.Replace('.', ',');
                    string a2 = a15.Replace('.', ',');
                    string a3 = a13.Replace('.', ',');
                    string a4 = a12.Replace('.', ',');
                    string a5 = a11.Replace('.', ',');
                    string a6 = a10.Replace('.', ',');

                   

                    //-----------------------0----------------------
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2PosNo[" + j + "]", a1));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2XPos[" + j + "]", a2));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2ZPos[" + j + "]", a3));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2WBasPos[" + j + "]", a4));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2WBitisPos[" + j + "]", a5));
                    threadWriteString.Start();
                    threadWriteString = new Thread(() => nxCompoletStringWrite("f2WDonmeSure[" + j + "]", a6));
                    threadWriteString.Start();

                    j++;
                }

                j = 0;
                MessageBox.Show("Veriler PLC'e gönderildi!!", "Başarılı");
            }
            else
            {
                MessageBox.Show("Grup Sayı ve Sonraki Grup Pozisyon bilgilerinin doldurunuz !!", "Hata");
            }
        }

        private void btnVeriGonder_Click_1(object sender, EventArgs e)
        {
            txtBxModelName.Text = txtMODELADI.Text;
        }

        private void gdvFikstur1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            int minValue = 1;
            int maxValue = 100;

            if (e.KeyCode == Keys.Enter)
            {

                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    int value = Convert.ToInt32(textBox1.Text);
                    if (value < minValue || value > maxValue)
                    {
                        MessageBox.Show("Lütfen " + minValue + " ile " + maxValue + " arasında bir değer girin! ", "Hata");
                        textBox1.Text = "";
                    }
                    else
                    {
                        threadWriteString = new Thread(() => nxCompoletStringWrite("jogHiz", textBox1.Text.ToString()));
                        threadWriteString.Start();
                    }

                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {


        }

        private void textBox15_TextChanged(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                string gelenDeger1 = textBox15.Text.Replace('.', ',');
                double a = Convert.ToDouble(gelenDeger1);

                if (a > 0.0 && a > 760.0)
                {
                    MessageBox.Show("0.0 ile 760.0 arasında bir değer giriniz!!", "Hata");
                }
            }
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                string gelenDeger2 = textBox13.Text.Replace('.', ',');
                double b = Convert.ToDouble(gelenDeger2);

                if (b > 0.0 && b > 400.0)
                {
                    MessageBox.Show("0.0 ile 400.0 arasında bir değer giriniz!!", "Hata");
                }
            }
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                string gelenDeger3 = textBox12.Text.Replace('.', ',');
                double c = Convert.ToDouble(gelenDeger3);

                if ((c < -400.0 || c > 0.0) && (c < 0.0 || c > 400.0))
                {
                    MessageBox.Show("-400.0 ile 400.0 arasında bir değer giriniz!!", "Hata");
                }
            }
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                string gelenDeger4 = textBox11.Text.Replace('.', ',');
                double d = Convert.ToDouble(gelenDeger4);

                if ((d < -400.0 || d > 0.0) && (d < 0.0 || d > 400.0))
                {
                    MessageBox.Show("-400.0 ile 400.0 arasında bir değer giriniz!!", "Hata");
                }
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                string gelenDeger5 = textBox10.Text.Replace('.', ',');
                double f = Convert.ToDouble(gelenDeger5);

                if ((f < -400.0 || f > 0.0) && (f < 0.0 || f > 400.0))
                {
                    MessageBox.Show("-400.0 ile 400.0 arasında bir değer giriniz!!", "Hata");
                }
            }
        }

        private void txtGrupPozisyon_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtGrupPozisyon1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtGrupSayi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtGrupSayi1_HideSelectionChanged(object sender, EventArgs e)
        {

        }

        private void txtGrupSayi1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox8_KeyPress_1(object sender, KeyPressEventArgs e)
        {

        }

        private void btnHome_Click_1(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("homeButon", true));
            threadWriteBool.Start();
        }

        private void btnResistorMotorFault_Click(object sender, EventArgs e)
        {
            threadWriteBool = new Thread(() => nxCompoletBoolWrite("recipeSend", true));
            threadWriteBool.Start();
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        public string nxCompoletDoubleRead(string variable)
        {
            try
            {
                string s = Convert.ToString(nxCompolet1.ReadVariable(variable));
                return s;
            }
            catch (Exception e)
            {
                return "-1";
            }
        }

        //public string nxCompoletDoubleWrite(string variable, double value)
        //{
        //    try
        //    {
        //        String veri = Convert.ToString(nxCompolet1.WriteVariable(variable, value));
        //        return veri;
        //    }
        //    catch
        //    {
        //        return "-1";
        //    }

        //}

        public bool nxCompoletBoolWrite2(string variable, bool value)  //NX WRITE
        {
            try
            {
                nxCompolet1.WriteVariable(variable, value);
                return true;


            }
            catch
            {
                return false;
            }
        }
        #endregion




        private void loopMainRead()
        {
            while (true)
            {
                Thread.Sleep(100);
                if (readLoopCounter == 5)
                {
                    readLoopCounter = 0;
                    try
                    {
                        if (nxCompoletBoolRead("connectionOk") && boolReadStatus)
                        {
                            txtXPos.Text = nxCompoletStringRead("axis[" + 0 + "]");
                            txtZPos.Text = nxCompoletStringRead("axis[" + 1 + "]");
                            txtFik1WPos.Text = nxCompoletStringRead("axis[" + 2 + "]");
                            txtFik2WPos.Text = nxCompoletStringRead("axis[" + 3 + "]");
                        }
                        else
                        {
                            MessageBox.Show("Makinanın konumu okunamadı !!!", "Hata");
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    readLoopCounter++;
                }
            }


        }


        //        if (readLoopCounter == 10)
        //                {
        //                    readLoopCounter = 0;
        //                    if (nxCompoletBoolRead("Donanim[" + 15 + "]") && !boolReadStatus)
        //                    {
        //                        btnResistorMotorFault.BackColor = Color.Green;
        //                    }
        //                    else
        //                    {
        //                        btnResistorMotorFault.BackColor = Color.FromArgb(41, 53, 65); ;
        //                    }


        //                    /**************************************HARDWARE**********************************/
        //                    uretilenAdet.Text = nxCompoletStringRead("uretilenAdet");
        //txtBxCycleTime.Text = nxCompoletStringRead("cevrimSure");


        //                    if (nxCompoletStringRead("uretimDurum") == "0")
        //                    {
        //                        textBox9.Text = "Üretim Yok";
        //                    }
        //                    else if (nxCompoletStringRead("uretimDurum") == "1")
        //                    {
        //                        textBox9.Text = "Üretim Yapılıyor...";
        //                    }
        //                    else if (nxCompoletStringRead("uretimDurum") == "2")
        //                    {
        //                        textBox9.Text = "Üretim Durduruldu !";
        //                    }

        //                }
        //                readLoopCounter++;
        //            }
        //}





    }




    public class INIKaydet
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INIKaydet(string dosyaYolu)
        {
            DOSYAYOLU = dosyaYolu;
        }
        private string DOSYAYOLU = String.Empty;
        public string Varsayilan { get; set; }
        public string Oku(string bolum, string ayaradi)
        {
            Varsayilan = Varsayilan ?? string.Empty;
            StringBuilder StrBuild = new StringBuilder(256);
            GetPrivateProfileString(bolum, ayaradi, Varsayilan, StrBuild, 255, DOSYAYOLU);
            return StrBuild.ToString();
        }
        public long Yaz(string bolum, string ayaradi, string deger)
        {
            return WritePrivateProfileString(bolum, ayaradi, deger, DOSYAYOLU);
        }
        public long Sil(string bolum, string ayaradi, string deger)
        {
            return WritePrivateProfileString(bolum, ayaradi, deger, DOSYAYOLU);
        }
    }
}

