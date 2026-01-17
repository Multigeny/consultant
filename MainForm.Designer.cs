using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace consultant
{
    public partial class MainForm : Form
    {
        private ConsultantController _controller;
        private Panel _currentPanel;

        public MainForm()
        {
            InitializeComponent();
            _controller = new ConsultantController();
            InitializeSystem();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(600, 500);
            this.Text = "Консультант для мигрантов";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }

        private void InitializeSystem()
        {
            var consultationTypes = _controller.StartSystem();
            ShowConsultationSelection(consultationTypes);
        }

        private void ShowConsultationSelection(List<ConsultationType> consultationTypes)
        {
            ClearCurrentPanel();

            var panel = new Panel { Dock = DockStyle.Fill };
            var y = 50;

            var label = new Label
            {
                Text = "Выберите тип консультации:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(50, y),
                Size = new Size(500, 30)
            };
            y += 40;

            foreach (var type in consultationTypes)
            {
                var button = new Button
                {
                    Text = GetConsultationDisplayName(type),
                    Tag = type,
                    Location = new Point(50, y),
                    Size = new Size(500, 40),
                    Font = new Font("Arial", 10)
                };

                button.Click += (s, e) =>
                {
                    var selectedType = (ConsultationType)((Button)s).Tag;
                    _controller.SelectConsultationType(selectedType);

                    if (selectedType == ConsultationType.Patent)
                        ShowPatentConsultation();
                    else
                        ShowCompensationConsultation();
                };

                panel.Controls.Add(button);
                y += 50;
            }

            this.Controls.Add(panel);
            _currentPanel = panel;
        }

        private void ShowPatentConsultation()
        {
            ClearCurrentPanel();

            var panel = new Panel { Dock = DockStyle.Fill };
            var y = 30;

            // Документ об образовании
            var lblEdu = new Label
            {
                Text = "Документ об образовании:",
                Location = new Point(50, y),
                Size = new Size(300, 25),
                Font = new Font("Arial", 10)
            };
            y += 30;

            var eduCombo = new ComboBox
            {
                Location = new Point(50, y),
                Size = new Size(400, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            eduCombo.Items.AddRange(_controller.GetEducationDocuments()
                .Select(d => GetEducationDisplayName(d)).ToArray());
            eduCombo.SelectedIndex = 0;
            y += 50;

            // НДФЛ - отдельная группа
            var ndflGroup = new GroupBox
            {
                Text = "Наличие выписки об оплате НДФЛ:",
                Location = new Point(50, y),
                Size = new Size(400, 60),
                Font = new Font("Arial", 10)
            };

            var ndflYes = new RadioButton
            {
                Text = "Да",
                Location = new Point(20, 25),
                Size = new Size(100, 25)
            };

            var ndflNo = new RadioButton
            {
                Text = "Нет",
                Location = new Point(150, 25),
                Size = new Size(100, 25),
                Checked = true
            };

            ndflGroup.Controls.Add(ndflYes);
            ndflGroup.Controls.Add(ndflNo);
            panel.Controls.Add(ndflGroup);
            y += 70;

            // ИНН - отдельная группа
            var innGroup = new GroupBox
            {
                Text = "Наличие ИНН:",
                Location = new Point(50, y),
                Size = new Size(400, 60),
                Font = new Font("Arial", 10)
            };

            var innYes = new RadioButton
            {
                Text = "Да",
                Location = new Point(20, 25),
                Size = new Size(100, 25)
            };

            var innNo = new RadioButton
            {
                Text = "Нет",
                Location = new Point(150, 25),
                Size = new Size(100, 25),
                Checked = true
            };

            innGroup.Controls.Add(innYes);
            innGroup.Controls.Add(innNo);
            panel.Controls.Add(innGroup);
            y += 70;

            // Фотографии - отдельная группа
            var photoGroup = new GroupBox
            {
                Text = "Наличие фотографий 3x4:",
                Location = new Point(50, y),
                Size = new Size(400, 60),
                Font = new Font("Arial", 10)
            };

            var photoYes = new RadioButton
            {
                Text = "Да",
                Location = new Point(20, 25),
                Size = new Size(100, 25)
            };

            var photoNo = new RadioButton
            {
                Text = "Нет",
                Location = new Point(150, 25),
                Size = new Size(100, 25),
                Checked = true
            };

            photoGroup.Controls.Add(photoYes);
            photoGroup.Controls.Add(photoNo);
            panel.Controls.Add(photoGroup);
            y += 80;

            // Кнопки
            var btnGetResult = new Button
            {
                Text = "Получить рекомендации",
                Location = new Point(50, y),
                Size = new Size(400, 40),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightBlue
            };

            btnGetResult.Click += (s, e) =>
            {
                try
                {
                    _controller.ProvideEducationDocument((EducationDocument)eduCombo.SelectedIndex);
                    _controller.ProvideNdflReceipt(ndflYes.Checked);
                    _controller.ProvideInn(innYes.Checked);
                    _controller.ProvidePhoto(photoYes.Checked);

                    var message = _controller.GetMessage();
                    ShowResult(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var btnBack = new Button
            {
                Text = "Назад",
                Location = new Point(50, y + 50),
                Size = new Size(400, 30),
                Font = new Font("Arial", 9)
            };
            btnBack.Click += (s, e) => InitializeSystem();

            panel.Controls.Add(lblEdu);
            panel.Controls.Add(eduCombo);
            panel.Controls.Add(btnGetResult);
            panel.Controls.Add(btnBack);

            this.Controls.Add(panel);
            _currentPanel = panel;
        }

        private void ShowCompensationConsultation()
        {
            ClearCurrentPanel();

            var panel = new Panel { Dock = DockStyle.Fill };
            var y = 50;

            var label = new Label
            {
                Text = "Являетесь ли вы участником программы переселения соотечественников?",
                Location = new Point(50, y),
                Size = new Size(500, 50),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            y += 60;

            var yesRadio = new RadioButton
            {
                Text = "Да, являюсь участником",
                Location = new Point(50, y),
                Size = new Size(300, 25),
                Font = new Font("Arial", 10)
            };
            y += 40;

            var noRadio = new RadioButton
            {
                Text = "Нет, не являюсь",
                Location = new Point(50, y),
                Size = new Size(300, 25),
                Font = new Font("Arial", 10),
                Checked = true
            };
            y += 60;

            var btnGetResult = new Button
            {
                Text = "Получить рекомендации",
                Location = new Point(50, y),
                Size = new Size(400, 40),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.LightGreen
            };

            btnGetResult.Click += (s, e) =>
            {
                try
                {
                    _controller.ProvideResettlementProgramStatus(yesRadio.Checked);
                    var message = _controller.GetMessage();
                    ShowResult(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            var btnBack = new Button
            {
                Text = "Назад",
                Location = new Point(50, y + 50),
                Size = new Size(400, 30),
                Font = new Font("Arial", 9)
            };
            btnBack.Click += (s, e) => InitializeSystem();

            panel.Controls.AddRange(new Control[] { label, yesRadio, noRadio, btnGetResult, btnBack });

            this.Controls.Add(panel);
            _currentPanel = panel;
        }

        private void ShowResult(string message)
        {
            ClearCurrentPanel();

            var panel = new Panel { Dock = DockStyle.Fill };
            var y = 30;

            var titleLabel = new Label
            {
                Text = "Рекомендации:",
                Location = new Point(50, y),
                Size = new Size(500, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            y += 40;

            var textBox = new TextBox
            {
                Location = new Point(50, y),
                Size = new Size(500, 200),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Text = message,
                Font = new Font("Arial", 10)
            };
            y += 220;

            var btnNewConsultation = new Button
            {
                Text = "Новая консультация",
                Location = new Point(50, y),
                Size = new Size(200, 40),
                Font = new Font("Arial", 10),
                BackColor = Color.LightGray
            };
            btnNewConsultation.Click += (s, e) => InitializeSystem();

            var btnExit = new Button
            {
                Text = "Выход",
                Location = new Point(300, y),
                Size = new Size(200, 40),
                Font = new Font("Arial", 10),
                BackColor = Color.LightCoral
            };
            btnExit.Click += (s, e) => Application.Exit();

            panel.Controls.AddRange(new Control[] { titleLabel, textBox, btnNewConsultation, btnExit });
            this.Controls.Add(panel);
            _currentPanel = panel;
        }

        private void ClearCurrentPanel()
        {
            if (_currentPanel != null)
            {
                this.Controls.Remove(_currentPanel);
                _currentPanel.Dispose();
                _currentPanel = null;
            }
        }

        private string GetConsultationDisplayName(ConsultationType type)
        {
            return type switch
            {
                ConsultationType.Patent => "Консультация по условиям получения патента",
                ConsultationType.Compensation => "Консультация по компенсации расходов по найму жилья",
                _ => type.ToString()
            };
        }

        private string GetEducationDisplayName(EducationDocument document)
        {
            return document switch
            {
                EducationDocument.None => "Отсутствует",
                EducationDocument.SovietDiploma => "Советский диплом",
                EducationDocument.RussianDiploma => "Российский диплом",
                EducationDocument.FreshLanguageCertificate => "Свежий сертификат о владении русским и знании истории РФ",
                _ => document.ToString()
            };
        }
    }
}
