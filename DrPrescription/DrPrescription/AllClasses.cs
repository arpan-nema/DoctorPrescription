using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrPrescription
{
    //public class PatientCount
    //{
    //    public int NoOfPatientCame { get; set; }
    //    public int NoOfPatientAttended { get; set; }
    //    public DateTime Day { get; set; }
    //}
    public class Readings
    {
        public string BPReading { get; set; }
        public int PulseReading { get; set; }
        public double FeverReading { get; set; }
        public string BloodSugarReading { get; set; }
        public string ECGReading { get; set; }
        public string RSReading { get; set; }
        public string CSVReading { get; set; }
        public string PAndAReading { get; set; }
        public string PallorReading { get; set; }
        public string GAndCReading { get; set; }
        public string IctReading { get; set; }
        public string LNReading { get; set; }
        public string OdemaReading { get; set; }
        public string LipidProfileReading { get; set; }
        public string CBCReading { get; set; }
        public string BMPRMPReading { get; set; }
        public string LFTReading { get; set; }
        public string KFTReading { get; set; }
        public string WidalReading { get; set; }
        public string UrineReReading { get; set; }
        public string T3T4TSHReading { get; set; }
        public string XRayReading { get; set; }
        public string USGReading { get; set; }
        public bool LipidProfileSelect { get; set; }
        public bool CBCSelect { get; set; }
        public bool BMPRMPSelect { get; set; }
        public bool LFTSelect { get; set; }
        public bool KFTSelect { get; set; }
        public bool WidalSelect { get; set; }
        public bool UrineReSelect { get; set; }
        public bool T3T4TSHSelect { get; set; }
        public bool XRaySelect { get; set; }
        public bool USGSelect{ get; set; }
    }

    public class Patient
    {
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public bool Attended { get; set; }
        public double Weight { get; set; }
    }
    public class Medicine
    {
        public string Name { get; set; }
        public string Dose { get; set; }
        public bool Morning { get; set; }
        public bool Evening  { get; set; }
        public bool Night { get; set; }
        public string Precautions { get; set; }
    }
    public class Prescription
    {
        public string PrescriptionId { get; set; }
        public Patient PatientDetails { get; set; }
        public Readings ReadingsDetails { get; set; }
        public List<string> Symptoms { get; set; }
        public string Remark { get; set; }
        public List<Medicine> PrescribedMedicines { get; set; }
        public int NumberOfDays { get; set; }
        public string ExtraExaminations { get; set; }
        public string Precautions { get; set; }

    }
}
