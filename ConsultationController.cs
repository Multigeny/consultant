using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consultant
{
    public class ConsultantController
    {
        private ConsultationRepository _repository;

        public ConsultantController()
        {
            _repository = new ConsultationRepository();
        }

        public List<ConsultationType> StartSystem()
        {
            // Возвращаем список доступных консультаций
            return _repository.GetConsultationList();
        }

        public void SelectConsultationType(ConsultationType type)
        {
            _repository.SelectConsultationType(type);
        }

        public List<EducationDocument> GetEducationDocuments()
        {
            return _repository.GetEducationDocumentList();
        }

        public void ProvideEducationDocument(EducationDocument educationDocument)
        {
            _repository.setEducationDocument(educationDocument);
        }

        public void ProvideNdflReceipt(bool hasReceipt)
        {
            _repository.setNdflReceipt(hasReceipt);
        }

        public void ProvideInn(bool hasInn)
        {
            _repository.setInn(hasInn);
        }

        public void ProvidePhoto(bool hasPhotos)
        {
            _repository.setPhoto(hasPhotos);
        }

        public void ProvideResettlementProgramStatus(bool isParticipant)
        {
            _repository.setResettlementProgramStatus(isParticipant);
        }

        public string GetMessage()
        {
            return _repository.GetMessage();
        }
    }
}
