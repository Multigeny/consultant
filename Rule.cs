using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consultant
{
    public class Rule
    {
        public List<EducationDocument> RequiredEducationDocuments { get; private set; }

        public Rule()
        {
            RequiredEducationDocuments = new List<EducationDocument>
        {
            EducationDocument.SovietDiploma,
            EducationDocument.RussianDiploma,
            EducationDocument.FreshLanguageCertificate
        };
        }

        // Основной метод согласно диаграмме + дополнительный параметр
        public string buildMessage(Applicant applicant, ConsultationType consultationType)
        {
            return consultationType switch
            {
                ConsultationType.Patent => BuildPatentMessage(applicant),
                ConsultationType.Compensation => BuildCompensationMessage(applicant),
                _ => "Неизвестный тип консультации"
            };
        }

        private string BuildPatentMessage(Applicant applicant)
        {
            var messages = new List<string>();

            // Проверка документов об образовании
            if (!RequiredEducationDocuments.Contains(applicant.EducationDocument) &&
                applicant.EducationDocument != EducationDocument.None)
            {
                messages.Add("Обратитесь в образовательную организацию для получения подходящего документа об образовании");
            }
            else if (applicant.EducationDocument == EducationDocument.None)
            {
                messages.Add("Обратитесь в образовательную организацию для получения документа об образовании");
            }

            // Проверка НДФЛ
            if (!applicant.HasNDFLReceipt)
            {
                messages.Add("Обратитесь в банк для получения выписки об оплате НДФЛ");
            }

            // Проверка ИНН
            if (!applicant.HasINN)
            {
                messages.Add("Обратитесь в ФНС для получения ИНН");
            }

            // Проверка фотографий
            if (!applicant.HasPhotos)
            {
                messages.Add("Обратитесь в фотосалон для изготовления фотографий 3x4");
            }

            if (messages.Count == 0)
            {
                return "Все необходимые документы для получения патента собраны!";
            }

            return string.Join(Environment.NewLine, messages);
        }

        private string BuildCompensationMessage(Applicant applicant)
        {
            if (!applicant.IsResettlementProgramParticipant)
            {
                return "Для получения компенсации необходимо быть участником программы переселения. Обратитесь в миграционную службу.";
            }

            return "Вы можете подать документы на компенсацию расходов по найму жилья.";
        }

        // Старый метод для совместимости (если нужен)
        public string buildMessage(Applicant applicant)
        {
            return buildMessage(applicant, ConsultationType.Patent);
        }
    }
}
