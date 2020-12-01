using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DrPrescription
{
    public partial class Form1 : Form
    {
        int TodaysPatientCount;
        List<Label> SerialNumberList = new List<Label>();
        List<Label> NameLblList = new List<Label>();
        List<Label> GenderLblList = new List<Label>();
        List<Label> AgeLblList = new List<Label>();

        List<CheckBox> AllCheckBoxes = new List<CheckBox>();
        List<Medicine> patientMed = new List<Medicine>();
        public Form1()
        {
            try
            {
                InitializeComponent();
                if (!System.IO.Directory.Exists(@".\Data"))
                {
                    System.IO.Directory.CreateDirectory(@".\Data");
                }
                if (!System.IO.Directory.Exists(@".\Data\AllPatientPrescription"))
                {
                    System.IO.Directory.CreateDirectory(@".\Data\AllPatientPrescription");
                }
                string jsonFilePath = string.Format(
                    @".\Data\AllPatientPrescription\DaysPatient{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_'));
                if (!System.IO.File.Exists(jsonFilePath))
                {
                    System.IO.File.Create(jsonFilePath).Close();
                }
                if (!System.IO.File.Exists(@".\Data\TotalPrescriptionCount.txt"))
                {
                    System.IO.File.Create(@".\Data\TotalPrescriptionCount.txt").Close();
                }
                var allComplains = ReadExcel.GetSymptoms();
                foreach (var c in allComplains)
                {
                    AllSymptoms.Items.Add(c);
                }
                
                var lst = ReadExcel.GetAllMedicines().OrderBy(x => x).ToList();
                lst.Insert(0, "==Select==");
                MedicinesComboBox.DataSource = lst;
               
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("If it is first time try to re strat your application. Other wise call your developer.");
            }
        }
        
        private void AllSymptoms_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (!patientSymptoms.Items.Contains(AllSymptoms.SelectedItem))
                {
                    patientSymptoms.Items.Add(AllSymptoms.SelectedItem);
                    PatientComplains.Text = string.Empty;
                    for (int i = 0; i < patientSymptoms.Items.Count; i++)
                    {
                        if (i % 2 == 0 && i != 0)
                        {
                            PatientComplains.Text += "\n";
                        }
                        PatientComplains.Text += patientSymptoms.Items[i].ToString();
                        if (i != patientSymptoms.Items.Count - 1)
                        {
                            PatientComplains.Text += ",";
                        }


                    }
                }
                
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("Contact developer and send the logs to the developer.");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (patientSymptoms.SelectedItem != null)
                {
                    patientSymptoms.Items.Remove(patientSymptoms.SelectedItem);
                }
                else if (patientSymptoms.Items.Count != 0)
                {
                    patientSymptoms.Items.Remove(patientSymptoms.Items[patientSymptoms.Items.Count - 1]);
                }
                PatientComplains.Text = string.Empty;
                for(int i=0;i<patientSymptoms.Items.Count;i++)
                {
                    if (i % 2 == 0 && i != 0)
                    {
                        PatientComplains.Text += "\n";
                    }
                    PatientComplains.Text += patientSymptoms.Items[i].ToString();
                    if (i != patientSymptoms.Items.Count - 1)
                    {
                        PatientComplains.Text += ",";
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("Contact developer and send the logs to the developer.");

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                patientSymptoms.Items.Add(AnyOtherSymtom.Text);
                AnyOtherSymtom.Text = string.Empty;
                PatientComplains.Text = string.Empty;
                for (int i = 0; i < patientSymptoms.Items.Count; i++)
                {
                    if (i % 2 == 0 && i != 0)
                    {
                        PatientComplains.Text += "\n";
                    }
                    PatientComplains.Text += patientSymptoms.Items[i].ToString();
                    if (i != patientSymptoms.Items.Count - 1)
                    {
                        PatientComplains.Text += ",";
                    }


                }

            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("Contact developer and send the logs to the developer.");
            }
            
        }
        //private void Add100Patients()
        //{
        //    Label lbl;
        //    Label namelbl;
        //    Label agelbl;
        //    Label genderlbl;
        //    Panel namePanel;
        //    Panel genderPanel;
        //    Panel agePanel;
        //    Button btn;
        //    CheckBox chk;

        //    try
        //    {
        //        //var tabCount = 8;
        //        List<Patient> patientList = new List<Patient>();
        //        if (!System.IO.File.Exists(string.Format(@".\Data\Patients{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_'))))
        //        {
        //            System.IO.File.Create(string.Format(@".\Data\Patients{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_'))).Close();
        //        }
        //        else
        //        {
        //            patientList = JsonConvert.DeserializeObject<List<Patient>>(System.IO.File.ReadAllText(
        //                string.Format(@".\Data\Patients{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_'))
        //                ));
        //            if (patientList == null)
        //            {
        //                patientList = new List<Patient>();
        //            }
        //        }
        //        TodaysPatientCount = patientList.Count;
        //        for (int i = 1; i <= 100; i++)
        //        {
        //            bool lblVisibility = false;
        //            if (i - 1 < patientList.Count)
        //            {
        //                lblVisibility = true;
        //            }


        //            lbl = new Label();
        //            lbl.Visible = lblVisibility;
        //            lbl.BackColor = System.Drawing.Color.Transparent;
        //            lbl.Dock = System.Windows.Forms.DockStyle.Fill;
        //            lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //            lbl.ForeColor = System.Drawing.Color.White;
        //            //lbl.Location = new System.Drawing.Point(1068, 31);
        //            lbl.Name = "PatientSNo" + i;
        //            lbl.Size = new System.Drawing.Size(258, 31);
        //            //lbl.TabIndex = tabCount;
        //            lbl.Text = i.ToString();
        //            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //            SerialNumberList.Add(lbl);
        //            //tabCount++;

        //            namePanel = new Panel();
        //            namePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //        | System.Windows.Forms.AnchorStyles.Left)
        //        | System.Windows.Forms.AnchorStyles.Right)));
        //            namePanel.Location = new System.Drawing.Point(275, 65);
        //            namePanel.Name = "namePanel" + i;
        //            namePanel.Size = new System.Drawing.Size(567, 25);
        //            //namePanel.TabIndex = tabCount;


        //            namelbl = new Label();
        //            namelbl.Visible = lblVisibility;
        //            namelbl.BackColor = System.Drawing.Color.Transparent;
        //            namelbl.Dock = System.Windows.Forms.DockStyle.Fill;
        //            namelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //            namelbl.ForeColor = System.Drawing.Color.White;
        //            //lbl.Location = new System.Drawing.Point(1068, 31);
        //            namelbl.Name = "namelbl" + i;
        //            namelbl.Size = new System.Drawing.Size(258, 31);
        //            namelbl.TabIndex = 1;
        //            namelbl.Text = "Patient Not Added Yet";
        //            if (lblVisibility)
        //            {
        //                namelbl.Text = patientList[i - 1].Name;
        //            }

        //            namelbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //            NameLblList.Add(namelbl);


        //            namePanel.Controls.Add(namelbl);

        //            //tabCount++;

        //            genderPanel = new Panel();
        //            genderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //        | System.Windows.Forms.AnchorStyles.Left)
        //        | System.Windows.Forms.AnchorStyles.Right)));
        //            genderPanel.Location = new System.Drawing.Point(275, 65);
        //            genderPanel.Name = "genderPanel" + i;
        //            genderPanel.Size = new System.Drawing.Size(567, 25);
        //            //genderPanel.TabIndex = tabCount;

        //            genderlbl = new Label();
        //            genderlbl.Visible = lblVisibility;
        //            genderlbl.BackColor = System.Drawing.Color.Transparent;
        //            genderlbl.Dock = System.Windows.Forms.DockStyle.Fill;
        //            genderlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //            genderlbl.ForeColor = System.Drawing.Color.White;
        //            //lbl.Location = new System.Drawing.Point(1068, 31);
        //            genderlbl.Name = "genderlbl" + i;
        //            genderlbl.Size = new System.Drawing.Size(258, 31);
        //            genderlbl.TabIndex = 1;
        //            genderlbl.Text = "Gender Not Selected Yet";
        //            if (lblVisibility)
        //            {
        //                genderlbl.Text = patientList[i - 1].Gender;
        //            }

        //            genderlbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //            GenderLblList.Add(genderlbl);


        //            genderPanel.Controls.Add(genderlbl);
        //            //tabCount++;



        //            agePanel = new Panel();
        //            agePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //        | System.Windows.Forms.AnchorStyles.Left)
        //        | System.Windows.Forms.AnchorStyles.Right)));
        //            agePanel.Location = new System.Drawing.Point(275, 65);
        //            agePanel.Name = "agePanel" + i;
        //            agePanel.Size = new System.Drawing.Size(567, 25);
        //            //agePanel.TabIndex = tabCount;

        //            agelbl = new Label();
        //            agelbl.Visible = lblVisibility;
        //            agelbl.BackColor = System.Drawing.Color.Transparent;
        //            agelbl.Dock = System.Windows.Forms.DockStyle.Fill;
        //            agelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //            agelbl.ForeColor = System.Drawing.Color.White;
        //            //lbl.Location = new System.Drawing.Point(1068, 31);
        //            agelbl.Name = "agelbl" + i;
        //            agelbl.Size = new System.Drawing.Size(258, 31);
        //            agelbl.TabIndex = 1;
        //            agelbl.Text = "Age Not Added Yet";
        //            if (lblVisibility)
        //            {
        //                agelbl.Text = patientList[i - 1].Age.ToString();
        //            }
        //            agelbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //            AgeLblList.Add(agelbl);

        //            agePanel.Controls.Add(agelbl);



        //            //tabCount++;
        //            chk = new CheckBox();
        //            chk.Visible = false;
        //            if (lblVisibility)
        //            {
        //                chk.Visible = true;
        //                chk.Checked = patientList[i - 1].Attended;
        //                chk.Enabled = !patientList[i - 1].Attended;
        //            }

        //            chk.AutoSize = true;
        //            chk.Dock = System.Windows.Forms.DockStyle.Fill;
        //            chk.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //            chk.ForeColor = System.Drawing.Color.White;
        //            chk.Location = new System.Drawing.Point(1068, 65);
        //            chk.Name = "PatientAttended" + i;
        //            chk.Size = new System.Drawing.Size(258, 25);
        //            //chk.TabIndex = tabCount;
        //            chk.Text = "Attended by doctor";
        //            chk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //            chk.UseVisualStyleBackColor = true;
        //            chk.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
        //            AllCheckBoxes.Add(chk);

        //            //tabCount++;
                  
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log(ex);
        //        MessageBox.Show("Contact developer and send the logs to the developer.");
        //    }
        //}

        //private void AddPatient(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var patientList = JsonConvert.DeserializeObject<List<Patient>>(System.IO.File.ReadAllText(
        //                string.Format(@".\Data\Patients{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_'))
        //                ));
        //        if (patientList == null)
        //        {
        //            patientList = new List<Patient>();
        //        }
        //        System.IO.File.WriteAllText(string.Format(@".\Data\Patients{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_')), JsonConvert.SerializeObject(patientList));

        //        AgeLblList[TodaysPatientCount].Visible = true;
        //        SerialNumberList[TodaysPatientCount].Visible = true;
        //        AllCheckBoxes[TodaysPatientCount].Visible = true;
        //        TodaysPatientCount++;

        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log(ex);
        //        MessageBox.Show("Contact developer and send the logs to the developer.");
        //    }

        //}

        //private void checkBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var patientList = JsonConvert.DeserializeObject<List<Patient>>(System.IO.File.ReadAllText(
        //                    string.Format(@".\Data\Patients{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_'))
        //                    ));
        //        if (patientList != null)
        //        {
        //            var indexOfCheckBox = Convert.ToInt32(((CheckBox)sender).Name.Split(new string[] { "PatientAttended" }, StringSplitOptions.RemoveEmptyEntries)[0]) - 1;
        //            //var lastPatient=patientList.Last();
        //            PrescriptionIdTxtBx.Text = DateTime.Now.ToLongDateString().Replace(" ", "_")+"_"+(indexOfCheckBox+1);
        //            patientNameTxt.Text = patientList[indexOfCheckBox].Name;
        //            patientAgeTxt.Value = patientList[indexOfCheckBox].Age;
        //            patientList[indexOfCheckBox].Attended = true;
        //            System.IO.File.WriteAllText(string.Format(@".\Data\Patients{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_')), JsonConvert.SerializeObject(patientList));

        //            if (patientList[indexOfCheckBox].Gender == "Male")
        //            {
        //                genderMaleRadio.Checked = true;
        //            }
        //            else if (patientList[indexOfCheckBox].Gender == "Female")
        //            {
        //                genderFemaleRadio.Checked = true;
        //            }
        //            else
        //            {
        //                genderOtherRadio.Checked = true;
        //            }
        //            ((CheckBox)sender).Enabled = false;
        //            tabControl1.SelectedIndex = 1;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Log(ex);
        //        MessageBox.Show("Contact developer and send the logs to the developer.");
        //    }
        //}

       
        private void AddMeds_Click(object sender, EventArgs e)
        {
            try
            {
                if (MedicinesComboBox.Text == "==Select==" || MedicinesComboBox.Text == string.Empty)
                {
                    MessageBox.Show("Please select a medicine.");
                    return;
                }
                if (DozeTxtBox.Text == string.Empty)
                {
                    MessageBox.Show("Please enter medicine doze. (1 if capsule or tablet) (if syrup write 1tfs or 2tfs) (if anything unique please write full description");
                    return;
                }
                if (!nightChkbx.Checked && !morningChkBx.Checked && !eveningChkBx.Checked)
                {
                    MessageBox.Show("Please select at least one of option of timing morning, evening or night" );
                    return;
                }
                Panel p = new Panel();
                p.Dock = DockStyle.Top;
                p.BorderStyle = BorderStyle.FixedSingle;
                p.Size = new Size(AllMeds.Size.Width, 60);
                p.Name = "p" + patientMed.Count;
                var x = new Label();

                var frequency = Common.DozeToPrint(eveningChkBx.Checked, morningChkBx.Checked, nightChkbx.Checked, DozeTxtBox.Text);
                x.Text = MedicinesComboBox.Text + Environment.NewLine +frequency;
                x.Name = "Medicine" + patientMed.Count;
                x.ForeColor = Color.Black;
                x.Dock = System.Windows.Forms.DockStyle.Fill;
                x.DoubleClick += new EventHandler(this.RemoveMed);
                x.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                p.Controls.Add(x);
                if (patientMed == null)
                {
                    patientMed = new List<Medicine>();
                }
                patientMed.Add(new Medicine()
                {
                    Name = MedicinesComboBox.Text,
                    Dose = DozeTxtBox.Text,
                    Night=nightChkbx.Checked,
                    Morning=morningChkBx.Checked,
                    Evening=eveningChkBx.Checked,
                    Precautions = precautionsTxt.Text,
                });
                MedicinesComboBox.Text = string.Empty;
                DozeTxtBox.Text = string.Empty;
                morningChkBx.Checked = false;
                eveningChkBx.Checked = false;
                nightChkbx.Checked = false;

                precautionsTxt.Text = string.Empty;
                
                AllMeds.Controls.Add(p);
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("Contact developer and send the logs to the developer.");
            }
        }
        private void RemoveMed(object sender, EventArgs e)
        {
            try
            {

                AllMeds.Controls.Remove(((Control)sender).Parent);
                var labelTxt = ((Label)sender).Text;
                if (patientMed.Any(x => x.Name + Environment.NewLine +
                Common.DozeToPrint(x.Evening,x.Morning,x.Night,x.Dose) == labelTxt))
                {
                    patientMed.Remove(patientMed.Where(x => x.Name + Environment.NewLine +  Common.DozeToPrint(x.Evening, x.Morning, x.Night, x.Dose) == labelTxt).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("Contact developer and send the logs to the developer.");
            }
            
        }

        private void SaveAndPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (PrescriptionIdTxtBx.Text == string.Empty)
                {
                    PrescriptionIdTxtBx.Text = "01_01";
                }
                if (patientNameTxt.Text == string.Empty)
                {
                    MessageBox.Show("Please enter patient name in patient details page.");
                    return;
                }
                if (patientAgeTxt.Value == 0)
                {
                    MessageBox.Show("Please enter patient age in patient details page.");
                    return;
                }
                if (!genderMaleRadio.Checked && !genderFemaleRadio.Checked && !genderOtherRadio.Checked)
                {
                    MessageBox.Show("Please enter patient gender in patient details page.");
                    return;
                }
                if (patientSymptoms.Items.Count == 0)
                {
                    MessageBox.Show("Please enter patient symptoms in patient details page.");
                    return;
                }
                if (patientMed.Count == 0)
                {
                    MessageBox.Show("Please add few medicines.");
                    return;
                }
                if (NoOfDaysNumber.Value == 0)
                {
                    MessageBox.Show("Please add number of days.");
                    return;
                }
                var precriptionObj = new Prescription()
                {
                    PrescriptionId = PrescriptionIdTxtBx.Text,
                    PatientDetails = new Patient()
                    {
                        Name = patientNameTxt.Text,
                        Age = Convert.ToInt32(patientAgeTxt.Value),
                        Weight = Convert.ToInt32(patientWeightTxt.Value)
                    },
                    Symptoms = new List<string>(),
                    ReadingsDetails = new Readings()
                    {
                        BloodSugarReading = bloodSugarTxtBx.Text,
                        BPReading = BpTxtBx.Text,
                        CSVReading = CVSTxtBx.Text,
                        PallorReading = PallorComboBx.Text,
                        PAndAReading = PAndATxtBx.Text,
                        RSReading = RSTxtBx.Text,
                        FeverReading = Convert.ToDouble(feverReading.Value),
                        PulseReading = Convert.ToInt32(pulseReading.Value),
                        ECGReading = EcgTxtBox.Text,
                        BMPRMPReading = BmpRmpTxtBx.Text,
                        GAndCReading = GAndCComboBx.Text,
                        CBCReading = CBCTxtBx.Text,
                        IctReading = IctComboBx.Text,
                        KFTReading = KftTxtBx.Text,
                        LFTReading = LftTxtBx.Text,
                        LipidProfileReading = LipidProfileTxtBx.Text,
                        LNReading = LNComboBx.Text,
                        OdemaReading = OedemaComboBx.Text,
                        T3T4TSHReading = T3T4TshTxtBx.Text,
                        UrineReReading = UrineReTxtBx.Text,
                        USGReading = UsgWandATxtBx.Text,
                        WidalReading = WidalTxtBx.Text,
                        XRayReading = XRayComboBx.Text,
                        BMPRMPSelect = BmpRmpChkBx.Checked,
                        CBCSelect = CbcChkBx.Checked,
                        KFTSelect = KftChkBx.Checked,
                        LFTSelect = LftChkBx.Checked,
                        LipidProfileSelect = lipidChkBx.Checked,
                        T3T4TSHSelect = T3T4TshChkBx.Checked,
                        UrineReSelect = UrineReChkBx.Checked,
                        USGSelect = UsgChkBx.Checked,
                        WidalSelect = WidalChkBx.Checked,
                        XRaySelect = XRayChkBx.Checked
                        

                    },
                    ExtraExaminations = ExtraExaminations.Text,
                    PrescribedMedicines = patientMed,
                    Precautions=CommonPrecautions.Text,
                    NumberOfDays = Convert.ToInt32(NoOfDaysNumber.Value)
                };
                if (genderMaleRadio.Checked)
                {
                    precriptionObj.PatientDetails.Gender = "Male";
                }
                if (genderFemaleRadio.Checked)
                {
                    precriptionObj.PatientDetails.Gender = "Female";
                }
                if (genderOtherRadio.Checked)
                {
                    precriptionObj.PatientDetails.Gender = "Other";
                }
                foreach (var i in patientSymptoms.Items)
                {
                    precriptionObj.Symptoms.Add(i.ToString());
                }

                if (patientRemarkTxt.Text != string.Empty)
                {
                    precriptionObj.Remark = patientRemarkTxt.Text;
                    //MedicinesComboBox doseComboBox frequencyComboBox extraTxt precautionsTxt
                    //RythmComboBox RateData AxisComboBox STSegmentData TWavesData PRIntervalData QRSComplexData QTIntervalData
                    //diastolicData pulseReading feverReading bloodSugarData EmptyStomachChkBox
                }

                var result = Common.PrintAndSave(precriptionObj);

                MessageBox.Show(result);
                tabControl1.SelectedIndex = 0;
                //Data Reset to the old value.
                //diastolicData.Value = 0;
                pulseReading.Value = 0;
                feverReading.Value = 0;
                //bloodSugarData.Value = 0;
                //EmptyStomachChkBox.Checked = false;
                //RythmComboBox.Text = string.Empty;
                //RateData.Value = 0;
                //AxisComboBox.Text = string.Empty;
                //STSegmentData.Text = string.Empty;
                //TWavesData.Text = string.Empty;
                //PRIntervalData.Value = 0;
                //PRIntervalData.Value = 0;
                //QTIntervalData.Value = 0;
                patientNameTxt.Text = string.Empty;
                patientAgeTxt.Value = 0;
                genderMaleRadio.Checked = genderFemaleRadio.Checked = genderOtherRadio.Checked = false;
                while (patientSymptoms.Items.Count > 0)
                {
                    patientSymptoms.Items.RemoveAt(0);
                }
                patientRemarkTxt.Text = string.Empty;
                patientMed = new List<Medicine>();
                MedicinesComboBox.Text = "==Select==";
                precautionsTxt.Text = string.Empty;
                while (AllMeds.Controls.Count > 0)
                {
                    AllMeds.Controls.RemoveAt(0);
                }
                patientWeightTxt.Value = 0;
                PallorComboBx.Text = string.Empty;
                BpTxtBx.Text = string.Empty;
                bloodSugarTxtBx.Text = string.Empty;
                RSTxtBx.Text = string.Empty;
                CVSTxtBx.Text = string.Empty;
                PAndATxtBx.Text = string.Empty;
                EcgTxtBox.Text = string.Empty;
                NoOfDaysNumber.Value = 0;
                PrescriptionIdTxtBx.Text = string.Empty;

                BmpRmpTxtBx.Text=string.Empty;
                GAndCComboBx.Text = string.Empty;
                CBCTxtBx.Text = string.Empty;
                IctComboBx.Text = string.Empty;
                KftTxtBx.Text = string.Empty;
                LftTxtBx.Text = string.Empty;
                LipidProfileTxtBx.Text = string.Empty;
                LNComboBx.Text = string.Empty;
                OedemaComboBx.Text = string.Empty;
                T3T4TshTxtBx.Text = string.Empty;
                UrineReTxtBx.Text = string.Empty;
                UsgWandATxtBx.Text = string.Empty;
                WidalTxtBx.Text = string.Empty;
                XRayComboBx.Text = string.Empty;
                BmpRmpChkBx.Checked = false;
                CbcChkBx.Checked = false;
                KftChkBx.Checked = false;
                LftChkBx.Checked = false;
                lipidChkBx.Checked = false;
                T3T4TshChkBx.Checked = false;
                UrineReChkBx.Checked = false;
                UsgChkBx.Checked = false;
                WidalChkBx.Checked = false;
                XRayChkBx.Checked = false;
                patientRemarkTxt.Text = string.Empty;
                CommonPrecautions.Text = string.Empty;
                ExtraExaminations.Text = string.Empty;
                PatientComplains.Text = "Complains";
                //systolicData.Value = 0;
                //QRSComplexData.Value = 0;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("Contact developer and send the logs to the developer.");
            }
        }

        private void AddNewMedicine_Click(object sender, EventArgs e)
        {
            try
            {
                if (addNewMedName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter new medicine name.");
                    return;
                }
                if (MorningDozeTxtBx.Text == string.Empty)
                {
                    MessageBox.Show("Please enter mornign doze.");
                    return;
                }
                if (EveningDozeTxtBx.Text == string.Empty)
                {
                    MessageBox.Show("Please enter evening doze.");
                    return;
                }
                if (NightDozeTxtBx.Text == string.Empty)
                {
                    MessageBox.Show("Please enter night doze.");
                    return;
                }
                AddMedStatus.Text = "Status-" + ReadExcel.AddNewMedicine(addNewMedName.Text,MorningDozeTxtBx.Text,EveningDozeTxtBx.Text,NightDozeTxtBx.Text);
                addNewMedName.Text = string.Empty;
                NightDozeTxtBx.Text = string.Empty;
                EveningDozeTxtBx.Text = string.Empty;
                MorningDozeTxtBx.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Common.Log(ex);
                MessageBox.Show("Contact developer and send the logs to the developer.");
            }

        }

        private void MedicinesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var med= ReadExcel.GetDoze(MedicinesComboBox.Text);
            DozeTxtBox.Text = med.Dose;
            morningChkBx.Checked = med.Morning;
            eveningChkBx.Checked = med.Evening;
            nightChkbx.Checked = med.Night;
        }



        //private void addPatientDetails_Click(object sender, EventArgs e)
        //{
        //    ListPageNameTxtBx.Text = ListPageNameTxtBx.Text;
        //    genderOtherRadio.Checked = ListPageOtherGenderRbtn.Checked;
        //    genderFemaleRadio.Checked = ListPageFemaleGenderRbtn.Checked;
        //    genderMaleRadio.Checked = ListPageMaleGenderRbtn.Checked;
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void SearchPrescriptionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var tempId = PrescriptionIdTxtBx.Text;
                var splitData = tempId.Split('_');
                if (splitData.Length != 4)
                {
                    MessageBox.Show("Not a valid Id add the new patient");
                    tabControl1.SelectedIndex = 0;
                    return;
                }
                var date = splitData[0] + "_" + splitData[1] + "_" + splitData[2];
                var fileDataStr = System.IO.File.ReadAllText(string.Format(
                       @".\Data\AllPatientPrescription\DaysPatient{0}.json", date));
                if (fileDataStr == string.Empty || fileDataStr == null)
                {
                    MessageBox.Show("No data for this date is present");
                    tabControl1.SelectedIndex = 0;
                    return;
                }
                var lst=JsonConvert.DeserializeObject<List<Prescription>>(fileDataStr);
                if (lst == null)
                {
                    MessageBox.Show("No data for this date is present");
                    tabControl1.SelectedIndex = 0;
                    return;
                }
                if (lst.All(x => x.PrescriptionId != tempId))
                {
                    MessageBox.Show("This Id doesnt exist");
                    tabControl1.SelectedIndex = 0;
                    return;
                }
                var precriptionObj = lst.Where(x => x.PrescriptionId == tempId).FirstOrDefault();
                patientNameTxt.Text = precriptionObj.PatientDetails.Name;
                patientAgeTxt.Value=precriptionObj.PatientDetails.Age;
                patientWeightTxt.Value=Convert.ToDecimal(precriptionObj.PatientDetails.Weight);
                bloodSugarTxtBx.Text = precriptionObj.ReadingsDetails.BloodSugarReading;
                BpTxtBx.Text = precriptionObj.ReadingsDetails.BPReading;
                CVSTxtBx.Text = precriptionObj.ReadingsDetails.CSVReading;
                PallorComboBx.Text = precriptionObj.ReadingsDetails.PallorReading;
                PAndATxtBx.Text = precriptionObj.ReadingsDetails.PAndAReading;
                RSTxtBx.Text = precriptionObj.ReadingsDetails.RSReading;
                feverReading.Value = Convert.ToDecimal(precriptionObj.ReadingsDetails.FeverReading);
                pulseReading.Value = precriptionObj.ReadingsDetails.PulseReading;
                EcgTxtBox.Text = precriptionObj.ReadingsDetails.ECGReading;
                BmpRmpTxtBx.Text = precriptionObj.ReadingsDetails.BMPRMPReading;
                GAndCComboBx.Text = precriptionObj.ReadingsDetails.GAndCReading;
                CBCTxtBx.Text = precriptionObj.ReadingsDetails.CBCReading;
                IctComboBx.Text = precriptionObj.ReadingsDetails.IctReading;
                KftTxtBx.Text = precriptionObj.ReadingsDetails.KFTReading;
                LftTxtBx.Text = precriptionObj.ReadingsDetails.LFTReading;
                LipidProfileTxtBx.Text = precriptionObj.ReadingsDetails.LipidProfileReading;
                LNComboBx.Text = precriptionObj.ReadingsDetails.LNReading;
                OedemaComboBx.Text = precriptionObj.ReadingsDetails.OdemaReading;
                T3T4TshTxtBx.Text = precriptionObj.ReadingsDetails.T3T4TSHReading;
                UrineReTxtBx.Text = precriptionObj.ReadingsDetails.UrineReReading;
                UsgWandATxtBx.Text = precriptionObj.ReadingsDetails.USGReading;
                WidalTxtBx.Text = precriptionObj.ReadingsDetails.WidalReading;
                XRayComboBx.Text = precriptionObj.ReadingsDetails.XRayReading;

                BmpRmpChkBx.Checked = precriptionObj.ReadingsDetails.BMPRMPSelect;
                CbcChkBx.Checked = precriptionObj.ReadingsDetails.CBCSelect;
                KftChkBx.Checked = precriptionObj.ReadingsDetails.KFTSelect;
                LftChkBx.Checked = precriptionObj.ReadingsDetails.LFTSelect;
                lipidChkBx.Checked = precriptionObj.ReadingsDetails.LipidProfileSelect;
                T3T4TshChkBx.Checked = precriptionObj.ReadingsDetails.T3T4TSHSelect;
                UrineReChkBx.Checked = precriptionObj.ReadingsDetails.UrineReSelect;
                UsgChkBx.Checked = precriptionObj.ReadingsDetails.USGSelect;
                WidalChkBx.Checked = precriptionObj.ReadingsDetails.WidalSelect;
                XRayChkBx.Checked = precriptionObj.ReadingsDetails.XRaySelect;
                ExtraExaminations.Text = precriptionObj.ExtraExaminations;
                CommonPrecautions.Text = precriptionObj.Precautions;
                patientRemarkTxt.Text = precriptionObj.Remark;

                while (patientSymptoms.Items.Count>0)
                {
                    patientSymptoms.Items.RemoveAt(0);
                }
                patientSymptoms.Items.AddRange(precriptionObj.Symptoms.ToArray());
                PatientComplains.Text = string.Empty;
                for (int i = 0; i < patientSymptoms.Items.Count; i++)
                {
                    if (i % 2 == 0 && i != 0)
                    {
                        PatientComplains.Text += "\n";
                    }
                    PatientComplains.Text += patientSymptoms.Items[i].ToString();
                    if (i != patientSymptoms.Items.Count - 1)
                    {
                        PatientComplains.Text += ",";
                    }


                }
                patientMed = precriptionObj.PrescribedMedicines;
                NoOfDaysNumber.Value = precriptionObj.NumberOfDays;
                if (precriptionObj.PatientDetails.Gender == "Male")
                {
                    genderMaleRadio.Checked=true;
                }
                if ( precriptionObj.PatientDetails.Gender == "Female")
                {
                    genderFemaleRadio.Checked=true;
                }
                if ( precriptionObj.PatientDetails.Gender == "Other")
                {
                    genderOtherRadio.Checked=true;
                }
                while (AllMeds.Controls.Count > 0)
                {
                    AllMeds.Controls.RemoveAt(0);
                }
                
                for (int i = 0; i < patientMed.Count; i++)
                {
                    Panel p = new Panel();
                    p.Dock = DockStyle.Top;
                    p.BorderStyle = BorderStyle.FixedSingle;
                    p.Size = new Size(AllMeds.Size.Width, 60);
                    p.Name = "p" + i;
                    var x = new Label();

                    var frequency = Common.DozeToPrint(patientMed[i].Evening, patientMed[i].Morning, patientMed[i].Night, patientMed[i].Dose);
                    x.Text = patientMed[i].Name + Environment.NewLine + frequency;
                    x.Name = "Medicine" + i;
                    x.ForeColor = Color.Black;
                    x.Dock = System.Windows.Forms.DockStyle.Fill;
                    x.DoubleClick += new EventHandler(this.RemoveMed);
                    x.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    p.Controls.Add(x);

                    AllMeds.Controls.Add(p);
                }
                


            }
            catch (Exception ex)
            {
                MessageBox.Show("Not able to fetch the old patient detail");
                Common.Log(ex);
            }

        }

    }
}
