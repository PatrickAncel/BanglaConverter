namespace BanglaConverter
{
    public partial class MainForm : Form, ITextEditor
    {

        /// <summary>
        /// A string containing the word "Bangla" written in Bangla letters.
        /// </summary>
        private string banglaInBangla
            = BanglaUnicodeData
                .MakeString(BanglaUnicodeData.CodePoint.Ba, BanglaUnicodeData.CodePoint.AKar, BanglaUnicodeData.CodePoint.Anusvar,
                    BanglaUnicodeData.CodePoint.La, BanglaUnicodeData.CodePoint.AKar);

        private readonly KeypressProcessor keypressProcessor;

        public void ConverterToggleHandler(bool enabled)
        {
            if (enabled)
            {
                lblLanguageMode.Text = "Bangla Converter On";
                this.Text = banglaInBangla;
            }
            else
            {
                lblLanguageMode.Text = "Bangla Converter Off";
                this.Text = "Bangla";
            }

            HighlightActiveLetters();
        }

        public void VowelModeChangeHandler(SharedData.VowelMode vowelMode)
        {
            DisplayVowelMode();
            SetLetterLabels();
        }

        public void ProcessorStateChangeHandler()
        {
            HighlightActiveLetters();
        }

        public void WriteText(string text)
        {
            txtWorkArea.SelectedText = text;
        }

        public string CurrentText
        {
            get
            {
                return txtWorkArea.Text;
            }
        }

        public int CurrentPosition
        {
            get
            {
                return txtWorkArea.SelectionStart;
            }
        }

        public MainForm()
        {
            InitializeComponent();

            keypressProcessor = new KeypressProcessor(this);

            // Calls the event handlers to respond to the
            // keypress processor's initial state.
            ConverterToggleHandler(keypressProcessor.ConverterEnabled);
            VowelModeChangeHandler(keypressProcessor.CurrentVowelMode);
            ProcessorStateChangeHandler();

            ApplySettings();

            SettingsForm.UpdateMainForm = ApplySettings;
        }

        /// <summary>
        /// Changes the background colors of the letter labels to indicate which ones are active,
        /// depending on whether the shift key is held.
        /// </summary>
        private void HighlightActiveLetters()
        {
            // Gets the list of letters to highlight.
            List<BanglaUnicodeData.CodePoint> activeLetters = keypressProcessor.GetActiveBanglaLetters();

            bool firstVowelActive = false, aActive = false,
                shortIActive = false, longIActive = false,
                shortUActive = false, longUActive = false,
                raActive = false, riActive = false,
                eActive = false, oiActive = false,
                oActive = false, ouActive = false;

            foreach (BanglaUnicodeData.CodePoint letter in activeLetters)
            {
                if (letter == BanglaUnicodeData.CodePoint.FirstVowel)
                {
                    firstVowelActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.A || letter == BanglaUnicodeData.CodePoint.AKar)
                {
                    aActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.HrasvaI || letter == BanglaUnicodeData.CodePoint.HrasvaIKar)
                {
                    shortIActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.DirghaI || letter == BanglaUnicodeData.CodePoint.DirghaIKar)
                {
                    longIActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.HrasvaU || letter == BanglaUnicodeData.CodePoint.HrasvaUKar)
                {
                    shortUActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.DirghaU || letter == BanglaUnicodeData.CodePoint.DirghaUKar)
                {
                    longUActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.Ra)
                {
                    raActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.RI || letter == BanglaUnicodeData.CodePoint.RIKar)
                {
                    riActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.E || letter == BanglaUnicodeData.CodePoint.EKar)
                {
                    eActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.OI || letter == BanglaUnicodeData.CodePoint.OIKar)
                {
                    oiActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.O || letter == BanglaUnicodeData.CodePoint.OKar)
                {
                    oActive = true;
                }
                else if (letter == BanglaUnicodeData.CodePoint.OU || letter == BanglaUnicodeData.CodePoint.OUKar)
                {
                    ouActive = true;
                }
            }

            lblFirstVowel.Visible = firstVowelActive;
            lblA.Visible = aActive;
            lblShortI.Visible = shortIActive;
            lblLongI.Visible = longIActive;
            lblShortU.Visible = shortUActive;
            lblLongU.Visible = longUActive;
            lblRa.Visible = raActive;
            lblRI.Visible = riActive;
            lblE.Visible = eActive;
            lblOI.Visible = oiActive;
            lblO.Visible = oActive;
            lblOU.Visible = ouActive;
        }

        private void DisplayVowelMode()
        {
            if (keypressProcessor.CurrentVowelMode == SharedData.VowelMode.FullVowel)
            {
                lblVowelMode.Text = "Full Vowel Mode";
            }
            else if (keypressProcessor.CurrentVowelMode == SharedData.VowelMode.VowelSign)
            {
                lblVowelMode.Text = "Vowel Sign Mode";
            }
        }

        private void SetLetterLabels()
        {
            if (keypressProcessor.CurrentVowelMode == SharedData.VowelMode.FullVowel)
            {
                lblFirstVowel.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.FirstVowel);
                lblA.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.A);
                lblShortI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.HrasvaI);
                lblLongI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.DirghaI);
                lblShortU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.HrasvaU);
                lblLongU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.DirghaU);
                lblRI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.RI);
                lblE.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.E);
                lblOI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.OI);
                lblO.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.O);
                lblOU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.OU);
            }
            else
            {
                lblFirstVowel.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.Invalid);
                lblA.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.AKar);
                lblShortI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.HrasvaIKar);
                lblLongI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.DirghaIKar);
                lblShortU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.HrasvaUKar);
                lblLongU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.DirghaUKar);
                lblRI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.RIKar);
                lblE.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.EKar);
                lblOI.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.OIKar);
                lblO.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.OKar);
                lblOU.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.OUKar);
            }
            lblRa.Text = BanglaUnicodeData.MakeString(BanglaUnicodeData.CodePoint.Ra);
        }

        private void txtWorkArea_KeyUp(object sender, KeyEventArgs e)
        {
            if (keypressProcessor.ConverterEnabled)
            {
                e.SuppressKeyPress = true;
            }
            keypressProcessor.InputEventHandler(SharedData.InputEventType.KeyUp, e.KeyCode.ToString());
        }

        private void txtWorkArea_KeyDown(object sender, KeyEventArgs e)
        {
            // If the key combination is Ctrl + C, Ctrl + V, Ctrl + X, or Ctrl + A, do nothing.
            if (e.Control && !e.Shift && !e.Alt && (e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X || e.KeyCode == Keys.A))
            {
                return;
            }
            // Otherwise, if the converter is enabled, suppress the input to prevent default behavior.
            else if (keypressProcessor.ConverterEnabled)
            {
                e.SuppressKeyPress = true;
            }
            keypressProcessor.InputEventHandler(SharedData.InputEventType.KeyDown, e.KeyCode.ToString());
        }

        private void txtWorkArea_SelectionChanged(object sender, EventArgs e)
        {
            // The KeypressProcessor must set the vowel mode according to the position of the new selection.
            keypressProcessor.AutoSetVowelMode();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // The textarea should always be exactly 40 pixels thinner than the form.
            txtWorkArea.Width = this.Width - 40;
        }

        /// <summary>
        /// Reads new settings from the SettingsManager and applies them to the form elements.
        /// </summary>
        private void ApplySettings()
        {
            // Reads the current settings.
            Color textColor = (Color)SettingsManager.GetSetting(SettingsManager.Setting.TextColor);
            Color backgroundColor = (Color)SettingsManager.GetSetting(SettingsManager.Setting.BackgroundColor);
            Color formColor = (Color)SettingsManager.GetSetting(SettingsManager.Setting.FormColor);
            float fontSize = (float)SettingsManager.GetSetting(SettingsManager.Setting.FontSize);

            txtWorkArea.ForeColor = textColor;
            txtWorkArea.BackColor = backgroundColor;
            this.BackColor = formColor;
            menuStrip1.BackColor = formColor;
            txtWorkArea.Font = new Font(txtWorkArea.Font.FontFamily, fontSize, txtWorkArea.Font.Style);

            bool formIsDark = formColor.GetBrightness() < 0.4;

            foreach (Control control in this.Controls)
            {
                if (control is Label || control is MenuStrip)
                {
                    // Selects a text color to contrast with the background.
                    control.ForeColor = formIsDark ? Color.WhiteSmoke : SystemColors.ControlText;
                }
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }
    }
}