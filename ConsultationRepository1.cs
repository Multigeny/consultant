using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consultant
{
    public class ConsultationRepository
    {
        private Applicant _currentApplicant;
        private ConsultationType? _currentConsultationType;
        private Rule _rule;

        public ConsultationRepository()
        {
            _currentApplicant = null;
            _currentConsultationType = null;
            _rule = new Rule();
        }

        public List<ConsultationType> GetConsultationList()
        {
            return Enum.GetValues(typeof(ConsultationType))
                       .Cast<ConsultationType>()
                       .ToList();
        }

        public void SelectConsultationType(ConsultationType type)
        {
            _currentConsultationType = type;
            _currentApplicant = new Applicant();
        }

        public List<EducationDocument> GetEducationDocumentList()
        {
            return Enum.GetValues(typeof(EducationDocument))
                       .Cast<EducationDocument>()
                       .ToList();
        }

        public void setEducationDocument(EducationDocument document)
        {
            ValidateActiveConsultation();

            if (_currentConsultationType != ConsultationType.Patent)
                throw new InvalidOperationException("Документ об образовании требуется только для консультации по патенту");

            _currentApplicant.EducationDocument = document;
        }

        public void setNdflReceipt(bool hasReceipt)
        {
            ValidateActiveConsultation();

            if (_currentConsultationType != ConsultationType.Patent)
                throw new InvalidOperationException("Выписка НДФЛ требуется только для консультации по патенту");

            _currentApplicant.HasNDFLReceipt = hasReceipt;
        }

        public void setInn(bool hasInn)
        {
            ValidateActiveConsultation();

            if (_currentConsultationType != ConsultationType.Patent)
                throw new InvalidOperationException("ИНН требуется только для консультации по патенту");

            _currentApplicant.HasINN = hasInn;
        }

        public void setPhoto(bool hasPhotos)
        {
            ValidateActiveConsultation();

            if (_currentConsultationType != ConsultationType.Patent)
                throw new InvalidOperationException("Фотографии требуются только для консультации по патенту");

            _currentApplicant.HasPhotos = hasPhotos;
        }

        public void setResettlementProgramStatus(bool isParticipant)
        {
            ValidateActiveConsultation();

            if (_currentConsultationType != ConsultationType.Compensation)
                throw new InvalidOperationException("Статус программы переселения требуется только для консультации по компенсации");

            _currentApplicant.IsResettlementProgramParticipant = isParticipant;
        }

        public string GetMessage()
        {
            ValidateActiveConsultation();
            return _rule.buildMessage(_currentApplicant, _currentConsultationType.Value);
        }

        private void ValidateActiveConsultation()
        {
            if (_currentApplicant == null || !_currentConsultationType.HasValue)
                throw new InvalidOperationException("Сначала выберите тип консультации");
        }
    }
}
