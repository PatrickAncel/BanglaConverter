namespace BanglaConverter
{
    public partial class MainForm : Form
    {

        /// <summary>
        /// A string containing the word "Bangla" written in Bangla letters.
        /// </summary>
        private string banglaInBangla
            = BanglaUnicodeData
                .MakeString(BanglaUnicodeData.CodePoint.Ba, BanglaUnicodeData.CodePoint.AKar, BanglaUnicodeData.CodePoint.Anusvar,
                    BanglaUnicodeData.CodePoint.La, BanglaUnicodeData.CodePoint.AKar);

        public MainForm()
        {
            InitializeComponent();
            DisplayVowelMode();
            DisplayLanguageMode();
            HighlightActiveLetters();

            DisplayVowelMode();
            SetLetterLabels();

            ApplySettings();

            KeypressProcessor.LanguageModeChangeCallback += DisplayLanguageMode;
            KeypressProcessor.LanguageModeChangeCallback += HighlightActiveLetters;

            KeypressProcessor.VowelModeChangeCallback += DisplayVowelMode;
            KeypressProcessor.VowelModeChangeCallback += SetLetterLabels;

            KeypressProcessor.KeyModifiersChangeCallback += HighlightActiveLetters;
            
            KeypressProcessor.DeliverOutput += WriteToWorkArea;
            KeypressProcessor.ReadTextBeforeCursor = ReadTextBeforeCursor;

            SettingsForm.UpdateMainForm = ApplySettings;
        }

        private void WriteToWorkArea(string text)
        {
            txtWorkArea.SelectedText = text;
        }

        /// <summary>
        /// Changes the background colors of the letter labels to indicate which ones are active,
        /// depending on whether the shift key is held.
        /// </summary>
        private void HighlightActiveLetters()
        {
            // Gets the list of letters to highlight.
            List<BanglaUnicodeData.CodePoint> activeLetters = KeypressProcessor.GetActiveBanglaLetters();

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
            if (KeypressProcessor.CurrentVowelMode == KeypressProcessor.VowelMode.FullVowel)
            {
                lblVowelMode.Text = "Full Vowel Mode";
            }
            else if (KeypressProcessor.CurrentVowelMode == KeypressProcessor.VowelMode.VowelSign)
            {
                lblVowelMode.Text = "Vowel Sign Mode";
            }
        }

        private void SetLetterLabels()
        {
            if (KeypressProcessor.CurrentVowelMode == KeypressProcessor.VowelMode.FullVowel)
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

        private void DisplayLanguageMode()
        {
            if (KeypressProcessor.CurrentLanguageMode == KeypressProcessor.LanguageMode.Bangla)
            {
                lblLanguageMode.Text = "Bangla Mode";
                this.Text = banglaInBangla;
            }
            else if (KeypressProcessor.CurrentLanguageMode == KeypressProcessor.LanguageMode.English)
            {
                lblLanguageMode.Text = "English Mode";
                this.Text = "English";
            }    
        }

        /// <summary>
        /// Reads the text immediately before the cursor or selection area.
        /// Continues reading all text until reaching any character that is not
        /// a Bangla vowel-sign or diacritic.
        /// </summary>
        private string ReadTextBeforeCursor()
        {

            lblSelectionStart.Text = txtWorkArea.SelectionStart.ToString();

            // Starts at the character directly before the cursor / selection area.
            int index = txtWorkArea.SelectionStart - 1;
            string text = "";

            // Keep going as long as the index is valid.
            while (index >= 0)
            {
                // Read the character at the index.
                char ch = txtWorkArea.Text[index];

                // Add the character to the front of the text.
                text = ch + text;

                // If the character is anything other than a Bangla vowel-sign or diacritic,
                // break out of the loop.
                if (!BanglaUnicodeData.IsBanglaVowelSign(ch) && !BanglaUnicodeData.IsBanglaDiacritic(ch))
                {
                    break;
                }

                // Move to the previous character.
                index--;
            }

            // Displays the text in the label.
            lblBeforeCursor.Text = text;

            return text;
        }

        private void txtWorkArea_KeyUp(object sender, KeyEventArgs e)
        {
            KeypressProcessor.KeyUpHandler(e);
        }

        private void txtWorkArea_KeyDown(object sender, KeyEventArgs e)
        {
            KeypressProcessor.KeyDownHandler(e);
        }

        private void txtWorkArea_SelectionChanged(object sender, EventArgs e)
        {
            // The KeypressProcessor must set the vowel mode according to the position of the new selection.
            KeypressProcessor.AutoSetVowelMode();
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
            txtWorkArea.Font = new Font(txtWorkArea.Font.FontFamily, fontSize, txtWorkArea.Font.Style);

        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
        }
    }
}