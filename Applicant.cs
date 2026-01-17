using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consultant
{

    public enum ConsultationType
    {
        Patent,         // Консультация по условиям получения патента
        Compensation    // Консультация по компенсации расходов по найму жилья
    }

    public enum EducationDocument
    {
        None,                       // Отсутствует
        SovietDiploma,              // Советский диплом
        RussianDiploma,             // Российский диплом
        FreshLanguageCertificate    // Свежий сертификат о владении русским и знанием истории РФ
    }

    public class Applicant
    {
        public EducationDocument EducationDocument { get; set; }
        public bool HasNDFLReceipt { get; set; }
        public bool HasINN { get; set; }
        public bool HasPhotos { get; set; }
        public bool IsResettlementProgramParticipant { get; set; }

        public Applicant()
        {
            EducationDocument = EducationDocument.None;
            HasNDFLReceipt = false;
            HasINN = false;
            HasPhotos = false;
            IsResettlementProgramParticipant = false;
        }
    }
}
