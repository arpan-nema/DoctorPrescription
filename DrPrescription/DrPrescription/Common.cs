using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using Newtonsoft.Json;


namespace DrPrescription
{
    public class Common
    {
        public static void Log(Exception e)
        {
            try
            {
                if (!Directory.Exists(@".\Data\Log"))
                {
                    Directory.CreateDirectory(@".\Data\Log");
                }
                string logString = "===========" + DateTime.Now.ToString() + "===========";
                logString += Environment.NewLine;
                logString += "Error Message: " + e.Message;
                logString += Environment.NewLine;
                logString += "Stack Trace: " + e.StackTrace;
                logString += Environment.NewLine;
                logString += "Source: " + e.Source;
                logString += Environment.NewLine;
                logString += "Inner Exception: " + (e.InnerException != null?e.InnerException.Message:"No Inner Exception");
                logString += Environment.NewLine;
                File.AppendAllText(@".\Data\Log\ExceptionLogFile.txt", logString);
            }
            catch (Exception)
            {
            }

        }
        public static string DozeToPrint(bool evening, bool morning, bool night,string doze)
        {
            var tempDoze = doze;
            string frequency = " ";
            while (tempDoze.Length < 7)
            {
                tempDoze += " ";
                frequency += " ";
            }

           
            if (doze == string.Empty || doze == null)
            {
                doze = "1";
            }
            if (doze.Length > 8)
            {
                return doze;
            }
            

            if (evening)
            {
                frequency += tempDoze;
            }
            else
            {
                frequency += "0        ";
            }
            if (morning)
            {
                frequency = doze + frequency;
            }
            else
            {
                frequency = "0" + frequency;
            }
            if (night)
            {
                frequency += doze;

            }
            else
            {
                frequency += "0";
            }
            
            return frequency;
        }
        public static string PrintAndSave(Prescription PrescriptionObj)
        {
            var result = string.Empty;
            try
            {
                int patientListCount = 0;
                if (System.IO.File.Exists(@".\Data\TotalPrescriptionCount.txt"))
                {
                    var data = System.IO.File.ReadAllText(@".\Data\TotalPrescriptionCount.txt");
                    if (data != string.Empty && System.Text.RegularExpressions.Regex.IsMatch(data, "[0-9]*"))
                    {
                        patientListCount = Convert.ToInt32(data);
                    }
                }
                string patientDetials = string.Empty;
                patientDetials += "Date- " + DateTime.Now.ToShortDateString();
                patientDetials += Environment.NewLine;
                patientDetials += Environment.NewLine;
                patientDetials += "Name: " + PrescriptionObj.PatientDetails.Name;
                patientDetials += "             ";
                patientDetials += "Age: " + PrescriptionObj.PatientDetails.Age;
                patientDetials += "             ";
                patientDetials += "Gender: " + PrescriptionObj.PatientDetails.Gender;
                patientDetials += "             ";
                patientDetials += "Weight: " + (PrescriptionObj.PatientDetails.Weight == 0 ? string.Empty : PrescriptionObj.PatientDetails.Weight.ToString());
                patientDetials += Environment.NewLine;
                patientDetials += Environment.NewLine;

                var allSymptoms = string.Empty;
                PrescriptionObj.Symptoms.ForEach(x => {
                    if (PrescriptionObj.Symptoms.Last() == x)
                        allSymptoms += x;
                    else
                        allSymptoms += x + "/";
                });
                patientDetials += @"C/O";
                patientDetials += Environment.NewLine;
                patientDetials += allSymptoms;
                patientDetials += Environment.NewLine;
                patientDetials += Environment.NewLine;
                patientDetials += Environment.NewLine;
                patientDetials += Environment.NewLine;
                var medsName = string.Empty;
                //var doseValues = string.Empty;

                PdfPTable temptable = new PdfPTable(2);
                temptable.SetWidthPercentage(new float[] { 60f, 40f }, PageSize.A4);
                var tempCell = new PdfPCell(new Paragraph());
                tempCell.BorderWidth = 0;
                tempCell.Padding = 0;
                tempCell.PaddingTop = 12;
                tempCell.HorizontalAlignment = Element.ALIGN_LEFT;
                temptable.AddCell(tempCell);
                var dozeTable = new PdfPTable(3);
                var morningCell = new PdfPCell(new Paragraph("M."));
                morningCell.BorderWidth = 0;
                var afterNoonCell = new PdfPCell(new Paragraph("AN."));
                afterNoonCell.BorderWidth = 0;
                var nightCell = new PdfPCell(new Paragraph("N."));
                nightCell.BorderWidth = 0;
                dozeTable.AddCell(morningCell);
                dozeTable.AddCell(afterNoonCell);
                dozeTable.AddCell(nightCell);
                tempCell = new PdfPCell(dozeTable);
                tempCell.BorderWidth = 0;
                tempCell.Padding = 0;
                tempCell.PaddingTop = 12;
                tempCell.HorizontalAlignment = Element.ALIGN_LEFT;
                temptable.AddCell(tempCell);

                foreach (var m in PrescriptionObj.PrescribedMedicines)
                {
                    medsName = m.Name;
                    medsName += Environment.NewLine;
                    medsName += m.Precautions;
                    medsName += Environment.NewLine;
                    var c = new PdfPCell(new Paragraph(medsName));
                    c.Border = 0;
                    temptable.AddCell(c);
                    if (m.Dose.Length < 8)
                    {
                        dozeTable = new PdfPTable(3);
                        morningCell = new PdfPCell(new Paragraph(m.Morning ? m.Dose : "0"));
                        morningCell.BorderWidth = 0;
                        afterNoonCell = new PdfPCell(new Paragraph(m.Evening ? m.Dose : "0"));
                        afterNoonCell.BorderWidth = 0;
                        nightCell = new PdfPCell(new Paragraph(m.Night ? m.Dose : "0"));
                        nightCell.BorderWidth = 0;
                        dozeTable.AddCell(morningCell);
                        dozeTable.AddCell(afterNoonCell);
                        dozeTable.AddCell(nightCell);
                    }
                    else
                    {
                        dozeTable = new PdfPTable(1);
                        morningCell = new PdfPCell(new Paragraph(m.Dose));
                        morningCell.BorderWidth = 0;
                        dozeTable.AddCell(morningCell);
                    }

                    

                    //doseValues =DozeToPrint(m.Evening, m.Morning, m.Night,m.Dose);
                    //doseValues += Environment.NewLine;
                    //doseValues += Environment.NewLine;
                    //doseValues += Environment.NewLine;
                    c = new PdfPCell(dozeTable);
                    c.Border = 0;
                    temptable.AddCell(c);
                }
                var noOfDaysData = string.Empty;
                noOfDaysData += Environment.NewLine;
                noOfDaysData += Environment.NewLine;
                noOfDaysData += "                                                 X " + PrescriptionObj.NumberOfDays + " Days";
                var resultPara = new Paragraph();
                result += Environment.NewLine;
                result += "G/C- " +PrescriptionObj.ReadingsDetails.GAndCReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "Pallor- " + PrescriptionObj.ReadingsDetails.PallorReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "Ict.- "+PrescriptionObj.ReadingsDetails.IctReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "LN- "+PrescriptionObj.ReadingsDetails.LNReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "Oedema- "+PrescriptionObj.ReadingsDetails.OdemaReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "Pulse- " + PrescriptionObj.ReadingsDetails.PulseReading+" /min";
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "Temp.- " + PrescriptionObj.ReadingsDetails.FeverReading+ " °F";
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "BP- " + PrescriptionObj.ReadingsDetails.BPReading+" mmhg";
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "R/S- " + PrescriptionObj.ReadingsDetails.RSReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "CVS- " + PrescriptionObj.ReadingsDetails.CSVReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "P/A- " + PrescriptionObj.ReadingsDetails.PAndAReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "ECG- " + PrescriptionObj.ReadingsDetails.ECGReading;
                result += Environment.NewLine;
                result += Environment.NewLine;
                result += "Blood Sugar R- " + PrescriptionObj.ReadingsDetails.BloodSugarReading+" mg";
                result += Environment.NewLine;
                result += Environment.NewLine;

                result += "Advised- ";
                result += Environment.NewLine;
                resultPara.Add(result);

                result = "CBC/ESR- " + PrescriptionObj.ReadingsDetails.CBCReading;
                //resultPara.Add(result);
                
                Font zapfdingbats = new Font(Font.FontFamily.ZAPFDINGBATS);
                Phrase phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.CBCSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);
                //result += Environment.NewLine;
                result = "BMP/RMP- " +PrescriptionObj.ReadingsDetails.BMPRMPReading;
                //result += Environment.NewLine;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.BMPRMPSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }

                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));

                resultPara.Add(phrase);
                resultPara.Add(new Chunk(Environment.NewLine));
                //------------------------------1-------------------------------------------------


                result = "LFT- " +PrescriptionObj.ReadingsDetails.LFTReading ;
                //result += Environment.NewLine;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.LFTSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);

                result = "KFT- " +PrescriptionObj.ReadingsDetails.KFTReading ;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.KFTSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);
                resultPara.Add(new Chunk(Environment.NewLine));
                //-------------------------------------2----------------------------------------------
                result = "Widal- " +PrescriptionObj.ReadingsDetails.WidalReading;
                //result += Environment.NewLine;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.WidalSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);
                result = "URINE R/E- " +PrescriptionObj.ReadingsDetails.UrineReReading ;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.UrineReSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);
                resultPara.Add(new Chunk(Environment.NewLine));
                //------------------------------------------3--------------------------------------------

                result = "T3 T4 TSH- " + PrescriptionObj.ReadingsDetails.T3T4TSHReading;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.T3T4TSHSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);
                result = "X-Ray- " +PrescriptionObj.ReadingsDetails.XRayReading ;

                phrase = new Phrase(result);

                if (PrescriptionObj.ReadingsDetails.XRaySelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);
                resultPara.Add(new Chunk(Environment.NewLine));
                //----------------------------------------4---------------------------------------------------

                result = "USG W/A- " +PrescriptionObj.ReadingsDetails.USGReading ;
                //result += Environment.NewLine;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.USGSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                if (PrescriptionObj.ReadingsDetails.USGSelect)
                {
                    result = "Flatunex 4 at night\nGerbisa 1 at night\n";
                    phrase.Add(new Phrase(result));
                }
                
                resultPara.Add(phrase);
                result = "Lipid Profile- " + PrescriptionObj.ReadingsDetails.LipidProfileReading ;
                //result += Environment.NewLine;
                phrase = new Phrase(result);
                if (PrescriptionObj.ReadingsDetails.LipidProfileSelect)
                {
                    phrase.Add(new Chunk("\u0033", zapfdingbats));
                }
                    
                phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                resultPara.Add(phrase);
                resultPara.Add(new Chunk(Environment.NewLine));
                //-------------------------------------------5------------------------------------------------
                if(PrescriptionObj.ExtraExaminations!=null && PrescriptionObj.ExtraExaminations != string.Empty)
                {
                   var extraLst= PrescriptionObj.ExtraExaminations.Split('/');
                    foreach (var e in extraLst)
                    {
                        result = e;
                        //result += Environment.NewLine;
                        phrase = new Phrase(result);
                        phrase.Add(new Chunk("\u0033", zapfdingbats));
                        phrase.Add(new Chunk(Environment.NewLine, zapfdingbats));
                        resultPara.Add(phrase);
                        resultPara.Add(new Chunk(Environment.NewLine));
                    }
                }
                

                var fileDataStr = System.IO.File.ReadAllText(string.Format(
                    @".\Data\AllPatientPrescription\DaysPatient{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_')
                    ));
                List<Prescription> allDayPrescriptions = new List<Prescription>();
                if (fileDataStr == null || fileDataStr == string.Empty)
                {
                    fileDataStr = string.Empty;
                }
                else
                {
                    allDayPrescriptions = JsonConvert.DeserializeObject<List<Prescription>>(fileDataStr);

                }
                if (allDayPrescriptions.Any(x => x.PrescriptionId == PrescriptionObj.PrescriptionId))
                {
                    allDayPrescriptions.Remove(allDayPrescriptions.Where(x => x.PrescriptionId == PrescriptionObj.PrescriptionId).FirstOrDefault());
                }
                var splitData = PrescriptionObj.PrescriptionId.Split('_');
                if (splitData.Length != 4 ||splitData[0]+" "+splitData[1]+" "+splitData[2]!=DateTime.Now.ToLongDateString())
                {
                    
                    

                    PrescriptionObj.PrescriptionId= DateTime.Now.ToLongDateString().Replace(" ", "_") + "_N" + patientListCount+1;
                }
                allDayPrescriptions.Add(PrescriptionObj);

                System.IO.File.WriteAllText(
                    string.Format(@".\Data\AllPatientPrescription\DaysPatient{0}.json", DateTime.Now.ToLongDateString().Replace(' ', '_')),
                    JsonConvert.SerializeObject(allDayPrescriptions));

                var fileList = System.IO.Directory.GetFiles(@".\Data\AllPatientPrescription").Where(x => x.Contains(DateTime.Now.ToLongDateString().Replace(' ', '_')));
                int count = 1;
                if (fileList != null)
                {
                    count = fileList.Count() + 1;
                }
                //MultiColumnText multilinetxt = new MultiColumnText();
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                //ColumnText columns =ColumnText.FitText(FontFactory.GetFont("Verdana"),patientDetials,new Rectangle(0,0,50,50),10f,1);

                string pdfFilename = string.Format(@".\Data\AllPatientPrescription\Patient{0}.pdf", count.ToString() + "_" + DateTime.Now.ToLongDateString().Replace(' ', '_'));
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFilename, FileMode.Create));

                //


                //var Renderer = new IronPdf.HtmlToPdf();
                //var htmlTemplate = System.IO.File.ReadAllText(@".\Data\PrescriptionTemplate.html");
                //var PDF = Renderer.RenderHtmlAsPdf(string.Format(htmlTemplate, patientDetials));
                //var OutputPath = pdfFilename;
                //PDF.SaveAs(OutputPath);
                //// This neat trick opens our PDF file so we can see the result in our default PDF viewer
                //System.Diagnostics.Process.Start(OutputPath);




                document.Open();
                var img = Image.GetInstance(@".\Data\HeaderImage.jpg");//daignosisImage.jpg
                img.ScaleAbsoluteWidth(PageSize.A4.Width - 20f);
                img.ScaleAbsoluteHeight(PageSize.A4.Height * 0.2f);
                //string text = result;
                Paragraph paragraph = new Paragraph();
                var ch = new Chunk("Id-" + PrescriptionObj.PrescriptionId);
                ch.Font = FontFactory.GetFont("Calibri (Body)", 8f, BaseColor.BLACK);
                paragraph.Add(ch);
                paragraph.Add(img);
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = Element.ALIGN_LEFT;
                paragraph.Font = FontFactory.GetFont("Calibri (Body)", 14f, BaseColor.BLACK);

                var detailsParaGraph = new Paragraph(resultPara);
                detailsParaGraph.SpacingBefore = 10;
                detailsParaGraph.SpacingAfter = 10;
                detailsParaGraph.Alignment = Element.ALIGN_LEFT;
                detailsParaGraph.Font = FontFactory.GetFont("Calibri (Body)", 14f, BaseColor.BLACK);
                detailsParaGraph.SetLeading(20f,4f);
                PdfPTable table = new PdfPTable(2);
                var cell = new PdfPCell(detailsParaGraph);
                cell.BorderWidth = 0;
                cell.BorderWidthRight = 1;
                cell.Padding = 0;
                cell.PaddingTop = 12;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);
                //detailsParaGraph = new Paragraph();
                //detailsParaGraph.Add(patientDetials);
                
                
                //detailsParaGraph.Add(temptable);
                //var c=new Chunk(medsName);
                //c.
                //detailsParaGraph.Add(c);
                //detailsParaGraph.Add(new Chunk(doseValues));
                //detailsParaGraph.Add(noOfDaysData);
                var testTempTable = new PdfPTable(1);
                
                testTempTable.AddCell(new Paragraph(patientDetials));
                var diagnosisTable = new PdfPTable(2);
                diagnosisTable.AddCell(new Paragraph("Rx"));

                var diagnosisPara = new Paragraph();
                diagnosisPara.Add("Diag.");
                diagnosisPara.Add(PrescriptionObj.Remark);
                diagnosisTable.AddCell(diagnosisPara);
                foreach (var r in diagnosisTable.Rows)
                {
                    foreach (var c in r.GetCells())
                    {
                        c.Border = 0;
                    }
                }
                testTempTable.AddCell(diagnosisTable);
                testTempTable.AddCell(new Paragraph());
                testTempTable.AddCell(temptable);
                testTempTable.AddCell(new Paragraph(noOfDaysData));
                if (PrescriptionObj.Precautions != string.Empty && PrescriptionObj.Precautions != null)
                {
                    testTempTable.AddCell(new Paragraph("Precautions-" + PrescriptionObj.Precautions));
                }
               
                
                string signTxt = string.Empty + Environment.NewLine + Environment.NewLine +
                  "____________________________" + Environment.NewLine + "        Signature                  ";
                var para = new Paragraph(signTxt, FontFactory.GetFont("Calibri (Body)", 12f, BaseColor.BLACK));
                para.Alignment = Element.ALIGN_CENTER;
                var newCell = new PdfPCell(para);
                newCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                newCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                testTempTable.AddCell(newCell);
                foreach (var r in testTempTable.Rows)
                {
                    foreach (var c in r.GetCells())
                    {
                        c.Border = 0;
                    }
                }

                cell = new PdfPCell(testTempTable);
                
                cell.BorderWidth = 0;
                cell.Padding = 0;
                cell.PaddingTop = 12;
                cell.PaddingLeft = 10;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);
                table.SetWidthPercentage(new float[2] { 175f, 424f }, PageSize.A4);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                
                paragraph.Add(table);
               
                //paragraph.Add(para);
                document.Add(paragraph);
                
                //writer.Close();
                document.Close();
                
                //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfFilename, FileMode.Create));
                
                Process.Start(pdfFilename);

                //PdfDocument pdf = new PdfDocument();
                //pdf.Info.Title =string.Format("Patient {0} {1}",count, DateTime.Now.ToLongDateString());
                //PdfPage pdfPage = pdf.AddPage();
                //pdfPage.Size = PageSize.A4;
                //XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                //XFont font = new XFont("Calibri (Body)", 11, XFontStyle.Bold);
                //Paragraph paragraph = new Paragraph();
                //paragraph.SpacingBefore = 10;
                //paragraph.SpacingAfter = 10;
                //paragraph.Alignment = Element.ALIGN_LEFT;
                //paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
                //paragraph.Add(text);
                //document.Add(paragraph);

                //graph.(result, font, XBrushes.Black, new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.Center);

                //string pdfFilename = string.Format(@".\Data\AllPatientPrescription\Patient{0}.pdf", count.ToString()+"_"+DateTime.Now.ToLongDateString().Replace(' ','_'));
                //pdf.Save(pdfFilename);
                result = "Success you can see the file "+ pdfFilename;
                patientListCount++;
                System.IO.File.WriteAllText(@".\Data\TotalPrescriptionCount.txt", patientListCount.ToString());
            }
            catch (Exception e)
            {
                Common.Log(e);
                result = "Some error occured";
            }
           
           

            return result;

        }
    }
}
